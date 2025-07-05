use crate::utils::get_base_url;
use crate::utils::update_current_user_tokens;
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
    app_handle: tauri::AppHandle,
    endpoint: String,
    method: String,
    body: Option<String>,
    headers: Option<HashMap<String, String>>,
) -> Result<ApiResponse, String> {
    let client = reqwest::Client::new();
    let full_url = format!("{}{}", get_base_url(), endpoint);
    let original_headers = headers.clone();

    // Primera llamada
    let response_result = send_request(&client, &full_url, &method, &body, headers).await;

    // Si hubo error 401 y se tiene refreshToken, intentamos refresh
    if let Ok(response) = &response_result {
        if let Some(error_msg) = &response.error {
            if error_msg.contains("401") {
                if let (Some(refresh_token), Some(access_token)) = (
                        get_refresh_token_from_headers(&original_headers),
                        get_access_token_from_headers(&original_headers),
                    )  {
                    if let Ok((new_token, new_refresh_token)) = refresh_access_token(&client, &refresh_token, &access_token).await {
                        let _ = update_current_user_tokens(&app_handle, &new_token, &new_refresh_token);
                        let mut new_headers = original_headers.unwrap_or_default();
                        new_headers.insert(
                            "Authorization".to_string(),
                            format!("Bearer {}", new_token),
                        );

                        return send_request(&client, &full_url, &method, &body, Some(new_headers))
                            .await;
                    }
                }
            }
        }
    }

    response_result
}

fn get_refresh_token_from_headers(
    headers: &Option<HashMap<String, String>>,
) -> Option<String> {
    headers.as_ref()?.get("X-Refresh-Token").cloned()
}

fn get_access_token_from_headers(
    headers: &Option<HashMap<String, String>>,
) -> Option<String> {
    headers.as_ref()?.get("Authorization").cloned()
}

async fn refresh_access_token(
    client: &reqwest::Client,
    refresh_token: &str,
    access_token: &str,
) -> Result<(String, String), String> {
    let refresh_url = format!("{}{}", get_base_url(), "Auth/refresh");

    log::info!("Endpoint a llamar:{}", refresh_url);

    let response = client
        .post(&refresh_url)
        .header("Content-Type", "application/json")
        .body(format!(
            r#"{{"accessToken": "{}", "refreshToken": "{}"}}"#,
            access_token.strip_prefix("Bearer ").unwrap_or(access_token),
            refresh_token
        ))
        .send()
        .await
        .map_err(|e| format!("Error enviando refresh: {}", e))?;

        if response.status().is_success() {
            let json: serde_json::Value = response.json().await.map_err(|e| format!("Error parseando respuesta de refresh: {}", e))?;

            let jwt = json.get("jwt").and_then(|v| v.as_str()).ok_or("Token no encontrado en respuesta de refresh")?;
            let new_refresh_token = json.get("refreshToken").and_then(|v| v.as_str()).ok_or("refreshToken no encontrado en respuesta de refresh")?;

            Ok((jwt.to_string(), new_refresh_token.to_string()))
        } 
        else {
            Err(format!(
                "Falló refresh con status {}",
                response.status().as_u16()
            ))
        }
}

async fn send_request(
    client: &reqwest::Client,
    url: &str,
    method: &str,
    body: &Option<String>,
    headers: Option<HashMap<String, String>>,
) -> Result<ApiResponse, String> {
    let mut request_builder = match method.to_uppercase().as_str() {
        "GET" => client.get(url),
        "POST" => client.post(url),
        "PUT" => client.put(url),
        "DELETE" => client.delete(url),
        "PATCH" => client.patch(url),
        _ => return Err("Método HTTP no soportado".to_string()),
    };

    request_builder = request_builder.header("Content-Type", "application/json");

    if let Some(custom_headers) = headers {
        for (key, value) in custom_headers {
            request_builder = request_builder.header(&key, &value);
        }
    }

    if let Some(request_body) = body {
        request_builder = request_builder.body(request_body.clone());
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