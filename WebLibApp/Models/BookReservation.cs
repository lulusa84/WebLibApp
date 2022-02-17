using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace WebLibApp.Models
{
    public class BookReservation
    {
        [Required]
        public int BookReservationId { get; set; }

        [Required]
        [Display(Name = "Copy")]
        public int CopyId { get; set; }

        // [NotMapped], virtual
        public Copy Copy { get; set; }

        [Required]
        [Display(Name = "AppUserId")]
        public string AppUserId { get; set; }

        //[NotMapped], virtual
        public ApplicationUser User { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime ReservationDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = " End Date")]
        public DateTime ReservationEndDate { get; set; }



    }
}
