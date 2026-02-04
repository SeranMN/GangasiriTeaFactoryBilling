using GangasiriTeaFactoryBilling.db;
using GangasiriTeaFactoryBilling.Models;
using System.ComponentModel;
using System.Globalization;

namespace GangasiriTeaFactoryBilling.Invoice
{
    partial class frmGenerateInvoice
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private ComboBox cmbMonth;
        private ComboBox cmbYear;
        private CheckedListBox clbSuppliers;
        private CheckBox chkSelectAll;
        private Button btnGenerate;
        private Button btnCancel;
        private Label lblStatus;
        private ProgressBar progressBar;
        private Label lblSupplierCount;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

            this.SuspendLayout();

            // Form properties
            this.Text = "Generate Invoices";
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.Size = new Size(500, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Main panel with padding
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            // Create form content
            CreateFormContent(mainPanel);

            this.Controls.Add(mainPanel);
            this.ResumeLayout(false);
        }

        private void CreateFormContent(Panel container)
        {
            int yPos = 0;
            int controlWidth = 400;

            // Title
            Label lblTitle = new Label
            {
                Text = "Generate Monthly Invoices",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(46, 125, 50),
                Location = new Point(0, yPos),
                Size = new Size(controlWidth, 30),
                TextAlign = ContentAlignment.MiddleLeft
            };
            container.Controls.Add(lblTitle);
            yPos += 40;

            // Description
            Label lblDescription = new Label
            {
                Text = "Select year and month to generate invoices",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray,
                Location = new Point(0, yPos),
                Size = new Size(controlWidth, 25),
                TextAlign = ContentAlignment.MiddleLeft
            };
            container.Controls.Add(lblDescription);
            yPos += 30;

            // Year selection
            Label lblYear = new Label
            {
                Text = "Year:",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Location = new Point(10, yPos),
                Size = new Size(60, 25),
                TextAlign = ContentAlignment.MiddleLeft
            };
            container.Controls.Add(lblYear);

            cmbYear = new ComboBox
            {
                Font = new Font("Segoe UI", 10),
                Size = new Size(120, 30),
                Location = new Point(70, yPos - 3),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Populate years (current year and 5 years back)
            int currentYear = DateTime.Now.Year;
            for (int year = currentYear; year >= currentYear - 1; year--)
            {
                cmbYear.Items.Add(year);
            }
            cmbYear.SelectedItem = currentYear;

            container.Controls.Add(cmbYear);

            // Month selection
            Label lblMonth = new Label
            {
                Text = "Month:",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Location = new Point(200, yPos),
                Size = new Size(60, 25),
                TextAlign = ContentAlignment.MiddleLeft
            };
            container.Controls.Add(lblMonth);

            cmbMonth = new ComboBox
            {
                Font = new Font("Segoe UI", 10),
                Size = new Size(150, 30),
                Location = new Point(260, yPos - 3),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Populate months
            string[] months = {
    "January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"
};
            cmbMonth.Items.AddRange(months);
            cmbMonth.SelectedIndex = DateTime.Now.Month - 1;

            container.Controls.Add(cmbMonth);
            yPos += 40;

            // Select All checkbox
            chkSelectAll = new CheckBox
            {
                Text = "Select All Suppliers",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(10, yPos),
                Size = new Size(200, 25),
                Checked = true
            };
            chkSelectAll.CheckedChanged += ChkSelectAll_CheckedChanged;
            container.Controls.Add(chkSelectAll);
            yPos += 30;

            // Supplier count label
            lblSupplierCount = new Label
            {
                Text = "Selected: 7 of 7 suppliers",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(0, 122, 204),
                Location = new Point(200, yPos - 30),
                Size = new Size(150, 25),
                TextAlign = ContentAlignment.MiddleRight
            };
            container.Controls.Add(lblSupplierCount);

            // Suppliers list label
            Label lblSuppliers = new Label
            {
                Text = "Select Suppliers:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(10, yPos),
                Size = new Size(150, 25),
                TextAlign = ContentAlignment.MiddleLeft
            };
            container.Controls.Add(lblSuppliers);
            yPos += 25;

            // Suppliers list
            clbSuppliers = new CheckedListBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(10, yPos),
                Size = new Size(controlWidth, 150),
                BorderStyle = BorderStyle.FixedSingle,
                CheckOnClick = true,
                BackColor = Color.FromArgb(250, 250, 250),
                ItemHeight = 22
            };

            // Load suppliers
            clbSuppliers.Items.Clear();
            var suppliers = DataAccess.GetAllSuppliers();
            foreach (var suppliar in suppliers)
            {
                clbSuppliers.Items.Add(new SuppliarItem
                {
                    DisplayName = suppliar.SupplierName,
                    SuppliarId = suppliar.SupplierID
                },true);
            }

           

            clbSuppliers.ItemCheck += (s, e) =>
            {
                // Use BeginInvoke to ensure CheckedItems is updated before counting
                this.BeginInvoke(new Action(() => UpdateSupplierCount()));
            };

            container.Controls.Add(clbSuppliers);
            yPos += 160;

            // Invoice Details Preview
            GroupBox previewGroup = new GroupBox
            {
                Text = "Invoice Details Preview",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(0, yPos),
                Size = new Size(controlWidth, 100),
                ForeColor = Color.FromArgb(64, 64, 64)
            };

            Label lblInvoicePrefix = new Label
            {
                Text = "Invoice Prefix: INV-2024-",
                Font = new Font("Segoe UI", 9),
                Location = new Point(20, 25),
                Size = new Size(200, 20)
            };

            Label lblDueDate = new Label
            {
                Text = "Due Date: 15th of next month",
                Font = new Font("Segoe UI", 9),
                Location = new Point(20, 50),
                Size = new Size(200, 20)
            };

            previewGroup.Controls.Add(lblInvoicePrefix);
            previewGroup.Controls.Add(lblDueDate);
            container.Controls.Add(previewGroup);
            yPos += 110;

            // Status label
            lblStatus = new Label
            {
                Text = "Ready to generate invoices",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Green,
                Location = new Point(10, yPos),
                Size = new Size(controlWidth, 25),
                TextAlign = ContentAlignment.MiddleLeft
            };
            container.Controls.Add(lblStatus);
            yPos += 30;

            // Buttons panel
            Panel buttonPanel = new Panel
            {
                Location = new Point(0, yPos),
                Size = new Size(controlWidth, 45),
                BackColor = Color.Transparent
            };

            btnGenerate = new Button
            {
                Text = "Generate Invoices",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(180, 40),
                Location = new Point(50, 0),
                BackColor = Color.FromArgb(46, 125, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnGenerate.FlatAppearance.BorderSize = 0;
            btnGenerate.Click += BtnGenerate_Click;

            btnCancel = new Button
            {
                Text = "Cancel",
                Font = new Font("Segoe UI", 10),
                Size = new Size(120, 40),
                Location = new Point(250, 0),
                BackColor = Color.FromArgb(96, 125, 139),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.Cancel
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += BtnCancel_Click;

            buttonPanel.Controls.Add(btnGenerate);
            buttonPanel.Controls.Add(btnCancel);
            container.Controls.Add(buttonPanel);

            // Initialize counts
            UpdateSupplierCount();
        }

        #endregion

        #region Event Handlers

        private void ChkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < clbSuppliers.Items.Count; i++)
            {
                clbSuppliers.SetItemChecked(i, chkSelectAll.Checked);
            }
            UpdateSupplierCount();
        }

        private void UpdateSupplierCount()
        {
            int selectedCount = clbSuppliers.CheckedItems.Count;
            int totalCount = clbSuppliers.Items.Count;

            lblSupplierCount.Text = $"Selected: {selectedCount} of {totalCount} suppliers";
            lblSupplierCount.ForeColor = selectedCount > 0 ? Color.FromArgb(46, 125, 50) : Color.Red;

            if (selectedCount == 0)
            {
                lblStatus.Text = "Please select at least one supplier";
                lblStatus.ForeColor = Color.Red;
                btnGenerate.Enabled = false;
            }
            else
            {
                lblStatus.Text = $"Ready to generate invoices for {selectedCount} supplier(s)";
                lblStatus.ForeColor = Color.Green;
                btnGenerate.Enabled = true;
            }
        }

        private async void BtnGenerate_Click(object sender, EventArgs e)
        {
            if (clbSuppliers.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please select at least one supplier.", "No Suppliers Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Create progress bar if not exists
            if (progressBar == null)
            {
                progressBar = new ProgressBar
                {
                    Location = new Point(0, lblStatus.Location.Y + 25),
                    Size = new Size(400, 25),
                    Visible = true
                };
                // Fix: Use the correct parent container for controls
                this.Controls.Add(progressBar);
            }
            else
            {
                progressBar.Visible = true;
            }

            progressBar.Value = 0;
            btnGenerate.Enabled = false;
            btnCancel.Enabled = false;

            string month = cmbMonth.SelectedItem.ToString();
            string year = cmbYear.SelectedItem.ToString();
            List<SuppliarItem> selectedSuppliers = new List<SuppliarItem>();

            foreach (var item in clbSuppliers.CheckedItems)
            {
               
                selectedSuppliers.Add((SuppliarItem)item);
                
            }

            // Simulate invoice generation process
            await GenerateInvoicesAsync(month, selectedSuppliers, year);

            // Show completion message
            MessageBox.Show(lblStatus.Text,"Invoice Generated", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #endregion

        #region Public Properties

        public string SelectedMonth => cmbMonth.SelectedItem?.ToString();
        public List<string> SelectedSuppliers
        {
            get
            {
                var suppliers = new List<string>();
                foreach (var item in clbSuppliers.CheckedItems)
                {
                    suppliers.Add(item.ToString());
                }
                return suppliers;
            }
        }

        #endregion
    }

}
