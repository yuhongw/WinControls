using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Tests
{
    [TestClass()]
    public class SiteSvcTests
    {

        const string RootPath = @"F:\Projs\cn\wincontrols\SwitchInfo\Web\Data";
        [TestMethod()]
        public void ReadDataTest()
        {
            SiteSvc svc = new SiteSvc(RootPath);
            var data = svc.GetSample();
            svc.SaveData(data);
            List<Site> sites = svc.ReadData();
            Assert.AreEqual(true,sites.Count>0);
        }

        [TestMethod()]
        public void SaveDataTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetSampleTest()
        {
            Assert.Fail();
        }
    }
}