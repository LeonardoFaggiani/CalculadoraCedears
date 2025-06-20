fn main() {
    tauri_build::build();

    let api_base_url = std::env::var("API_BASE_URL")
        .unwrap_or("https://localhost:7016/api/".to_string());

    println!("cargo:rustc-env=API_BASE_URL={}", api_base_url);
}
