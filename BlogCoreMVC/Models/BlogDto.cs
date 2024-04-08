using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BlogCoreMVC.Models
{
    public class BlogDto
    {
        [Required, MaxLength(100)]
        public string Title { get; set; } = "";

        [Required]
        public string Description { get; set; } = "";
        [Required, MinLength(50)]
        public string Content { get; set; } = "";
                
        public IFormFile? ImageFile { get; set; }
    }
}
