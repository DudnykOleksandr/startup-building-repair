using Remont.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remont.Business
{
	public class ManagerBase
	{
		public IRemontRepository Repository { get; private set; }

		public ManagerBase(IRemontRepository repository = null)
		{
			Repository = repository;
		}

		public IEnumerable<Skill> GetSkills()
		{
			return Repository.GetSkills();
		}

		public IEnumerable<Worker> GetWorkers()
		{
			return Repository.GetWorkers();
		}

		public IEnumerable<UserProfile> GetUserProfiles()
		{
			return Repository.GetUserProfiles();
		}

		public void UpdateWorker(UserProfile user, IList<Skill> skills)
		{
			Repository.UpdateWorker(user, skills);
		}
	}
}
