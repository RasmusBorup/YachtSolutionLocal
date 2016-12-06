using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Yachtsolution.DataLayer
{
    public class Employee
    {
        public int EmpID { get; set; }
        public string Name { get; set; }
        public double Salary { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Ssn { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public byte[] Picture { get; set; }

        public Employee() { }
    }

    /// <summary>
    /// This is the class EmployeeDB.
    /// </summary>
    public sealed class EmployeeDB
    {
        private static volatile EmployeeDB instance;
        private string employeePath;

        /// <summary>
        /// This is the constructor for the class EmployeeDB.
        /// </summary>
        private EmployeeDB()
        {
            employeePath = "Employees.json";
        }

        /// <summary>
        /// This method is a multi threaded singleton for the class EmployeeDB.
        /// </summary>
        /// <returns>instance</returns>
        public static EmployeeDB GetInstance()
        {
            if (instance == null)
            {
                instance = new EmployeeDB();
            }
            return instance;
        }

        /// <summary>
        /// This method creates an object of the class Employee and inserts it in the database.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="jobTitle"></param>
        /// <param name="jobMail"></param>
        /// <param name="jobPhone"></param>
        /// <param name="ssn"></param>
        /// <param name="salary"></param>
        /// <param name="photo"></param>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns>success</returns>
        public string CreateEmployee(string name, string jobTitle, string jobMail, string jobPhone, string ssn, string salary, string imageLocation, string userName, string passWord)
        {
            if (name == "")
                return "emptyName";
            if (jobTitle == "")
                return "emptyTitle";
            if (jobMail == "")
                return "emptyMail";
            if (jobPhone == "")
                return "emptyPhone";
            if (ssn == "")
                return "emptySSN";
            double salaryD = 0;
            try
            {
                salaryD = Convert.ToDouble(salary);
            }
            catch (Exception)
            {
                return "emptySalary";
            }
            if (userName == "")
                return "emptyUsername";
            if (passWord == "")
                return "emptyPassword";

            List<Employee> employees = GetAllEmployees();
            Employee employee = new Employee();

            employee.EmpID = NextID();
            employee.Name = name;
            employee.Salary = salaryD;
            employee.Title = jobTitle;
            employee.Email = jobMail;
            employee.Phone = jobPhone;
            employee.Ssn = ssn;
            employee.Username = userName;
            employee.Password = passWord;

            if (!string.IsNullOrEmpty(imageLocation))
            {
                employee.Picture = new MemoryStream(File.ReadAllBytes(imageLocation)).ToArray();
            }
            employees.Add(employee);
            File.WriteAllText(employeePath, JsonConvert.SerializeObject(employees));

            return "success";
        }

        private int NextID()
        {
            for (int i = 0; i < GetAllEmployees().Count + 1; i++)
            {
                if (!GetAllEmployees().Exists(e => e.EmpID == i))
                {
                    return i;
                }
            }
            return 0;
        }

        /// <summary>
        /// This method finds and deletes an object of the class Employee from the database by its instance variable idOfEmployee.
        /// </summary>
        /// <param name="deleteID"></param>
        /// <returns>success</returns>
        public string DeleteEmployee(int deleteID)
        {
            try
            {
                List<Employee> employees = GetAllEmployees();
                Employee empToRemove = employees.SingleOrDefault(e => e.EmpID == deleteID);

                employees.Remove(empToRemove);
                File.WriteAllText(employeePath, JsonConvert.SerializeObject(employees));
                return "success";
            }
            catch (Exception)
            {
                return "fail";
            }
        }

        /// <summary>
        /// This method finds and updates an object of the class Employee in the database by its instance variable idOfEmployee.
        /// </summary>
        /// <param name="idOfEmployee"></param>
        /// <param name="name"></param>
        /// <param name="jobTitle"></param>
        /// <param name="jobMail"></param>
        /// <param name="jobPhone"></param>
        /// <param name="ssn"></param>
        /// <param name="salary"></param>
        /// <param name="photo"></param>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns>success</returns>
        public string UpdateEmployeeByID(int idOfEmployee, string name, string jobTitle, string jobMail, string jobPhone, string ssn, string salary, string imageLocation, string userName, string passWord)
        {
            if (name == "")
                return "emptyName";
            if (jobTitle == "")
                return "emptyTitle";
            if (jobMail == "")
                return "emptyMail";
            if (jobPhone == "")
                return "emptyPhone";
            if (ssn == "")
                return "emptySSN";
            double salaryD = 0;
            try
            {
                salaryD = Convert.ToDouble(salary);
            }
            catch (Exception)
            {
                return "emptySalary";
            }
            if (userName == "")
                return "emptyUsername";
            if (passWord == "")
                return "emptyPassword";

            List<Employee> employees = GetAllEmployees();
            Employee empToUpdate = GetAllEmployees().SingleOrDefault(e => e.EmpID == idOfEmployee);

            empToUpdate = employees.Single(e => e.EmpID == idOfEmployee);
            empToUpdate.Name = name;
            empToUpdate.Title = jobTitle;
            empToUpdate.Email = jobMail;
            empToUpdate.Phone = jobPhone;
            empToUpdate.Ssn = ssn;
            empToUpdate.Salary = salaryD;
            empToUpdate.Username = userName;
            empToUpdate.Password = passWord;
            if (!string.IsNullOrEmpty(imageLocation))
            {
                empToUpdate.Picture = new MemoryStream(File.ReadAllBytes(imageLocation)).ToArray();
            }
            File.WriteAllText(employeePath, JsonConvert.SerializeObject(employees));

            return "success";
        }

        /// <summary>
        /// This method returns all the objects of the class Employee that lies in the database.
        /// </summary>
        /// <returns>employees</returns>
        public List<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();
            if (File.Exists(employeePath))
            {
                employees = JsonConvert.DeserializeObject<List<Employee>>(File.ReadAllText(employeePath));
            }

            return employees;
        }

        /// <summary>
        /// This method finds and returns a list of objects of the class Employee that lies in the database by its instance variable name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>employees</returns>
        public List<Employee> FindEmployeeByName(string name)
        {
            List<Employee> employees = GetAllEmployees();
            List<Employee> empsToFind = employees.Where(e => e.Name == name).ToList();

            return empsToFind;
        }

        /// <summary>
        /// This method finds and returns an object of the class Employee that lies in the database by its instance variable idOfEmployee.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>employee</returns>
        public Employee FindEmployeeById(int employeeId)
        {

            List<Employee> employees = GetAllEmployees();
            Employee empToFind = employees.SingleOrDefault(e => e.EmpID == employeeId);

            return empToFind;
        }

        /// <summary>
        /// This method finds and returns an object of the class Employee that lies in the database by its instance variable ssn.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>employee</returns>
        public Employee FindEmployeeBySSN(string ssn)
        {
            return GetAllEmployees().SingleOrDefault(e => e.Ssn == ssn);
        }

        /// <summary>
        /// This method finds and returns an object of the class Employee that lies in the database by its instance variable userName and passWord.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>employee</returns>
        public Employee FindEmployeeByLogin(string userName, string password)
        {
            return GetAllEmployees().SingleOrDefault(e => e.Username == userName && e.Password == password);
        }
    }
}