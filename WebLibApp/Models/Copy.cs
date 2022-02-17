using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace WebLibApp.Models
{
    public class Copy
    {
       
        [Required]
        public int CopyId { get; set; }

        [Required]
        public CopyType? CopyType { get; set; }

        [Required]
        [Display(Name = "Book")]
        public int BookId { get; set; }

        //[NotMapped], virtual
        public Book Book { get; set; }

        //1 to many, if registered there's at least 1 copy       
        public ICollection<BookReservation> BookReservations { get; set; }

        //1 to many, if registered there's at least 1 copy
        public ICollection<BorrowHistory> BorrowHistories { get; set; }

        //public Nullable<int> BorrowHistoryId { get; set; }
        //public Nullable<int> BookReservationId { get; set; }
    }
}

