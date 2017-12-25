using MVCControlsToolkit.Controls;
using Remont.Business;
using Remont.Core;
using Remont.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Remont.Controllers
{
	public class HomeController : Controller
	{
		public IHomeManager Manager { get; private set; }

		public HomeController(IHomeManager manager)
		{
			Manager = manager;
		}

		public ActionResult Index(SkillFilter filter = null)
		{
			ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

			if (filter != null && filter.SelectedSkillIds != null && filter.SelectedSkillIds.Count() > 0)
			{
				ViewBag.Workers = Manager.GetWorkers().Where(w => filter.SelectedSkillIds.Intersect(w.Skills.Select(s => s.SkillId)).Count() > 0).ToList();
			}
			else
			{
				ViewBag.Workers = Manager.GetWorkers().ToList();
			}
			var choiseList = ChoiceListHelper.Create(Manager.GetSkills(), (t => t.SkillId), (t => t.Code));
			ViewBag.ChoiseList = choiseList;

			return View(filter);
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your app description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}
