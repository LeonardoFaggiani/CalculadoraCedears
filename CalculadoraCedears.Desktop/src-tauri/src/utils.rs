use serde::{Deserialize, Serialize};

#[derive(Serialize, Deserialize)]
pub struct UserInfo {
    pub sub: String,
    pub email: String,
    pub name: String,
    #[serde(skip_deserializing)] 
    pub token: String,
}

pub fn get_base_url() -> String {
    env!("API_BASE_URL").to_string()
}


pub fn decode_jwt_unverified(token: &str) -> Result<UserInfo, String> {
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

    Ok(user_info)
}