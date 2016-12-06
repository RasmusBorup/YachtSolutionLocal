using System;
using System.Collections.Generic;
using Yachtsolution.DataLayer;

namespace Yachtsolution.ControlLayer
{
    /// <summary>
    /// This is the class InventoryController.
    /// </summary>
    public sealed class InventoryController
    {
        private static object _syncRoot = new Object();
        private static volatile InventoryController _instance;
        private InventoryDB inventoryDB;

        /// <summary>
        /// This is the constructor for the class InventoryController.
        /// </summary>
        private InventoryController()
        {
            inventoryDB = InventoryDB.getInstance();
        }

        /// <summary>
        /// This method is a multi threaded singleton for the class InventoryController.
        /// </summary>
        /// <returns>_instance</returns>
        public static InventoryController GetInstance()
        {
            if(_instance == null)
            {
                lock (_syncRoot)
                {
                    if (_instance == null)
                    {
                        _instance = new InventoryController();
                    }
                }
            }

            return _instance;
        }

        /// <summary>
        /// This method creates an inventory.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="amount"></param>
        /// <param name="location"></param>
        /// <param name="manufacturer"></param>
        /// <param name="minimumAmount"></param>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <param name="serialNr"></param>
        /// <param name="photo"></param>
        /// <param name="partFor"></param>
        /// <param name="suppliers"></param>
        /// <returns>success</returns>
        public string InsertItem(string description, int amount, string location, string manufacturer, int minimumAmount, string name, string price, string serialNr, string imageLocation, string partFor, string suppliers, string role)
        {
            return inventoryDB.InsertItem(description,amount,location,manufacturer,minimumAmount,name,price,serialNr, imageLocation, partFor, suppliers, role);
        }

        /// <summary>
        /// This method returns all the inventories.
        /// </summary>
        /// <returns>inventories</returns>
        public List<Inventory> GetAllInventories()
        {
            return inventoryDB.GetAllInventories();
        }

        public List<Inventory> GetInventoriesByRole(string role)
        {
            return inventoryDB.GetInventoriesByRole(role);
        }

        /// <summary>
        /// This method finds inventories by its name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>inventories</returns>
        public List<Inventory> FindItemByName(string name)
        {
            return inventoryDB.FindItemByName(name);
        }

        /// <summary>
        /// This method finds inventories by its serial.
        /// </summary>
        /// <param name="serial"></param>
        /// <returns>inventories</returns>
        public Inventory FindItemBySerialNr(string serial)
        {
            return inventoryDB.FindItemBySerial(serial);
        }

        /// <summary>
        /// This method deletes a inventory.
        /// </summary>
        /// <param name="serialNr"></param>
        /// <returns>success</returns>
        public string DeleteItemBySerialNr(string serialNr)
        {
            return inventoryDB.DeleteItem(serialNr);
        }

        /// <summary>
        /// This method updates a inventory.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="amount"></param>
        /// <param name="location"></param>
        /// <param name="manufacturer"></param>
        /// <param name="minimumAmount"></param>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <param name="serialNr"></param>
        /// <param name="photo"></param>
        /// <param name="partFor"></param>
        /// <param name="suppliers"></param>
        /// <returns>boolean</returns>
        public string UpdateItemBySerialNr(string description, int amount, string location, string manufacturer, int minimumAmount, string name, string price, string serialNr, string imageLocation, string partFor, string suppliers)
        {
           return inventoryDB.UpdateItemBySerial(description, amount, location, manufacturer, minimumAmount, name, price, serialNr, imageLocation, partFor, suppliers);
        }

        /// <summary>
        /// This method find all inventories that do not meet their minimum amount requirements.
        /// </summary>
        /// <returns>inventories</returns>
        public List<Inventory> findAllItemsNotMinim()
        {
            return inventoryDB.FindAllItemsNotMinim();
        }
    }
}