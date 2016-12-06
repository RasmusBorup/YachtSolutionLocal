using System;
using System.Collections.Generic;
using Yachtsolution.DataLayer;

namespace Yachtsolution.ControlLayer
{
    /// <summary>
    /// This is the class SettingsController.
    /// </summary>
    public sealed class SettingsController
    {
        private static object _syncRoot = new Object();
        private static volatile SettingsController _instance;

        private SettingsDB sDB;
        private LogBookController logbookCtr;

        /// <summary>
        /// This is the constructor for the class SettingsController.
        /// </summary>
        private SettingsController()
        {
            logbookCtr = LogBookController.GetInstance();
            sDB = SettingsDB.GetInstance();
        }

        /// <summary>
        /// This method is a multi threaded singleton for the class SettingsController.
        /// </summary>
        /// <returns></returns>
        public static SettingsController GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SettingsController();
            }

            return _instance;
        }

        /// <summary>
        /// this method creates logbooks.
        /// </summary>
        public bool FillDatabase()
        {
            if (logbookCtr.FillDatabase() && sDB.DefaultRoles() && sDB.DefaultTabs())//Insert other methods here that adds default data and returns a boolean value indicating success
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<string> GetRoles()
        {
            return sDB.GetRoleNames();
        }

        public List<string> GetTabs(string role)
        {
            return sDB.GetTabs(role);
        }

        public void SaveTabs(string role, bool logbook, bool jobs, bool inventory, bool employees)
        {
            sDB.SaveTabs(role, logbook, jobs, inventory, employees);
        }

        public bool DeleteRole(string role)
        {
            return sDB.DeleteRole(role);
        }

        public bool CreateRole(string role)
        {
            return sDB.CreateRole(role);
        }
    }
}
