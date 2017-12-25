using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;

namespace Remont.Core
{
    public class InitDb : DropCreateDatabaseIfModelChanges<RemontContext>
    {
        protected override void Seed(RemontContext context)
        {
            SqlConnection.ClearAllPools();

			WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserProfileId", "UserName", autoCreateTables: true);

			context.InitRolesUser();

            context.Seed();

        }
    }
}