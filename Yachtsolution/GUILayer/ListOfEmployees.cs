using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Yachtsolution.ControlLayer;
using Yachtsolution.DataLayer;

namespace Yachtsolution.GUILayer
{
    /// <summary>
    /// This is the class ListOfEmployees and is a subclass to the class MyFormPage.
    /// </summary>
    public partial class ListOfEmployees : MyFormPage
    {
        private EmployeeController employeeCtr;
        private List<Employee> employees; 

        /// <summary>
        /// This is the constructor for the class ListOfEmployees.
        /// </summary>
        public ListOfEmployees()
        {
            InitializeComponent();
            this.panel = panelEmployees;
            employeeCtr = EmployeeController.GetInstance();
            employees = employeeCtr.ListAllEmployees();

            ShowEmployees(employeeCtr.ListAllEmployees());
        }

        /// <summary>
        /// This method handle the HeaderText to the columns in the list dgvEmployees.
        /// </summary>
        /// <param name="employees"></param>
        private void ShowEmployees(List<Employee> employees)
        {
            dgvEmployees.DataSource = employees;
            dgvEmployees.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEmployees.Columns[0].HeaderText = @"Employee ID";
            dgvEmployees.Columns[1].HeaderText = @"name";
            dgvEmployees.Columns[2].HeaderText = @"Job Title";
            dgvEmployees.Columns[3].HeaderText = @"Email";
            dgvEmployees.Columns[4].HeaderText = @"Phone";
            dgvEmployees.Columns[5].HeaderText = @"SSN";
            dgvEmployees.Columns[6].HeaderText = @"Salary";
            dgvEmployees.Columns[8].HeaderText = @"User name";


            dgvEmployees.Columns[7].Visible = false;
            dgvEmployees.Columns[9].Visible = false;

        }

        /// <summary>
        /// This method opens the windows form CreateCrewMember.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateEmployee_Click(object sender, EventArgs e)
        {
            CreateCrewMember createCrewMember = new CreateCrewMember(this);
            createCrewMember.ShowDialog();
        }

        /// <summary>
        /// this method opens the windows form employeeToUpdate and sends the selected employee it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Employee employeeToUpdate = dgvEmployees.SelectedRows[0].DataBoundItem as Employee;
            UpdateEmployee updateEmp = new UpdateEmployee(employeeToUpdate, this);
            updateEmp.ShowDialog();
        }

        private void UpdateEmployee()
        {
            Employee employeeToUpdate = dgvEmployees.SelectedRows[0].DataBoundItem as Employee;
            UpdateEmployee updateEmp = new UpdateEmployee(employeeToUpdate, this);
            updateEmp.ShowDialog();
        }

        /// <summary>
        /// This method call the RefreshTable method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefreshEmployees_Click(object sender, EventArgs e)
        {
            RefreshTable();
        }

        /// <summary>
        /// This method shows all the employees in the database.
        /// </summary>
        public void RefreshTable()
        {
            ShowEmployees(employeeCtr.ListAllEmployees());
            tbSearchField.Text = "";
        }

        /// <summary>
        /// This method searches after employees while typing in the search box.
        /// </summary>
        public void SearchWhileTyping()
        {
            string search = tbSearchField.Text;
            List<Employee> result = employees.Where(i => i.Name.ToLower().Contains(search.ToLower())|| i.Title.ToLower().Contains(search.ToLower()) || i.Ssn.ToLower().Contains(search.ToLower()) || i.Email.ToLower().Contains(search.ToLower())).ToList();

            dgvEmployees.DataSource = result;
        }

        /// <summary>
        /// This method calls the method SearchWhileTyping when the value in the text box tbSearchField is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbSearchField_TextChanged(object sender, EventArgs e)
        {
            SearchWhileTyping();
        }

        /// <summary>
        /// This method calls the method UpdateEmployee when there is double clicked on a cell in the data grid view dgvEmployees.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvEmployees_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            UpdateEmployee();
        }
    }
}