using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
	public class SignUpViewModel
	{
        [Required(ErrorMessage = "UserName Is Required")]
        public string UserName { get; set; }

		[Required(ErrorMessage = "FirstName Is Required")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "LastName Is Required")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "Email Is Required")]
		[EmailAddress(ErrorMessage = "Invaild Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password Is Required")]
		[DataType(DataType.Password)]
		[MinLength(5, ErrorMessage = "Password Min length is 5")]
		public string Password { get; set; }

		[Required(ErrorMessage = "ConfirmPassword Is Required")]
		[DataType(DataType.Password)]
		[Compare(nameof(SignUpViewModel.Password),ErrorMessage = "Confirmed Password does not match password !!")]
		public string ConfirmPassword { get; set; }

		[Required(ErrorMessage = "IsAgree Is Required")]
		public bool IsAgree { get; set; }
    }
}
