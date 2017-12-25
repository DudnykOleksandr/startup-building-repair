using Postal;
using Remont.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace Remont.Business
{
	public class AccountManager : ManagerBase, IAccountManager
	{
		public AccountManager(IRemontRepository repository)
			: base(repository)
		{
		}

		public bool Login(string userName, string password, bool rememberMe)
		{
			return WebSecurity.Login(userName, password, persistCookie: rememberMe);
		}
		public void LogOff()
		{
			WebSecurity.Logout();
		}

		public void Register(string userName, string password, string email, UrlHelper url)
		{
			string confirmationToken =
						WebSecurity.CreateUserAndAccount(userName, password, new { Email = email }, true);
			dynamic emailObj = new Email("RegEmail");
			Roles.AddUserToRole(userName, "User");

			emailObj.To = email;
			emailObj.UserName = userName;
			emailObj.ConfirmationToken = url.Action("RegisterConfirmation", "Account", new { ct = confirmationToken }, "http");

			emailObj.Send();
		}

		public bool ConfirmAccount(string ct)
		{
			return WebSecurity.ConfirmAccount(ct);
		}

		public bool ChangePassword(string userName, string oldPassword, string newPassword)
		{
			return WebSecurity.ChangePassword(userName, oldPassword, newPassword);
		}

		public void ProfileEdit(string userName, int[] skillIds)
		{
			var user = GetUserProfile(userName);

			if (user != null)
			{
				var skillsList = Repository.GetSkills().Where(s => skillIds.Contains(s.SkillId)).ToList();
				Repository.UpdateWorker(user, skillsList);
			}
		}

		public bool ResetPassword(string email, UrlHelper url, out string errorCode)
		{
			UserProfile user;
			var userName = GetUserName(email);
			if (!string.IsNullOrEmpty(userName))
			{
				user = GetUserProfile(userName.ToString());
			}
			else
			{
				errorCode = ErrorCodes.UserNotFoundError.ToString();
				return false;
			}
			// Generae password token that will be used in the email link to authenticate user
			var token = WebSecurity.GeneratePasswordResetToken(user.UserName);
			dynamic emailObj = new Email("ChngPasswordEmail");
			emailObj.To = email;
			emailObj.UserName = user.UserName;
			emailObj.ConfirmationToken = url.Action("ResetPassword", "Account", new { rt = token }, "http");

			try
			{
				emailObj.Send();
			}
			catch
			{
				errorCode = ErrorCodes.EmailSentError.ToString();
				return false;
			}

			errorCode = string.Empty;
			return true;
		}

		public bool ResetPassword(string returnToken, string password)
		{
			return WebSecurity.ResetPassword(returnToken, password);
		}
		public string GetUserName()
		{
			return WebSecurity.CurrentUserName;
		}
		public string GetUserName(string email)
		{
			var foundUserName = (from u in Repository.GetUserProfiles()
								 where u.Email == email
								 select u.UserName).FirstOrDefault();
			return foundUserName;
		}
		public UserProfile GetUserProfile(string userName = "")
		{
			return Repository.GetUserProfiles().SingleOrDefault(u => u.UserName == (string.IsNullOrEmpty(userName) ? GetUserName() : userName));
		}
	}
}
