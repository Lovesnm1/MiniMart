using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using OfficeOpenXml;
using Excel = Microsoft.Office.Interop.Excel;

namespace DesignUI
{
    public partial class Hotline_Frm : Form
    {
        public Hotline_Frm()
        {
            InitializeComponent();
        }
        MiniMartEntities db = new MiniMartEntities();
        private void ImportExcel(string path)
        {
            using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(path)))
            {
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets[0];
                DataTable dt = new DataTable();
                for ( int i = excelWorksheet.Dimension.Start.Column; i<=excelWorksheet.Dimension.End.Column;i++)
                {
                    dt.Columns.Add(excelWorksheet.Cells[1, i].Value.ToString());
                }
                for (int i = excelWorksheet.Dimension.Start.Row+1;i<=excelWorksheet.Dimension.End.Row;i++)
                {
                    List<string> listRows = new List<string>();
                    for (int j = excelWorksheet.Dimension.Start.Column;j<=excelWorksheet.Dimension.End.Column;j++)
                    {
                        listRows.Add(excelWorksheet.Cells[i,j].Value.ToString());
                    }
                    dt.Rows.Add(listRows.ToArray());
                }
                dgvHotLine.DataSource = dt;
            }
        }
        private void Hotline_Frm_Load(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Import Excel";
            openFileDialog.Filter = "Excel Files|*.xlsx|All Files|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ImportExcel(openFileDialog.FileName);
                    MessageBox.Show("Import File Successfully");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Import File Error\n" + ex.Message);
                }
            }    
        }

        private void dgvHotLine_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvHotLine.Rows.Count)
            {
                DataGridViewRow selectedRow = dgvHotLine.Rows[e.RowIndex];

                // Lấy thông tin từ dòng được chọn
                txtPhone.ReadOnly = true;
                txtPhone.Text = selectedRow.Cells["Phone"].Value.ToString();
                cmbStatus.Text = selectedRow.Cells["Status"].Value.ToString();               
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtPhone.Text = "";
            cmbStatus.SelectedIndex = -1;
            txtPhone.ReadOnly = false;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtPhone.Text == "")
            {
                MessageBox.Show("Nothing to Update");
            }
            else
            {
                foreach (DataGridViewRow row in dgvHotLine.Rows)
                {
                    string phone = row.Cells["Phone"].Value.ToString();
                    if (phone == txtPhone.Text)
                    {
                        row.Cells["Status"].Value = cmbStatus.Text;
                        txtPhone.Text = "";
                        cmbStatus.SelectedIndex = -1;
                        txtPhone.ReadOnly = false;
                        break;
                    }    
                }
            }
              
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtPhone.Text == "")
            {
                MessageBox.Show("Nothing to Delete");
            }
            else
            {
                foreach (DataGridViewRow row in dgvHotLine.Rows)
                {
                    string phone = row.Cells["Phone"].Value.ToString();
                    if (phone == txtPhone.Text)
                    {
                        dgvHotLine.Rows.Remove(row);
                        txtPhone.Text = "";
                        cmbStatus.SelectedIndex = -1;
                        txtPhone.ReadOnly = false;
                        break;
                    }
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
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
                            for (int col = 1; col <= dgvHotLine.Columns.Count; col++)
                            {
                                worksheet.Cells[1, col].Value = dgvHotLine.Columns[col - 1].HeaderText;
                            }

                            // Write data from DataGridView to the worksheet
                            for (int row = 0; row < dgvHotLine.Rows.Count; row++)
                            {
                                for (int col = 0; col < dgvHotLine.Columns.Count; col++)
                                {
                                    worksheet.Cells[row + 2, col + 1].Value = dgvHotLine.Rows[row].Cells[col].Value;
                                }
                            }
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
