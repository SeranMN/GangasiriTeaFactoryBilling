using GangasiriTeaFactoryBilling.Advanced;

namespace GangasiriTeaFactoryBilling.Suppliars
{
    partial class frmSupplierDetails
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 240, 240);

            // Create main panel
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(15)
            };

            // Create tab control
            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                ItemSize = new Size(120, 30),
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                Appearance = TabAppearance.FlatButtons
            };

            // Create tabs
            TabPage tabGeneral = CreateGeneralTab();
            TabPage tabDailyCollection = CreateDailyCollectionTab();
            TabPage tabInvoice = CreateInvoiceTab();
            TabPage tabAdvanced = CreateAdvancedTab();
            TabPage tabTeaPacket = CreateTeaPacketTab();

            tabControl.Controls.Add(tabGeneral);
            tabControl.Controls.Add(tabDailyCollection);
            tabControl.Controls.Add(tabInvoice);
            tabControl.Controls.Add(tabAdvanced);
            tabControl.Controls.Add(tabTeaPacket);

            mainPanel.Controls.Add(tabControl);
            this.Controls.Add(mainPanel);
        }

        private TabPage CreateGeneralTab()
        {
            TabPage tabPage = new TabPage("General");
            tabPage.BackColor = Color.White;
            tabPage.Padding = new Padding(10);
            tabPage.AutoScroll = true; // Add scrolling if content overflows

            // Use a TableLayoutPanel for better layout control
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                Padding = new Padding(10)
            };

            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            // Left Panel - Contact Details
            Panel contactPanel = CreateContactPanel(supplierId, supplierName);
            mainLayout.Controls.Add(contactPanel, 0, 0);

            // Right Panel - Financial Summary
            Panel financialPanel = CreateFinancialPanel();
            mainLayout.Controls.Add(financialPanel, 1, 0);

            tabPage.Controls.Add(mainLayout);
            return tabPage;
        }

        private Panel CreateContactPanel(string supplierId, string supplierName)
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(15, 10, 15, 10)
            };

            GroupBox contactGroup = new GroupBox
            {
                Text = "Contact Details",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                Dock = DockStyle.Fill,
                ForeColor = Color.FromArgb(0, 122, 204),
                Padding = new Padding(20, 25, 20, 20)
            };

            TableLayoutPanel contactLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 6,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None
            };

            // Set row heights
            contactLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35)); // ID
            contactLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35)); // Name
            contactLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35)); // Line (New)
            contactLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 80)); // Address
            contactLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35)); // Telephone
            contactLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Space

            // Supplier No
            AddContactField(contactLayout, "Supplier No:", supplierId, 0);

            // Supplier Name
            AddEditableField(contactLayout, "Supplier:", ref txtSupplierName, supplierName, 1);

            // Line (Added)
            AddLineField(contactLayout, "Line:", ref cmbLine, supplier.LineID, 2);

            // Address
            AddEditableField(contactLayout, "Address:", ref txtAddress, supplier.Address, 3, true);

            // Add separator line
            //Panel line1 = new Panel
            //{
            //    Dock = DockStyle.Fill,
            //    Height = 1,
            //    BackColor = Color.FromArgb(200, 200, 200),
            //    Margin = new Padding(0, 10, 0, 10)
            //};
            //contactLayout.SetColumnSpan(line1, 2);
            //contactLayout.Controls.Add(line1, 0, 3);

            // Telephone
            AddEditableField(contactLayout, "Telephone:", ref txtTelephone, supplier.Telephone, 4);

            // Add another separator line
            Panel line2 = new Panel
            {
                Dock = DockStyle.Fill,
                Height = 1,
                BackColor = Color.FromArgb(200, 200, 200),
                Margin = new Padding(0, 10, 0, 10)
            };
            contactLayout.SetColumnSpan(line2, 2);
            
            contactLayout.RowCount += 2;
            contactLayout.Controls.Add(line2, 0, 5);

            // Update Button
            Button btnUpdate = new Button
            {
                Text = "💾 Update Details",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(150, 40),
                BackColor = Color.FromArgb(0, 150, 136),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Dock = DockStyle.Top,
                Margin = new Padding(0, 10, 0, 0)
            };
            btnUpdate.FlatAppearance.BorderSize = 0;
            btnUpdate.Click += BtnUpdate_Click;
            
            Panel btnPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 5, 0, 5) };
            btnPanel.Controls.Add(btnUpdate);
            
            // Add button to layout (spanning 2 columns)
            // We need a new row for the button
            contactLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            contactLayout.Controls.Add(btnPanel, 0, 6);
            contactLayout.SetColumnSpan(btnPanel, 2);

            contactGroup.Controls.Add(contactLayout);
            panel.Controls.Add(contactGroup);

            return panel;
        }

        private void AddEditableField(TableLayoutPanel panel, string label, ref TextBox textBox, string value, int row, bool isMultiline = false)
        {
             Label lbl = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 5, 10, 5)
            };
            panel.Controls.Add(lbl, 0, row);

            textBox = new TextBox
            {
                Text = value,
                Font = new Font("Segoe UI", 10),
                Multiline = isMultiline,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 5, 0, 5)
            };
            if(isMultiline) textBox.Height = 70;
            
            panel.Controls.Add(textBox, 1, row);
        }

        private void AddContactField(TableLayoutPanel panel, string label, string value, int row, bool isMultiline = false)
        {
            Label lbl = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 5, 10, 5)
            };
            panel.Controls.Add(lbl, 0, row);

            if (isMultiline)
            {
                TextBox txtValue = new TextBox
                {
                    Text = value,
                    Font = new Font("Segoe UI", 10),
                    Multiline = true,
                    ReadOnly = true,
                    BackColor = Color.FromArgb(245, 245, 245),
                    BorderStyle = BorderStyle.None,
                    Dock = DockStyle.Fill,
                    Margin = new Padding(0, 5, 0, 5),
                    Height = 70
                };
                panel.Controls.Add(txtValue, 1, row);
            }
            else
            {
                Label lblValue = new Label
                {
                    Text = value,
                    Font = new Font("Segoe UI", 10),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Dock = DockStyle.Fill,
                    ForeColor = Color.FromArgb(64, 64, 64),
                    Margin = new Padding(0, 5, 0, 5)
                };
                panel.Controls.Add(lblValue, 1, row);
            }
        }

        private void AddLineField(TableLayoutPanel panel, string label, ref ComboBox comboBox, string currentLineId, int row)
        {
            Label lbl = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 5, 10, 5)
            };
            panel.Controls.Add(lbl, 0, row);

            comboBox = new ComboBox
            {
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 5, 0, 5)
            };
            
            // Populate
            foreach(var item in lineItems)
            {
                comboBox.Items.Add(item);
                if(item.LineID == currentLineId)
                    comboBox.SelectedItem = item;
            }
            if(comboBox.SelectedIndex < 0 && comboBox.Items.Count > 0) comboBox.SelectedIndex = 0;

            panel.Controls.Add(comboBox, 1, row);
        }

        private Panel CreateFinancialPanel()
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(15, 10, 15, 10)
            };

            GroupBox financialGroup = new GroupBox
            {
                Text = "Financial Summary",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                Dock = DockStyle.Fill,
                ForeColor = Color.FromArgb(46, 125, 50),
                Padding = new Padding(20, 25, 20, 20)
            };

            TableLayoutPanel financialLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                Padding = new Padding(5)
            };

            financialLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            financialLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            financialLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            financialLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25));

            // Amount Due
            Panel amountPanel = CreateFinancialItem("Amount Due",supplier.DueAmount.ToString("N2"), Color.FromArgb(211, 47, 47));
            financialLayout.Controls.Add(amountPanel, 0, 0);

            // Daily Collection Weight
            Panel weightPanel = CreateFinancialItem($"Daily Collection Weight for {DateTime.Now.ToString("MMMM")}", $"{TotalWeightForMonth} kg", Color.FromArgb(0, 150, 136));
            financialLayout.Controls.Add(weightPanel, 0, 1);

            // Total Advanced
            Panel advancedPanel = CreateFinancialItem($"Total Advanced for {DateTime.Now.ToString("MMMM")}", TotalAdvanceForMonth, Color.FromArgb(255, 152, 0));
            financialLayout.Controls.Add(advancedPanel, 0, 2);

            // Tea Packet Buy
            Panel teaPanel = CreateFinancialItem($"Tea Packet Buy for {DateTime.Now.ToString("MMMM")}", TotalTeaPacketForMonth, Color.FromArgb(123, 31, 162));
            financialLayout.Controls.Add(teaPanel, 0, 3);

            financialGroup.Controls.Add(financialLayout);
            panel.Controls.Add(financialGroup);

            return panel;
        }

        private Panel CreateFinancialItem(string title, string value, Color valueColor)
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(15, 10, 15, 10),
                Margin = new Padding(5),
                BackColor = Color.FromArgb(250, 250, 250),
                BorderStyle = BorderStyle.FixedSingle,
                MinimumSize = new Size(0, 80)
            };

            // Title
            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(64, 64, 64),
                Dock = DockStyle.Top,
                Height = 25,
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Value
            Label lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = valueColor,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(0, 5, 0, 0)
            };

            panel.Controls.Add(lblValue);
            panel.Controls.Add(lblTitle);

            return panel;
        }

        private TabPage CreateDailyCollectionTab()
        {
            TabPage tabPage = new TabPage("Daily Collection");
            tabPage.BackColor = Color.White;

            // Header panel
            Panel headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(0, 122, 204),
                Padding = new Padding(10)
            };

            Label headerLabel = new Label
            {
                Text = "Daily Collections",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Left
            };

            Button btnAddCollection = new Button
            {
                Text = "➕ Add Daily Collection",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(200, 35),
                BackColor = Color.FromArgb(0, 150, 136),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Right,
                Cursor = Cursors.Hand
            };
            btnAddCollection.FlatAppearance.BorderSize = 0;
            btnAddCollection.Click += BtnAddCollection_Click;

            headerPanel.Controls.Add(headerLabel);
            headerPanel.Controls.Add(btnAddCollection);

            // Create DataGridView for daily collections
            dailyCollectionGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 10),
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            // Style the grid
            dailyCollectionGrid.EnableHeadersVisualStyles = false;
            dailyCollectionGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 122, 204);
            dailyCollectionGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dailyCollectionGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            dailyCollectionGrid.ColumnHeadersHeight = 40;
            dailyCollectionGrid.CellDoubleClick += DailyCollectionGrid_CellDoubleClick;

            // Context Menu for Daily Collection
            ContextMenuStrip collectionMenu = new ContextMenuStrip();
            ToolStripMenuItem deleteCollectionItem = new ToolStripMenuItem("Delete");
            deleteCollectionItem.Click += DeleteCollectionItem_Click;
            collectionMenu.Items.Add(deleteCollectionItem);
            dailyCollectionGrid.ContextMenuStrip = collectionMenu;

            // Add columns
            dailyCollectionGrid.Columns.Add("Date", "Date");
            dailyCollectionGrid.Columns.Add("CollectionID", "Collection ID");
            dailyCollectionGrid.Columns.Add("Weight", "Weight (kg)");
            dailyCollectionGrid.Columns.Add("Transport", "Transport Added");
            dailyCollectionGrid.Columns.Add("Remarks", "Remarks");

            // Add sample data
            LoadDailyCollections();

            tabPage.Controls.Add(dailyCollectionGrid);
            tabPage.Controls.Add(headerPanel);

            return tabPage;
        }

        private TabPage CreateInvoiceTab()
        {
            TabPage tabPage = new TabPage("Invoice");
            tabPage.BackColor = Color.White;

            // Header panel
            Panel headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(46, 125, 50),
                Padding = new Padding(10)
            };

            Label headerLabel = new Label
            {
                Text = "Invoices",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Left
            };

            Button btnNewInvoice = new Button
            {
                Text = "➕ New Invoice",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(150, 35),
                BackColor = Color.FromArgb(0, 150, 136),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Right,
                Cursor = Cursors.Hand
            };
            btnNewInvoice.FlatAppearance.BorderSize = 0;
            btnNewInvoice.Click += BtnNewInvoice_Click;

            headerPanel.Controls.Add(headerLabel);
            headerPanel.Controls.Add(btnNewInvoice);

            // Create DataGridView for invoices
            invoiceGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 10),
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            // Style the grid
            invoiceGrid.EnableHeadersVisualStyles = false;
            invoiceGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(46, 125, 50);
            invoiceGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            invoiceGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            invoiceGrid.ColumnHeadersHeight = 40;
            invoiceGrid.CellDoubleClick += InvoiceGrid_CellDoubleClick;

            // Add columns
            invoiceGrid.Columns.Add("InvoiceNo", "Invoice No");
            invoiceGrid.Columns.Add("Date", "Month");
            invoiceGrid.Columns.Add("Description", "Total Weight");
            invoiceGrid.Columns.Add("Amount", "Net Amount");
            //invoiceGrid.Columns.Add("Status", "Status");
            //invoiceGrid.Columns.Add("DueDate", "Due Date");

            // Add sample data
            LoadInvoices();

            tabPage.Controls.Add(invoiceGrid);
            tabPage.Controls.Add(headerPanel);

            return tabPage;
        }

        private TabPage CreateAdvancedTab()
        {
            TabPage tabPage = new TabPage("Advanced");
            tabPage.BackColor = Color.White;

            // Header panel
            Panel headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(255, 152, 0),
                Padding = new Padding(10)
            };

            Label headerLabel = new Label
            {
                Text = "Advanced Payments",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Left
            };

            Button btnNewAdvanced = new Button
            {
                Text = "➕ New Advance",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(150, 35),
                BackColor = Color.FromArgb(0, 150, 136),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Right,
                Cursor = Cursors.Hand,
                
            };
            btnNewAdvanced.FlatAppearance.BorderSize = 0;
            btnNewAdvanced.Click += BtnNewAdvanced_Click;
            headerPanel.Controls.Add(headerLabel);
            headerPanel.Controls.Add(btnNewAdvanced);

            // Create DataGridView for advanced payments
            advancedGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 10),
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            // Style the grid
            advancedGrid.EnableHeadersVisualStyles = false;
            advancedGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(255, 152, 0);
            advancedGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            advancedGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            advancedGrid.ColumnHeadersHeight = 40;
            advancedGrid.CellDoubleClick += AdvancedGrid_CellDoubleClick;

            // Context Menu for Advances
            ContextMenuStrip advanceMenu = new ContextMenuStrip();
            ToolStripMenuItem deleteAdvanceItem = new ToolStripMenuItem("Delete");
            deleteAdvanceItem.Click += DeleteAdvanceItem_Click;
            advanceMenu.Items.Add(deleteAdvanceItem);
            advancedGrid.ContextMenuStrip = advanceMenu;

            // Add columns
            advancedGrid.Columns.Add("AdvanceNo", "Advance No");
            advancedGrid.Columns.Add("Date", "Date");
            advancedGrid.Columns.Add("Amount", "Amount");
            advancedGrid.Columns.Add("Purpose", "Purpose");
            advancedGrid.Columns.Add("Status", "Status");
            advancedGrid.Columns.Add("Balance", "Balance");

            // Add sample data
            LoadAdvances();

            tabPage.Controls.Add(advancedGrid);
            tabPage.Controls.Add(headerPanel);

            return tabPage;
        }

        private void BtnNewAdvanced_Click(object sender, EventArgs e)
        {
            using (frmAddAddvanced addForm = new frmAddAddvanced(int.Parse(supplierId)))
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    // Add new advance to grid
                    var advanceData = addForm.Result;
                    string newId = "ADV-" + (advancedGrid.Rows.Count + 1).ToString("D3");

                    advancedGrid.Rows.Add(
                        newId,
                        advanceData.SupplierName,
                        advanceData.AdvanceDate.ToString("yyyy-MM-dd"),
                        advanceData.Amount.ToString("N2"),
                        advanceData.Description,
                        "✅ Active"
                    );

                    MessageBox.Show("Advance added successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private TabPage CreateTeaPacketTab()
        {
            TabPage tabPage = new TabPage("Tea Packet Sell");
            tabPage.BackColor = Color.White;

            // Header panel
            Panel headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(123, 31, 162),
                Padding = new Padding(10)
            };

            Label headerLabel = new Label
            {
                Text = "Tea Packet Sales",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Left
            };

            Button btnNewSale = new Button
            {
                Text = "➕ New Sale",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(150, 35),
                BackColor = Color.FromArgb(0, 150, 136),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Right,
                Cursor = Cursors.Hand
            };
            btnNewSale.FlatAppearance.BorderSize = 0;

            headerPanel.Controls.Add(headerLabel);
            headerPanel.Controls.Add(btnNewSale);

            // Create DataGridView for tea packet sales
            teaPacketGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 10)
            };

            // Style the grid
            teaPacketGrid.EnableHeadersVisualStyles = false;
            teaPacketGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(123, 31, 162);
            teaPacketGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            teaPacketGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            teaPacketGrid.ColumnHeadersHeight = 40;

            // Add columns
            teaPacketGrid.Columns.Add("Date", "Date");
            teaPacketGrid.Columns.Add("Quantity", "Quantity");
            teaPacketGrid.Columns.Add("UnitPrice", "Unit Price");
            teaPacketGrid.Columns.Add("Total", "Total Amount");

            // Add sample data
            LoadTeaPacketSales();

            tabPage.Controls.Add(teaPacketGrid);
            tabPage.Controls.Add(headerPanel);

            return tabPage;
        }


        #endregion
    }
}