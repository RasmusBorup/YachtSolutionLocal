using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Yachtsolution.ControlLayer;
using Yachtsolution.DataLayer;

namespace Yachtsolution.GUILayer
{
    /// <summary>
    /// This is the class UpdateInventory and is a subclass to the class MyFormPage.
    /// </summary>
    public partial class UpdateInventory : Form
    {
        private InventoryController inventoryCtr;
        private InventoryManagement inv;
        private ImageController imageCTR;

        /// <summary>
        /// This is the constructor for the class UpdateInventory.
        /// </summary>
        /// <param name="inv"></param>
        /// <param name="inventoryToUpdate"></param>
        public UpdateInventory(InventoryManagement inv, Inventory inventoryToUpdate)
        {
            this.inv = inv;
            inventoryCtr = InventoryController.GetInstance();
            imageCTR = ImageController.GetInstance();
            InitializeComponent();

            this.Name = "Inventory handler";
            this.ResumeLayout(false);
            tbName.Text = inventoryToUpdate.name;
            numericUpDownAmount.Value = inventoryToUpdate.amount;
            tbSerialNr.Text = inventoryToUpdate.serialNo;
            tbPrice.Text += inventoryToUpdate.price;
            numericUpDownMinAmount.Value = (decimal)inventoryToUpdate.minimumAmount;
            tbLocation.Text = inventoryToUpdate.location;
            tbManufacturer.Text = inventoryToUpdate.manufacturer;
            rtbDescription.Text = inventoryToUpdate.description;
            tbPartFor.Text = inventoryToUpdate.partFor;
            rtbSuppliers.Text = inventoryToUpdate.suppliers;
            if (inventoryToUpdate.photo != null)
            {
                MemoryStream stream = new MemoryStream(inventoryToUpdate.photo);
                pbPhoto.Image = Image.FromStream(stream);
            }
        }

        /// <summary>
        /// This method creates an item in the inventory.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveUpdateItem_Click(object sender, EventArgs e)
        {
            string name = tbName.Text;
            int amount = (int)numericUpDownAmount.Value;
            string serial = tbSerialNr.Text;
            string description = rtbDescription.Text;
            string price = tbPrice.Text;
            int minimumAmount = (int)numericUpDownMinAmount.Value;
            string location = tbLocation.Text;
            string manufacturer = tbManufacturer.Text;
            string imageLocation = "";
            string partFor = tbPartFor.Text;
            string suppliers = rtbSuppliers.Text;

            string message = "";
            string result = inventoryCtr.UpdateItemBySerialNr(description, amount, location, manufacturer, minimumAmount, name, price, serial, imageLocation, partFor, suppliers);
            switch (result)
            {
                case "emptyName":
                    message = "Please enter a name in the name field!";
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
        /// This method returns the path of the image.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowseImage_Click(object sender, EventArgs e)
        {
            pbPhoto.ImageLocation = imageCTR.BrowseImage();
        }

        /// <summary>
        /// This method closes this windows form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateItemCancel_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }

        /// <summary>
        /// This method checks on key press whether or not the input for name contains letters or more than one comma.
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
        /// This method call the NumberChecker method when there is a key press on the text box tbPrice.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            NumberChecker(tbPrice, e);
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