using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Yachtsolution.ControlLayer;

namespace Yachtsolution.GUILayer
{
    /// <summary>
    /// This is the class CreateInventory and is a subclass to the class Form.
    /// </summary>
    public partial class CreateInventory : Form
    {
        private InventoryController inventoryCtr;
        private ImageController imageCTR;
        private InventoryManagement inv;
        private string role;

        /// <summary>
        /// This is the constructor for the class CreateInventory.
        /// </summary>
        /// <param name="inv"></param>
        public CreateInventory(InventoryManagement inv, string role)
        {
            this.inv = inv;
            this.role = role;
            inventoryCtr = InventoryController.GetInstance();
            imageCTR = ImageController.GetInstance();
            InitializeComponent();
        }

        /// <summary>
        /// This method creates an inventory.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateItem_Click(object sender, EventArgs e)
        {
            string message = "";
            string name = tbName.Text;
            int amount = (int)numAmount.Value;
            string serial = tbSerialNr.Text;
            string description = rtbDescription.Text;
            string price = tbPrice.Text;
            int minimumAmount = (int)numMinAmount.Value;
            string location = tbLocation.Text;
            string manufacturer = tbManufacturer.Text;
            string imageLocation = "";
            string partFor = tbPartFor.Text;
            string suppliers = rtbSuppliers.Text;

            string result = inventoryCtr.InsertItem(description, amount, location, manufacturer, minimumAmount, name, price, serial, imageLocation, partFor, suppliers, role);
            switch (result)
            {
                case "emptyName":
                    message = "Please enter a name in the name field!";
                    break;
                case "emptySerial":
                    message = "Please enter a serial number in the serial number field!";
                    break;
                case "usedSerial":
                    message = "An item has already been registered with that serial number!";
                    break;
                case "emptyLocation":
                    message = "Please enter a location in the location field!";
                    break;
                case "emptyManufacturer":
                    message = "Please enter a manufacturer in the manufacturer field!";
                    break;
                case "emptyPrice":
                    message = "Please enter a valid price in the price field!";
                    break;
                case "success":
                    message = "Item saved successfully";
                    this.Dispose();
                    this.Close();
                    inv.RefreshDGV();
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

        /// <summary>
        /// This method returns the path to the image.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowseImage_Click(object sender, EventArgs e)
        {
            pbPhoto.ImageLocation = imageCTR.BrowseImage();
        }

        /// <summary>
        /// This method call the NumberChecker method when there is a key press on the text box tbPrice.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            NumberChecker(tbPrice, e);
        }

        /// <summary>
        /// This method checks on key press whether or not the input for tbNewValue contains letters or more than one comma.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="e"></param>
        private static void NumberChecker(Control name, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(name.Text, @","))
            {
                if ((e.KeyChar >= 48 && e.KeyChar <= 57) || e.KeyChar == 08 || e.KeyChar == 44 || e.KeyChar == 45)
                {
                    return;
                }
            }

            else
            {
                if ((e.KeyChar >= 48 && e.KeyChar <= 57) || e.KeyChar == 08 || e.KeyChar == 45)
                {
                    return;
                }
            }

            e.Handled = true;
        }

        /// <summary>
        /// This method call the NumberChecker method when there is a key press on the text box tbSerialNr.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbSerialNr_KeyPress(object sender, KeyPressEventArgs e)
        {
            NumberChecker(tbSerialNr, e);
        }
    }
}