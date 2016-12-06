using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Yachtsolution.DataLayer;

namespace Yachtsolution_Tests
{
    [TestClass]
    public class LogbookTest
    {
        private static LogBookDB logDB;
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            logDB = LogBookDB.GetInstance();
            LogBook lb = new LogBook();
            lb.Date = DateTime.Today;
            List<LogBook> lbs = new List<LogBook>();
            lbs.Add(lb);
            File.WriteAllText("Logbooks.json", JsonConvert.SerializeObject(lbs));
            File.WriteAllText("LogItems.json", JsonConvert.SerializeObject(new List<LogItem>()));
        }

        [TestMethod]
        public void CreateLogbookTest()
        {
            string fiveThousand = new string('*', 5001);
            string name = new string('*', 51);

            //Invalid chief engineer - too long
            string result = logDB.CreateLogBook(DateTime.Today.AddDays(1), name, "", "");
            Assert.AreEqual("tooLongEngineer", result);

            //Invalid description - too long
            result = logDB.CreateLogBook(DateTime.Today.AddDays(1), "", "", fiveThousand);
            Assert.AreEqual("tooLongDescription", result);

            //Invalid description - too long
            result = logDB.CreateLogBook(DateTime.Today.AddDays(1), "", fiveThousand, "");
            Assert.AreEqual("tooLongRemarks", result);

            //Successful insert of logbook
            result = logDB.CreateLogBook(DateTime.Today.AddDays(1), "", "", "");
            Assert.AreEqual("success", result);

            //Check that there are indeed one registered logbook
            Assert.AreEqual(2, logDB.GetAllLogBooks().Count);
        }

        [TestMethod]
        public void CreateLogItemTest()
        {
            //Invalid name - empty
            string result = logDB.CreateLogItem("", "", "", false);
            Assert.AreEqual("emptyName", result);

            //Invalid unit of measurement - empty
            result = logDB.CreateLogItem("1", "", "", false);
            Assert.AreEqual("emptyUnit", result);

            //Successful insertion of logitem
            result = logDB.CreateLogItem("1", "1", "1", false);
            Assert.AreEqual("success", result);

            //Invalid name - in use
            result = logDB.CreateLogItem("1", "1", "1", false);
            Assert.AreEqual("usedName", result);
        }

        [TestMethod]
        public void UpdateLogBookTest()
        {
            string fiveThousand = new string('*', 5001);
            string name = new string('*', 51);

            //Invalid chief engineer - too long
            string result = logDB.UpdateLogBook(DateTime.Today, "", "", name);
            Assert.AreEqual("tooLongEngineer", result);

            //Invalid description - too long
            result = logDB.UpdateLogBook(DateTime.Today, fiveThousand, "", "");
            Assert.AreEqual("tooLongDescription", result);

            //Invalid description - too long
            result = logDB.UpdateLogBook(DateTime.Today, "", fiveThousand, "");
            Assert.AreEqual("tooLongRemarks", result);

            //Successful update of logbook
            result = logDB.UpdateLogBook(DateTime.Today, "", "", "");
            Assert.AreEqual("success", result);
        }

        [TestMethod]
        public void UpdateLogItemTest()
        {
            //Invalid name - empty
            string result = logDB.UpdateLogItem("", "", "", "");
            Assert.AreEqual("emptyName", result);

            //Invalid unit of measurement - empty
            result = logDB.UpdateLogItem("1", "", "", "");
            Assert.AreEqual("emptyUnit", result);

            //Invalid name - newname in use
            result = logDB.UpdateLogItem("1", "1", "1", "");
            Assert.AreEqual("usedName", result);

            //Successful update
            result = logDB.UpdateLogItem("1", "1", "2", "1");
            Assert.AreEqual("success", result);
        }
    }
}
