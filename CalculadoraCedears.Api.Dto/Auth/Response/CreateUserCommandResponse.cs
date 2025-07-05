namespace CalculadoraCedears.Api.Dto.Auth.Response
{
    public class CreateUserCommandResponse
    {
        public CreateUserCommandResponse(string jwt, string refreshToken)
        {
            this.Jwt = jwt;
            this.RefreshToken = refreshToken;
        }

        public string Jwt { get; }
        public string RefreshToken { get; }

    }
}
