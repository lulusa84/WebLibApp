using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebLibApp.Models
{
    public class BorrowRules
    {
        public int Duration { get; set; }

        [DataType(DataType.Currency)]
        public int DelayFee { get; set; }

        [DataType(DataType.Currency)]
        public int ExtraDelayFee { get; set; }

    }
}
