using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebLibApp.ViewModels
{
    public class BookEditViewModel : BookCreateViewModel
    {
        public int BookId { get; set; }
        public string ExistingPhotoPath { get; set; }
    }
}