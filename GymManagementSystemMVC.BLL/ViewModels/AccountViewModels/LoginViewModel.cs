using System.ComponentModel.DataAnnotations;

namespace GymManagementSystemMVC.BLL.ViewModels.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;

        public bool RememberMe { get; set; }
    }
}
