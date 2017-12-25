using Remont.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remont.Business
{
	public interface IManagerBase
	{
		IRemontRepository Repository { get; }

		IEnumerable<Skill> GetSkills();

		IEnumerable<Worker> GetWorkers();

		IEnumerable<UserProfile> GetUserProfiles();

		void UpdateWorker(UserProfile user, IList<Skill> skills);
	}
}
