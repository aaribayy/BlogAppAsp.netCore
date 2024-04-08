using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace BlogCoreMVC.Models
{
    public class AuthorTb
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = "";

        [MaxLength(50)]
        public string Username { get; set; } = "";

        [MaxLength(50)]
        public string Password { get; set; } = "";
        public ICollection<BlogTb> Blogs { get; set; } 

    }
}
