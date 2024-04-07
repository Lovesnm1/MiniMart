using DocumentFormat.OpenXml.ExtendedProperties;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace DesignUI
{
    public partial class StatisticsFollowInvoice : Form
    {
        public StatisticsFollowInvoice()
        {
            InitializeComponent();
        }
        MiniMartEntities db = new MiniMartEntities();
        private void LoadDataToDGV()
        {
            try
            {

                // Truy vấn LINQ để lấy dữ liệu 
                var InvoiceData = (from line in db.InvoiceDetails
                                  group line by line.ID_Invoice into g
                                  from s in g
                                  join a in db.Invoices on s.ID_Invoice equals a.ID
                                  where a.Hide == false
                                  orderby a.CreatedDate
                                  select new
                                  {
                                      Id = s.ID_Invoice,
                                      Total = g.Sum(pc => pc.Total).ToString(),
                                      CreatedDate = a.CreatedDate,
                                  }).Distinct();

                dgvInvoice.DataSource = InvoiceData.ToList();
                SumGoods();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong while loading Staff Info!!.\n\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StatisticsFollowInvoice_Load(object sender, EventArgs e)
        {
            LoadDataToDGV();
            btnLoc.Enabled = false;
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            txtID.Text = string.Empty;
            dtpBD.CustomFormat = " ";
            dtpKT.CustomFormat = " ";
            btnLoc.Enabled = false;
            LoadDataToDGV();
        }

        private void dtpBD_ValueChanged(object sender, EventArgs e)
        {
            dtpBD.CustomFormat = "yyyy-MM-dd";
            btnLoc.Enabled = true;
        }

        private void dtpBD_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                dtpBD.CustomFormat = " ";
            }
        }

        private void dtpKT_ValueChanged(object sender, EventArgs e)
        {
            dtpKT.CustomFormat = "yyyy-MM-dd";
            btnLoc.Enabled = true;
        }

        private void dtpKT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                dtpKT.CustomFormat = " ";
            }
        }

        private void txtID_TextChanged(object sender, EventArgs e)
        {
            btnLoc.Enabled = true;
        }
        private void SumGoods()
        {
            int sum = 0;
            foreach (DataGridViewRow row in dgvInvoice.Rows)
            {
                sum = sum + Int32.Parse(row.Cells["Total"].Value.ToString());
            }
            txtAmount.Text = sum.ToString();
        }
        private void btnLoc_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text) != true)
            {
                if (dtpBD.Text == " " && dtpKT.Text == " ")
                {
                    string id = txtID.Text;
                    var InvoiceData = (from line in db.InvoiceDetails
                                       group line by line.ID_Invoice into g
                                       from s in g
                                       join a in db.Invoices on s.ID_Invoice equals a.ID
                                       where a.Hide == false && a.ID.Contains(id)
                                       select new
                                       {
                                           Id = s.ID_Invoice,
                                           Total = g.Sum(pc => pc.Total).ToString(),
                                           CreatedDate = a.CreatedDate,
                                       }).Distinct();

                    dgvInvoice.DataSource = InvoiceData.ToList();
                    SumGoods();
                }
                else if (dtpBD.Text != " " && dtpKT.Text == " ")
                {
                    string id = txtID.Text;
                    DateTime BD = Convert.ToDateTime(dtpBD.Text);
                    var InvoiceData = (from line in db.InvoiceDetails
                                       group line by line.ID_Invoice into g
                                       from s in g
                                       join a in db.Invoices on s.ID_Invoice equals a.ID
                                       where a.Hide == false && a.ID.Contains(id) && a.CreatedDate >=BD
                                       select new
                                       {
                                           Id = s.ID_Invoice,
                                           Total = g.Sum(pc => pc.Total).ToString(),
                                           CreatedDate = a.CreatedDate,
                                       }).Distinct();

                    dgvInvoice.DataSource = InvoiceData.ToList();
                    SumGoods();
                }    
                else if (dtpBD.Text == " " && dtpKT.Text != " ")
                {
                    string id = txtID.Text;
                    DateTime KT = Convert.ToDateTime(dtpKT.Text);
                    var InvoiceData = (from line in db.InvoiceDetails
                                       group line by line.ID_Invoice into g
                                       from s in g
                                       join a in db.Invoices on s.ID_Invoice equals a.ID
                                       where a.Hide == false && a.ID.Contains(id) && a.CreatedDate <=KT
                                       select new
                                       {
                                           Id = s.ID_Invoice,
                                           Total = g.Sum(pc => pc.Total).ToString(),
                                           CreatedDate = a.CreatedDate,
                                       }).Distinct();

                    dgvInvoice.DataSource = InvoiceData.ToList();
                    SumGoods();
                }  
                else if (dtpBD.Text != " " && dtpKT.Text != " ")
                {
                    string id = txtID.Text;
                    DateTime BD = Convert.ToDateTime(dtpBD.Text);
                    DateTime KT = Convert.ToDateTime(dtpKT.Text);
                    var InvoiceData = (from line in db.InvoiceDetails
                                       group line by line.ID_Invoice into g
                                       from s in g
                                       join a in db.Invoices on s.ID_Invoice equals a.ID
                                       where a.Hide == false && a.ID.Contains(id) && a.CreatedDate <= KT && a.CreatedDate >=BD
                                       select new
                                       {
                                           Id = s.ID_Invoice,
                                           Total = g.Sum(pc => pc.Total).ToString(),
                                           CreatedDate = a.CreatedDate,
                                       }).Distinct();

                    dgvInvoice.DataSource = InvoiceData.ToList();
                    SumGoods();
                }    

            }   
            else
            {
                if (dtpBD.Text == " " && dtpKT.Text == " ")
                {
                    string id = txtID.Text;
                    var InvoiceData = (from line in db.InvoiceDetails
                                       group line by line.ID_Invoice into g
                                       from s in g
                                       join a in db.Invoices on s.ID_Invoice equals a.ID
                                       where a.Hide == false 
                                       select new
                                       {
                                           Id = s.ID_Invoice,
                                           Total = g.Sum(pc => pc.Total).ToString(),
                                           CreatedDate = a.CreatedDate,
                                       }).Distinct();

                    dgvInvoice.DataSource = InvoiceData.ToList();
                    SumGoods();
                }
                else if (dtpBD.Text != " " && dtpKT.Text == " ")
                {
                    string id = txtID.Text;
                    DateTime BD = Convert.ToDateTime(dtpBD.Text);
                    var InvoiceData = (from line in db.InvoiceDetails
                                       group line by line.ID_Invoice into g
                                       from s in g
                                       join a in db.Invoices on s.ID_Invoice equals a.ID
                                       where a.Hide == false &&  a.CreatedDate >= BD
                                       select new
                                       {
                                           Id = s.ID_Invoice,
                                           Total = g.Sum(pc => pc.Total).ToString(),
                                           CreatedDate = a.CreatedDate,
                                       }).Distinct();

                    dgvInvoice.DataSource = InvoiceData.ToList();
                    SumGoods();
                }
                else if (dtpBD.Text == " " && dtpKT.Text != " ")
                {
                    string id = txtID.Text;
                    DateTime KT = Convert.ToDateTime(dtpKT.Text);
                    var InvoiceData = (from line in db.InvoiceDetails
                                       group line by line.ID_Invoice into g
                                       from s in g
                                       join a in db.Invoices on s.ID_Invoice equals a.ID
                                       where a.Hide == false &&  a.CreatedDate <= KT
                                       select new
                                       {
                                           Id = s.ID_Invoice,
                                           Total = g.Sum(pc => pc.Total).ToString(),
                                           CreatedDate = a.CreatedDate,
                                       }).Distinct();

                    dgvInvoice.DataSource = InvoiceData.ToList();
                    SumGoods();
                }
                else if (dtpBD.Text != " " && dtpKT.Text != " ")
                {
                    string id = txtID.Text;
                    DateTime BD = Convert.ToDateTime(dtpBD.Text);
                    DateTime KT = Convert.ToDateTime(dtpKT.Text);
                    var InvoiceData = (from line in db.InvoiceDetails
                                       group line by line.ID_Invoice into g
                                       from s in g
                                       join a in db.Invoices on s.ID_Invoice equals a.ID
                                       where a.Hide == false &&  a.CreatedDate <= KT && a.CreatedDate >= BD
                                       select new
                                       {
                                           Id = s.ID_Invoice,
                                           Total = g.Sum(pc => pc.Total).ToString(),
                                           CreatedDate = a.CreatedDate,
                                       }).Distinct();

                    dgvInvoice.DataSource = InvoiceData.ToList();
                    SumGoods();
                }
            }    
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Excel Files|*.xlsx|All Files|*.*";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get the selected file path
                    string filePath = saveFileDialog.FileName;

                    try
                    {
                        // Create a new Excel package
                        using (ExcelPackage package = new ExcelPackage())
                        {
                            // Add a new worksheet to the Excel package
                            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("SinhVienData");

                            // Write headers to the worksheet
                            for (int col = 1; col <= dgvInvoice.Columns.Count; col++)
                            {
                                worksheet.Cells[1, col].Value = dgvInvoice.Columns[col - 1].HeaderText;
                            }

                            // Write data from DataGridView to the worksheet
                            for (int row = 0; row < dgvInvoice.Rows.Count; row++)
                            {
                                for (int col = 0; col < dgvInvoice.Columns.Count; col++)
                                {
                                    worksheet.Cells[row + 2, col + 1].Value = dgvInvoice.Rows[row].Cells[col].Value;
                                }
                            }
                            worksheet.Cells[dgvInvoice.Rows.Count + 3, dgvInvoice.Columns.Count + 1].Value = "TotalAmount";
                            worksheet.Cells[dgvInvoice.Rows.Count + 3, dgvInvoice.Columns.Count + 2].Value = txtAmount.Text;
                            worksheet.Cells.AutoFitColumns();
                            // Save the Excel package to the selected file
                            package.SaveAs(new FileInfo(filePath));

                            MessageBox.Show("Export successful.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error exporting data to Excel: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
