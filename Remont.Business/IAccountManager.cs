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
	public interface IAccountManager : IManagerBase
	{
		bool Login(string userName, string password, bool rememberMe);

		void LogOff();

		void Register(string userName, string password, string email, UrlHelper url);

		bool ConfirmAccount(string ct);

		bool ChangePassword(string userName, string oldPassword, string newPassword);

		void ProfileEdit(string userName, int[] skillIds);

		bool ResetPassword(string email, UrlHelper url, out string errorCode);

		bool ResetPassword(string returnToken, string password);

		string GetUserName();

		UserProfile GetUserProfile(string userName = "");

	}
}
