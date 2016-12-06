using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Yachtsolution.DataLayer
{
    public class Job
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Role { get; set; }
        public string Note { get; set; }
        public string NameOfEmployee { get; set; }
        public string Subgroup { get; set; }
        public int JobFrequency { get; set; }
        public int ID { get; set; }
        public int RoutineID { get; set; }
        public bool IsDone { get; set; }
        public DateTime Date { get; set; }
        public byte[] photo { get; set; }

        public Job() { }
    }

    public class Subgroup
    {
        public string Name { get; set; }
    }
    /// <summary>
    /// This is the class JobDB.
    /// </summary>
    public sealed class JobDB
    {
        private static volatile JobDB instance = null;
        private static object syncRoot = new Object();
        private string jobsPath;
        private string subgroupPath;

        /// <summary>
        /// This is the constructor for the class JobDB.
        /// </summary>
        private JobDB()
        {
            jobsPath = "Jobs.json";
            subgroupPath = "Subgroups.json";
        }

        /// <summary>
        /// This method is a multi threaded singleton for the class JobDB.
        /// </summary>
        /// <returns>instance</returns>
        public static JobDB GetInstance()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new JobDB();
                    }
                }
            }

            return instance;
        }

        private int NextID(List<Job> jobs)
        {
            int i = 0;
            while (i < jobs.Count + 1)
            {
                if (!jobs.Exists(j => j.ID == i))
                {
                    return i;
                }
                i++;
            }
            return i;
        }

        /// <summary>
        /// This method creates an object of the class Job and inserts it in the database.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="nameOfEmployee"></param>
        /// <param name="timeBetweenJobs"></param>
        /// <param name="done"></param>
        /// <param name="note"></param>
        /// <param name="role"></param>
        /// <param name="routineCheck"></param>
        /// <param name="photo"></param>
        /// <param name="subGroup"></param>
        /// <returns>success</returns>
        public string CreateJob(string title, string description, string nameOfEmployee, int timeBetweenJobs, bool done, string note, string role, string imageLocation, string subgroup)
        {
            if (title == "" || title.Length > 50)
                return "emptyTitle";
            if (timeBetweenJobs < 0)
                return "negativeFrequency";
            if (subgroup == "" || FindSubGroupByName(subgroup) == null)
                return "emptySubgroup";
            

            Job job = new Job();
            List<Job> jobs = ListAllJobs();
            job.Title = title;
            job.Description = description;
            job.NameOfEmployee = nameOfEmployee;
            job.JobFrequency = timeBetweenJobs;
            job.IsDone = done;
            job.Note = note;
            job.Role = role;
            job.Date = DateTime.Today.Date;
            job.ID = NextID(jobs);


            if (!string.IsNullOrEmpty(imageLocation))
            {
                job.photo = ImageByteArray(imageLocation);
            }
            job.Subgroup = subgroup;
            jobs.Add(job);
            job.RoutineID = job.ID;

            SaveJobs(jobs);

            if (job.JobFrequency > 0)
            {
                CreateRoutineJob(job.RoutineID, title, description, timeBetweenJobs, note, role, imageLocation, subgroup);
            }
            return "success";
        }

        /// <summary>
        /// This method creates objects of the class job and inserts them in the database.
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="nameOfWorker"></param>
        /// <param name="timeBetweenJobs"></param>
        /// <param name="note"></param>
        /// <param name="role"></param>
        /// <param name="photo"></param>
        /// <param name="subGroup"></param>
        /// <returns>success</returns>
        private void CreateRoutineJob(int jobId, string title, string description, int timeBetweenJobs, string note, string role, string imageLocation, string subGroup)
        {
            Job job = new Job();
            List<Job> jobs = ListAllJobs();
            for (int i = 1; i < 5; i++)
            {
                job = new Job();
                job.Title = title;
                job.Description = description;
                job.NameOfEmployee = null;
                job.JobFrequency = timeBetweenJobs;
                job.IsDone = false;
                job.Note = note;
                job.Role = role;
                job.RoutineID = jobId;
                job.Subgroup = subGroup;
                double daysToAdd = timeBetweenJobs * i;
                job.ID = NextID(jobs);
                job.Date = DateTime.Today.AddDays(daysToAdd);
                job.photo = ImageByteArray(imageLocation);
                jobs.Add(job);
            }
            SaveJobs(jobs);
        }

        /// <summary>
        /// This method returns all the objects of the class job that lies in the database.
        /// </summary>
        /// <returns>jobs</returns>
        public List<Job> ListAllJobs()
        {
            List<Job> jobs = new List<Job>();

            if (File.Exists(jobsPath))
            {
                jobs = JsonConvert.DeserializeObject<List<Job>>(File.ReadAllText(jobsPath));
            }

            return jobs;
        }

        /// <summary>
        /// This method finds and updates a job from the database by its instance variable ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="workerId"></param>
        /// <param name="timeBetweenJobs"></param>
        /// <param name="done"></param>
        /// <param name="note"></param>
        /// <param name="role"></param>
        /// <param name="updateRoutine"></param>
        /// <param name="photo"></param>
        /// <param name="subGroup"></param>
        /// <returns>success</returns>
        public string UpdateJob(int id, string title, string description, string employeeName, int timeBetweenJobs, bool done, string note, string role, bool updateRoutine, string imageLocation, string subgroup)
        {
            if (ListAllJobs().SingleOrDefault(j => j.ID == id) == null)
                return "jobNotFound";
            if (title == "" || title.Length > 55)
                return "emptyTitle";
            if (timeBetweenJobs < 0)
                return "negativeFrequency";
            if (subgroup == "" || FindSubGroupByName(subgroup) == null)
                return "emptySubgroup";

            List<Job> jobs = ListAllJobs();
            Job job = jobs.SingleOrDefault(j => j.ID == id);

            job.Title = title;
            job.Description = description;
            job.NameOfEmployee = employeeName;
            job.JobFrequency = timeBetweenJobs;
            job.IsDone = done;
            job.Note = note;
            job.Role = role;
            job.photo = ImageByteArray(imageLocation);
            job.Subgroup = subgroup;

            SaveJobs(jobs);

            if (updateRoutine)
            {
                List<Job> routineJobs = jobs.Where(j => j.RoutineID == job.RoutineID).ToList();
                foreach (Job oldJob in routineJobs)
                {
                    oldJob.Title = title;
                    oldJob.Description = description;
                    oldJob.Note = note;
                    oldJob.Role = role;
                    oldJob.JobFrequency = timeBetweenJobs;
                    oldJob.photo = ImageByteArray(imageLocation);
                    oldJob.Subgroup = subgroup;
                }

                if (routineJobs[0].JobFrequency != timeBetweenJobs)
                {
                    //Update future jobs to fit with a possible change in frequency of a routine job
                    routineJobs = routineJobs.Where(j => j.Date >= DateTime.Today).OrderBy(j => j.Date).ToList();
                    DateTime first = routineJobs.First().Date;
                    int i = 0;

                    foreach (Job oldJob in routineJobs)
                    {
                        oldJob.Date = first.AddDays(timeBetweenJobs*i);
                        i++;
                    }
                }
            }
            SaveJobs(jobs);

            return "success";
        }

        private byte[] ImageByteArray(string imageLocation)
        {
            if (!string.IsNullOrEmpty(imageLocation))
            {
                return new MemoryStream(File.ReadAllBytes(imageLocation)).ToArray();
            }
            return null;
        }

        private void SaveJobs(List<Job> jobs)
        {
            File.WriteAllText(jobsPath, JsonConvert.SerializeObject(jobs));
        }

        /// <summary>
        /// This method finds and deletes an object of the class Job that lies in the database by its instance variable ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>success</returns>
        public string DeleteJob(int id, bool deleteRoutine)
        {

            try
            {
                List<Job> jobs = ListAllJobs();
                Job jobToDelete = jobs.Single(j => j.ID == id);
                jobs.Remove(jobToDelete);

                SaveJobs(jobs);

                if (deleteRoutine)
                {
                    DeleteRoutine(jobToDelete);
                }

                return "success";
            }

            catch (Exception exception)
            {
                Console.WriteLine("Couldn't delete the job.");
                Console.WriteLine("Error: " + exception.Message);
                return "fail";
            }
        }

        /// <summary>
        /// This method finds and deletes a list of objects of the class job that lies in the database by its instance variable routineId.
        /// </summary>
        /// <param name="routineJob"></param>
        /// <returns>success</returns>
        public bool DeleteRoutine(Job routineJob)
        {
            try
            {
                List<Job> jobs = ListAllJobs();

                jobs.RemoveAll(j => j.RoutineID == routineJob.RoutineID);
                SaveJobs(jobs);
                return true;
            }

            catch (Exception exception)
            {
                Console.WriteLine("Couldn't delete the routine jobs.");
                Console.WriteLine("Error: " + exception.Message);
                return false;
            }
        }

        /// <summary>
        /// This method checks if an object of the class job is a routine job by its instance variable routineId.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>success</returns>
        public bool CheckIfRoutine(int id)
        {
            try
            {
                Job job = ListAllJobs().SingleOrDefault(j => j.ID == id);
                int noOfJobsInRoutine = ListAllJobs().Count(j => j.RoutineID == job.RoutineID);

                if (noOfJobsInRoutine > 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            catch (Exception exception)
            {
                Console.WriteLine("Couldn't check the job if it is routine.");
                Console.WriteLine("Error: " + exception.Message);
                return false;
            }
        }

        /// <summary>
        /// This method Checks the object of the class Job for routine jobs.
        /// </summary>
        /// <returns>success</returns>
        public bool CheckRoutines()
        {
            bool success = false;
            List<Job> jobs = ListAllJobs();

            try
            {
                List<Job> routines = jobs.Where(j => j.JobFrequency > 0).ToList();

                while (routines.Count > 0)
                {
                    Job routine = routines.First();
                    List<Job> jobsInRoutine = routines.Where(j => j.RoutineID == routine.RoutineID).ToList();

                    jobsInRoutine = jobsInRoutine.Where(j => j.Date >= DateTime.Today).OrderBy(d => d.Date).ToList();
                    int futureJobs = jobsInRoutine.Count;
                    int i = 1;

                    while (futureJobs < 6)
                    {
                        Job newJob = new Job();
                        newJob.Date = routine.Date;
                        newJob.Description = routine.Description;
                        newJob.ID = NextID(jobs);
                        newJob.IsDone = false;
                        newJob.JobFrequency = routine.JobFrequency;
                        newJob.NameOfEmployee = routine.NameOfEmployee;
                        newJob.Note = routine.Note;
                        newJob.Role = routine.Role;
                        newJob.RoutineID = routine.RoutineID;
                        newJob.Subgroup = routine.Subgroup;
                        newJob.Title = routine.Title;
                        newJob.photo = routine.photo;

                        DateTime nextDate = new DateTime();
                        if (jobsInRoutine.Count() != 0)
                        {
                            nextDate = jobsInRoutine.Last().Date;
                        }
                        else
                        {
                            nextDate = routines.Last(j => j.RoutineID == routine.RoutineID).Date;
                        }
                        int daysToAdd = i * newJob.JobFrequency;
                        nextDate = nextDate.AddDays(daysToAdd);
                        newJob.Date = nextDate;

                        i++;
                        jobs.Add(newJob);
                        futureJobs = jobs.FindAll(j => j.RoutineID == routine.RoutineID).FindAll(j => j.Date >= DateTime.Today).Count;
                    }
                    routines = routines.Where(j => j.RoutineID != routine.RoutineID).ToList();
                }

                SaveJobs(jobs);
                success = true;
            }

            catch (Exception exception)
            {
                Console.WriteLine("Couldn't check the routine jobs.");
                Console.WriteLine("Error: " + exception.Message);
                success = false;
            }

            return success;
        }

        private void SaveSubgroups(List<Subgroup> subgroups)
        {
            File.WriteAllText(subgroupPath, JsonConvert.SerializeObject(subgroups));
        }

        /// <summary>
        /// This method creates an object of the class Subgroup and inserts it in the database.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>success</returns>
        public string InsertSubGroup(string name)
        {
            if (name == "")
                return "emptyName";
            if (FindSubGroupByName(name) != null)
                return "usedName";

            Subgroup sg = new Subgroup();
            List<Subgroup> subgroups = GetAllSubGroups();

            sg.Name = name;
            subgroups.Add(sg);
            SaveSubgroups(subgroups);
            return "success";
        }

        /// <summary>
        /// This method returns all the objects of the class Subgroup that lies in the database.
        /// </summary>
        /// <returns>subGroups</returns>
        public List<Subgroup> GetAllSubGroups()
        {
            List<Subgroup> subgroups = new List<Subgroup>();

            if (File.Exists(subgroupPath))
            {
                subgroups = JsonConvert.DeserializeObject<List<Subgroup>>(File.ReadAllText(subgroupPath));
            }

            return subgroups;
        }

        /// <summary>
        /// This method finds and returns a list of objects of the class Subgroup that lies in the database by its instance variable name.
        /// </summary>
        /// <returns>subGroup</returns>
        public Subgroup FindSubGroupByName(string name)
        {
            try
            {
                return GetAllSubGroups().Single(s => s.Name == name);
            }

            catch (Exception exception)
            {
                Console.WriteLine("Couldn't find the subgroups by name.");
                Console.WriteLine("error: " + exception.Message);
                return null;
            }
        }

        /// <summary>
        /// This method finds and updates an object of the class Subgroup that lies in the database by its instance variable subGroup.
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        /// <returns>success</returns>
        public bool UpdateSubGroup(string oldName, string newName)
        {
            if (FindSubGroupByName(newName) == null)
            {
                try
                {
                    List<Subgroup> subgroups = GetAllSubGroups();
                    Subgroup sg = subgroups.Single(s => s.Name == oldName);
                    sg.Name = newName;
                    SaveSubgroups(subgroups);

                    return true;
                }

                catch (Exception exception)
                {
                    Console.WriteLine("Couldn't update the subGroup.");
                    Console.WriteLine("Error: " + exception.Message);

                }
            }
            return false;
        }

        /// <summary>
        /// This method finds and deletes an object of the class Subgroup that lies in the database by its instance variable subGroup.
        /// </summary>
        /// <param name="grpID"></param>
        /// <returns>success</returns>
        public bool DeleteSubGroup(string name)
        {
            try
            {
                List<Subgroup> subgroups = GetAllSubGroups();
                subgroups.RemoveAll(s => s.Name == name);

                SaveSubgroups(subgroups);
                return true;
            }

            catch (Exception exception)
            {
                Console.WriteLine("Couldn't delete the subgroup.");
                Console.WriteLine("Error: " + exception.Message);
                return false;
            }
        }

        /// <summary>
        /// This method finds and return a list of objects of the class Job that lies in the database by its instance variable role, Date and/or subGroup.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="subGroup"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="roleName"></param>
        /// <param name="groupName"></param>
        /// <returns>jobs</returns>
        public List<Job> DynamicSeach(bool start, bool end, bool subGroup, DateTime startDate, DateTime endDate, string roleName, string groupName)
        {
            List<Job> jobs = ListAllJobs();

            try
            {
                if (roleName != "" && roleName != "Administrator")
                {
                    jobs = jobs.Where(j => j.Role == roleName).ToList();
                }

                if (start)
                {
                    jobs = jobs.Where(j => j.Date >= startDate).ToList();
                }

                if (end)
                {
                    jobs = jobs.Where(j => j.Date <= endDate).ToList();
                }

                if (subGroup)
                {
                    jobs = jobs.Where(j => j.Subgroup == groupName).ToList();
                }

                if (startDate == DateTime.Today)
                {
                    List<Job> jobsToAdd = ListAllJobs().Where(j => j.Date < DateTime.Today && j.Role == roleName && j.IsDone == false).ToList();

                    foreach (Job job in jobsToAdd)
                    {
                        jobs.Add(job);
                    }
                }
            }

            catch (Exception exception)
            {
                Console.WriteLine("Couldn't find the jobs.");
                Console.WriteLine("Error: " + exception.Message);
                jobs = ListAllJobs();
            }

            return jobs;
        }
    }
}