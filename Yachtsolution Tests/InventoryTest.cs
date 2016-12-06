using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Yachtsolution.DataLayer;

namespace Yachtsolution_Tests
{
    [TestClass]
    public class InventoryTest
    {
        private static InventoryDB iDB;

        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            iDB = InventoryDB.getInstance();
            File.WriteAllText("Inventory.json", JsonConvert.SerializeObject(new List<Inventory>()));
        }

        [TestMethod]
        public void CreateItemTest()
        {
            //Successfull creation of an item
            string result = iDB.InsertItem("desc", 123, "loca", "manu", 100, "name", "1000", "123321", "",
                "partfor", "supplier", "role");
            Assert.AreEqual("success", result);

            //Invalid item name - empty
            result = iDB.InsertItem("", 1, "", "asd", 0, "", "123", "123", "", "", "", "");
            Assert.AreEqual("emptyName", result);

            //Invalid item name - greater than 255 chars
            result = iDB.InsertItem("", 1, "", "asd", 0, "asdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasd", "123", "123", "", "", "", "");
            Assert.AreEqual("tooLongName", result);

            //Invalid serial - empty
            result = iDB.InsertItem("", 1, "", "asd", 0, "asd", "123", "", "", "", "", "");
            Assert.AreEqual("emptySerial", result);

            //Invalid serial - greater than 255 chars
            result = iDB.InsertItem("", 1, "", "asd", 0, "asd", "123", "1231231231123123123112312312311231231231123123123112312312311231231231123123123112312312311231231231123123123112312312311231231231123123123112312312311231231231123123123112312312311231231231123123123112312312311231231231123123123112312312311231231231123456", "", "", "", "");
            Assert.AreEqual("tooLongSerial", result);

            //Invalid serial - duplicate serial
            Assert.AreEqual(1, iDB.GetAllInventories().Count);

            //Invalid location - empty location
            result = iDB.InsertItem("", 1, "", "asd", 0, "asd", "123", "123", "", "", "", "");
            Assert.AreEqual("emptyLocation", result);

            //Invalid location - greater than 255 chars
            result = iDB.InsertItem("", 1, "asdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasdasdaasdasd", "asd", 0, "asd", "123", "123", "", "", "", "");
            Assert.AreEqual("tooLongLocation", result);

            //Invalid manufacturer - empty manufacturer
            result = iDB.InsertItem("", 1, "asd", "", 0, "asd", "123", "123", "", "", "", "");
            Assert.AreEqual("emptyManufacturer", result);

            //Invalid manufacturer - greater than 255 chars
            result = iDB.InsertItem("", 1, "asd", "asdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdasasdas", 0, "asd", "123", "123", "", "", "", "");
            Assert.AreEqual("tooLongManufacturer", result);
        }


        [TestMethod]
        public void UpdateItemTest()
        {
            string result = iDB.UpdateItemBySerial("desc", 123, "loca","manu",100,"name","1000","123321","","partfor","supplies");
            Assert.AreEqual("success", result);

            //Invalid item name - empty
            result = iDB.UpdateItemBySerial("desc", 123, "loca", "manu", 100, "", "1000", "123321", "", "partfor", "supplies");
            Assert.AreEqual("emptyName", result);

            //Invalid item name - greater than 255
            result = iDB.UpdateItemBySerial("desc", 123, "loca", "manu", 100, "namednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamednamed", "1000", "123321", "", "partfor", "supplies");
            Assert.AreEqual("tooLongName", result);

            //Invalid item location - empty
            result = iDB.UpdateItemBySerial("desc", 123, "", "manu", 100, "name", "1000", "123321", "", "partfor", "supplies");
            Assert.AreEqual("emptyLocation", result);


            //Invalid item location - greater than 255
            result = iDB.UpdateItemBySerial("desc", 123, "locatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocatlocat", "manu", 100, "name", "1000", "123321", "", "partfor", "supplies");
            Assert.AreEqual("tooLongLocation", result);

            //Invalid item manufacturer - empty
            result = iDB.UpdateItemBySerial("desc", 123, "loca", "", 100, "name", "1000", "123321", "", "partfor", "supplies");
            Assert.AreEqual("emptyManufacturer", result);


            //Invalid item manufacturer - greater than 255
            result = iDB.UpdateItemBySerial("desc", 123, "loca", "manufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanufmanuf", 100, "name", "1000", "123321", "", "partfor", "supplies");
            Assert.AreEqual("tooLongManufacturer", result);


        }
        
   
    }
}
