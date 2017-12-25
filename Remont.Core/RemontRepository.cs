using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remont.Core
{
	public class RemontRepository : IRemontRepository
	{
		private RemontContext db;

		public RemontRepository()
		{
			db = new RemontContext();
		}

		public IEnumerable<Skill> GetSkills()
		{
			return db.Skills;
		}

		public IEnumerable<UserProfile> GetUserProfiles()
		{
			return db.UserProfiles;
		}

		public IEnumerable<Worker> GetWorkers()
		{
			return db.Workers;
		}


		public void UpdateWorker(UserProfile _user, IList<Skill> skills)
		{
			var user = db.UserProfiles.Where(u => u.UserProfileId == _user.UserProfileId).Single();

			if (user.Worker != null)
			{
				user.Worker.Skills.Clear();
				user.Worker.Skills = skills;
			}
			else
			{
				user.Worker = new Worker() { Skills = skills, UserProfile = user };
				db.Entry(user.Worker).State = EntityState.Added;
			}
			db.SaveChanges();
		}
	}
}
