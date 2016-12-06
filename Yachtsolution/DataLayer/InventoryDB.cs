using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Newtonsoft.Json;

namespace Yachtsolution.DataLayer
{
    public class Inventory
    {
        public string name { get; set; }
        public int amount { get; set; }
        public string description { get; set; }
        public double price { get; set; }
        public int minimumAmount { get; set; }
        public string location { get; set; }
        public string manufacturer { get; set; }
        public string serialNo { get; set; }
        public byte[] photo { get; set; }
        public string partFor { get; set; }
        public string suppliers { get; set; }
        public string role { get; set; }

        public Inventory()
        {

        }
    }

    /// <summary>
    /// This is the class InventoryDb.
    /// </summary>
    public sealed class InventoryDB
    {
        private static volatile InventoryDB instance;
        private string inventoryPath;

        /// <summary>
        /// This is the constructor for the class InventoryDB.
        /// </summary>
        private InventoryDB()
        {
            inventoryPath = "Inventory.json";
        }

        /// <summary>
        /// This method is a multi threaded singleton for the class InventoryDB.
        /// </summary>
        /// <returns>instance</returns>
        public static InventoryDB getInstance()
        {
            if (instance == null)
            {
                instance = new InventoryDB();
            }
            return instance;
        }

        /// <summary>
        /// This method returns all the objects of the class Inventory that lies in the database.
        /// </summary>
        /// <returns>inventories</returns>
        public List<Inventory> GetAllInventories()
        {
            List<Inventory> items = new List<Inventory>();
            if (File.Exists(inventoryPath))
            {
                items = JsonConvert.DeserializeObject<List<Inventory>>(File.ReadAllText(inventoryPath));
            }
            return items;
        }

        public List<Inventory> GetInventoriesByRole(string role)
        {
            List<Inventory> inventories = GetAllInventories();
            if (role != "Administrator")
            {
                inventories = inventories.Where(i => i.role == role).ToList();
            }
            return inventories;
        }

        /// <summary>
        /// This method finds and returns a list of objects of the class Inventory that lies in the database by its instance variable name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>inventories</returns>
        public List<Inventory> FindItemByName(string name)
        {
            List<Inventory> itemsToBeFound = new List<Inventory>();
            List<Inventory> items = new List<Inventory>();
            if (File.Exists(inventoryPath))
            {
                items = JsonConvert.DeserializeObject<List<Inventory>>(File.ReadAllText(inventoryPath));
                itemsToBeFound = items.Where(i => i.name == name).ToList();
            }

            return itemsToBeFound;
        }

        /// <summary>
        /// This method finds and returns a list of objects of the class Inventory that lies in the database by its instance variable serialNo.
        /// </summary>
        /// <param name="serial"></param>
        /// <returns>inventories</returns>
        public Inventory FindItemBySerial(string serial)
        {
            Inventory itemToBeFound = new Inventory();
            List<Inventory> items = GetAllInventories();
            try
            {
                itemToBeFound = items.SingleOrDefault(i => i.serialNo == serial);
            }
            catch (Exception e)
            {
                Console.WriteLine("Couldn't find inventories by serial number.");
                Console.WriteLine("Error: " + e.Message);
            }

            return itemToBeFound;
        }

        /// <summary>
        /// This method creates an object of the class Inventory and inserts it in the database.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="amount"></param>
        /// <param name="location"></param>
        /// <param name="manufacturer"></param>
        /// <param name="minimumAmount"></param>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <param name="serialNo"></param>
        /// <returns>success</returns>
        public string InsertItem(string description, int amount, string location, string manufacturer, int minimumAmount, string name, string price, string serialNo, string imageLocation, string partFor, string suppliers, string role)
        {
            if (name == "")
                return "emptyName";
            if (name.Length > 255)
                return "tooLongName";
            if (serialNo == "")
                return "emptySerial";
            if (serialNo.Length > 255)
                return "tooLongSerial";
            if (FindItemBySerial(serialNo) != null)
                return "usedSerial";
            if (location == "")
                return "emptyLocation";
            if (location.Length > 255)
                return "tooLongLocation";
            if (manufacturer == "")
                return "emptyManufacturer";
            if (manufacturer.Length > 255)
                return "tooLongManufacturer";
            double priceD = 0;
            try
            {
                priceD = Convert.ToDouble(price);
            }
            catch (Exception)
            {
                return "emptyPrice";
            }

            List<Inventory> items = GetAllInventories();
            Inventory newItem = new Inventory();

            newItem.description = description;
            newItem.amount = amount;
            newItem.location = location;
            newItem.manufacturer = manufacturer;
            newItem.minimumAmount = minimumAmount;
            newItem.name = name;
            newItem.price = priceD;
            newItem.serialNo = serialNo;
            newItem.partFor = partFor;
            newItem.suppliers = suppliers;
            newItem.role = role;

            if (!string.IsNullOrEmpty(imageLocation))
            {
                newItem.photo = new MemoryStream(File.ReadAllBytes(imageLocation)).ToArray();
            }

            items.Add(newItem);
            File.WriteAllText(inventoryPath, JsonConvert.SerializeObject(items));

            return "success";
        }

        /// <summary>
        /// This method finds and deletes an object of the class Inventory from the database by its instance variable serialNo.
        /// </summary>
        /// <param name="serialNr"></param>
        /// <returns>success</returns>
        public string DeleteItem(string serialNr)
        {
            try
            {
                List<Inventory> items = GetAllInventories();
                Inventory itemToRemove = items.SingleOrDefault(i => i.serialNo == serialNr);
                items.Remove(itemToRemove);
                File.WriteAllText(inventoryPath, JsonConvert.SerializeObject(items));
                return "success";
            }
            catch (Exception)
            {
                return "Fail";
            }
        }

        /// <summary>
        /// This method finds and updates an object of the class Inventory from the database by its instance variable serialNo.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="amount"></param>
        /// <param name="location"></param>
        /// <param name="manufacturer"></param>
        /// <param name="minimumAmount"></param>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <param name="serialNr"></param>
        /// <returns>success</returns>
        public string UpdateItemBySerial(string description, int amount, string location, string manufacturer, int minimumAmount, string name, string price, string serialNo, string imageLocation, string partFor, string suppliers)
        {
            if (name == "")
                return "emptyName";
            if (name.Length > 255)
                return "tooLongName";
            if (location == "")
                return "emptyLocation";
            if (location.Length > 255)
                return "tooLongLocation";
            if (manufacturer == "")
                return "emptyManufacturer";
            if (manufacturer.Length > 255)
                return "tooLongManufacturer";

            double priceD = 0;
            try
            {
                priceD = Convert.ToDouble(price);
            }
            catch (Exception)
            {
                return "emptyPrice";
            }

            List<Inventory> items = GetAllInventories();
            Inventory itemToChange = items.SingleOrDefault(i => i.serialNo == serialNo);

            itemToChange.description = description;
            itemToChange.amount = amount;
            itemToChange.location = location;
            itemToChange.manufacturer = manufacturer;
            itemToChange.minimumAmount = minimumAmount;
            itemToChange.name = name;
            itemToChange.price = priceD;
            if (!string.IsNullOrEmpty(imageLocation))
            {
                itemToChange.photo = new MemoryStream(File.ReadAllBytes(imageLocation)).ToArray();
            }
            itemToChange.partFor = partFor;
            itemToChange.suppliers = suppliers;

            File.WriteAllText(inventoryPath, JsonConvert.SerializeObject(items));

            return "success";
        }

        /// <summary>
        /// This method finds and returns a list of objects of the class Employee from the database that doesn't meet their minimum requirements.
        /// </summary>
        /// <returns>inventories</returns>
        public List<Inventory> FindAllItemsNotMinim()
        {
            List<Inventory> items = GetAllInventories();
            items = items.Where(i => i.amount < i.minimumAmount).ToList();

            return items;
        }
    }
}