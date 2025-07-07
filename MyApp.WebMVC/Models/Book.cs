namespace MyApp.WebMVC.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Publisher { get; set; }
        public Guid UserId { get; set; }
    }
}
