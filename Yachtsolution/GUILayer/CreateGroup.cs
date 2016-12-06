using System;
using System.Linq;
using System.Windows.Forms;
using Yachtsolution.ControlLayer;

namespace Yachtsolution.GUILayer
{
    /// <summary>
    /// This is the class CreateGroup and is a subclass to the class Form.
    /// </summary>
    public partial class CreateGroup : Form
    {
        private JobController jobCtr;
        private ComboBox cb;

        /// <summary>
        /// This is the constructor for the class CreateGroup.
        /// </summary>
        /// <param name="cb"></param>
        public CreateGroup(ComboBox cb)
        {
            InitializeComponent();
            this.cb = cb;
            jobCtr = JobController.GetInstance();
        }

        /// <summary>
        /// This method creates a sub group.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveGroup_Click(object sender, EventArgs e)
        {
            string message = "";
            string result = jobCtr.InsertSubGroup(tbName.Text);
            switch (result)
            {
                case "emptyName":
                    message = "Please enter a name in the name field!";
                    break;
                case "usedName":
                    message = "A subgroup already exists with that name!";
                    break;
                case "success":
                    message = "Sub Group saved";
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
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }
    }
}