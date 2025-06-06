namespace CalculadoraCedears.Api.Dto.Users.Response
{
    public class CreateUserCommandResponse
    {
        public CreateUserCommandResponse(string jwt)
        {
            this.Jwt = jwt;
        }
        public string Jwt { get; set; }

    }
}
