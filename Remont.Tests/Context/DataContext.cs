using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remont.Tests.Context
{
    public class DataContext
    {
        private static RemontTestContext remontContext;

        private DataContext()
        {
        }

        public static IRemontTestContext GetInstance()
        {
            if (remontContext == null)
            {
                lock (typeof(RemontTestContext))
                {
                    if (remontContext == null)
                        remontContext = new RemontTestContext();
                }
            }

            return remontContext;
        }
    }
}
