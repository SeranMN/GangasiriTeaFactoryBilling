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
        public LineData Result { get; private set; }

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

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                Result = new LineData
                {
                    LineName = txtLineName.Text,
                    Description = txtDescription.Text,
                    TransportFee = decimal.Parse(txtTransportFee.Text)
                };

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
