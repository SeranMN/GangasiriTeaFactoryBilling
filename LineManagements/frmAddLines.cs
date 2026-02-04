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

namespace GangasiriTeaFactoryBilling.LineManagements
{
    public partial class frmAddLines : Form
    {

        public bool IsEditMode { get; set; } = false;
        public string LineId { get; set; }
        public Line Result { get; private set; }

        public class LineData
        {
            public string LineName { get; set; }
            public string Description { get; set; }
            public decimal TransportFee { get; set; }
        }
        public frmAddLines()
        {
            InitializeComponent();
        }

        public void LoadLineData(string lineName, string description, decimal transportFee)
        {
            txtLineName.Text = lineName;
            txtDescription.Text = description;
            txtTransportFee.Text = transportFee.ToString("N2");
        }

        public void SetEditMode(string lineId, string name, string description, decimal fee)
        {
             IsEditMode = true;
             LineId = lineId;
             LoadLineData(name, description, fee);
             
             // Update UI
             if (btnSave != null) btnSave.Text = "💾 Update Line";
             this.Text = "✏️ Edit Line - " + lineId;
             
             // Update title label if accessible, or relying on form title
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // DEBUG: Check state
            // MessageBox.Show($"Debug: IsEditMode={IsEditMode}, LineId={LineId}", "Debug Info");

            if (ValidateForm())
            {
                Result = new Line
                {
                    LineName = txtLineName.Text,
                    Description = txtDescription.Text,
                    TransportFee = decimal.Parse(txtTransportFee.Text),
                    Status = "Active"
                };
                if (IsEditMode)
                {
                    Result.LineID = LineId;
                    bool success = DataAccess.UpdateLine(Result);
                    if (!success)
                    {
                        MessageBox.Show("Failed to update line. It may have been deleted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    string newId = "L-" + (DataAccess.GetCounts("Lines")+1).ToString("D3");
                    Result.LineID = newId;
                    DataAccess.AddLine(Result);
                }
                    
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtLineName.Text))
            {
                MessageBox.Show("Please enter line name", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLineName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTransportFee.Text))
            {
                MessageBox.Show("Please enter transport fee", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTransportFee.Focus();
                return false;
            }

            if (!decimal.TryParse(txtTransportFee.Text, out decimal fee) || fee < 0)
            {
                MessageBox.Show("Please enter a valid transport fee", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTransportFee.Focus();
                return false;
            }

            return true;
        }
    }
}
