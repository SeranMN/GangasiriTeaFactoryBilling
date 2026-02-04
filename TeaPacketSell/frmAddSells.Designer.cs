namespace GangasiriTeaFactoryBilling.TeaPacketSell
{
    partial class frmAddSells
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
        private TextBox txtAmount;

        private Button btnSave;
        private Button btnCancel;
        private TextBox txtQty;
        private Label total;
        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form properties
            this.Text = IsEditMode ? "Edit Advance" : "Add New Advance";
            this.Size = new Size(550, 450);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimumSize = new Size(500, 600);
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
            this.Resize += FrmAddAdvance_Resize;
        }

        private void CreateForm()
        {
            mainPanel.Controls.Clear();

            // Title
            Label titleLabel = new Label
            {
                Text = IsEditMode ? "✏️ Edit Advance" : "💵 Add New Advance",
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

            // Create form layout
            formLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 5,
                Padding = new Padding(10),
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None
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
                DropDownStyle = ComboBoxStyle.DropDown,

                Margin = new Padding(0, 5, 0, 5)
            };
            cmbSupplier.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbSupplier.AutoCompleteSource = AutoCompleteSource.ListItems;

            // Row 1: Date
            Label lblDate = new Label
            {
                Text = "Date:",
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

            // Row 2: Amount
            Label lblAmount = new Label
            {
                Text = "Price:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(5),
                Margin = new Padding(0, 5, 0, 5)
            };

            txtAmount = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Dock = DockStyle.Fill,
                Height = 40,
                Padding = new Padding(10),
                PlaceholderText = "Enter amount",
                Margin = new Padding(0, 5, 0, 5)
            };

            Label lblQty = new Label
            {
                Text = "Qty.:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(5),
                Margin = new Padding(0, 5, 0, 5)
            };

            txtQty = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Dock = DockStyle.Fill,
                Height = 40,
                Margin = new Padding(0, 5, 0, 5),
            };

            

            // Row 3: Description
            Label lblTotal = new Label
            {
                Text = "Total:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(5),
                Margin = new Padding(0, 5, 0, 5)
            };

            total = new Label
            {
                Font = new Font("Segoe UI", 11),
                Dock = DockStyle.Fill,
                Height = 40,
                Padding = new Padding(10),
                Margin = new Padding(0, 5, 0, 5)
            };

            // Row 4: Spacer
            Label spacer1 = new Label { Text = "", Dock = DockStyle.Fill };
            Label spacer2 = new Label { Text = "", Dock = DockStyle.Fill };

            // Add controls to table layout
            formLayout.Controls.Add(lblSupplier, 0, 0);
            formLayout.Controls.Add(cmbSupplier, 1, 0);
            formLayout.Controls.Add(lblDate, 0, 1);
            formLayout.Controls.Add(dtpDate, 1, 1);
            formLayout.Controls.Add(lblAmount, 0, 2);
            formLayout.Controls.Add(txtAmount, 1, 2);
            formLayout.Controls.Add(lblTotal, 0, 4);
            formLayout.Controls.Add(total, 1, 4);
            formLayout.Controls.Add(lblQty,0,3);
            formLayout.Controls.Add(txtQty,1,3);
            //formLayout.Controls.Add(spacer1, 0, 4);
            //formLayout.Controls.Add(spacer2, 0, 4);

            // Set row heights
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            //formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            //formLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Spacer

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
                Text = IsEditMode ? "💾 Update" : "💾 Save",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(160, 45),
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

            // Add to form container
            formContainer.Controls.Add(formLayout);
            formContainer.Controls.Add(buttonsPanel);

            // Add to main panel
            mainPanel.Controls.Add(formContainer);
            mainPanel.Controls.Add(titleLabel);
        }

        private void FrmAddAdvance_Resize(object sender, EventArgs e)
        {
            // Adjust form layout width when resizing
            if (formLayout != null)
            {
                formLayout.Width = mainPanel.Width - 60;
            }
        }

    }


    #endregion

}