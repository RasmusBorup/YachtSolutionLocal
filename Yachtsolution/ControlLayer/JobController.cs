using System;
using System.Collections.Generic;
using Yachtsolution.DataLayer;

namespace Yachtsolution.ControlLayer
{
    /// <summary>
    /// This is the class JobController.
    /// </summary>
    public sealed class JobController
    {
        private static object _syncRoot = new Object();
        private static volatile JobController _instance;
        private JobDB jobDB;

        /// <summary>
        /// This is the constructor for the class JobController.
        /// </summary>
        private JobController()
        {
            jobDB = JobDB.GetInstance();
        }

        /// <summary>
        /// This method is a multi threaded singleton for the class JobController.
        /// </summary>
        /// <returns>_instance</returns>
        public static JobController GetInstance()
        {
            if (_instance == null)
            {
                _instance = new JobController();
            }

            return _instance;
        }

        /// <summary>
        /// This method returns a list of all the jobs.
        /// </summary>
        /// <returns>jobs</returns>
        public List<Job> ListAllJobs()
        {
            return jobDB.ListAllJobs();
        }

        /// <summary>
        /// This method creates a job.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="note"></param>
        /// <param name="nameOfWorker"></param>
        /// <param name="workerID"></param>
        /// <param name="timeBetweenJobs"></param>
        /// <param name="done"></param>
        /// <param name="role"></param>
        /// <param name="photo"></param>
        /// <param name="subGroup"></param>
        /// <returns>boolean</returns>
        public string CreateJob(string title, string description, string note, string nameOfWorker, int timeBetweenJobs, bool done, string role, string photo, string subGroup)
        {
            return jobDB.CreateJob(title, description, nameOfWorker, timeBetweenJobs, done, note, role, photo, subGroup);
        }

        /// <summary>
        /// This method updates a job.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="workerID"></param>
        /// <param name="timeBetweenJobs"></param>
        /// <param name="done"></param>
        /// <param name="note"></param>
        /// <param name="role"></param>
        /// <param name="updateRoutine"></param>
        /// <param name="photo"></param>
        /// <param name="subGroup"></param>
        /// <returns>boolean</returns>
        public string UpdateJob(int id, string title, string description, string nameOfEmloyee, int timeBetweenJobs, bool done, string note, string role, bool updateRoutine, string imageLocation, string subGroup)
        {
            return jobDB.UpdateJob(id, title, description, nameOfEmloyee, timeBetweenJobs, done, note, role, updateRoutine, imageLocation, subGroup);
        }

        /// <summary>
        /// This method deletes a job.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deleteRoutine"></param>
        /// <returns>boolean</returns>
        public string DeleteJob(int id, bool deleteRoutine)
        {
            return jobDB.DeleteJob(id, deleteRoutine);
        }

        /// <summary>
        /// This method checks if a job is a routine.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>boolean</returns>
        public bool CheckIfRoutine(int id)
        {
            return jobDB.CheckIfRoutine(id);
        }

        /// <summary>
        /// This method checks the routine jobs.
        /// </summary>
        public bool CheckRoutines()
        {
            return jobDB.CheckRoutines();
        }

        /// <summary>
        /// This method creates a subgroup.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>boolean</returns>
        public string InsertSubGroup(string name)
        {
            return jobDB.InsertSubGroup(name);
        }

        /// <summary>
        /// This method gets all the subgroups.
        /// </summary>
        /// <returns>subgroups</returns>
        public List<Subgroup> GetAllSubGroups()
        {
            return jobDB.GetAllSubGroups();
        }

        /// <summary>
        /// This method finds a subgroup by its name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>subgroup</returns>
        public Subgroup FindSubGroupByName(string name)
        {
            return jobDB.FindSubGroupByName(name);
        }

        /// <summary>
        /// This method updates a subgroup.
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        /// <returns>boolean</returns>
        public bool UpdateSubGroup(string oldName, string newName)
        {
            return jobDB.UpdateSubGroup(oldName, newName);
        }

        /// <summary>
        /// This method deletes a subgroup.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>boolean</returns>
        public bool DeleteSubGroup(string name)
        {
            return jobDB.DeleteSubGroup(name);
        }

        /// <summary>
        /// This method finds jobs by its instance variables.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="role"></param>
        /// <param name="subGroup"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="roleName"></param>
        /// <param name="groupName"></param>
        /// <returns>jobs</returns>
        public List<Job> DynamicSearch(bool start, bool end, bool subGroup, DateTime startDate, DateTime endDate, string roleName, string groupName)
        {
            return jobDB.DynamicSeach(start, end, subGroup, startDate, endDate, roleName, groupName);
        }
    }
}