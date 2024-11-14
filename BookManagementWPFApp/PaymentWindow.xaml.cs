using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows;

namespace BookManagementWPFApp
{
    public partial class PaymentWindow : Window
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
            public Client_Metadata client_metadata { get; set; }
        }
        public class Client_Metadata
        {
            public string name { get; set; }
            public string display_name { get; set; }
            public string logo_uri { get; set; }
            public string[] scopes { get; set; }
            public string ui_type { get; set; }
        }


        private const string SandboxEmail = "sb-2kqvs34016732@business.example.com";
        private const string pass = "VG/_qw2U";
        private readonly HttpClient _httpClient;
        IConfiguration configuration;
        private string _clientId;
        private string _clientSecret;
        private string BaseURL = "https://api.sandbox.paypal.com/v1/oauth2/token";
        public PaymentWindow()
        {
            InitializeComponent();
            LoadData();
            _httpClient = new HttpClient();
            configuration = new ConfigurationBuilder()
                .AddJsonFile("Credentials.json", true, true)
                .Build();
            _clientId = configuration["PAYPAL:CLIENTID"];
            _clientSecret = configuration["PAYPAL:SECRET"];
        }

        private void LoadData()
        {

        }

        private async void btn_payClick_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Generate the access token first
                var response = await GetAuthorizationResponse();
                MessageBox.Show(response.access_token);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task<AuthorizationResponseData> GetAuthorizationResponse()
        {
            var byteArray = Encoding.ASCII.GetBytes($"{_clientId}:{_clientSecret}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var keyValuePairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            };

            var response = await _httpClient.PostAsync(BaseURL, new FormUrlEncodedContent(keyValuePairs));
            var responseAsString = await response.Content.ReadAsStringAsync();
            var authorizationResp = JsonConvert.DeserializeObject<AuthorizationResponseData>(responseAsString);
            return authorizationResp;
        }
    }
}
