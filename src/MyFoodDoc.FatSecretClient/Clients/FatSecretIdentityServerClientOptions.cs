namespace MyFoodDoc.FatSecretClient.Clients
{
    public class FatSecretIdentityServerClientOptions
    {
        public string Address { get; set; }

        public string GrantType { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Scope { get; set; }
    }
}
