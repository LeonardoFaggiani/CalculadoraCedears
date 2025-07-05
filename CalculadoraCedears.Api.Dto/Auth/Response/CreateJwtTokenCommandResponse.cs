namespace CalculadoraCedears.Api.Dto.Auth.Response
{
    public class CreateJwtTokenCommandResponse
    {
        public CreateJwtTokenCommandResponse(string jwt, string refreshToken)
        {
            this.Jwt = jwt;
            this.RefreshToken = refreshToken;
        }
        public string Jwt { get; }
        public string RefreshToken { get; }
    }
}
