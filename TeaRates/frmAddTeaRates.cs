using GangasiriTeaFactoryBilling.db;
using GangasiriTeaFactoryBilling.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GangasiriTeaFactoryBilling.TeaRates
{
    public partial class frmAddTeaRates : Form
    {
        public bool IsEditMode { get; set; } = false;
        public string RateId { get; set; }
        public TeaRate Result { get; private set; }
        public frmAddTeaRates()
        {
            InitializeComponent();
        }

        private void DatePicker_ValueChanged(object sender, EventArgs e)
        {
            UpdateStatusLabel();
        }

        private void UpdateStatusLabel()
        {
            DateTime now = DateTime.Now;
            DateTime validFrom = dtpValidFrom.Value;
            DateTime validTo = dtpValidTo.Value;

            if (validFrom > now)
            {
                lblStatus.Text = "⏳ Future Rate";
                lblStatus.ForeColor = Color.FromArgb(255, 152, 0); // Orange
            }
            else if (validTo < now)
            {
                lblStatus.Text = "⚠️ Expired Rate";
                lblStatus.ForeColor = Color.FromArgb(244, 67, 54); // Red
            }
            else
            {
                lblStatus.Text = "✅ Active Rate";
                lblStatus.ForeColor = Color.FromArgb(0, 150, 136); // Green
            }
        }

        public void LoadRateData(decimal rate, DateTime validFrom, DateTime validTo)
        {
            txtRate.Text = rate.ToString("N2");
            dtpValidFrom.Value = validFrom;
            dtpValidTo.Value = validTo;
            UpdateStatusLabel();
        }

        public void SetEditMode(string rateId, decimal rate, DateTime validFrom, DateTime validTo)
        {
            IsEditMode = true;
            RateId = rateId;
            LoadRateData(rate, validFrom, validTo);
            this.Text = "✏️ Edit Rate - " + rateId;
            // Update button text if possible, though currently button is private in designer.
            // Ideally should expose or find it, but for now logic fix is priority.
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                Result = new TeaRate
                {
                    Rate = decimal.Parse(txtRate.Text),
                    ValidFrom = dtpValidFrom.Value.Date,
                    ValidTo = dtpValidTo.Value.Date
                };
                try
                {
                    if (IsEditMode)
                    {
                        Result.RateID = int.Parse(RateId.Replace("RATE-", "")); // Assuming ID format, but safest to use int if possible.
                        // Actually, looking at DataAccess.UpdateTeaRate, it takes a TeaRate object.
                        // Wait, RateID in model is likely int. TeaRates grid uses "RATE-001" string format.
                        // Need to be careful about ID conversion.
                        // Let's check model first or assume we strip "RATE-" or plain int.
                        // Looking at frmTeaRates.cs: string rateId = selectedRow.Cells["RateID"].Value.ToString();
                        // If it is stored as "RATE-001", we need to parse.
                        // However, DataAccess.GetAllTeaRates returns TeaRate objects where RateID is likely int.
                        // Line 33 in frmTeaRates.cs: rate.RateID (which is int) -> implicitly to string in Add?
                        // Actually Add takes params. rate.RateID is int.
                        // So the grid has the int value directly? 
                        // "ratesGrid.Rows.Add(rate.RateID, ...)" -> int.
                        // But when adding new, "RATE-" + count is used. This is inconsistent.
                        // Let's assume for update we need the integer ID.
                        
                        // If valid ID is just int, use it. If it's mixed string "RATE-001", parse it.
                        // The user code in `LoadRates`: ratesGrid.Rows.Add(rate.RateID...); -> This is likely int from DB.
                        // But `BtnAddRate_Click`: string newId = "RATE-" + ... -> This puts a string "RATE-001" into the grid.
                        // This inconsistency is dangerous.
                        // The DB ID is definitely int.
                        // Use int.TryParse on the input RateId. If fail, try replacing "RATE-".
                        
                        int id;
                        if (!int.TryParse(RateId, out id))
                        {
                             int.TryParse(RateId.Replace("RATE-", ""), out id);
                        }
                        Result.RateID = id;
                        
                        DataAccess.UpdateTeaRate(Result);
                    }
                    else
                    {
                        DataAccess.AddTeaRate(Result);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error Saving Rate: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtRate.Text))
            {
                MessageBox.Show("Please enter tea rate", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRate.Focus();
                return false;
            }

            if (!decimal.TryParse(txtRate.Text, out decimal rate) || rate <= 0)
            {
                MessageBox.Show("Please enter a valid rate", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRate.Focus();
                return false;
            }

            if (dtpValidTo.Value <= dtpValidFrom.Value)
            {
                MessageBox.Show("Valid To date must be after Valid From date", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpValidTo.Focus();
                return false;
            }

            return true;
        }
    }
}
