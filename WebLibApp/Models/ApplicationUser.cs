using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebLibApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string City { get; set; }
        //0 to many
        public ICollection<BorrowHistory> BorrowHistories { get; set; }
        public ICollection<BookReservation> BookReservations { get; set; }
    }
    }


