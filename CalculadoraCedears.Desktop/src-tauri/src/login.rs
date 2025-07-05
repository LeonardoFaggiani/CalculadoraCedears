use crate::utils::get_base_url;
use crate::utils::decode_jwt_unverified;
pub use crate::utils::UserInfo;

use serde::{Deserialize, Serialize};
use tauri::Window;
use url::Url;

#[derive(Debug, Serialize, Deserialize, Clone)]
pub struct OAuthConfig {
    pub client_id: String,
    pub client_secret: String,
    pub auth_url: String,
    pub token_url: String,
    pub user_info_url: String,
    pub scope: String,
}

#[derive(Debug, Serialize, Deserialize, Clone)]
pub struct OAuthConfigs {
    pub google: OAuthConfig,
}

#[derive(Debug, Clone)]
pub struct TokenResponse {
    pub token: String,
    pub refresh_token: String,
}

const CONFIG_STR: &str = include_str!("../../src-tauri/oauth_config.json");


#[tauri::command]
pub async fn login_with_provider(_window: Window, provider: String) -> Result<UserInfo, String> {

    let config = get_provider_config(&provider)?;
    let redirect_url = format!("{}{}", get_base_url(), "Auth/callback");

    let token_response  = perform_oauth_flow(&config, &redirect_url)?;    

    let user_info = decode_jwt_unverified(&token_response.token, &token_response.refresh_token)?;

    Ok(user_info)
}

fn get_provider_config(provider: &str) -> Result<OAuthConfig, String> {
    let configs = load_oauth_configs()?;
    match provider {
        "google" => Ok(configs.google),
        _ => Err(format!("Unsupported provider: {}", provider)),
    }
}

fn perform_oauth_flow(config: &OAuthConfig, redirect_url: &str) ->  Result<TokenResponse, String> {
    let oauth_config = tauri_plugin_oauth::OauthConfig {
        ports: Some(vec![8000, 8001, 8002]),
        response: None,
    };

    let (tx, rx) = std::sync::mpsc::channel::<TokenResponse>();
    let tx_clone = tx.clone();

    tauri_plugin_oauth::start_with_config(oauth_config, move |url| {
        if let Ok(url_obj) = Url::parse(&url) {            
            let mut token = None;
            let mut refresh_token = None;
            
            for (key, value) in url_obj.query_pairs() {
                match key.as_ref() {
                    "token" => token = Some(value.to_string()),
                    "refreshToken" => refresh_token = Some(value.to_string()),
                    _ => {}
                }  
            }

            if let (Some(tok), Some(refresh_tok)) = (token, refresh_token) {      

                let _ = tx_clone.send(TokenResponse {
                    token: tok,
                    refresh_token: refresh_tok,
                });
            }
        }
    })
    .map_err(|err| format!("Failed to start OAuth listener: {}", err))?;

    let mut auth_url = Url::parse(&config.auth_url)
        .map_err(|err| format!("Invalid auth URL: {}", err))?;

    auth_url.query_pairs_mut()
        .append_pair("client_id", &config.client_id)
        .append_pair("redirect_uri", redirect_url)
        .append_pair("scope", &config.scope)
        .append_pair("response_type", "code")
        .append_pair("state", &generate_random_string(16));

    tauri_plugin_opener::open_url(auth_url.as_str(), None::<&str>)
        .map_err(|err| format!("Failed to open browser: {}", err))?;

    rx.recv().map_err(|err| format!("Failed to receive token: {}", err))
}

fn generate_random_string(length: usize) -> String {
    use rand::{thread_rng, Rng};
    const CHARSET: &[u8] = b"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    let mut rng = thread_rng();

    (0..length)
        .map(|_| {
            let idx = rng.gen_range(0..CHARSET.len());
            CHARSET[idx] as char
        })
        .collect()
}

fn load_oauth_configs() -> Result<OAuthConfigs, String> {
    let configs: OAuthConfigs = serde_json::from_str(CONFIG_STR)
        .map_err(|e| format!("Failed to parse config: {}", e))?;

    Ok(configs)
}
