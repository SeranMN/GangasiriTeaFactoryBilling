using GangasiriTeaFactoryBilling.db;
using GangasiriTeaFactoryBilling.Models;
using GangasiriTeaFactoryBilling.PdfHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GangasiriTeaFactoryBilling.Invoice
{
    public partial class frmInvoiceDetails : Form
    {
        private Label lblFactoryName;
        private Label lblAddress;
        private Label lblTelephoneO;
        private Label lblTelephoneF;
        private Label lblTelephoneM;

        // Month/Name Section
        private Label lblMonth;
        private Label lblMonthValue;
        private Label lblNo;
        private Label lblNoValue;
        private Label lblName;
        private Label lblNameValue;
        private Label lblLine;
        private Label lblLineValue;

        // Tea Leave Amount Section
        private GroupBox grpTeaLeave;
        private Label lblRate;
        private Label lblWeight;
        private Label lblTeaAmount;

        // Transport
        private Label lblTransport;
        private Label lblTransportAmount;

        // Total
        private Label lblTotal;
        private Label lblTotalAmount;

        // Deductions Section
        private GroupBox grpDeductions;
        private Label lblTransportFee;
        private Label lblTransportFeeAmount;
        private Label lblTeaDeduction;
        private Label lblTeaDeductionAmount;
        private Label lblAdvance;
        private Label lblAdvanceAmount;

        // Net Amount and Amount Due
        private Label lblNetAmount;
        private Label lblNetAmountValue;
        private Label lblTotalDeductions;
        private Label lblTotalDeductionValue;

        // Payment Mode
        private Label lblPayMode;
        private Label lblPayModeValue;

        // Buttons
        private Button btnPrint;
        private Button btnClose;
        private Button btnEdit;

        // Right side panels for date-wise data
        private Panel pnlRightSide;
        private Label lblRightTitle;
        private FlowLayoutPanel flpDateDetails;

        private readonly string receiptNo;
        public frmInvoiceDetails(string reciptNo)
        {
            this.receiptNo = reciptNo;
            InitializeComponent();
            SetupForm();
            LoadInvoiceData();
        }

        private void SetupForm()
        {
            // Form setup - increased width to accommodate right side
            this.Text = "Invoice Detail - Gangasiri Tea Factory";
            this.Size = new Size(900, 750); // Increased width
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            // Factory Header - centered
            lblFactoryName = new Label
            {
                Text = "Gangasiri Tea Factory",
                Location = new Point(300, 20),
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                AutoSize = true
            };

            lblAddress = new Label
            {
                Text = "Weerapana, Opatha",
                Location = new Point(330, 50),
                Font = new Font("Arial", 10),
                AutoSize = true
            };

            lblTelephoneO = new Label
            {
                Text = "O : 0763169992",
                Location = new Point(200, 80),
                Font = new Font("Arial", 10),
                AutoSize = true
            };

            lblTelephoneF = new Label
            {
                Text = "F : 0763169993",
                Location = new Point(330, 80),
                Font = new Font("Arial", 10),
                AutoSize = true
            };

            lblTelephoneM = new Label
            {
                Text = "M : 076319994",
                Location = new Point(460, 80),
                Font = new Font("Arial", 10),
                AutoSize = true
            };

            // Separator Line - spanning both sections
            var separator1 = new Label
            {
                BorderStyle = BorderStyle.Fixed3D,
                Size = new Size(800, 2),
                Location = new Point(25, 110)
            };

            // Left Side Section (existing invoice details)
            // Month/Name Section
            lblMonth = new Label
            {
                Text = "මාසය: ",
                Location = new Point(50, 130),
                Font = new Font("Arial", 10, FontStyle.Bold),
                AutoSize = true
            };

            lblMonthValue = new Label
            {
                Text = "",
                Location = new Point(120, 130),
                Font = new Font("Arial", 10),
                AutoSize = true
            };

            lblNo = new Label
            {
                Text = "අංකය:",
                Location = new Point(50, 160),
                Font = new Font("Arial", 10, FontStyle.Bold),
                AutoSize = true
            };

            lblNoValue = new Label
            {
                Text = "001",
                Location = new Point(120, 160),
                Font = new Font("Arial", 10),
                AutoSize = true
            };

            lblName = new Label
            {
                Text = "නම:",
                Location = new Point(250, 160),
                Font = new Font("Arial", 10, FontStyle.Bold),
                AutoSize = true
            };

            lblNameValue = new Label
            {
                Text = "John Doe",
                Location = new Point(310, 160),
                Font = new Font("Arial", 10),
                AutoSize = true
            };

            lblLine = new Label
            {
                Text = "ප්‍රාදේශිකය:",
                Location = new Point(50, 190),
                Font = new Font("Arial", 10, FontStyle.Bold),
                AutoSize = true
            };

            lblLineValue = new Label
            {
                Text = "A01",
                Location = new Point(150, 190),
                Font = new Font("Arial", 10),
                AutoSize = true
            };

            // Tea Leave Amount Group
            grpTeaLeave = new GroupBox
            {
                Text = "තේ දළු විස්තර",
                Location = new Point(50, 220),
                Size = new Size(350, 100),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            lblRate = new Label
            {
                Text = "Rate: LKR 50.00",
                Location = new Point(20, 25),
                Parent = grpTeaLeave,
                AutoSize = true
            };

            lblWeight = new Label
            {
                Text = "Weight: 100 kg",
                Location = new Point(20, 50),
                Parent = grpTeaLeave,
                AutoSize = true
            };

            lblTeaAmount = new Label
            {
                Text = "Amount: LKR 5,000.00",
                Location = new Point(200, 25),
                Parent = grpTeaLeave,
                Font = new Font("Arial", 10, FontStyle.Bold),
                AutoSize = true
            };

            // Transport
            lblTransport = new Label
            {
                Text = "ප්‍රවාහන දීමනා:",
                Location = new Point(50, 320),
                Font = new Font("Arial", 10, FontStyle.Bold),
                AutoSize = true
            };

            lblTransportAmount = new Label
            {
                Text = "LKR 500.00",
                Location = new Point(250, 320),
                Font = new Font("Arial", 10),
                AutoSize = true
            };

            // Total
            lblTotal = new Label
            {
                Text = "මේ මාසයට මුළු ගෙවීම්:",
                Location = new Point(50, 350),
                Font = new Font("Arial", 11, FontStyle.Bold),
                AutoSize = true
            };

            lblTotalAmount = new Label
            {
                Text = "LKR 5,500.00",
                Location = new Point(250, 350),
                Font = new Font("Arial", 11, FontStyle.Bold),
                ForeColor = Color.Blue,
                AutoSize = true
            };

            // Separator Line
            var separator2 = new Label
            {
                BorderStyle = BorderStyle.Fixed3D,
                Size = new Size(350, 2),
                Location = new Point(25, 380)
            };

            // Deductions Group
            grpDeductions = new GroupBox
            {
                Text = "අඩුකිරීම්",
                Location = new Point(50, 390),
                Size = new Size(350, 120),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            lblTransportFee = new Label
            {
                Text = "ප්‍රවාහන ගාස්තු:",
                Location = new Point(20, 30),
                Parent = grpDeductions,
                AutoSize = true
            };

            lblTransportFeeAmount = new Label
            {
                Text = "LKR 100.00",
                Location = new Point(200, 30),
                Parent = grpDeductions,
                AutoSize = true
            };

            lblTeaDeduction = new Label
            {
                Text = "හිඟ:",
                Location = new Point(20, 60),
                Parent = grpDeductions,
                AutoSize = true
            };

            lblTeaDeductionAmount = new Label
            {
                Text = "LKR 200.00",
                Location = new Point(200, 60),
                Parent = grpDeductions,
                AutoSize = true
            };

            lblAdvance = new Label
            {
                Text = "අත්තිකාරම්:",
                Location = new Point(20, 90),
                Parent = grpDeductions,
                AutoSize = true
            };

            lblAdvanceAmount = new Label
            {
                Text = "LKR 1,000.00",
                Location = new Point(200, 90),
                Parent = grpDeductions,
                AutoSize = true
            };

            // Net Amount
            lblNetAmount = new Label
            {
                Text = "ශුද්ධ ගෙවීම්",
                Location = new Point(50, 560),
                Font = new Font("Arial", 11, FontStyle.Bold),
                AutoSize = true
            };

            lblNetAmountValue = new Label
            {
                Text = "LKR 4,200.00",
                Location = new Point(250, 560),
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                AutoSize = true
            };

            // Amount Due
            lblTotalDeductions = new Label
            {
                Text = "මුළු අඩු කිරීම්",
                Location = new Point(50, 530),
                Font = new Font("Arial", 11, FontStyle.Bold),
                AutoSize = true
            };

            lblTotalDeductionValue = new Label
            {
                Text = "LKR 4,200.00",
                Location = new Point(250, 530),
                Font = new Font("Arial", 11, FontStyle.Bold),
                ForeColor = Color.Green,
                AutoSize = true
            };

            // Payment Mode
            lblPayMode = new Label
            {
                Text = "Pay Mode:",
                Location = new Point(50, 590),
                Font = new Font("Arial", 10, FontStyle.Bold),
                AutoSize = true
            };

            lblPayModeValue = new Label
            {
                Text = "Cash",
                Location = new Point(150, 590),
                Font = new Font("Arial", 10),
                AutoSize = true
            };

            // Right Side Section - Date-wise breakdown
            pnlRightSide = new Panel
            {
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(450, 130),
                Size = new Size(400, 500),
                BackColor = Color.WhiteSmoke
            };

            lblRightTitle = new Label
            {
                Text = "මාසය තුළ ගෙවීම් විස්තර",
                Location = new Point(10, 10),
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                AutoSize = true,
                Parent = pnlRightSide
            };

            // FlowLayoutPanel to display date-wise details
            flpDateDetails = new FlowLayoutPanel
            {
                Location = new Point(10, 40),
                Size = new Size(380, 500),
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Parent = pnlRightSide
            };

            // Buttons - repositioned
            btnPrint = new Button
            {
                Text = "Print",
                Location = new Point(200, 650),
                Size = new Size(80, 30),
                BackColor = Color.SteelBlue,
                ForeColor = Color.White
            };
            btnPrint.Click += BtnPrint_Click;

            btnEdit = new Button
            {
                Text = "Edit",
                Location = new Point(100, 650),
                Size = new Size(80, 30),
                BackColor = Color.Orange,
                ForeColor = Color.White
            };
            // Add btnEdit.Click event handler if needed

            btnClose = new Button
            {
                Text = "Close",
                Location = new Point(500, 650),
                Size = new Size(80, 30),
                BackColor = Color.Gray,
                ForeColor = Color.White
            };
            btnClose.Click += BtnClose_Click;

            // Add controls to form
            this.Controls.AddRange(new Control[]
            {
                lblFactoryName, lblAddress, lblTelephoneO, lblTelephoneF, lblTelephoneM,
                separator1, lblMonth, lblMonthValue, lblNo, lblNoValue, lblName, lblNameValue,
                lblLine, lblLineValue, grpTeaLeave, lblTransport, lblTransportAmount,
                lblTotal, lblTotalAmount, separator2, grpDeductions, lblNetAmount,
                lblNetAmountValue, lblTotalDeductions, lblTotalDeductionValue, lblPayMode, lblPayModeValue,
                btnPrint, btnEdit, btnClose, pnlRightSide
            });
        }

        private void LoadInvoiceData()
        {
            try
            {
                var Invoice = DataAccess.GetInvoicesByInvoiceNumber(receiptNo).FirstOrDefault();

                if (Invoice != null)
                {
                    Invoice.TotalAmount = Invoice.TransportAllowance + Invoice.RatePerKg * Invoice.TotalWeight;
                    Invoice.TotalDeductions = Invoice.TotalAdvances + Invoice.PreviousBalance + Invoice.TransportFee;
                    Invoice.NetAmount = Invoice.TotalAmount - Invoice.TotalDeductions;

                    lblMonthValue.Text = Invoice.InvoiceMonth;
                    lblNoValue.Text = Invoice.SupplierID.ToString();
                    lblNameValue.Text = Invoice.SupplierName;
                    lblLineValue.Text = Invoice.LineName ?? "N/A";
                    lblRate.Text = $"Rate: {Invoice.RatePerKg.ToString("N2")}";
                    lblWeight.Text = $"මුළු බර: {Invoice.TotalWeight}kg";
                    lblTeaAmount.Text = $"වටිනාකම: LKR {(Invoice.RatePerKg * Invoice.TotalWeight).ToString("N2")}";
                    lblTransportAmount.Text = $"LKR {Invoice.TransportAllowance.ToString("N2")}";
                    lblTotalAmount.Text = $"LKR {Invoice.TotalAmount.ToString("N2")}";
                    lblTransportFeeAmount.Text = $"LKR {Invoice.TransportFee.ToString("N2")}";
                    lblTeaDeductionAmount.Text = $"LKR {Invoice.PreviousBalance.ToString("N2")}";
                    lblAdvanceAmount.Text = $"LKR {Invoice.TotalAdvances.ToString("N2")}";
                    lblTotalDeductionValue.Text = $"LKR {Invoice.TotalDeductions.ToString("N2")}";
                    lblNetAmountValue.Text = $"LKR {Invoice.NetAmount.ToString("N2")}";
                    // lblPayModeValue.Text = Invoice.PaymentMode ?? "Cash";

                    // Extract month and year from InvoiceMonth (assuming format like "January 2024")
                    
                    var month = GetMonthNumber(Invoice.InvoiceMonth.ToLower());
                    // Load date-wise tea leaves and advances
                    LoadDateWiseDetails(Invoice.SupplierID, month, Invoice.Year);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading invoice data: " + ex.Message);
            }
        }

        private int GetMonthNumber(string monthName)
        {
            switch (monthName.ToLower())
            {
                case "january": return 1;
                case "february": return 2;
                case "march": return 3;
                case "april": return 4;
                case "may": return 5;
                case "june": return 6;
                case "july": return 7;
                case "august": return 8;
                case "september": return 9;
                case "october": return 10;
                case "november": return 11;
                case "december": return 12;
                default: return DateTime.Now.Month;
            }
        }

        private void LoadDateWiseDetails(int supplierId, int month, int year)
        {
            try
            {
                // Clear existing controls
                flpDateDetails.Controls.Clear();

                // Get collections data
                var collectionEntries = DataAccess.GetSupplierWeightByMonth(supplierId, year, month);

                // Get advances data for the same month
                var advanceEntries = DataAccess.GetSupplierAdvanceByMonth(supplierId, year, month);

                if ((collectionEntries == null || collectionEntries.Count == 0) &&
                    (advanceEntries == null || advanceEntries.Count == 0))
                {
                    // Add a message if no data found
                    Label noDataLabel = new Label
                    {
                        Text = "No date-wise data available",
                        Font = new Font("Arial", 10, FontStyle.Italic),
                        ForeColor = Color.Gray,
                        AutoSize = true,
                        Margin = new Padding(0, 10, 0, 0)
                    };
                    flpDateDetails.Controls.Add(noDataLabel);
                    return;
                }

                // Combine data from both tables
                var dateWiseData = CombineDateWiseData(collectionEntries, advanceEntries, month, year);

                // Sort by date
                dateWiseData = dateWiseData.OrderBy(d => d.Date).ToList();

                // Add headers
                Panel headerPanel = CreateHeaderPanel();
                flpDateDetails.Controls.Add(headerPanel);

                // Add each date entry
                foreach (var entry in dateWiseData)
                {
                    Panel datePanel = CreateDateEntryPanel(entry);
                    flpDateDetails.Controls.Add(datePanel);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading date-wise details: " + ex.Message);
            }
        }

        private List<DateWiseEntry> CombineDateWiseData(
            List<DailyCollection> collectionEntries,
            List<Advance> advanceEntries,
            int month, int year)
        {
            var dateWiseData = new List<DateWiseEntry>();
            var allDates = new HashSet<DateTime>();

            // Add dates from collections
            if (collectionEntries != null)
            {
                foreach (var collection in collectionEntries)
                {
                    allDates.Add(collection.CollectionDate.Date);
                }
            }

            // Add dates from advances
            if (advanceEntries != null)
            {
                foreach (var advance in advanceEntries)
                {
                    allDates.Add(advance.AdvanceDate.Date);
                }
            }

            // Create DateWiseEntry for each date
            foreach (var date in allDates.OrderBy(d => d))
            {
                var entry = new DateWiseEntry
                {
                    Date = date,
                    TeaLeavesWeight = 0,
                    AdvanceAmount = 0
                };

                // Sum collections for this date
                if (collectionEntries != null)
                {
                    entry.TeaLeavesWeight = collectionEntries
                        .Where(c => c.CollectionDate.Date == date)
                        .Sum(c => c.Weight);
                }

                // Sum advances for this date
                if (advanceEntries != null)
                {
                    entry.AdvanceAmount = advanceEntries
                        .Where(a => a.AdvanceDate.Date == date)
                        .Sum(a => a.Amount);
                }

                dateWiseData.Add(entry);
            }

            return dateWiseData;
        }

        private Panel CreateHeaderPanel()
        {
            Panel headerPanel = new Panel
            {
                Size = new Size(300, 30),
                BackColor = Color.LightSteelBlue,
                Margin = new Padding(0, 5, 0, 5)
            };

            Label lblDateHeader = new Label
            {
                Text = "දිනය",
                Location = new Point(10, 5),
                Font = new Font("Arial", 10, FontStyle.Bold),
                AutoSize = true
            };

            Label lblTeaHeader = new Label
            {
                Text = "තේ දළු බර (kg)",
                Location = new Point(100, 5),
                Font = new Font("Arial", 10, FontStyle.Bold),
                AutoSize = true
            };

            Label lblAdvanceHeader = new Label
            {
                Text = "අත්තිකාරම්",
                Location = new Point(200, 5),
                Font = new Font("Arial", 10, FontStyle.Bold),
                AutoSize = true
            };

            headerPanel.Controls.Add(lblDateHeader);
            headerPanel.Controls.Add(lblTeaHeader);
            headerPanel.Controls.Add(lblAdvanceHeader);

            return headerPanel;
        }

        private Panel CreateDateEntryPanel(DateWiseEntry entry)
        {
            Panel datePanel = new Panel
            {
                Size = new Size(350, 60),
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 2, 0, 2),
                BackColor = Color.White
            };

            // Date label
            Label lblDate = new Label
            {
                Text = entry.Date.ToString("dd/MM/yyyy"),
                Location = new Point(10, 5),
                Font = new Font("Arial", 10, FontStyle.Bold),
                AutoSize = true
            };

            // Tea leaves section (show only if weight > 0)
            if (entry.TeaLeavesWeight > 0)
            {
                Label lblTeaLabel = new Label
                {
                    Text = "තේ දළු:",
                    Location = new Point(10, 30),
                    Font = new Font("Arial", 9),
                    AutoSize = true
                };

                Label lblTeaWeight = new Label
                {
                    Text = $"{entry.TeaLeavesWeight} kg",
                    Location = new Point(100, 30),
                    Font = new Font("Arial", 9, FontStyle.Bold),
                    AutoSize = true,
                    ForeColor = Color.DarkGreen
                };

                datePanel.Controls.Add(lblTeaLabel);
                datePanel.Controls.Add(lblTeaWeight);
            }

            // Advance section (show only if advance > 0)
            if (entry.AdvanceAmount > 0)
            {
                Label lblAdvanceLabel = new Label
                {
                    Text = "අත්තිකාරම්:",
                    Location = new Point(180, 30),
                    Font = new Font("Arial", 9),
                    AutoSize = true
                };

                Label lblAdvanceValue = new Label
                {
                    Text = $"LKR {entry.AdvanceAmount:N2}",
                    Location = new Point(250, 30),
                    Font = new Font("Arial", 9, FontStyle.Bold),
                    AutoSize = true,
                    ForeColor = Color.DarkRed
                };

                datePanel.Controls.Add(lblAdvanceLabel);
                datePanel.Controls.Add(lblAdvanceValue);
            }

            datePanel.Controls.Add(lblDate);

            return datePanel;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                var invoice = DataAccess.GetInvoicesByInvoiceNumber(receiptNo).FirstOrDefault();
                if (invoice == null)
                {
                    MessageBox.Show("Invoice not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // Generate and save PDF
                var pdfHelper = new CreateInvoicePDF(invoice);
                string pdfFilePath = pdfHelper.GenerateInvoicePDF(receiptNo);

                // Show success message
                DialogResult result = MessageBox.Show("PDF generated successfully! Do you want to open it?",
                    "Success", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                     new System.Diagnostics.Process { StartInfo = new System.Diagnostics.ProcessStartInfo(pdfFilePath) { UseShellExecute = true } }.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating PDF: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }

    // Helper class for date-wise entries
    public class DateWiseEntry
    {
        public DateTime Date { get; set; }
        public decimal TeaLeavesWeight { get; set; }
        public decimal AdvanceAmount { get; set; }
    }

    // Assuming you have these data classes in your DataAccess layer
    // If not, you'll need to create them
    public class CollectionEntry
    {
        public DateTime Date { get; set; }
        public decimal Weight { get; set; }
        public int SupplierID { get; set; }
    }

    public class AdvanceEntry
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public int SupplierID { get; set; }
    }
}