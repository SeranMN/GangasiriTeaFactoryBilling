namespace GangasiriTeaFactoryBilling.TeaCollection
{
    partial class AddCollection
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
        private ComboBox cmbSupplier;
        private DateTimePicker dtpDate;
        private TextBox txtWeight;
        private CheckBox chkTransport;
        private TextBox txtTransport;
        private Label lblTotalAmount;
        private Label lblRs;
        private TextBox txtNotes;
        private Button btnSave;
        private Button btnCancel;

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form properties
            this.Text = IsEditMode ? "Edit Collection" : "Add Tea Collection";
            this.Size = new Size(600, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.Sizable; // Allow resizing
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
            this.Resize += FrmAddCollection_Resize;
        }

        private void CreateForm()
        {
            mainPanel.Controls.Clear();

            // Title
            Label titleLabel = new Label
            {
                Text = IsEditMode ? "✏️ Edit Collection" : "🍃 Add Tea Collection",
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
                Margin = new Padding(0, 60, 0, 0)
            };

            // Create form layout with ScrollablePanel
            Panel scrollPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            formLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 2,
                RowCount = 6,
                Padding = new Padding(10),
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            // Set column widths (30% labels, 70% controls)
            formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));

            // Row 0: Supplier
            Label lblSupplier = new Label
            {
                Text = "Supplier:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(5),
                Margin = new Padding(0, 5, 0, 5)
            };

            cmbSupplier = new ComboBox
            {
                Font = new Font("Segoe UI", 11),
                Dock = DockStyle.Fill,
                Height = 40,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Margin = new Padding(0, 5, 0, 5)
            };

            // Row 1: Date
            Label lblDate = new Label
            {
                Text = "Collection Date:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(5),
                Margin = new Padding(0, 5, 0, 5)
            };

            dtpDate = new DateTimePicker
            {
                Font = new Font("Segoe UI", 11),
                Dock = DockStyle.Fill,
                Height = 40,
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now,
                Margin = new Padding(0, 5, 0, 5)
            };

            // Row 2: Weight
            Label lblWeight = new Label
            {
                Text = "Weight (kg):",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(5),
                Margin = new Padding(0, 5, 0, 5)
            };

            Panel weightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Height = 40,
                Margin = new Padding(0, 5, 0, 5)
            };

            txtWeight = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Dock = DockStyle.Left,
                Width = 150,
                Height = 40,
                Padding = new Padding(10),
                PlaceholderText = "0.00"
            };

            Label lblKg = new Label
            {
                Text = "kg",
                Font = new Font("Segoe UI", 11),
                Dock = DockStyle.Left,
                Location = new Point(160, 10),
                AutoSize = true
            };

            weightPanel.Controls.Add(txtWeight);
            weightPanel.Controls.Add(lblKg);

            // Row 3: Transport
            Label lblTransport = new Label
            {
                Text = "Transport:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(5),
                Margin = new Padding(0, 5, 0, 5)
            };

            Panel transportPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Height = 40,
                Margin = new Padding(0, 5, 0, 5)
            };

            chkTransport = new CheckBox
            {
                Text = "Add Transport Charges",
                Font = new Font("Segoe UI", 11),
                Dock = DockStyle.Left,
                Location = new Point(0, 10),
                AutoSize = true,
                Cursor = Cursors.Hand
            };
            chkTransport.CheckedChanged += ChkTransport_CheckedChanged;

            txtTransport = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(180, 5),
                Size = new Size(150, 35),
                Padding = new Padding(10),
                PlaceholderText = "Amount",
                Enabled = false
            };

            Label lblRs = new Label
            {
                Text = "₹",
                Font = new Font("Segoe UI", 11),
                Location = new Point(340, 10),
                AutoSize = true
            };

            transportPanel.Controls.Add(chkTransport);
            transportPanel.Controls.Add(txtTransport);
            transportPanel.Controls.Add(lblRs);

            // Row 4: Calculation
            Label lblCalculation = new Label
            {
                Text = "Calculation:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(5),
                Margin = new Padding(0, 5, 0, 5)
            };

            Panel calcPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Height = 100,
                BackColor = Color.FromArgb(245, 245, 245),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(15),
                Margin = new Padding(0, 5, 0, 5)
            };

            Label lblRate = new Label
            {
                Text = $"Rate: ₹{currentRate:F2} per kg",
                Font = new Font("Segoe UI", 10),
                Location = new Point(10, 10),
                AutoSize = true
            };

            Label lblTotalLabel = new Label
            {
                Text = "Total Amount:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(10, 40),
                AutoSize = true
            };

            lblTotalAmount = new Label
            {
                Text = "₹0.00",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 150, 136),
                Location = new Point(150, 35),
                AutoSize = true
            };

            calcPanel.Controls.Add(lblRate);
            calcPanel.Controls.Add(lblTotalLabel);
            calcPanel.Controls.Add(lblTotalAmount);

            // Row 5: Notes
            Label lblNotes = new Label
            {
                Text = "Notes:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(5),
                Margin = new Padding(0, 5, 0, 5)
            };

            txtNotes = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Dock = DockStyle.Fill,
                Height = 80,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Padding = new Padding(10),
                PlaceholderText = "Optional notes",
                Margin = new Padding(0, 5, 0, 5)
            };

            // Add controls to table layout
            formLayout.Controls.Add(lblSupplier, 0, 0);
            formLayout.Controls.Add(cmbSupplier, 1, 0);
            formLayout.Controls.Add(lblDate, 0, 1);
            formLayout.Controls.Add(dtpDate, 1, 1);
            formLayout.Controls.Add(lblWeight, 0, 2);
            formLayout.Controls.Add(weightPanel, 1, 2);
            formLayout.Controls.Add(lblTransport, 0, 3);
            formLayout.Controls.Add(transportPanel, 1, 3);
            formLayout.Controls.Add(lblCalculation, 0, 4);
            formLayout.Controls.Add(calcPanel, 1, 4);
            formLayout.Controls.Add(lblNotes, 0, 5);
            formLayout.Controls.Add(txtNotes, 1, 5);

            // Set row heights
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 120));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));

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
                Text = IsEditMode ? "💾 Update Collection" : "💾 Save Collection",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(180, 45),
                BackColor = Color.FromArgb(0, 150, 136),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new Padding(5)
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

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
            buttonFlow.Controls.Add(btnCancel);
            buttonsPanel.Controls.Add(buttonFlow);

            // Add to scroll panel
            scrollPanel.Controls.Add(formLayout);

            // Add to form container
            formContainer.Controls.Add(scrollPanel);
            formContainer.Controls.Add(buttonsPanel);

            // Add to main panel
            mainPanel.Controls.Add(formContainer);
            mainPanel.Controls.Add(titleLabel);

            // Wire up events
            txtWeight.TextChanged += TxtWeight_TextChanged;
            txtTransport.TextChanged += TxtTransport_TextChanged;
        }

        private void FrmAddCollection_Resize(object sender, EventArgs e)
        {
            // Adjust form layout width when resizing
            if (formLayout != null)
            {
                formLayout.Width = mainPanel.Width - 60; // Account for padding

                // Adjust transport panel layout
                if (txtTransport != null && chkTransport != null)
                {
                    txtTransport.Location = new Point(180, 5);
                    lblRs.Location = new Point(340, 10);
                }
            }
        }

        #endregion
    }
}