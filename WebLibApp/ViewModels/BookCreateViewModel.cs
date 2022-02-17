using Microsoft.AspNetCore.Http;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebLibApp.Models;

namespace WebLibApp.ViewModels
{
    public class BookCreateViewModel
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Title { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "Title cannot exceed 50 characters")]
        public string Author { get; set; }

        [Required]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "il contenuto deve essere maggiore di 0")]
        public int Edt { get; set; }

        public ICollection<Copy> Copies { get; set; }
        
        public IFormFile Photo { get; set; }

        //public List<IFormFile> Photo { get; set; }
    }

}