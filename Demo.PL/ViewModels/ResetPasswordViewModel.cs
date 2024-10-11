using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
	public class ResetPasswordViewModel
	{
		[Required(ErrorMessage = "Password Is Required")]
		[DataType(DataType.Password)]
		[MinLength(5, ErrorMessage = "Password Min length is 5")]
		public string Password { get; set; }

		[Required(ErrorMessage = "ConfirmPassword Is Required")]
		[DataType(DataType.Password)]
		[Compare(nameof(SignUpViewModel.Password), ErrorMessage = "Confirmed Password does not match password !!")]
		public string ConfirmPassword { get; set; }
	}
}
