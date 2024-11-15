namespace BookManagementWPFApp.Dtos
{
    // This class is the response data we get from PAYPAL generate token response
    public class AuthorizationResponseData
    {
        public string scope { get; set; }
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string app_id { get; set; }
        public int expires_in { get; set; }
        public string[] supported_authn_schemes { get; set; }
        public string nonce { get; set; }
        public ClientMetadata client_metadata { get; set; }
    }
}
