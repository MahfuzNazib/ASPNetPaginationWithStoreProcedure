namespace TestApp.Models
{
    public class PageSummary
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public int first_page { get; set; }
        public int last_page { get; set; }

        public string RedirectUrlMethodName { get; set; } = "Index";

    }
}
