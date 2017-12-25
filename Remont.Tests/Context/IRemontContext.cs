using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remont.Tests.Context
{
    public interface IRemontTestContext
    {
        DbSet<UserProfile> UserProfiles { get; set; }
        DbSet<Worker> Workers { get; set; }
        DbQuery<Skill> Skills { get; }
    }
}
