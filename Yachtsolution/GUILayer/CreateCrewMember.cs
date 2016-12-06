using System;
using System.Windows.Forms;
using Yachtsolution.ControlLayer;

namespace Yachtsolution.GUILayer
{
    /// <summary>
    /// This is the class CreateCrewMember and is a subclass to the class Form.
    /// </summary>
    public partial class CreateCrewMember : Form
    {
        private EmployeeController employeeCtr;
        private SettingsController settingsCTR;
        private ImageController imageCTR;
        private ListOfEmployees list;

        /// <summary>
        /// This is the constructor for the class CreateCrewMember.
        /// </summary>
        public CreateCrewMember(ListOfEmployees list)
        {
            InitializeComponent();
            employeeCtr = EmployeeController.GetInstance();
            settingsCTR = SettingsController.GetInstance();
            imageCTR = ImageController.GetInstance();
            this.list = list;
            cbJobRole.DataSource = settingsCTR.GetRoles();
        }

        /// <summary>
        /// This method creates an employee.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRegister_Click(object sender, EventArgs e)
        {
            string name = tbName.Text;
            string jobEmail = tbEmail.Text;
            string jobPhone = tbPhone.Text;
            string ssn = tbSSN.Text;
            string salary = tbSalary.Text;
            string userName = tbUserName.Text;
            string passWord = tbPassWord.Text;
            string jobTitle = "";
            string imageLocation = "";
            if (cbJobRole.SelectedItem != null)
            {
                jobTitle = cbJobRole.SelectedItem.ToString();
            }
            string result = employeeCtr.CreateEmployee(name, jobTitle, jobEmail, jobPhone, ssn, salary, imageLocation, userName, passWord);
            string message = "";
            switch (result)
            {
                case "emptyName":
                    message = "Please enter a name in the name field!";
                    break;
                case "emptyTitle":
                    message = "Please select a role from the dropdown!";
                    break;
                case "emptyMail":
                    message = "Please enter a valid email address in the email field!";
                    break;
                case "emptyPhone":
                    message = "Please enter a phone number in the phone number field!";
                    break;
                case "emptySSN":
                    message = "Please enter an ssn number in the snn field!";
                    break;
                case "emptySalary":
                    message = "Please enter a valid salary in the salary field!";
                    break;
                case "emptyUsername":
                    message = "Please enter a username in the username field!";
                    break;
                case "emptyPassword":
                    message = "Please enter a password in the password field!";
                    break;
                case "success":
                    message = "Employee registered successfully!";
                    Close();
                    list.RefreshTable();
                    break;
            }

            MessageBox.Show(message);
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
        /// This method closes this windows form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}