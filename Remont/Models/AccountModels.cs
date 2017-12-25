using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace Remont.Models
{

	public class LocalPasswordModel
	{
		[Required]
		[DataType(DataType.Password)]
		public string OldPassword { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 6)]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }

		[DataType(DataType.Password)]
		[Compare("NewPassword")]
		public string ConfirmPassword { get; set; }
	}

	public class LoginModel
	{
		[Required]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		public bool RememberMe { get; set; }
	}

	public class RegisterModel
	{
		[Required]
		public string UserName { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 6)]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Compare("Password")]
		public string ConfirmPassword { get; set; }

		[Required]
		[EmailAddress(ErrorMessageResourceType = typeof(Resources.Resource),
				  ErrorMessageResourceName = "EmailNotValid", ErrorMessage = null)]
		public string Email { get; set; }
	}

	public class LostPasswordModel
	{
		[Required]
		[EmailAddress(ErrorMessageResourceType = typeof(Resources.Resource),
		  ErrorMessageResourceName = "EmailNotValid", ErrorMessage = null)]
		public string Email { get; set; }
	}

	public class ResetPasswordModel
	{
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Compare("Password")]
		public string ConfirmPassword { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resources.Resource),
				  ErrorMessageResourceName = "ErrorMsg")]
		public string ReturnToken { get; set; }
	}
}
