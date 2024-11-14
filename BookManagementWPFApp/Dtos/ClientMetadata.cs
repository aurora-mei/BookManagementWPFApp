namespace BookManagementWPFApp.Dtos
{
    public class ClientMetadata
    {
        public string name { get; set; }
        public string display_name { get; set; }
        public string logo_uri { get; set; }
        public string[] scopes { get; set; }
        public string ui_type { get; set; }
    }
}
