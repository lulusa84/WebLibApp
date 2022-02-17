using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using LanguageExt.ClassInstances.Pred;
using Gremlin.Net.Process.Traversal;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json.Serialization;
using System.Diagnostics.CodeAnalysis;
using Bogus;

namespace WebLibApp.Models
{   //pubblication
    public class Book
    {
        [Required]
        public int BookId { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Title cannot exceed 50 characters")]
        public string Title { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "Author cannot exceed 50 characters")]
        public string Author { get; set; }

        //public int AuthorId  { get; set; }
        //public Author Author { get; set; }

        [Required]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "il contenuto deve essere maggiore di 0")]
        public int Edt { get; set; }
       
        //1 to many, if registered there's at least 1 copy
        public ICollection<Copy> Copies { get; set; }

        public string PhotoPath { get; set; }

    }
}
