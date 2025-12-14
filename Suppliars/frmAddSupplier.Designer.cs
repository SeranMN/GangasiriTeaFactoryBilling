namespace GangasiriTeaFactoryBilling.Suppliars
{
    partial class frmAddSupplier
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
        /// 

        private Panel mainPanel;
        private TableLayoutPanel formLayout;
        private TextBox txtSupplierNumber;
        private TextBox txtFullName;
        private TextBox txtTelephone;
        private ComboBox cmbLine;
        private TextBox txtAddress;
        private RadioButton rbActive;
        private RadioButton rbInactive;
        private Button btnSave;
        private Button btnSaveAndNew;
        private Button btnCancel;
        private Button btnAutoGenerate;
        private void InitializeComponent()
        {
            this.Text = "Add New Supplier";
            this.Size = new Size(600, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimumSize = new Size(550, 550);
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.Padding = new Padding(20);

            // Main container
            mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            // Create form with proper layout
            CreateForm();

            this.Controls.Add(mainPanel);
            this.ResumeLayout(false);

            // Handle resize
            this.Resize += FrmAddSupplier_Resize;
        }

        private void CreateForm()
        {
            mainPanel.Controls.Clear();

            // Title
            Label titleLabel = new Label
            {
                Text = "📝 Supplier Information",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(64, 64, 64),
                Dock = DockStyle.Top,
                Height = 50,
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Form container
            Panel formContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20),
                Margin = new Padding(0, 60, 0, 0),
                AutoScroll = true
            };

            // Create form layout
            formLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 2,
                RowCount = 7,
                Padding = new Padding(10),
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            // Set column widths (30% labels, 70% controls)
            formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));

            // Row 0: Supplier Number
            Label lblNumber = new Label
            {
                Text = "Supplier Number:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(5),
                Margin = new Padding(0, 5, 0, 5)
            };

            Panel numberPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Height = 40,
                Margin = new Padding(0, 5, 0, 5)
            };

            txtSupplierNumber = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Dock = DockStyle.Left,
                Width = 200,
                Height = 40,
                Padding = new Padding(10)
            };

            Button btnAutoGenerate = new Button
            {
                Text = "Auto Generate",
                Font = new Font("Segoe UI", 10),
                Size = new Size(130, 40),
                Location = new Point(210, 0),
                BackColor = Color.FromArgb(96, 125, 139),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAutoGenerate.FlatAppearance.BorderSize = 0;
            btnAutoGenerate.Click += BtnAutoGenerate_Click;

            numberPanel.Controls.Add(txtSupplierNumber);
            numberPanel.Controls.Add(btnAutoGenerate);

            // Row 1: Full Name
            Label lblName = new Label
            {
                Text = "Full Name:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(5),
                Margin = new Padding(0, 5, 0, 5)
            };

            txtFullName = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Dock = DockStyle.Fill,
                Height = 40,
                Padding = new Padding(10),
                Margin = new Padding(0, 5, 0, 5)
            };

            // Row 2: Telephone
            Label lblTelephone = new Label
            {
                Text = "Telephone:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(5),
                Margin = new Padding(0, 5, 0, 5)
            };

            txtTelephone = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Dock = DockStyle.Fill,
                Height = 40,
                Padding = new Padding(10),
                PlaceholderText = "07_-______",
                Margin = new Padding(0, 5, 0, 5)
            };

            // Row 3: Line
            Label lblLine = new Label
            {
                Text = "Select Line:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(5),
                Margin = new Padding(0, 5, 0, 5)
            };

            cmbLine = new ComboBox
            {
                Font = new Font("Segoe UI", 11),
                Dock = DockStyle.Fill,
                Height = 40,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Margin = new Padding(0, 5, 0, 5)
            };

            cmbLine.Items.Add("Line 1");
            cmbLine.Items.Add("Line 2");
            cmbLine.Items.Add("Line 3");
            cmbLine.SelectedIndex = 0;

            // Row 4: Address
            Label lblAddress = new Label
            {
                Text = "Address:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(5),
                Margin = new Padding(0, 5, 0, 5)
            };

            txtAddress = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Dock = DockStyle.Fill,
                Height = 80,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Padding = new Padding(10),
                Margin = new Padding(0, 5, 0, 5)
            };

            // Row 5: Status
            Label lblStatus = new Label
            {
                Text = "Status:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(5),
                Margin = new Padding(0, 5, 0, 5)
            };

            Panel statusPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Height = 40,
                Margin = new Padding(0, 5, 0, 5)
            };

            rbActive = new RadioButton
            {
                Text = "Active",
                Font = new Font("Segoe UI", 11),
                Location = new Point(10, 10),
                Size = new Size(100, 30),
                Checked = true
            };

            rbInactive = new RadioButton
            {
                Text = "Inactive",
                Font = new Font("Segoe UI", 11),
                Location = new Point(120, 10),
                Size = new Size(100, 30)
            };

            statusPanel.Controls.Add(rbActive);
            statusPanel.Controls.Add(rbInactive);

            // Row 6: Spacer
            Label spacer1 = new Label { Text = "", Dock = DockStyle.Fill };
            Label spacer2 = new Label { Text = "", Dock = DockStyle.Fill };

            // Add controls to table layout
            formLayout.Controls.Add(lblNumber, 0, 0);
            formLayout.Controls.Add(numberPanel, 1, 0);
            formLayout.Controls.Add(lblName, 0, 1);
            formLayout.Controls.Add(txtFullName, 1, 1);
            formLayout.Controls.Add(lblTelephone, 0, 2);
            formLayout.Controls.Add(txtTelephone, 1, 2);
            formLayout.Controls.Add(lblLine, 0, 3);
            formLayout.Controls.Add(cmbLine, 1, 3);
            formLayout.Controls.Add(lblAddress, 0, 4);
            formLayout.Controls.Add(txtAddress, 1, 4);
            formLayout.Controls.Add(lblStatus, 0, 5);
            formLayout.Controls.Add(statusPanel, 1, 5);
            formLayout.Controls.Add(spacer1, 0, 6);
            formLayout.Controls.Add(spacer2, 1, 6);

            // Set row heights
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Spacer

            // Buttons panel
            Panel buttonsPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 70,
                BackColor = Color.Transparent,
                Padding = new Padding(20, 10, 20, 10)
            };

            FlowLayoutPanel buttonFlow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false
            };

            btnSave = new Button
            {
                Text = "💾 Save Supplier",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(150, 45),
                BackColor = Color.FromArgb(0, 150, 136),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new Padding(5)
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            btnSaveAndNew = new Button
            {
                Text = "💾 Save & Add Another",
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                Size = new Size(180, 45),
                BackColor = Color.FromArgb(33, 150, 243),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new Padding(5)
            };
            btnSaveAndNew.FlatAppearance.BorderSize = 0;
            btnSaveAndNew.Click += BtnSaveAndNew_Click;

            btnCancel = new Button
            {
                Text = "✕ Cancel",
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                Size = new Size(120, 45),
                BackColor = Color.FromArgb(158, 158, 158),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new Padding(5)
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            buttonFlow.Controls.Add(btnSave);
            buttonFlow.Controls.Add(btnSaveAndNew);
            buttonFlow.Controls.Add(btnCancel);

            buttonsPanel.Controls.Add(buttonFlow);

            // Add to form container
            formContainer.Controls.Add(formLayout);
            formContainer.Controls.Add(buttonsPanel);

            // Add to main panel
            mainPanel.Controls.Add(formContainer);
            mainPanel.Controls.Add(titleLabel);
        }

        private void FrmAddSupplier_Resize(object sender, EventArgs e)
        {
            // Adjust form layout width when resizing
            if (formLayout != null)
            {
                formLayout.Width = mainPanel.Width - 60;

                // Adjust number panel layout
                if (txtSupplierNumber != null && btnAutoGenerate != null)
                {
                    btnAutoGenerate.Location = new Point(210, 0);
                }
            }
        }

    }


    #endregion
}
