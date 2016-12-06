using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Yachtsolution.DataLayer
{
    public class LogReading
    {
        public double TodaysReading { get; set; }
        public LogItem LogItem1 { get; set; }
    }

    public class LogItem
    {
        public string Name { get; set; }
        public string UnitOfMeasurement { get; set; }
        public string Description { get; set; }
    }

    public class LogBook
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public string ChiefEngineer { get; set; }
        public List<LogReading> Readings { get; set; }

        public LogBook()
        {
            Readings = new List<LogReading>();
        }
    }

    /// <summary>
    /// This is the class LogBookDB.
    /// </summary>
    public sealed class LogBookDB
    {
        private static LogBookDB instance;
        private string logbookPath;
        private string logItemPath;

        /// <summary>
        /// This is the constructor for the class LogBookDB.
        /// </summary>
        private LogBookDB()
        {
            logbookPath = "Logbooks.json";
            logItemPath = "LogItems.json";
        }

        /// <summary>
        /// This method is a multi threaded singleton for the class LogBookDB.
        /// </summary>
        /// <returns>LogBookDB</returns>
        public static LogBookDB GetInstance()
        {
            if (instance == null)
            {
                instance = new LogBookDB();
            }
            return instance;
        }

        /// <summary>
        /// This method creates an object of the class LogBook and inserts it in the database.
        /// </summary>
        /// <param name="chiefEngineer"></param>
        /// <param name="remarks"></param>
        /// <param name="description"></param>
        /// <returns>success</returns>
        public string CreateLogBook(DateTime date, string chiefEngineer, string remarks, string description)
        {
            if (chiefEngineer.Length > 50)
                return "tooLongEngineer";
            if (description.Length > 5000)
                return "tooLongDescription";
            if (remarks.Length > 5000)
                return "tooLongRemarks";
            if (FindLogBook(date) != null)
                return "logbookExists";

            List<LogBook> logbooks = GetAllLogBooks();
            LogBook logBook = new LogBook();

            logBook.Date = date;
            logBook.Description = description;
            logBook.Remarks = remarks;
            logBook.ChiefEngineer = chiefEngineer;

            foreach (LogItem item in GetAllLogItems())
            {
                logBook.Readings.Add(CreateLogReading(item));
            }
            logbooks.Add(logBook);
            SaveLogbooks(logbooks);
            return "success";
        }

        public void SaveLogbooks(List<LogBook> logbooks)
        {
            File.WriteAllText(logbookPath, JsonConvert.SerializeObject(logbooks));
        }

        /// <summary>
        /// This method creates an object of the class LogItem and inserts it in the database.
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="unitOfMeasurement"></param>
        /// <param name="description"></param>
        /// <param name="forUpdate"></param>
        /// <returns>boolean</returns>
        public string CreateLogItem(string itemName, string unitOfMeasurement, string description, bool forUpdate)
        {
            if (itemName == "" || itemName.Length > 255)
            {
                return "emptyName";
            }
            if (unitOfMeasurement == "" || unitOfMeasurement.Length > 255)
            {
                return "emptyUnit";
            }
            if (FindLogItem(itemName) != null)
            {
                return "usedName";
            }

            List<LogItem> logItems = GetAllLogItems();
            LogItem logItem = new LogItem();

            logItem.Name = itemName;
            logItem.UnitOfMeasurement = unitOfMeasurement;
            logItem.Description = description;

            logItems.Add(logItem);
            SaveLogItems(logItems);

            if (!forUpdate)
            {
                List<LogBook> logbooks = GetAllLogBooks();
                foreach (LogBook logBook in logbooks)
                {
                    logBook.Readings.Add(CreateLogReading(logItem));
                }
                SaveLogbooks(logbooks);
            }
            return "success";

        }

        private void SaveLogItems(List<LogItem> logItems)
        {
            File.WriteAllText(logItemPath, JsonConvert.SerializeObject(logItems));
        }

        private LogReading CreateLogReading(LogItem item)
        {
            LogReading reading = new LogReading();

            reading.LogItem1 = item;
            reading.TodaysReading = 0;
            return reading;

        }

        /// <summary>
        /// This method returns all the objects of the class LogBook that lies in the database.
        /// </summary>
        /// <returns>List<LogBook></returns>
        public List<LogBook> GetAllLogBooks()
        {
            List<LogBook> logBooks = new List<LogBook>();

            if (File.Exists(logbookPath))
            {
                logBooks = JsonConvert.DeserializeObject<List<LogBook>>(File.ReadAllText(logbookPath));
            }

            return logBooks;
        }

        /// <summary>
        /// This method returns all the objects of the class LogItem that lies in the database.
        /// </summary>
        /// <param name="date"></param>
        /// <returns>logItems</returns>
        public List<LogItem> GetAllLogItems()
        {
            List<LogItem> logItems = new List<LogItem>();

            if (File.Exists(logItemPath))
            {
                logItems = JsonConvert.DeserializeObject<List<LogItem>>(File.ReadAllText(logItemPath));
            }

            return logItems;
        }

        /// <summary>
        /// This method finds and returns an object of the class LogBook that lies in the database by its instance variable Date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns>logbook</returns>
        public LogBook FindLogBook(DateTime date)
        {
            try
            {
                return GetAllLogBooks().SingleOrDefault(l => l.Date == date);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Couldn't find the logbooks by Date.");
                Console.WriteLine("Error: " + exception.Message);
                return null;
            }
        }

        /// <summary>
        /// This method finds and returns an object of the class LogItem that lies in the database by its instance variable Name.
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns>LogItem</returns>
        public LogItem FindLogItem(string itemName)
        {
            try
            {
                return GetAllLogItems().SingleOrDefault(l => l.Name == itemName);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Couldn't find the log items by item name.");
                Console.WriteLine("Error: " + exception.Message);
                return null;
            }
        }

        /// <summary>
        /// This method finds and returns an object of the class LogItemReading that lies in the database by its instance variable LogItem and Date.
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="date"></param>
        /// <returns>logItemReading</returns>
        public LogReading FindLogItemReading(string itemName, DateTime date)
        {
            try
            {
                return FindLogBook(date).Readings.Single(r => r.LogItem1.Name == itemName);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Couldn't find the log item reading by Date and item name.");
                Console.WriteLine("Error: " + exception.Message);
                return null;
            }
        }

        /// <summary>
        /// This method finds and returns a list of objects of the class LogItemReading that lies in the database by its instance variable Date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns>logItemReadings</returns>
        public List<LogReading> FindLogItemReadings(DateTime date)
        {
            try
            {
                return FindLogBook(date).Readings;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Couldn't find the log item readings by Date.");
                Console.WriteLine("Error: " + exception.Message);
                return new List<LogReading>();
            }
        }

        /// <summary>
        /// This method finds and updates an object of the class LogBook that lies in the database by its instance variable Date.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="description"></param>
        /// <param name="remarks"></param>
        /// <param name="chiefEngineer"></param>
        /// <returns>success</returns>
        public string UpdateLogBook(DateTime date, string description, string remarks, string chiefEngineer)
        {
            if (chiefEngineer.Length > 50)
                return "tooLongEngineer";
            if (description.Length > 5000)
                return "tooLongDescription";
            if (remarks.Length > 5000)
                return "tooLongRemarks";
            if (FindLogBook(date) == null)
                return "logbookDoesNotExist";

            List<LogBook> logbooks = GetAllLogBooks();
            LogBook logBookToUpdate = logbooks.Single(l => l.Date == date);
            logBookToUpdate.Description = description;
            logBookToUpdate.Remarks = remarks;
            logBookToUpdate.ChiefEngineer = chiefEngineer;

            SaveLogbooks(logbooks);
            return "success";
        }

        /// <summary>
        /// This method finds and updates an object of the class LogItem that lies in the database by its instance variable itemName.
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="unitOfMeasurement"></param>
        /// <param name="newName"></param>
        /// <param name="description"></param>
        /// <returns>success</returns>
        public string UpdateLogItem(string itemName, string unitOfMeasurement, string newName, string description)
        {

            if (itemName == "" || itemName.Length > 255)
                return "emptyName";
            if (unitOfMeasurement == "" || unitOfMeasurement.Length > 255)
                return "emptyUnit";
            if (FindLogItem(newName) != null)
                return "usedName";

            List<LogItem> logItems = GetAllLogItems();
            List<LogBook> logbooks = GetAllLogBooks();
            LogItem itemToUpdate = logItems.Single(l => l.Name == itemName);

            if (!itemName.Equals(newName))
            {
                foreach (LogBook logBook in logbooks)
                {
                    foreach (LogReading reading in logBook.Readings.Where(r => r.LogItem1.Name == itemName))
                    {
                        reading.LogItem1 = itemToUpdate;
                    }
                }
            }

            itemToUpdate.UnitOfMeasurement = unitOfMeasurement;
            itemToUpdate.Description = description;
            SaveLogbooks(logbooks);
            SaveLogItems(logItems);
            return "success";
        }

        /// <summary>
        /// This method finds and updates an object of the class LogItemReading that lies in the database by its instance variable itemName.
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="date"></param>
        /// <param name="todaysReading"></param>
        /// <returns>boolean</returns>
        public bool UpdateLogItemReading(string itemName, DateTime date, string todaysReading)
        {
            List<LogBook> logbooks = GetAllLogBooks();
            List<LogReading> readings = logbooks.Single(l => l.Date == date).Readings;
            LogReading readingToUpdate = readings.Single(r => r.LogItem1.Name == itemName);
            double todaysReadingD = 0;

            try
            {
                todaysReadingD = Convert.ToDouble(todaysReading);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Couldn't convert TodaysReading to a double.");
                Console.WriteLine("Error: " + exception.Message);
            }
            readingToUpdate.TodaysReading = todaysReadingD;
            SaveLogbooks(logbooks);
            return true;
        }

        public bool UpdateLogReadingsFromDGV(DataGridViewRowCollection rows, DateTime date)
        {
            try
            {
                List<LogBook> logbooks = GetAllLogBooks();
                List<LogReading> readings = logbooks.Single(l => l.Date == date).Readings;
                foreach (DataGridViewRow row in rows)
                {
                    readings.Single(r => r.LogItem1.Name == row.Cells[0].Value.ToString()).TodaysReading = (double)row.Cells[2].Value;
                }
                SaveLogbooks(logbooks);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// This method finds and deletes an object of the class LogItemReading that lies in the database by its instance variable LogItem.
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns>boolean</returns>
        public bool DeleteLogItem(string itemName)
        {
            List<LogBook> logbooks = GetAllLogBooks();
            List<LogItem> logItems = GetAllLogItems();
            LogItem item = FindLogItem(itemName);

            try
            {
                foreach (LogBook logbook in logbooks)
                {
                    logbook.Readings.RemoveAll(r => r.LogItem1.Name == itemName);
                }
                logItems.RemoveAll(i => i.Name == item.Name);
                SaveLogItems(logItems);
                SaveLogbooks(logbooks);
                return true;
            }

            catch (Exception exception)
            {
                Console.WriteLine("Couldn't delete the log item.");
                Console.WriteLine("Error: " + exception.Message);
                return false;
            }
        }

        /// <summary>
        /// This method saves logbooks and log items to the database that are created in the system.
        /// </summary>
        /// <returns></returns>
        public bool FillDatabase()
        {
            InsertLogbooks();
            InsertDefaultLogItems();
            return true;
        }

        /// <summary>
        /// This method creates log items without saving them in the database.
        /// </summary>
        /// <returns>boolean</returns>
        private void InsertDefaultLogItems()
        {
            if (FindLogItem("Generator 1 Oil Level") == null)
            {
                CreateLogItem("Generator 1 Oil Level", "mm", "", false);
            }

            if (FindLogItem("Generator 1 Oil Preasure") == null)
            {
                CreateLogItem("Generator 1 Oil Preasure", "Bar", "", false);
            }

            if (FindLogItem("Generator 1 Hour Count") == null)
            {
                CreateLogItem("Generator 1 Hour Count", "Hours", "", false);
            }

            if (FindLogItem("Generator 1 Temperature") == null)
            {
                CreateLogItem("Generator 1 Temperature", "C°", "", false);
            }

            if (FindLogItem("Generator 2 Oil Level") == null)
            {
                CreateLogItem("Generator 2 Oil Level", "mm", "", false);
            }

            if (FindLogItem("Generator 2 Oil Preasure") == null)
            {
                CreateLogItem("Generator 2 Oil Preasure", "Bar", "", false);
            }

            if (FindLogItem("Generator 2 Hour Count") == null)
            {
                CreateLogItem("Generator 2 Hour Count", "Hours", "", false);
            }

            if (FindLogItem("Generator 2 Temperature") == null)
            {
                CreateLogItem("Generator 2 Temperature", "C°", "", false);
            }

            if (FindLogItem("Water Level") == null)
            {
                CreateLogItem("Water Level", "1/0", "Yes/No, 1 for yes 0 for no", false);
            }

            for (int i = 0; i < 20; i++)
            {
                if (FindLogItem(i.ToString()) == null)
                {
                    CreateLogItem(i.ToString(), i.ToString(), i.ToString(), false);
                }
            }
        }

        /// <summary>
        /// this method creates and prepare it to be saved in the database.
        /// </summary>
        /// <returns></returns>
        private void InsertLogbooks()
        {
            DateTime date = DateTime.Today.AddYears(-1);

            try
            {
                if (FindLogBook(date) == null)
                {
                    DateTime checkDate = DateTime.Today;
                    int i = 1;
                    List<LogBook> logs = GetAllLogBooks();

                    while (i < 1000 && checkDate != date)
                    {
                        checkDate = checkDate.AddDays(-1);

                        if (logs.SingleOrDefault(l => l.Date == checkDate) == null)
                        {
                            LogBook log = new LogBook();
                            log.Date = checkDate;
                            log.Description = "";
                            log.Remarks = "";
                            log.ChiefEngineer = "";
                            logs.Add(log);
                        }
                        i++;
                    }
                    SaveLogbooks(logs);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error: " + exception.Message);
            }
        }
    }
}