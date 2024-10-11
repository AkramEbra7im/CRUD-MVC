using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Email Is Required")]
		[EmailAddress(ErrorMessage = "Invaild Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password Is Required")]
		[DataType(DataType.Password)]
		[MinLength(5, ErrorMessage = "Password Min length is 5")]
		public string Password { get; set; }

		public bool RememberMe { get; set; }
    }
}
