using System;
using System.Windows.Forms;
using Yachtsolution.ControlLayer;

namespace Yachtsolution.GUILayer
{
    /// <summary>
    /// This is the class CreateLogItem and is a subclass to the class Form.
    /// </summary>
    public partial class CreateLogItem : Form
    {
        private LogBookController logbookCtr;
        private LogBook logbook;
        //private Button btnRegisterAll;

        /// <summary>
        /// This is the constructor for the class CreateLogItem.
        /// </summary>
        /// <param name="lb"></param>
        public CreateLogItem(LogBook logbook)
        {
            InitializeComponent();
            logbookCtr = LogBookController.GetInstance();
            this.logbook = logbook;
        }

        /// <summary>
        /// This method creates a log item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRegister_Click(object sender, EventArgs e)
        {
            string logItemName = tbLogItemName.Text;
            string unitOfMeasurement = tbUnitOfMeasurement.Text;
            string description = rtbDescription.Text;

            string result = logbookCtr.CreateLogItem(logItemName, unitOfMeasurement, description);
            string message = "";
            switch (result)
            {
                case "emptyName":
                    message = "Please enter a valid item name in the name field no longer than 255 characters long!";
                    break;
                case "emptyUnit":
                    message = "Please enter a valid unit of measurement in the unit of measurement field no longer than 255 characters long!";
                    break;
                case "usedName":
                    message = "A log item allready exists with that name!";
                    break;
                case "success":
                    message = "Log item saved successfully";
                    CloseCreate();
                    break;
            }
            MessageBox.Show(message);
                    logbook.ShowDGVReadings();
        }

        /// <summary>
        /// This method calls the method CloseCreate.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            CloseCreate();
        }

        /// <summary>
        /// This method Closes this windows form.
        /// </summary>
        private void CloseCreate()
        {
            logbook.ShowDGVReadings();
            Close();
            Dispose();
        }
    }
}