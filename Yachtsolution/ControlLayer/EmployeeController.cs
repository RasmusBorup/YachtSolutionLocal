using System;
using System.Collections.Generic;
using Yachtsolution.DataLayer;

namespace Yachtsolution.ControlLayer
{
    /// <summary>
    /// This is the class EmployeeController.
    /// </summary>
    public sealed class EmployeeController
    {
        private static object syncRoot = new Object();
        private static volatile EmployeeController instance;
        private EmployeeDB employeeDB;

        /// <summary>
        /// This is the constructor for the class EmployeeController.
        /// </summary>
        private EmployeeController()
        {
            employeeDB = EmployeeDB.GetInstance();
        }

        /// <summary>
        /// This method is a multi threaded singleton for the class EmployeeController.
        /// </summary>
        /// <returns>instance</returns>
        public static EmployeeController GetInstance()
        {
            if (instance == null)
            {
                instance = new EmployeeController();
            }
            return instance;
        }

        /// <summary>
        /// This method returns all the employees.
        /// </summary>
        /// <returns>employees</returns>
        public List<Employee> ListAllEmployees()
        {
            return employeeDB.GetAllEmployees();
        }

        /// <summary>
        /// This method creates an employee.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="jobTitle"></param>
        /// <param Name="jobMail"></param>
        /// <param name="jobPhone"></param>
        /// <param name="ssn"></param>
        /// <param name="salary"></param>
        /// <param name="photo"></param>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns>success</returns>
        public string CreateEmployee(string name, string jobTitle, string jobMail, string jobPhone, string ssn, string salary, string imageLocation, string userName, string passWord)
        {
            return employeeDB.CreateEmployee(name, jobTitle, jobMail, jobPhone, ssn, salary, imageLocation, userName, passWord);
        }

        /// <summary>
        /// This method deletes an employee.
        /// </summary>
        /// <param name="deleteID"></param>
        /// <returns>success</returns>
        public string DeleteEmployee(int deleteID)
        {
            return employeeDB.DeleteEmployee(deleteID);
        }

        /// <summary>
        /// This method updates an employee.
        /// </summary>
        /// <param name="idOfEmployee"></param>
        /// <param name="name"></param>
        /// <param name="jobTitle"></param>
        /// <param name="jobMail"></param>
        /// <param name="jobPhone"></param>
        /// <param name="SSN"></param>
        /// <param name="salary"></param>
        /// <param name="photo"></param>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns>success</returns>
        public string UpdateEmployeeByID(int idOfEmployee, string name, string jobTitle, string jobMail, string jobPhone, string SSN, string salary, string imageLocation, string userName, string passWord)
        {
            return employeeDB.UpdateEmployeeByID(idOfEmployee, name, jobTitle, jobMail, jobPhone, SSN, salary, imageLocation, userName, passWord);
        }

        /// <summary>
        /// This method finds employees by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>employees</returns>
        public List<Employee> FindEmployeeByName(string name)
        {
            return employeeDB.FindEmployeeByName(name);
        }

        /// <summary>
        /// This method finds employee by ssn.
        /// </summary>
        /// <param name="ssn"></param>
        /// <returns>employee</returns>
        public Employee FindEmployeeBySsn(string ssn)
        {
            return employeeDB.FindEmployeeBySSN(ssn);
        }

        /// <summary>
        /// This method finds employee by employeeId.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>employee</returns>
        public Employee FindEmployeeByID(int employeeId)
        {
            return employeeDB.FindEmployeeById(employeeId);
        }

        /// <summary>
        /// This method finds an employee by its userName and passWord.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>employee</returns>
        public Employee FindEmployeeByLogin(string userName, string password)
        {
            return employeeDB.FindEmployeeByLogin(userName, password);
        }
    }
}