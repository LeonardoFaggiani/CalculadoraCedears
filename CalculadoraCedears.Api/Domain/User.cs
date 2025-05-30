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
        }

        public new int Id { get; set; }
        public string ThirdPartyUserId { get; protected set; }
        public string Email { get; protected set; }
        public DateTime LastLogin { get; protected set; }
        public virtual ICollection<CedearsStockHolding> CedearsStockHoldings { get; protected set; }

        public void SetLastLogin()
        {
            this.LastLogin = DateTime.Now;
        }
    }
}
