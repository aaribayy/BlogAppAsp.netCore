using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BlogCoreMVC.Models
{
    public class BlogTb
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        [MinLength(50)]
        public string Content { get; set; } = "";

        [MaxLength(100)]
        public string ImageFileName { get; set; } = "";
        public DateTime PublishedDate { get; set; }

        public int AuthorId { get; set; }
        public AuthorTb author { get; set; }



    }
}
