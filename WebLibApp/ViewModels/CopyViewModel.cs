using WebLibApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace WebLibApp.ViewModels
{
    public class CopyViewModel 

    {  [Key]
        public int id { get; set; }
        public Copy Copy { get; set; }

        public string PageTitle { get; set; }

        public bool IsAvailable { get; set; }

        public bool IsReservationAllowed { get; set; }

        public ICollection<BorrowHistory> BorrowHistories { get; set; }
        public ICollection<BookReservation> BookReservations { get; set; }
    }
}
