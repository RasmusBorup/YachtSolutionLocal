using System;
using System.Windows.Forms;
using Yachtsolution.ControlLayer;

namespace Yachtsolution.GUILayer
{
    /// <summary>
    /// This is the class UpdateSubGroup and is a subclass to the class Form.
    /// </summary>
    public partial class UpdateSubGroup : Form
    {
        private JobController jobCtr;
        private ComboBox cb;
        private string groupName;

        /// <summary>
        /// This is the constructor for the class UpdateSubGroup.
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="cb"></param>
        public UpdateSubGroup(string groupName, ComboBox cb)
        {
            InitializeComponent();
            jobCtr = JobController.GetInstance();
            this.cb = cb;
            this.groupName = groupName;
            tbName.Text = jobCtr.FindSubGroupByName(groupName).Name;
        }

        /// <summary>
        /// This method deletes a subgroup.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult answer = MessageBox.Show("Are you sure you want to delete the Sub Group?", "Warning",
                MessageBoxButtons.YesNo);
            if (answer == DialogResult.Yes)
            {
                if (jobCtr.DeleteSubGroup(groupName))
                {
                    MessageBox.Show("Sub Group Deleted");
                    cb.DataSource = jobCtr.GetAllSubGroups();
                    cb.DisplayMember = "name";
                    Close();
                    Dispose();
                }

                else
                {
                    MessageBox.Show("Something went wrong");
                }
            }
        }

        /// <summary>
        /// This method updates a subgroup.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (tbName.Text != "")
            {
                if (jobCtr.UpdateSubGroup(jobCtr.FindSubGroupByName(groupName).Name, tbName.Text))
                {
                    MessageBox.Show("Subgroup updated");
                    groupName = tbName.Text;
                    cb.DataSource = jobCtr.GetAllSubGroups();
                    cb.Text = tbName.Text;
                    Close();
                    Dispose();
                }

                else
                {
                    MessageBox.Show("Subgroup Couldn't be updated");
                }
            }
        }

        /// <summary>
        /// This method closes this windows form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }
    }
}