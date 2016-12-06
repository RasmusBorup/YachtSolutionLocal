using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Yachtsolution.DataLayer;

namespace Yachtsolution.ControlLayer
{
    /// <summary>
    /// This is the class LogBookController.
    /// </summary>
    public sealed class LogBookController
    {
        private static object _syncRoot = new Object();
        private static volatile LogBookController _instance;
        private LogBookDB logbookDB;
        private JobController jobCtr;

        /// <summary>
        /// This is the constructor for the class LogBookController.
        /// </summary>
        private LogBookController()
        {
            logbookDB = LogBookDB.GetInstance();
            jobCtr = JobController.GetInstance();
        }

        /// <summary>
        /// This method is a multi threaded singleton for the class LogBookController.
        /// </summary>
        /// <returns>_instance</returns>
        public static LogBookController GetInstance()
        {
            if (_instance == null)
            {
                lock (_syncRoot)
                {
                    if (_instance == null)
                    {
                        _instance = new LogBookController();
                    }
                }
            }

            return _instance;
        }

        /// <summary>
        /// This method gets all the logbooks.
        /// </summary>
        /// <returns>logbooks</returns>
        public List<LogBook> GetAllLogBooks()
        {
            return logbookDB.GetAllLogBooks();
        }

        /// <summary>
        /// This method finds logbooks by Date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns>logitems</returns>
        public List<LogItem> GetAllLogItems()
        {
            return logbookDB.GetAllLogItems();
        }

        /// <summary>
        /// This method creates a logbook.
        /// </summary>
        /// <param name="logItemName"></param>
        /// <param name="measurementUnit"></param>
        /// <returns>boolean</returns>
        public string CreateLogBook(DateTime date, string chiefEngineer, string remarks, string description)
        {
            return logbookDB.CreateLogBook(date, chiefEngineer, remarks, description);
        }

        /// <summary>
        /// This method creates a log item.
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="description"></param>
        /// <returns>boolean</returns>
        public string CreateLogItem(string itemName, string unitOfMeasurement, string description)
        {
            return logbookDB.CreateLogItem(itemName, unitOfMeasurement, description, false);
        }

        /// <summary>
        /// this method updates a logbook.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="description"></param>
        /// <param name="remarks"></param>
        /// <param name="chiefEngineer"></param>
        /// <returns>boolean</returns>
        public string UpdateLogBook(DateTime date, string description, string remarks, string chiefEngineer)
        {
                return logbookDB.UpdateLogBook(date, description, remarks, chiefEngineer);
        }

        /// <summary>
        /// This method updates a log item.
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="unitOfMeasurement"></param>
        /// <param name="newName"></param>
        /// <param name="description"></param>
        /// <returns>boolean</returns>
        public string UpdateLogItem(string itemName, string unitOfMeasurement, string newName, string description)
        {
            return logbookDB.UpdateLogItem(itemName, unitOfMeasurement, newName, description);
        }

        /// <summary>
        /// This method updates a log item reading.
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="date"></param>
        /// <param name="todaysReading"></param>
        /// <returns>boolean</returns>
        public bool UpdateLogItemReading(string itemName, DateTime date, string todaysReading)
        {
            return logbookDB.UpdateLogItemReading(itemName, date, todaysReading);
        }

        public bool UpdateLogReadingsFromDGV(DataGridViewRowCollection rows, DateTime date)
        {
            return logbookDB.UpdateLogReadingsFromDGV(rows, date);
        }

        /// <summary>
        /// This method find a logbook by Date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns>logbook</returns>
        public LogBook FindLogBook(DateTime date)
        {
            return logbookDB.FindLogBook(date);
        }

        /// <summary>
        /// This method finds a log item by itemName.
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns>LogItem</returns>
        public LogItem FindLogItem(string itemName)
        {
            return logbookDB.FindLogItem(itemName);
        }

        /// <summary>
        /// This method finds log item readings by Date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns>logItemReadings</returns>
        public List<LogReading> FindLogItemReadings(DateTime date)
        {
            return logbookDB.FindLogItemReadings(date);
        }

        /// <summary>
        /// This method finds a log item reading by itemName and Date.
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="date"></param>
        /// <returns>logItemReading</returns>
        public LogReading FindLogItemReading(string itemName, DateTime date)
        {
            return logbookDB.FindLogItemReading(itemName, date);
        }

        /// <summary>
        /// This method deletes a log item by itemName.
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns>boolean</returns>
        public bool DeleteLogItem(string itemName)
        {
            return logbookDB.DeleteLogItem(itemName);
        }

        /// <summary>
        /// This method fills the database with logbooks and log items.
        /// </summary>
        /// <returns></returns>
        public bool FillDatabase()
        {
            return logbookDB.FillDatabase();
        }
    }
}