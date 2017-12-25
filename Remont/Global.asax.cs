using Griffin.MvcContrib.Localization;
using Griffin.MvcContrib.Localization.Types;
using Ninject;
using Ninject.Web.Common;
using Remont.Business;
using Remont.Core;
using Remont.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Resources;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebMatrix.WebData;

namespace Remont
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : NinjectHttpApplication
	{
		protected override IKernel CreateKernel()
		{
			var kernel = new StandardKernel();

			var resourceManagers = new ResourceManager[1];
			resourceManagers[0] = Resources.Resource.ResourceManager;
			kernel.Bind<ILocalizedStringProvider>().To<ResourceStringProvider>().
				WithConstructorArgument("resourceManager", resourceManagers);
			kernel.Unbind<ModelValidatorProvider>();

			kernel.Bind<IRemontRepository>().To<RemontRepository>();
			kernel.Bind<IAccountManager>().To<AccountManager>();
			kernel.Bind<IHomeManager>().To<HomeManager>();
			

			return kernel;
		}

		protected override void OnApplicationStarted()
		{
			AreaRegistration.RegisterAllAreas();

			MVCControlsToolkit.Core.Extensions.Register();

			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			Database.SetInitializer<RemontContext>(new InitDb());
			using (RemontContext context = new RemontContext())
			{
				context.Database.Initialize(true);
			}

			AuthConfig.RegisterAuth();

			var stringProvider = new ResourceStringProvider(Resources.Resource.ResourceManager);
			ModelMetadataProviders.Current = new LocalizedModelMetadataProvider(stringProvider);
			ModelValidatorProviders.Providers.Clear();
			ModelValidatorProviders.Providers.Add(new LocalizedModelValidatorProvider());

		}
	}
}