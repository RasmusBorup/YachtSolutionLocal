using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Yachtsolution.ControlLayer;
using Yachtsolution.DataLayer;

namespace Yachtsolution.GUILayer
{
    /// <summary>
    /// This is the class LogBook and is a subclass to the class MyFormPage.
    /// </summary>
    public partial class LogBook : MyFormPage
    {
        private LogBookController logbookCtr;

        //Used for PrintPage for values that must not be reset
        private int iRow;
        private bool nPage = true;
        private bool fPage = true;
        private StringFormat strFormat = new StringFormat();
        private List<int> arrColumnLefts = new List<int>();
        private List<int> arrColumnWidths = new List<int>();

        /// <summary>
        /// This is the constructor for the class LogBook.
        /// </summary>
        public LogBook()
        {
            InitializeComponent();
            logbookCtr = LogBookController.GetInstance();
            panel = pnlLogBook;

            logbookCtr.CreateLogBook(DateTime.Today, "", "", "");
            CheckForMissingLogBooks();

            try
            {
                dtpLogBookDate.MinDate = logbookCtr.GetAllLogBooks().OrderBy(l => l.Date).First().Date;
                dtpLogBookDate.MaxDate = logbookCtr.GetAllLogBooks().OrderBy(l => l.Date).Last().Date;
                dtpLogBookDate.Value = DateTime.Today;
                rtbDescription.Text = logbookCtr.FindLogBook(DateTime.Today).Description;
                rtbRemarks.Text = logbookCtr.FindLogBook(dtpLogBookDate.Value.Date).Remarks;
                cbChief.Text = logbookCtr.FindLogBook(dtpLogBookDate.Value.Date).ChiefEngineer;
            }
            catch (Exception)
            {

            }

            Text = "Log Book";
            ShowDGVReadings();
        }

        private void CheckForMissingLogBooks()
        {
            if (logbookCtr.GetAllLogBooks().Count > 1)
            {
                bool notDone = true;
                int i = 1;
                while (notDone && i < 365)
                {
                    if (logbookCtr.FindLogBook(DateTime.Today.AddDays(-i)) == null)
                    {
                        logbookCtr.CreateLogBook(DateTime.Today.AddDays(-i), "", "", "");
                    }
                    else
                    {
                        notDone = false;
                    }
                    i++;
                }
            }
        }

        /// <summary>
        /// This method creates the columns in the data grid view readings.
        /// </summary>
        public void ShowDGVReadings()
        {
            DataTable readings = new DataTable();
            readings.Columns.Add("Item", typeof(string));
            readings.Columns.Add("Unit", typeof(string));
            readings.Columns.Add("Reading", typeof(double));

            List<LogReading> readingsList = logbookCtr.FindLogBook(dtpLogBookDate.Value.Date).Readings;

            foreach (LogReading reading in readingsList)
            {
                Object[] o = { reading.LogItem1.Name, reading.LogItem1.UnitOfMeasurement, reading.TodaysReading };
                readings.Rows.Add(o);
            }
            dgvLogItems.DataSource = readings;

            dgvLogItems.Columns[0].ReadOnly = true;
            dgvLogItems.Columns[0].FillWeight = 45;
            dgvLogItems.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvLogItems.Columns[1].ReadOnly = true;
            dgvLogItems.Columns[1].FillWeight = 15;
            dgvLogItems.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            if (dtpLogBookDate.Value.Date < DateTime.Today.AddDays(-7))
            {
                dgvLogItems.Columns[2].ReadOnly = true;
            }
            else
            {
                dgvLogItems.Columns[2].ReadOnly = false;
            }
            dgvLogItems.Columns[2].FillWeight = 15;
            dgvLogItems.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        /// <summary>
        /// This method updates the log item reading and the logbook.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            logbookCtr.UpdateLogReadingsFromDGV(dgvLogItems.Rows, dtpLogBookDate.Value);

            string result = logbookCtr.UpdateLogBook(dtpLogBookDate.Value.Date, rtbDescription.Text, rtbRemarks.Text, cbChief.Text);
            string message = "";
        }

        /// <summary>
        /// This method opens the windows form CreateLogItem.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRegisterLogItem_Click(object sender, EventArgs e)
        {
            CreateLogItem cli = new CreateLogItem(this);
            cli.ShowDialog();
        }

        /// <summary>
        /// This method opens the windows form UpdateLogItem.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDetails_Click(object sender, EventArgs e)
        {
            ShowDetails();
        }

        /// <summary>
        /// This method shows the details of a logbook.
        /// </summary>
        private void ShowDetails()
        {
            try
            {
                if (dgvLogItems.RowCount > 0 || dgvLogItems.SelectedRows[0] != null)
                {
                    LogReading readingToUpdate =
                        dgvLogItems.SelectedCells[0].OwningRow.DataBoundItem as LogReading;
                    readingToUpdate =
                        logbookCtr.FindLogItemReading(dgvLogItems.SelectedCells[0].OwningRow.Cells[0].Value.ToString(),
                    dtpLogBookDate.Value.Date);
                    LogItem logToUpdate = logbookCtr.FindLogItem(readingToUpdate.LogItem1.Name);
                    UpdateLogItem update = new UpdateLogItem(this, logToUpdate, dtpLogBookDate.Value.Date);
                    update.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Details can not be shown when nothing is selected");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error: " + exception.Message);
            }
        }

        /// <summary>
        /// This method deletes a log item reading.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvLogItems.RowCount != 0)
            {
                string itemName = dgvLogItems.SelectedCells[0].OwningRow.Cells[0].Value.ToString();
                DialogResult answer = MessageBox.Show("Are you sure you want to delete the log item?", "Warning", MessageBoxButtons.YesNo);

                if (answer == DialogResult.Yes)
                {
                    if (logbookCtr.DeleteLogItem(itemName))
                    {
                        MessageBox.Show("The item was deleted.");
                        ShowDGVReadings();
                    }
                    else
                    {
                        MessageBox.Show("There was a problem with deleting this log item");
                    }
                }
            }
        }

        /// <summary>
        /// This method sets the DateTimePicker dtpLogBookDate one day back.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPreviousDate_Click(object sender, EventArgs e)
        {
            if (dtpLogBookDate.MinDate == dtpLogBookDate.MaxDate)
            {
                dtpLogBookDate.MinDate = logbookCtr.GetAllLogBooks().OrderBy(l => l.Date).First().Date;
            }
            if (dtpLogBookDate.Value.AddDays(-1) >= dtpLogBookDate.MinDate)
            {
                dtpLogBookDate.Value = dtpLogBookDate.Value.AddDays(-1);
            }
            else
            {
                MessageBox.Show("There is no logbook for this Date yet");
            }
        }

        /// <summary>
        /// This method sets the DateTimePicker dtpLogBookDate one day forward.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNextDate_Click(object sender, EventArgs e)
        {
            if (dtpLogBookDate.Value.AddDays(1) <= dtpLogBookDate.MaxDate)
            {
                dtpLogBookDate.Value = dtpLogBookDate.Value.AddDays(1);
            }
            else
            {
                MessageBox.Show("There is no logbook for this Date yet");
            }
        }

        /// <summary>
        /// This method finds a logbook by its Date.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (logbookCtr.FindLogBook(dtpLogBookDate.Value.Date) != null)
            {
                ShowDGVReadings();
                rtbDescription.Text = logbookCtr.FindLogBook(dtpLogBookDate.Value.Date).Description;
                rtbRemarks.Text = logbookCtr.FindLogBook(dtpLogBookDate.Value.Date).Remarks;
                cbChief.Text = logbookCtr.FindLogBook(dtpLogBookDate.Value.Date).ChiefEngineer;
            }
            else
            {
                dtpLogBookDate.Value = DateTime.Today;
                MessageBox.Show("There is no logbook for this Date");
            }
        }

        /// <summary>
        /// This method ???.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvLogItems_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridViewTextBoxEditingControl tb = (DataGridViewTextBoxEditingControl)e.Control;
            tb.KeyPress += dataGridViewTextBox_KeyPress;
            e.Control.KeyPress += dataGridViewTextBox_KeyPress;
        }

        /// <summary>
        /// This method checks on key press whether or not the input for dgvLogItems contains letters or more than one comma.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(dgvLogItems.SelectedCells[0].Value.ToString(), @","))
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

        /// <summary>
        /// This method checks if the current cell contains changes that aren't saved.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvLogItems_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvLogItems.IsCurrentCellDirty)
            {
                dgvLogItems.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvLogItems_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowDetails();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            iRow = 0;
            fPage = true;
            nPage = true;

            //Open the print dialog
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDocument1;
            printDialog.UseEXDialog = true;

            //Get the document
            if (DialogResult.OK == printDialog.ShowDialog())
            {
                printDocument1.DocumentName = "Test Page Print";
                printDocument1.Print();
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            iRow = 0;
            fPage = true;
            nPage = true;

            //Open the print preview dialog
            PrintPreviewDialog objPPdialog = new PrintPreviewDialog();
            objPPdialog.Document = printDocument1;
            objPPdialog.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            bool bMorePagesToPrint = false;

            strFormat.Alignment = StringAlignment.Near;
            strFormat.LineAlignment = StringAlignment.Center;
            strFormat.Trimming = StringTrimming.EllipsisCharacter;

            int count = 0;
            int check;
            int leftMargin = e.MarginBounds.Left;
            int topMargin = e.MarginBounds.Top;
            int tmpWidth = 0;
            int totalWidth = 0;
            int headerHeight = 0;

            foreach (DataGridViewColumn dgvGridCol in dgvLogItems.Columns)
            {
                if (dgvLogItems.Columns[count].Visible)
                {
                    totalWidth += dgvGridCol.Width;
                }
                count++;
            }
            count = 0;
            ///////////////////////////////////////////////////////////////////////////////

            //For the first page to print set the cell width and header height
            if (fPage)
            {
                foreach (DataGridViewColumn GridCol in dgvLogItems.Columns)
                {
                    if (dgvLogItems.Columns[count].Visible)
                    {
                        tmpWidth = (int)(Math.Floor(((double)GridCol.Width /
                        totalWidth * totalWidth * (e.MarginBounds.Width / totalWidth))));

                        headerHeight = (int)(e.Graphics.MeasureString(GridCol.HeaderText,
                            GridCol.InheritedStyle.Font, tmpWidth).Height) + 11;

                        // Save width and height of headers
                        arrColumnLefts.Add(leftMargin);
                        arrColumnWidths.Add(tmpWidth);
                        leftMargin += tmpWidth;
                    }
                    count++;
                }
            }

            //////////////////////////////////////////////////////////////////////////////////////////////
            while (iRow <= dgvLogItems.RowCount - 1 && bMorePagesToPrint == false)
            {
                DataGridViewRow GridRow = dgvLogItems.Rows[iRow];

                //Set the cell height
                int cellHeight = GridRow.Height + 5;

                count = 0;
                if (topMargin + cellHeight >= e.MarginBounds.Height + e.MarginBounds.Top)
                {
                    nPage = true;
                    fPage = false;
                    bMorePagesToPrint = true;
                }
                else
                {
                    if (nPage)
                    {
                        //Draw Header

                        e.Graphics.DrawString("Log book Summary",
                            new Font(dgvLogItems.Font, FontStyle.Bold),
                            Brushes.Black, e.MarginBounds.Left,
                            e.MarginBounds.Top - e.Graphics.MeasureString("Log book Summary",
                            new Font(dgvLogItems.Font, FontStyle.Bold),
                            e.MarginBounds.Width).Height - 13);

                        String strDate = DateTime.Now.ToLongDateString() + " " +
                            DateTime.Now.ToShortTimeString();

                        //Draw Date
                        e.Graphics.DrawString(strDate,
                            new Font(dgvLogItems.Font, FontStyle.Bold), Brushes.Black,
                            e.MarginBounds.Left +
                            (e.MarginBounds.Width - e.Graphics.MeasureString(strDate,
                            new Font(dgvLogItems.Font, FontStyle.Bold),
                            e.MarginBounds.Width).Width),
                            e.MarginBounds.Top - e.Graphics.MeasureString("Job Summary",
                            new Font(new Font(dgvLogItems.Font, FontStyle.Bold),
                            FontStyle.Bold), e.MarginBounds.Width).Height - 13);

                        //Draw Columns                 
                        topMargin = e.MarginBounds.Top;
                        check = 0;
                        foreach (DataGridViewColumn GridCol in dgvLogItems.Columns)
                        {
                            if (dgvLogItems.Columns[count].Visible)
                            {
                                e.Graphics.FillRectangle(new SolidBrush(Color.LightGray),
                                new Rectangle(arrColumnLefts[check], topMargin,
                                arrColumnWidths[check], headerHeight));

                                e.Graphics.DrawRectangle(Pens.Black,
                                    new Rectangle(arrColumnLefts[check], topMargin,
                                    arrColumnWidths[check], headerHeight));

                                e.Graphics.DrawString(GridCol.HeaderText,
                                    GridCol.InheritedStyle.Font,
                                    new SolidBrush(GridCol.InheritedStyle.ForeColor),
                                    new RectangleF(arrColumnLefts[check], topMargin,
                                    arrColumnWidths[check], headerHeight), strFormat);
                                check++;
                            }
                            count++;
                        }
                        nPage = false;
                        topMargin += headerHeight;
                    }
                    count = 0;
                    check = 0;
                    //Draw Columns Contents                
                    foreach (DataGridViewCell Cel in GridRow.Cells)
                    {
                        if (dgvLogItems.Columns[count].Visible)
                        {
                            if (Cel.Value != null)
                            {
                                e.Graphics.DrawString(Cel.Value.ToString(),
                                    Cel.Style.Font = new Font("Arial", 8F),
                                    new SolidBrush(Cel.InheritedStyle.ForeColor),
                                    new RectangleF(arrColumnLefts[check],
                                    topMargin,
                                    arrColumnWidths[check], cellHeight),
                                    strFormat);
                                Cel.Style.Font = new Font("Microsoft Sans Serif", 11.25F);
                            }

                            //Drawing Cells Borders 
                            e.Graphics.DrawRectangle(Pens.Black,
                                new Rectangle(arrColumnLefts[check], topMargin,
                                arrColumnWidths[check], cellHeight));
                            check++;
                        }
                        count++;
                    }
                }
                iRow++;
                topMargin += cellHeight;
            }
            if (bMorePagesToPrint)
            {
                e.HasMorePages = true;
                fPage = true;
                nPage = true;
            }
            else
            {
                e.HasMorePages = false;
            }
        }
    }
}
