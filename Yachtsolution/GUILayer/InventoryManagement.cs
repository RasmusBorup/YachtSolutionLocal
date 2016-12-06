using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Yachtsolution.ControlLayer;
using Yachtsolution.DataLayer;

namespace Yachtsolution.GUILayer
{
    /// <summary>
    /// This is the class InventoryManagement and is a subclass to the class Form.
    /// </summary>
    public partial class InventoryManagement : MyFormPage
    {
        private InventoryController inventoryCtr;
        private List<Inventory> inventories;
        private string role;

        /// <summary>
        /// This is the constructor for the class InventoryManagement.
        /// </summary>
        public InventoryManagement(string role)
        {
            InitializeComponent();
            this.panel = PanelMasterPanel;

            inventoryCtr = InventoryController.GetInstance();
            inventories = inventoryCtr.GetAllInventories();
            this.role = role;

            dgvDataAllItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgvDataAllItems.DataSource = inventoryCtr.GetInventoriesByRole(role);
            dgvDataAllItems.Columns[0].HeaderText = @"name";
            dgvDataAllItems.Columns[1].HeaderText = @"Amount";
            dgvDataAllItems.Columns[2].HeaderText = @"Note";
            dgvDataAllItems.Columns[3].HeaderText = @"Price";
            dgvDataAllItems.Columns[4].HeaderText = @"Minimum Amount";
            dgvDataAllItems.Columns[5].HeaderText = @"Location";
            dgvDataAllItems.Columns[6].HeaderText = @"Manufacturer";
            dgvDataAllItems.Columns[7].HeaderText = @"Serial No.";
            dgvDataAllItems.Columns[8].Visible = false;
            dgvDataAllItems.Columns[9].HeaderText = @"Used For";
            dgvDataAllItems.Columns[10].HeaderText = @"Suppliers";
            dgvDataAllItems.Columns[11].Visible = false;

            dgvDataAllItems.Columns[0].FillWeight = 17;
            dgvDataAllItems.Columns[1].FillWeight = 11;
            dgvDataAllItems.Columns[2].FillWeight = 15;
            dgvDataAllItems.Columns[3].FillWeight = 12;
            dgvDataAllItems.Columns[4].FillWeight = 10;
            dgvDataAllItems.Columns[5].FillWeight = 15;
            dgvDataAllItems.Columns[6].FillWeight = 16;
            dgvDataAllItems.Columns[7].FillWeight = 13;
            dgvDataAllItems.Columns[9].FillWeight = 15;
            dgvDataAllItems.Columns[10].FillWeight = 16;
        }

        /// <summary>
        /// This method opens the class updateInventory and send the marked item to the class.
        /// </summary>
        private void updateItem()
        {
            var updateCheck = dgvDataAllItems.SelectedRows.Count;

            if (updateCheck != 0)
            {
                Inventory inventoryToUpdate = dgvDataAllItems.SelectedRows[0].DataBoundItem as Inventory;
                Form updateInventory = new UpdateInventory(this, inventoryToUpdate);
                updateInventory.ShowDialog();
            }
        }

        /// <summary>
        /// This method opens the windows form CreateInventory.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateItem_Click(object sender, EventArgs e)
        {
            Form CreateInventory = new CreateInventory(this, role);
            CreateInventory.ShowDialog();
        }

        /// <summary>
        /// This method deletes an item in the inventory by serial.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteBySerial_Click(object sender, EventArgs e)
        {
            if (dgvDataAllItems.SelectedRows.Count == 1)
            {
                DialogResult answer = MessageBox.Show(@"Are you sure you want to delete the item?", @"Delete?", MessageBoxButtons.YesNo);
                if (answer == DialogResult.Yes)
                {
                    Inventory itemToDelete = dgvDataAllItems.SelectedRows[0].DataBoundItem as Inventory;
                    string result = inventoryCtr.DeleteItemBySerialNr(itemToDelete.serialNo);
                    string message = "";
                    switch (result)
                    {
                        case "fail":
                            message = "Something unexpected hapened and prevented the delete operation from succeeding!";
                            break;
                        case "success":
                            message = "The item was deleted!";
                            break;
                    }
                    MessageBox.Show(message);
                }
            }
            RefreshDGV();
        }

        /// <summary>
        /// This method refreshes the data grid view dgvDataAllItems.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewAllItems_Click(object sender, EventArgs e)
        {
            RefreshDGV();
        }

        /// <summary>
        /// This method resets the list dgvDataAllItems.
        /// </summary>
        public void RefreshDGV()
        {
            dgvDataAllItems.DataSource = inventoryCtr.GetAllInventories();
            inventories = inventoryCtr.GetAllInventories();
        }

        /// <summary>
        /// This method opens the class UpdateItem when one of the items in the data grid view dgvDataAllItems is double clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            updateItem();
        }

        /// <summary>
        /// This method searches after items while the user types in the search box.
        /// </summary>
        private void SearchWhileTyping()
        {
            string search = tbSearchField.Text;
            List<Inventory> result = inventories.Where(i => i.name.ToLower().Contains(search) || i.serialNo.ToLower().Contains(search) || i.location.ToLower().Contains(search) || i.manufacturer.ToLower().Contains(search) || i.suppliers.ToLower().Contains(search) || i.suppliers.ToLower().Contains(search)).ToList();
            dgvDataAllItems.DataSource = result;
        }

        /// <summary>
        /// This method activates when the text in the text box tbSearchField changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbSearchField_TextChanged(object sender, EventArgs e)
        {
            SearchWhileTyping();
        }

        /// <summary>
        /// This method checks if the items meets the minimum amount of items.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMinimumAmountCheck_Click(object sender, EventArgs e)
        {
            if (inventoryCtr.findAllItemsNotMinim() == null)
            {
                MessageBox.Show(@"All items meet the minimum requirement");
            }

            else
            {
                dgvDataAllItems.DataSource = inventoryCtr.findAllItemsNotMinim();
            }
        }

        /// <summary>
        /// This method paints the rows if the amount is equal or under the minimum amount.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDataAllItems_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            foreach (DataGridViewRow row in dgvDataAllItems.Rows)
            {
                Inventory item = row.DataBoundItem as Inventory;
                if (item.amount < item.minimumAmount)
                {
                    row.DefaultCellStyle.BackColor = Color.Red;
                }

                if (item.amount == item.minimumAmount)
                {
                    row.DefaultCellStyle.BackColor = Color.Yellow;
                }
            }
        }

        /// <summary>
        /// This method calls the updateItem method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateItem_Click(object sender, EventArgs e)
        {
            updateItem();
        }
    }
}