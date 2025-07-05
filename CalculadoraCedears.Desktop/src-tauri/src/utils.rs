use serde::{Deserialize, Serialize};
use serde_json::json;
use serde_json::Value;
use tauri_plugin_store::{StoreBuilder};
use crate::AppHandle;

#[derive(Debug, Serialize, Deserialize)]
struct User {
    token: String,
    refresh_token: String,
}

#[derive(Serialize, Deserialize)]
pub struct UserInfo {
    pub sub: String,
    pub email: String,
    pub name: String,
    #[serde(skip_deserializing)] 
    pub token: String,
    #[serde(skip_deserializing)] 
    pub refresh_token: String,
}

pub fn get_base_url() -> String {
    env!("API_BASE_URL").to_string()
}

pub fn decode_jwt_unverified(token: &str, refresh_token: &str) -> Result<UserInfo, String> {
    use base64::engine::general_purpose::URL_SAFE_NO_PAD;
    use base64::Engine;

    let parts: Vec<&str> = token.split('.').collect();
    if parts.len() != 3 {
        return Err("Invalid JWT format".into());
    }

    let payload = parts[1];
    let decoded = URL_SAFE_NO_PAD
        .decode(payload)
        .map_err(|e| format!("Base64 decode error: {}", e))?;

    let mut user_info: UserInfo = serde_json::from_slice(&decoded)
        .map_err(|e| format!("JSON parse error: {}", e))?;

    user_info.token = token.to_string();
    user_info.refresh_token = refresh_token.to_string();

    Ok(user_info)
}

pub fn update_current_user_tokens(app_handle: &AppHandle, new_token: &str, new_refresh_token: &str) -> Result<(), String> {
    let store = StoreBuilder::new(app_handle, "user-store.json").build()
        .map_err(|e| format!("Error creando el store: {e}"))?;

    let current_user = store.get("currentUser").unwrap_or(json!({}));
    let mut current_user_obj = current_user.as_object().cloned().unwrap_or_default();

    current_user_obj.insert("token".to_string(), Value::String(new_token.to_string()));
    current_user_obj.insert("refresh_token".to_string(), Value::String(new_refresh_token.to_string()));

    store.set("currentUser", Value::Object(current_user_obj));
    store.save().map_err(|e| format!("Error guardando store: {e}"))?;

    Ok(())
}