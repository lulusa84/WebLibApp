using WebLibApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebLibApp.ViewModels
{
    public class HomeDetailsViewModel
    {

        /* public HomeDetailsViewModel(Book book)
         {
             Book = book;
         }*/

        public Book Book { get; set; }
        public string PageTitle { get; set; }



    }
}
