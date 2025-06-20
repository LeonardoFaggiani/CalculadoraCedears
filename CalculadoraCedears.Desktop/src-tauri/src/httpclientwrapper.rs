use crate::utils::get_base_url;
use reqwest;
use serde::{Deserialize, Serialize};
use serde_json::Value;
use std::collections::HashMap;

#[derive(Debug, Serialize, Deserialize)]
pub struct ApiResponse {
    pub success: bool,
    pub data: Option<Value>,
    pub error: Option<String>,
}

#[tauri::command]
pub async fn http_request(
    endpoint: String,
    method: String,
    body: Option<String>,
    headers: Option<HashMap<String, String>>,
) -> Result<ApiResponse, String> {
    let client = reqwest::Client::new();

    let full_url = format!("{}{}", get_base_url(), endpoint);

    log::info!("Endpoiunt que se llama en httpwrapper: {}", full_url);

    let mut request_builder = match method.to_uppercase().as_str() {
        "GET" => client.get(&full_url),
        "POST" => client.post(&full_url),
        "PUT" => client.put(&full_url),
        "DELETE" => client.delete(&full_url),
        "PATCH" => client.patch(&full_url),
        _ => return Err("Método HTTP no soportado".to_string()),
    };

    request_builder = request_builder.header("Content-Type", "application/json");

    if let Some(custom_headers) = headers {
        for (key, value) in custom_headers {
            request_builder = request_builder.header(&key, &value);
        }
    }

    if let Some(request_body) = body {
        request_builder = request_builder.body(request_body);
    }

    match request_builder.send().await {
        Ok(response) => {
            let status = response.status();

            if status.is_success() {
                match response.json::<Value>().await {
                    Ok(json_data) => Ok(ApiResponse {
                        success: true,
                        data: Some(json_data),
                        error: None,
                    }),
                    Err(e) => Err(format!("Error parseando respuesta: {}", e)),
                }
            } else {
                let error_text = response
                    .text()
                    .await
                    .unwrap_or_else(|_| "Error desconocido".to_string());
                Ok(ApiResponse {
                    success: false,
                    data: None,
                    error: Some(format!("HTTP {}: {}", status.as_u16(), error_text)),
                })
            }
        }
        Err(e) => Err(format!("Error de conexión: {}", e)),
    }
}
