using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Yachtsolution.ControlLayer;
using Yachtsolution.DataLayer;

namespace Yachtsolution.GUILayer
{
    /// <summary>
    /// This is the class UpdateEmployee and is a subclass to the class MyFormPage.
    /// </summary>
    public partial class UpdateEmployee : Form
    {
        private Employee employeeToUpdate;
        private EmployeeController employeeCtr;
        private SettingsController settingsCTR;
        private ImageController imageCTR;
        private ListOfEmployees list;

        /// <summary>
        /// This is the constructor for the class UpdateEmployee.
        /// </summary>
        /// <param name="employeeToUpdate"></param>
        /// <param name="list"></param>
        public UpdateEmployee(Employee employeeToUpdate, ListOfEmployees list)
        {
            InitializeComponent();
            employeeCtr = EmployeeController.GetInstance();
            this.employeeToUpdate = employeeToUpdate;
            settingsCTR = SettingsController.GetInstance();
            imageCTR = ImageController.GetInstance();
            cbJobRole.DataSource = settingsCTR.GetRoles();
            tbName.Text = employeeToUpdate.Name;
            cbJobRole.Text = employeeToUpdate.Title;
            tbEmail.Text = employeeToUpdate.Email;
            tbPhone.Text = employeeToUpdate.Phone;
            tbSSN.Text = employeeToUpdate.Ssn;
            tbSalary.Text = employeeToUpdate.Salary.ToString();

            if(employeeToUpdate.Picture != null)
            {
                pbPhoto.Image = Image.FromStream(new MemoryStream(employeeToUpdate.Picture));
            }
            tbUserName.Text = employeeToUpdate.Username;
            tbPassWord.Text = employeeToUpdate.Password;
            this.list = list;
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
        /// This method updates an employee.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string name = tbName.Text;
            string jobTitle = cbJobRole.SelectedItem.ToString();
            string jobEmail = tbEmail.Text;
            string jobPhone = tbPhone.Text;
            string ssn = tbSSN.Text;
            string salary = tbSalary.Text;
            string userName = tbUserName.Text;
            string passWord = tbPassWord.Text;
            string picture = pbPhoto.ImageLocation;
            string result = employeeCtr.UpdateEmployeeByID(employeeToUpdate.EmpID, name, jobTitle, jobEmail, jobPhone, ssn, salary, picture, userName, passWord);
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
                    message = "Employee updated successfully!";
                    Close();
                    list.RefreshTable();
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
        }

        /// <summary>
        /// This method deletes the employee.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult answer = MessageBox.Show("Are you sure you would like to delete this employee from the system?", "Warning", MessageBoxButtons.YesNo);
            if (answer == DialogResult.Yes)
            {
                string result = employeeCtr.DeleteEmployee(employeeToUpdate.EmpID);
                string message = "";
                switch (result)
                {
                    case "fail":
                        message = "Something unexpected prevented the delete operation from succeeding!";
                        break;
                    case "success":
                        message = "The employee was deleted!";
                        break;
                }
                MessageBox.Show(message);
                list.RefreshTable();
                Close();
                Dispose();
            }
        }
    }
}