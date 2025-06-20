fn main() {
    tauri_build::build();

      // Embedear la variable API_BASE_URL en el binario
    if let Ok(api_base_url) = std::env::var("API_BASE_URL") {
        println!("cargo:rustc-env=API_BASE_URL={}", api_base_url);
    } else {
        // Esto permite tener un valor por defecto si no se define
        println!("cargo:rustc-env=API_BASE_URL=https://localhost:7016/api/");
    }
}
