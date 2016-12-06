using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Yachtsolution.DataLayer
{
    class Role1
    {
        public string role { get; set; }
        public List<Tab1> tabs { get; set; }

        public Role1()
        {
            tabs = new List<Tab1>();
        }
    }

    class Tab1
    {
        public string tab { get; set; }
    }
    class SettingsDB
    {
        private string rolePath;
        private string tabPath;

        private static SettingsDB instance;
        private SettingsDB()
        {
            rolePath = "Roles.json";
            tabPath = "Tabs.json";

        }

        public static SettingsDB GetInstance()
        {
            if (instance == null)
            {
                instance = new SettingsDB();
            }
            return instance;
        }

        public List<string> GetRoleNames()
        {
            List<Role1> roles = new List<Role1>();
            List<string> stringRoles = new List<string>();
            if (File.Exists(rolePath))
            {
                roles = JsonConvert.DeserializeObject<List<Role1>>(File.ReadAllText(rolePath));
                foreach (Role1 role in roles)
                {
                    stringRoles.Add(role.role);
                }
            }
            return stringRoles;
        }

        private List<Role1> GetAllRoles()
        {
            List<Role1> roles = new List<Role1>();
            if (File.Exists(rolePath))
            {
                roles = JsonConvert.DeserializeObject<List<Role1>>(File.ReadAllText(rolePath));
            }

            return roles;
        }

        public List<string> GetTabs(string role)
        {
            List<string> tabs = new List<string>();
            if (role != "Administrator")
            {
                foreach (Tab1 tab in FindRole(role).tabs)
                {
                    if (tab != null)
                    {
                        tabs.Add(tab.tab);
                    }
                }
            }
            return tabs;
        }

        private List<Tab1> GetAllTabs()
        {
            List<Tab1> tabs = new List<Tab1>();
            if (File.Exists(tabPath))
            {
                tabs = JsonConvert.DeserializeObject<List<Tab1>>(File.ReadAllText(tabPath));
            }
            return tabs;
        }

        private Tab1 FindTab(string tab)
        {
            return GetAllTabs().SingleOrDefault(t => t.tab == tab);
        }

        public bool CreateRole(string role)
        {
            List<Role1> roles = GetAllRoles();
            if (FindRole(role) == null)
            {
                Role1 roleToInsert = new Role1();
                roleToInsert.role = role;
                roles.Add(roleToInsert);
                SaveRoles(roles);
                return true;
            }
            return false;
        }

        private void SaveRoles(List<Role1> roles)
        {
            File.WriteAllText(rolePath, JsonConvert.SerializeObject(roles));
        }

        private Role1 FindRole(string role)
        {
            Role1 roleToFind = GetAllRoles().SingleOrDefault(r => r.role == role);
            return roleToFind;
        }

        public bool DeleteRole(string role)
        {
            List<Role1> roles = GetAllRoles();
            Role1 roleToDelete = FindRole(role);
            if (roleToDelete != null)
            {
                roles.RemoveAll(r => r.role == role);
                SaveRoles(roles);
            }

            return true;
        }

        private bool CreateTab(string title)
        {
            List<Tab1> tabs = GetAllTabs();
            if (FindTab(title) == null)
            {
                Tab1 tab = new Tab1();
                tab.tab = title;
                tabs.Add(tab);
                SaveAllTabs(tabs);
            }

            return true;
        }

        private void SaveAllTabs(List<Tab1> tabs)
        {
            File.WriteAllText(tabPath, JsonConvert.SerializeObject(tabs));
        }

        public void SaveTabs(string role, bool logbook, bool jobs, bool inventory, bool employees)
        {
            List<Role1> roles = GetAllRoles();
            Role1 roleToUpdate = roles.SingleOrDefault(r => r.role == role);
            Tab1 logTab = roleToUpdate.tabs.SingleOrDefault(t => t.tab == "LogBook");
            Tab1 jobsTab = roleToUpdate.tabs.SingleOrDefault(t => t.tab == "Jobs");
            Tab1 invTab = roleToUpdate.tabs.SingleOrDefault(t => t.tab == "Inventory");
            Tab1 empTab = roleToUpdate.tabs.SingleOrDefault(t => t.tab == "Employee Management");
            
            if (!logbook && logTab != null)
            {
                roleToUpdate.tabs.Remove(logTab);
            }
            else if (logbook && logTab == null)
            {
                roleToUpdate.tabs.Add(GetAllTabs().SingleOrDefault(t => t.tab == "LogBook"));
            }
            if (!jobs && jobsTab != null)
            {
                roleToUpdate.tabs.Remove(jobsTab);
            }
            else if (jobs && jobsTab == null)
            {
                roleToUpdate.tabs.Add(GetAllTabs().SingleOrDefault(t => t.tab == "Jobs"));
            }
            if (!inventory && invTab != null)
            {
                roleToUpdate.tabs.Remove(invTab);
            }
            else if (inventory && invTab == null)
            {
                roleToUpdate.tabs.Add(GetAllTabs().SingleOrDefault(t => t.tab == "Inventory"));
            }
            if (!employees && empTab != null)
            {
                roleToUpdate.tabs.Remove(empTab);
            }
            else if (employees && empTab == null)
            {
                roleToUpdate.tabs.Add(GetAllTabs().SingleOrDefault(t => t.tab == "Employee Management"));
            }
            SaveRoles(roles);

        }

        public bool DefaultRoles()
        {
            CreateRole("Captain");
            CreateRole("Administrator");
            CreateRole("Chief Engineer");
            CreateRole("Chief Officer");
            CreateRole("Chef");
            CreateRole("Chief Stewardess");
            return true;
        }

        public bool DefaultTabs()
        {
            if (CreateTab("LogBook") && CreateTab("Jobs") && CreateTab("Inventory") && CreateTab("Employee Management"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
