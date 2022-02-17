using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebLibApp.ViewModels
{
    public class PrivacyViewModel : PageModel
    {
        private readonly ILogger<PrivacyViewModel> _logger;

        public PrivacyViewModel(ILogger<PrivacyViewModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
