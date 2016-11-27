using Microsoft.VisualStudio.TestTools.UnitTesting;
using AccuracyDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using AccuracyDAL.Models;
using System.Diagnostics;

namespace AccuracyDAL.Tests
{
    [TestClass()]
    public class HelperTests
    {
        [TestMethod()]
        public void GetPropertlyArrayTrueClass()
        {
            PropertyInfo[] pi = Helper.GetPropertlyArray(typeof(Testrun));
            if (pi == null)
                Assert.Fail();
            else
                Debug.WriteLine(pi.Length);

        }
        [TestMethod()]
        public void GetPropertlyArrayNULL()
        {
            PropertyInfo[] pi = Helper.GetPropertlyArray(typeof(Nullable));
            if (pi == null)
                Assert.Fail();
            else
                Debug.WriteLine(pi.Length);
        }
    }
}