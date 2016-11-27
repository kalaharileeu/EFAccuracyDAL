using Microsoft.VisualStudio.TestTools.UnitTesting;
using AccuracyDAL.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuracyDAL.Repos.Tests
{
    [TestClass()]
    public class TestpointRepoTests
    {
        [TestMethod()]
        public void GetColumnDataTest()
        {
            Database.SetInitializer(new MyDataInitializer());
            //Temporay list for results
            List<float> templist = new List<float>();

            using (var repo = new TestpointRepo())
            {
                templist = repo.GetColumnData("Vdcpcu", 2);
            }

            //Fail
            if(templist.Count == 0)Assert.Fail();
        }
    }
}