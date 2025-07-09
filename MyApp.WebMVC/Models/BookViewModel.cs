namespace MyApp.WebMVC.Models
{
    public class BookViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Publisher { get; set; }
        public Guid UserId { get; set; }
    }
}
