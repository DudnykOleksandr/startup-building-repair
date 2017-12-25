using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using Remont.Models;
using Postal;
using Remont.Filters;
using System.Data;
using MVCControlsToolkit.Controls;
using Remont.Core;
using Remont.Business;

namespace Remont.Controllers
{
	[Authorize]
	[InitializeSimpleMembership]
	public class AccountController : Controller
	{
		public IAccountManager Manager { get; private set; }

		public AccountController(IAccountManager manager)
		{
			Manager = manager;
		}
		//
		// GET: /Account/Login

		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}

		//
		// POST: /Account/Login

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult Login(LoginModel model, string returnUrl)
		{
			if (ModelState.IsValid && Manager.Login(model.UserName, model.Password, model.RememberMe))
			{
				return RedirectToLocal(returnUrl);
			}
			ModelState.AddModelError("", Resources.Resource.LoginPassError);
			return View(model);
		}

		//
		// POST: /Account/LogOff

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LogOff()
		{
			Manager.LogOff();

			return RedirectToAction("Index", "Home");
		}

		//
		// GET: /Account/Register

		[AllowAnonymous]
		public ActionResult Register()
		{
			return View();
		}

		//
		// POST: /Account/Register

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult Register(RegisterModel model)
		{
			if (ModelState.IsValid)
			{
				// Attempt to register the user
				try
				{
					Manager.Register(model.UserName, model.Password, model.Email, Url);

					return RedirectToAction("RegisterStepTwo", "Account");
				}
				catch (MembershipCreateUserException e)
				{
					ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		[AllowAnonymous]
		public ActionResult RegisterStepTwo()
		{
			return View();
		}

		[AllowAnonymous]
		public ActionResult RegisterConfirmation(string ct)
		{
			if (Manager.ConfirmAccount(ct))
			{
				return RedirectToAction("ConfirmationSuccess");
			}
			return RedirectToAction("ConfirmationFailure");
		}

		[AllowAnonymous]
		public ActionResult ConfirmationSuccess()
		{
			return View();
		}

		[AllowAnonymous]
		public ActionResult ProfileEdit(int[] skillIds)
		{
			Manager.ProfileEdit(User.Identity.Name, skillIds);

			return RedirectToAction("Manage");
		}

		[AllowAnonymous]
		public ActionResult ConfirmationFailure()
		{
			return View();
		}

		//
		// GET: /Account/Manage

		public ActionResult Manage(ManageMessageId? message)
		{
			ViewBag.StatusMessage =
				message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed." : "";

			ViewBag.ReturnUrl = Url.Action("Manage");

			var user = Manager.GetUserProfile();

			var skills = new MultiSelectList(Manager.GetSkills(), "SkillId", "Code", user.Worker != null ?
					user.Worker.Skills.Select(s => s.SkillId.ToString()) : null);
			ViewBag.Skills = skills;

			var choiseList = ChoiceListHelper.Create(Manager.GetSkills(), (t => t.SkillId), (t => t.Code));
			ViewBag.ChoiseList = choiseList;
			ViewBag.Worker = user.Worker;

			return View();
		}

		//
		// POST: /Account/Manage

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Manage(LocalPasswordModel model)
		{
			ViewBag.ReturnUrl = Url.Action("Manage");
			if (ModelState.IsValid)
			{
				// ChangePassword will throw an exception rather than return false in certain failure scenarios.
				bool changePasswordSucceeded;
				try
				{
					changePasswordSucceeded = Manager.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
				}
				catch (Exception)
				{
					changePasswordSucceeded = false;
				}

				if (changePasswordSucceeded)
				{
					return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
				}
				else
				{
					ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		// GET: Account/LostPassword
		[AllowAnonymous]
		public ActionResult LostPassword()
		{
			return View();
		}

		// POST: Account/LostPassword
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult LostPassword(LostPasswordModel model)
		{
			if (ModelState.IsValid)
			{
				string errorCode = string.Empty;
				if (Manager.ResetPassword(model.Email, Url, out errorCode))
				{
					ViewBag.Message = Resources.Resource.EmailSentMsg;
				}
				else
				{
					if (errorCode == ErrorCodes.EmailSentError.ToString())
					{
						ModelState.AddModelError("", Resources.Resource.EmailSendError);
					}
					else if (errorCode == ErrorCodes.UserNotFoundError.ToString())
					{
						ModelState.AddModelError("", Resources.Resource.NoUserWithThatEmailError);
					}
				}
			}
			return View(model);
		}

		[AllowAnonymous]
		public ActionResult ResetPassword(string rt)
		{
			ResetPasswordModel model = new ResetPasswordModel();
			model.ReturnToken = rt;
			return View(model);
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult ResetPassword(ResetPasswordModel model)
		{
			if (ModelState.IsValid)
			{
				bool resetResponse = Manager.ResetPassword(model.ReturnToken, model.Password);
				if (resetResponse)
				{
					ViewBag.Message = Resources.Resource.SuccessfullyChangedMsg;
				}
				else
				{
					ViewBag.Message = Resources.Resource.ErrorMsg;
				}
			}
			return View(model);
		}

		#region Helpers
		private ActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			else
			{
				return RedirectToAction("Index", "Home");
			}
		}

		public enum ManageMessageId
		{
			ChangePasswordSuccess,
			SetPasswordSuccess,
			RemoveLoginSuccess,
		}

		private static string ErrorCodeToString(MembershipCreateStatus createStatus)
		{
			switch (createStatus)
			{
				case MembershipCreateStatus.DuplicateUserName:
					return "User name already exists. Please enter a different user name.";

				case MembershipCreateStatus.DuplicateEmail:
					return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

				case MembershipCreateStatus.InvalidPassword:
					return "The password provided is invalid. Please enter a valid password value.";

				case MembershipCreateStatus.InvalidEmail:
					return "The e-mail address provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidAnswer:
					return "The password retrieval answer provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidQuestion:
					return "The password retrieval question provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidUserName:
					return "The user name provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.ProviderError:
					return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

				case MembershipCreateStatus.UserRejected:
					return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

				default:
					return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
			}
		}
		#endregion
	}
}
