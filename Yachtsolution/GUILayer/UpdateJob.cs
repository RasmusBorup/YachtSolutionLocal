using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Yachtsolution.ControlLayer;
using Yachtsolution.DataLayer;

namespace Yachtsolution.GUILayer
{
    /// <summary>
    /// This is the class UpdateJob and is a subclass to the class MyFormPage.
    /// </summary>
    public partial class UpdateJob : Form
    {
        private JobController jobCtr;
        private ImageController imageCTR;
        private Job jobToUpdate;
        private string userRole;
        private string tbWorkerInitText = "Who finished the job?";
        private bool edited;

        /// <summary>
        /// This is the constructor for the class UpdateJob.
        /// </summary>
        /// <param name="jobToUpdate"></param>
        public UpdateJob(Job jobToUpdate,string userRole)
        {
            InitializeComponent();
            this.jobToUpdate = jobToUpdate;
            jobCtr = JobController.GetInstance();
            imageCTR = ImageController.GetInstance();
            this.SuspendLayout();
            this.userRole = userRole;

            cbSubGroup.DataSource = jobCtr.GetAllSubGroups();
            cbSubGroup.DisplayMember = "name";
            cbSubGroup.SelectedIndex = -1;

            this.Name = "UpdateJob";
            this.ResumeLayout(false);
            tbTitle.Text = jobToUpdate.Title;
            tbDescription.Text = jobToUpdate.Description;
            tbNote.Text = jobToUpdate.Note;
            chbJobDone.Checked = jobToUpdate.IsDone;
            numTimeBetweenJobs.Value = jobToUpdate.JobFrequency;
            cbSubGroup.Text = jobToUpdate.Subgroup;

            if (jobToUpdate.photo != null)
            {
                pbPhoto.Image = Image.FromStream(new MemoryStream(jobToUpdate.photo));
            }

            if (jobToUpdate.NameOfEmployee == "")
            {
                tbWorker.Text = tbWorkerInitText;
                tbWorker.ForeColor = Color.Gray;

                if (!chbJobDone.Checked)
                {
                    tbWorker.Enabled = false;
                }
            }

            else
            {
                tbWorker.Text = jobToUpdate.NameOfEmployee;
            }
        }

        /// <summary>
        /// This method updates a job.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateJobButton_Click(object sender, EventArgs e)
        {
            int jobID = jobToUpdate.ID;
            string title = tbTitle.Text;
            string description = tbDescription.Text;
            string nameOfEmployee = "";
            if (tbWorker.Text != tbWorkerInitText)
            {
                nameOfEmployee = tbWorker.Text;
            }
            int jobFrequency = (int)numTimeBetweenJobs.Value;
            string note = tbNote.Text;
            bool jobDone = chbJobDone.Checked;
            string sg = cbSubGroup.Text;
            string imageLocation = pbPhoto.ImageLocation;

            bool isRoutineJob = jobCtr.CheckIfRoutine(jobToUpdate.ID);
            bool updateRoutine = false;
            if (isRoutineJob) //if it is a routine job
            {
                //Ask if the whole routine is to be updated
                DialogResult answer = MessageBox.Show("Do you want to update the whole routine? " +
                                                      "If you click yes all jobs in the same routine as this one will be updated. " +
                                                      "If you click No only this one will be updated", "Warning",
                                                      MessageBoxButtons.YesNoCancel);
                if (answer == DialogResult.Yes)
                {
                    updateRoutine = true;
                }
                else if (answer == DialogResult.Cancel)
                {
                    return;
                }
            }
            string result = jobCtr.UpdateJob(jobID, title, description, nameOfEmployee, jobFrequency, jobDone, note, userRole, updateRoutine, imageLocation, sg);
            string message = "";
            switch (result)
            {
                case "emptyTitle":
                    message = "Please enter a title in the title field!";
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
        /// This method deletes a job.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteJob_click(object sender, EventArgs e)
        {
            //Check if it is a routine job
            bool isRoutineJob = jobCtr.CheckIfRoutine(jobToUpdate.ID);
            bool deleteRoutine = false;
            if (isRoutineJob) //if it is a routine job
            {
                //Ask if the whole routine is to be updated
                DialogResult answer = MessageBox.Show("Do you want to delete the whole routine? " +
                                                      "If you click yes all jobs in the same routine as this one will be deleted. " +
                                                      "If you click No only this one will be deleted", "Warning", MessageBoxButtons.YesNoCancel);
                if (answer == DialogResult.Yes)
                {
                    deleteRoutine = true;
                }

                else if (answer == DialogResult.Cancel)
                {
                    return;
                }
            }

            string result = jobCtr.DeleteJob(jobToUpdate.ID, deleteRoutine);
            string message = "";
            switch (result)
            {
                case "fail":
                    message = "Something unexpected hapened and prevented the delete operation from succeeding!";
                    break;
                case "success":
                    message = "The job was deleted!";
                    Close();
                    break;
            }

            MessageBox.Show(message);
        }

        /// <summary>
        /// This method lets the user find an image to the job.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            pbPhoto.ImageLocation = imageCTR.BrowseImage();
        }

        /// <summary>
        /// This method opens the windows form UpdateSubGroup.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateSubGroup_Click(object sender, EventArgs e)
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
        /// This method opens the windows form CreateGroup.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewSubgroup_Click(object sender, EventArgs e)
        {
            CreateGroup cg = new CreateGroup(cbSubGroup);
            cg.Text = "New Group";
            cg.ShowDialog();
        }

        /// <summary>
        /// This method closes tis windows form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            Dispose();
        }

        /// <summary>
        /// This method changes the fore color on the text box tbWorker to black when it is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbWorker_KeyPress(object sender, KeyPressEventArgs e)
        {
            tbWorker.ForeColor = Color.Black;
            edited = !char.IsControl(e.KeyChar);
        }

        /// <summary>
        /// This method changes the fore color on the text box tbWorker to gray and clears the text box when the keyboard key Enter is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbWorker_Enter(object sender, EventArgs e)
        {
            if (!edited)
            {
                tbWorker.ForeColor = Color.Gray;
                tbWorker.Clear();
            }
        }

        /// <summary>
        /// This method changes the fore color on the text box tbWorker to gray when it is no longer the active text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbWorker_Leave(object sender, EventArgs e)
        {
            if (!edited)
            {
                tbWorker.ForeColor = Color.Gray;
                tbWorker.Text = tbWorkerInitText;
            }
        }

        /// <summary>
        /// This method enables or disables the text box tbWorker when the value in the check box chbJobDone has changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chbJobDone_CheckedChanged(object sender, EventArgs e)
        {
            if (chbJobDone.Checked)
            {
                tbWorker.Enabled = true;
            }

            else
            {
                tbWorker.Enabled = false;
            }
        }
    }
}