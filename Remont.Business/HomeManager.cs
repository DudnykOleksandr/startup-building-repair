using Remont.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remont.Business
{
	public class HomeManager : ManagerBase, IHomeManager
	{
		public HomeManager(IRemontRepository repository)
			: base(repository)
		{
		}
	}
}
