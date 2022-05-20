using System.ComponentModel.DataAnnotations;
using System.Web;

namespace FacebookPro.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Name is Required")]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is Required")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "mobile is Required")]
       
        public int mobile { get; set; }
        [Required(ErrorMessage = "City is Required")]
        public string city { get; set; }
        [Required(ErrorMessage = "Country is Required")]
        public string country { get; set; }
        public string Message { get; set; }
        public string ImagePath { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }

    }
}