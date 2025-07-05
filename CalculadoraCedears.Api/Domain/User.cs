using CalculadoraCedears.Api.Infrastructure.Extensions;

using NetDevPack.Domain;

namespace CalculadoraCedears.Api.Domain
{
    public class User : Entity
    {
        protected User() { }

        public User(string thirdPartyUserId, string email)
        {
            this.ThirdPartyUserId = thirdPartyUserId;
            this.Email = email;
            this.CedearsStockHoldings = new List<CedearsStockHolding>();
            this.SetRefreshToken();
        }

        public new int Id { get; set; }
        public string ThirdPartyUserId { get; protected set; }
        public string Email { get; protected set; }
        public string RefreshToken { get; protected set; }
        public DateTime LastLogin { get; protected set; }
        public DateTime ExpiresAt { get; protected set; }
        public virtual ICollection<CedearsStockHolding> CedearsStockHoldings { get; protected set; }

        public void SetLastLogin()
        {
            this.LastLogin = DateTime.UtcNow;
        }

        public void SetRefreshToken()
        {
            this.ExpiresAt = DateTime.UtcNow.AddDays(7);
            this.RefreshToken = StringExtensions.GetRefreshToken();
        }

        public void SetLogOut()
        {
            this.ExpiresAt = DateTime.UtcNow.AddHours(-1);
        }
    }
}
