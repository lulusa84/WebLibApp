using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
//using WebLibApp.Migrations;

namespace WebLibApp.Models
{
    public class Author

    {
        
        [Key]
        [Required]
        public int AuthorId { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "Name cannot exceed 50 characters")]
        public string AuthorName { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Author cannot exceed 50 characters")]
        public string AuthorSurname { get; set; }


        [Required]
        public virtual ICollection<Book> Books { get; set; }

        /*public ICollection<int> bookIdCollection { get; set; } */
    }
}
