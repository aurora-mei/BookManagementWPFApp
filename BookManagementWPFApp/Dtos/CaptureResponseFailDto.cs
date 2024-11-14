namespace BookManagementWPFApp.Dtos;

public class CaptureResponseFailDto
{
    public string name { get; set; }
    public Details[] details { get; set; }
    public string message { get; set; }
    public string debug_id { get; set; }
    public Links[] links { get; set; }

    public class Details
    {
        public string issue { get; set; }
        public string description { get; set; }
    }

    public class Links
    {
        public string href { get; set; }
        public string rel { get; set; }
        public string method { get; set; }
    }
}