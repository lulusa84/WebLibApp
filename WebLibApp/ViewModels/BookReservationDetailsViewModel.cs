using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLibApp.Models;

namespace WebLibApp.ViewModels
{
    public class BookReservationDetailsViewModel
    {
        public BookReservation BookReservation { get; set; }
        public string PageTitle { get; set; }
        public string UserName { get; set; }
    }
}
