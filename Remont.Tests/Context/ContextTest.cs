using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;

namespace Remont.Tests.Context
{
    [TestClass]
    public class ContextTest
    {
        [TestMethod]
        public void Test()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));

            Database.SetInitializer<RemontTestContext>(new InitTestDb());
            using (RemontTestContext testcontext = new RemontTestContext())
            {
                testcontext.Database.Initialize(true);
            }
        }
    }
}
