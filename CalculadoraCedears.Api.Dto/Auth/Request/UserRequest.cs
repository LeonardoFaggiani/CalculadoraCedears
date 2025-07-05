namespace CalculadoraCedears.Api.Dto.Auth.Request
{
    public class UserRequest
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string GoogleToken { get; set; }
    }
}
