using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Yachtsolution.ControlLayer;

namespace Yachtsolution.GUILayer
{
    /// <summary>
    /// This is the class CreateJob and is a subclass to the class Form.
    /// </summary>
    public partial class CreateJob : Form
    {
        private JobController jobCtr;
        private ImageController imageCTR;
        private string userRole;

        /// <summary>
        /// This is the constructor for the class CreateJob.
        /// </summary>
        public CreateJob(string userRole)
        {
            InitializeComponent();
            this.jobCtr = JobController.GetInstance();
            imageCTR = ImageController.GetInstance();
            UpdateGroupComboBox();
            cbSubGroup.SelectedIndex = -1;
            this.userRole = userRole;
            WorkerTextBox(false);
        }

        /// <summary>
        /// This method creates a job.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void save_button_Click(object sender, EventArgs e)
        {
            string title = tbTitle.Text;
            bool done_job = chbJobDone.Checked;
            string worker = "";
            if (done_job)
            {
                worker = tbWorker.Text;
            }
            int time_between_jobs = (int)numTimeBetweenJobs.Value;
            string description = tbDescription.Text;
            string note = tbNote.Text;
            string sg = cbSubGroup.Text;
            string imageLocation = pbPhoto.ImageLocation;
            
            string result = jobCtr.CreateJob(title, description, note, worker, time_between_jobs, done_job, userRole, imageLocation, sg);
            string message = "";
            switch (result)
            {
                case "emptyTitle":
                    message = "Please enter a title in the title field no longer than 55 characters long!";
                    break;
                case "emptySubgroup":
                    message = "Please select a subgroup from the dropdown list!";
                    break;
                case "success":
                    message = "Job saved succesfully!";
                    Close();
                    
                    break;
            }
            
            MessageBox.Show(message);
        }

        /// <summary>
        /// This method closes this windows form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void back_button_Click(object sender, EventArgs e)
        {
            Close();
        }
        
        /// <summary>
        /// This method returns the path to the image.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddPhoto_Click(object sender, EventArgs e)
        {
            pbPhoto.ImageLocation = imageCTR.BrowseImage();
        }

        /// <summary>
        /// This method opens the windows form CreateGroup.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewGroup_Click(object sender, EventArgs e)
        {
            CreateGroup cg = new CreateGroup(cbSubGroup);
            cg.Text = "New Group";
            cg.ShowDialog();
            cbSubGroup.DataSource = jobCtr.GetAllSubGroups();
            cbSubGroup.DisplayMember = "name";
            cbSubGroup.SelectedItem = jobCtr.GetAllSubGroups().Last();
        }

        /// <summary>
        /// This method opens the windows form UpdateSubGroup.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateGroup_Click(object sender, EventArgs e)
        {
            string sg = cbSubGroup.Text;

            if (sg != "")
            {
                UpdateSubGroup ug = new UpdateSubGroup(sg, cbSubGroup);
                ug.Text = "Update Group";
                ug.ShowDialog();
            }
            else
            {
                MessageBox.Show("You need to select a 'Main Group'.");
            }
        }

        /// <summary>
        /// This method fills up the combo box cbSubGroup.
        /// </summary>
        public void UpdateGroupComboBox()
        {
            cbSubGroup.DataSource = jobCtr.GetAllSubGroups();
            cbSubGroup.DisplayMember = "name";
        }

        /// <summary>
        /// This method enables or disables the text box tbWorker when the value in the check box chbJobDone has changed.
        /// </summary>
        /// <param name="enabled"></param>
        private void WorkerTextBox(bool enabled)
        {
            tbWorker.Enabled = enabled;
        }

        /// <summary>
        /// This method checks if the value of the check box chbJobDone has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chbJobDone_CheckedChanged(object sender, EventArgs e)
        {
            WorkerTextBox(chbJobDone.Checked);
        }
    }
}