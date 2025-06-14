// Learn more about Tauri commands at https://tauri.app/develop/calling-rust/
use tauri::{Emitter, Window};
use tauri_plugin_oauth::OauthConfig;
use tauri_plugin_opener;
use tauri_plugin_shell;
use tauri_plugin_store;
use tauri_plugin_updater;

mod httpclientwrapper;
mod login;

// Importar funciones públicas
pub use httpclientwrapper::{http_request, ApiResponse};
pub use login::{login_with_provider, UserInfo};

#[tauri::command]
fn start_oauth_server(window: Window) -> Result<u16, String> {
    let config = OauthConfig {
        ports: Some(vec![8000, 8001, 8002]),
        response: Some("Login successful. You can close this window.".into()),
    };

    tauri_plugin_oauth::start_with_config(config, move |url| {
        // Send the OAuth URL back to the frontend
        let _ = window.emit("oauth_redirect", url);
    })
    .map_err(|err| err.to_string())
}

#[cfg_attr(mobile, tauri::mobile_entry_point)]
pub fn run() {
    tauri::Builder::default()
        .plugin(tauri_plugin_updater::Builder::new().build())
        .plugin(tauri_plugin_websocket::init())
        .plugin(tauri_plugin_shell::init())
        .plugin(tauri_plugin_http::init())
        .plugin(tauri_plugin_opener::init())
        .plugin(tauri_plugin_oauth::init())
        .plugin(tauri_plugin_store::Builder::default().build())
        .invoke_handler(tauri::generate_handler![
            start_oauth_server,
            login::login_with_provider,
            httpclientwrapper::http_request
        ])
        .run(tauri::generate_context!())
        .expect("error while running tauri application");
}
