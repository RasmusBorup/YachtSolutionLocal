using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Yachtsolution.ControlLayer;
using Yachtsolution.DataLayer;

namespace Yachtsolution.GUILayer
{
    /// <summary>
    /// This is the class ListOfJobs and is a subclass to the class MyFormPage.
    /// </summary>
    public partial class ListOfJobs : MyFormPage
    {
        private JobController jobCtr;
        private string role;

        /// <summary>
        /// this is the constructor for the class ListOfJobs.
        /// </summary>
        public ListOfJobs(string role)
        {
            InitializeComponent();
            panel = listOfJobs_panel;
            jobCtr = JobController.GetInstance();
            jobCtr.CheckRoutines();
            this.role = role;
            ResetBoxes();

        }

        /// <summary>
        /// This method resets the boxes in the class ListOfJobs.
        /// </summary>
        private void ResetBoxes()
        {
            dateStart_dateTimePicker.Value = DateTime.Today;
            dateEnd_dateTimePicker.Value = DateTime.Today;
            cbSubGroup.SelectedItem = null;
            cbSubGroup.DataSource = jobCtr.GetAllSubGroups();
            cbSubGroup.DisplayMember = "name";

            chbStartDate.Checked = true;
            chbEndDate.Checked = true;
            chbSubGroup.Checked = false;

            DynamicSearch();
        }

        /// <summary>
        /// This method adds the list of jobs to the list jobs_ListView.
        /// </summary>
        /// <param name="jobs"></param>
        private void AddJobsToList(List<Job> jobs)
        {
            jobs = jobs.OrderBy(j => j.Date).ToList();
            jobListGridView.DataSource = jobs;

            jobListGridView.Columns[3].Visible = false;
            jobListGridView.Columns[4].Visible = false;
            jobListGridView.Columns[6].Visible = false;
            jobListGridView.Columns[7].Visible = false;
            jobListGridView.Columns[8].Visible = false;
            jobListGridView.Columns[9].HeaderText = "Is Done";
            jobListGridView.Columns[10].Visible = false;
            jobListGridView.Columns[11].Visible = false;
        }

        /// <summary>
        /// This method find jobs by search criteria.
        /// </summary>
        public void DynamicSearch()
        {
            cbSubGroup.DataSource = jobCtr.GetAllSubGroups();
            List<Job> jobs = jobCtr.DynamicSearch(chbStartDate.Checked, chbEndDate.Checked, chbSubGroup.Checked, dateStart_dateTimePicker.Value.Date, dateEnd_dateTimePicker.Value.Date, role, cbSubGroup.Text);
            AddJobsToList(jobs);
        }

        /// <summary>
        /// This method colors the rows with delayed jobs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void jobListGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            foreach (DataGridViewRow row in jobListGridView.Rows)
            {
                Job job = row.DataBoundItem as Job;

                if (job != null)
                {
                    if (job.Date < DateTime.Today.Date && job.IsDone == false)
                    {
                        row.DefaultCellStyle.BackColor = Color.Red;
                    }
                }
            }
        }

        /// <summary>
        /// This method call the DynamicSearch method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            DynamicSearch();
        }

        /// <summary>
        /// This method call the ResetBoxes method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetBoxes();
        }

        /// <summary>
        /// This method opens the windows form UpdateJob.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateJob_Click(object sender, EventArgs e)
        {
            UpdateJob();
        }

        /// <summary>
        /// This method opens the windows form updateJob.
        /// </summary>
        private void UpdateJob()
        {
            Job jobToUpdate = jobListGridView.SelectedRows[0].DataBoundItem as Job;
            Form updateJob = new UpdateJob(jobToUpdate, role);
            updateJob.ShowDialog();
            DynamicSearch();
        }

        /// <summary>
        /// This method opens the windows form CreateJob.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateJob_Click(object sender, EventArgs e)
        {
            CreateJob cj = new CreateJob(role);
            cj.ShowDialog();
            DynamicSearch();
        }

        private void jobListGridView_CellDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {
            UpdateJob();
        }
    }
}