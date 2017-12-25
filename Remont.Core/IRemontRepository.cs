using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remont.Core
{
	public interface IRemontRepository
	{
		IEnumerable<Skill> GetSkills();
		IEnumerable<UserProfile> GetUserProfiles();
		IEnumerable<Worker> GetWorkers();

		void UpdateWorker(UserProfile user, IList<Skill> skills);
	}
}
