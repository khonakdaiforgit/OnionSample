using System.ComponentModel.DataAnnotations;

namespace MyApp.WebMVC.Models
{
    public class AddBookViewModel
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Publisher is required")]
        public string Publisher { get; set; }
    }
}
