using System.ComponentModel.DataAnnotations;

namespace WebLibApp.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
