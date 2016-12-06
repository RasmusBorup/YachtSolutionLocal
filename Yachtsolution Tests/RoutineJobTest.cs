using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Yachtsolution.DataLayer;

namespace Yachtsolution_Tests
{
    [TestClass]
    public class RoutineJobTest
    {
        private static JobDB jobDB;

        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            jobDB = JobDB.GetInstance();
            File.WriteAllText("Jobs.json", JsonConvert.SerializeObject(new List<Job>()));
            Subgroup sg = new Subgroup();
            sg.Name = "Test";
            List<Subgroup> subgroups = new List<Subgroup>();
            subgroups.Add(sg);
            File.WriteAllText("Subgroups.json", JsonConvert.SerializeObject(subgroups));
        }

        [TestMethod]
        public void CreateJobTest()
        {
            //Invalid job title - empty
            string result = jobDB.CreateJob("", "", "", 0, false, "", "", "", "");
            Assert.AreEqual("emptyTitle", result);

            //Invalid job title - too long
            result = jobDB.CreateJob("123456789012345678901234567890123456789012345678901234567890", "", "", 0, false, "", "", "", "");
            Assert.AreEqual("emptyTitle", result);

            //Invalid frequency - Negative
            result = jobDB.CreateJob("JobTest", "", "", -1, false, "", "", "", "");
            Assert.AreEqual("negativeFrequency", result);

            //Invalid subgroup - empty
            result = jobDB.CreateJob("JobTest", "", "", 0, false, "", "", "", "");
            Assert.AreEqual("emptySubgroup", result);

            //Invalid subgroup - not found
            result = jobDB.CreateJob("JobTest", "", "", 0, false, "", "", "", "Inavalid subgroup");
            Assert.AreEqual("emptySubgroup", result);

            //Successfull creation of a single time job
            result = jobDB.CreateJob("JobTest", "", "", 0, false, "", "", "", "Test");
            Assert.AreEqual("success", result);

            //Make sure only 1 job was inserted
            Assert.AreEqual(1, jobDB.ListAllJobs().Count);

            //Successfull creation of a routine job
            result = jobDB.CreateJob("JobTest", "", "", 1, false, "", "", "", "Test");
            Assert.AreEqual("success", result);

            //Make sure another 5 jobs was inserted
            Assert.AreEqual(6, jobDB.ListAllJobs().Count);
        }

        [TestMethod]
        public void UpdateJobTest()
        {
            //Invalid job title - empty
            string result = jobDB.UpdateJob(1,"", "", "", 0, false, "", "", false, "", "");
            Assert.AreEqual("emptyTitle", result);

            //Invalid job title - too long
            result = jobDB.UpdateJob(1,"123456789012345678901234567890123456789012345678901234567890", "", "", 0, false, "", "", false, "", "");
            Assert.AreEqual("emptyTitle", result);

            //Invalid frequency - Negative
            result = jobDB.UpdateJob(1, "JobTest1", "", "", -1, false, "", "", false, "", "");
            Assert.AreEqual("negativeFrequency", result);

            //Invalid subgroup - empty
            result = jobDB.UpdateJob(1, "JobTest1", "", "", 0, false, "", "", false, "", "");
            Assert.AreEqual("emptySubgroup", result);

            //Invalid subgroup - not found
            result = jobDB.UpdateJob(1, "JobTest1", "", "", 0, false, "", "", false, "", "Inavalid subgroup");
            Assert.AreEqual("emptySubgroup", result);

            //Successfull update of a single time job
            result = jobDB.UpdateJob(1, "JobTest1", "", "", 0, false, "", "", false, "", "Test");
            Assert.AreEqual("success", result);

            //Invalid id
            result = jobDB.UpdateJob(100, "JobTest1", "", "", 0, false, "", "", false, "", "Test");
            Assert.AreEqual("jobNotFound", result);
        }
        
        [TestMethod]
        public void DeleteJobTest()
        {
            //Invalid id - not found
            string result = jobDB.DeleteJob(-1, false);
            Assert.AreEqual("fail", result);

            //Successful deletion - single job from a routine
            result = jobDB.DeleteJob(5, false);

            //Make sure one job was deleted
            Assert.AreEqual(5, jobDB.ListAllJobs().Count);

            //Successfull deletion - rest of the routine
            result = jobDB.DeleteJob(1, true);

            //Make sure only one non routine job remain
            Assert.AreEqual(1, jobDB.ListAllJobs().Count);
        }
    }
}
