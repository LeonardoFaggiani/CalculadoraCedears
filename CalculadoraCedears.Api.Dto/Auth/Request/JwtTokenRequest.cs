namespace CalculadoraCedears.Api.Dto.Auth.Request
{
    public class JwtTokenRequest
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
