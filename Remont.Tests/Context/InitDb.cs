
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;

namespace Remont.Tests.Context
{
    public class InitTestDb : DropCreateDatabaseAlways<RemontTestContext>
    {
        protected override void Seed(RemontTestContext context)
        {
            SqlConnection.ClearAllPools();

            context.Seed();

            var up = new UserProfile() { Email = "dudniksasha@mail.com", UserName = "admin" };
            //var dataContext=DataContext.GetInstance();

            context.UserProfiles.Add(up);
            context.SaveChanges();
        }
    }
}