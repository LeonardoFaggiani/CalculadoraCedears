pub fn get_base_url() -> String {
    std::env::var("API_BASE_URL").unwrap_or_else(|_| "https://localhost:7016/api/".to_string())
}