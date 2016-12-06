using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Yachtsolution.ControlLayer;
using Yachtsolution.DataLayer;

namespace Yachtsolution.GUILayer
{
    /// <summary>
    /// This is the class UpdateLogItem and is a subclass to the class MyFormPage.
    /// </summary>
    public partial class UpdateLogItem : Form
    {
        private LogBookController logbookCtr;
        private LogBook lb;
        private LogItem logToUpdate;
        private DateTime dateOfReading;

        /// <summary>
        /// This is the constructor for the class UpdateLogItem.
        /// </summary>
        /// <param name="lb"></param>
        /// <param name="logToUpdate"></param>
        /// <param name="date"></param>
        public UpdateLogItem(LogBook lb, LogItem logToUpdate, DateTime date)
        {
            InitializeComponent();
            logbookCtr = LogBookController.GetInstance();
            this.lb = lb;
            this.logToUpdate = logToUpdate;
            dateOfReading = date;
            tbLogItemName.Text = logToUpdate.Name;
            tbUnitOfMeasurement.Text = logToUpdate.UnitOfMeasurement;
            LogReading reading = logbookCtr.FindLogItemReading(logToUpdate.Name, date);
            tbTodaysReading.Text = reading.TodaysReading.ToString();
            if (date < DateTime.Today.AddDays(-7))
            {
                tbTodaysReading.ReadOnly = true;
            }
            rtbItemDescription.Text = logToUpdate.Description;
        }

        /// <summary>
        /// This method closes this windows form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            CloseUpdate();
        }

        /// <summary>
        /// This method updates the log item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string name = tbLogItemName.Text;
            string unitOFMeasurement = tbUnitOfMeasurement.Text;
            string todaysReading = tbTodaysReading.Text;
            string description = rtbItemDescription.Text;
            logbookCtr.UpdateLogItem(logToUpdate.Name, unitOFMeasurement, name, description);
            logbookCtr.UpdateLogItemReading(name, dateOfReading, todaysReading);
        }

        /// <summary>
        /// This method deletes the log item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult answer = MessageBox.Show("Are you sure you want to delete the log item?", "Warning", MessageBoxButtons.YesNo);
            if (answer == DialogResult.Yes)
            {
                if (logbookCtr.DeleteLogItem(logToUpdate.Name))
                {
                    MessageBox.Show("The item was deleted.");
                    CloseUpdate();
                }

                else
                {
                    MessageBox.Show("There was a problem with deleting this log item");
                }
            }
        }

        /// <summary>
        /// This method closes this windows form.
        /// </summary>
        private void CloseUpdate()
        {
            lb.ShowDGVReadings();
            Dispose();
            Close();
        }

        /// <summary>
        /// This method checks on key press whether or not the input for tbTodaysReading contains letters or more than one comma.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbTodaysReading_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(tbTodaysReading.Text, @","))
            {
                if ((e.KeyChar >= 48 && e.KeyChar <= 57) || e.KeyChar == 08 || e.KeyChar == 44)
                {
                    return;
                }
            }

            else
            {
                if ((e.KeyChar >= 48 && e.KeyChar <= 57) || e.KeyChar == 08)
                {
                    return;
                }
            }

            e.Handled = true;
        }
    }
}