using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using OfficeOpenXml;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;

namespace DesignUI
{
    public partial class HRM_Frm : Form
    {
        public HRM_Frm()
        {
            InitializeComponent();
        }
        MiniMartEntities db = new MiniMartEntities();
        private void LoadDataToDGV()
        {
            try
            {
                
                    // Truy vấn LINQ để lấy dữ liệu từ bảng SINH_VIEN
                    var sinhVienData = from nv in db.Staffs
                                       where nv.Hide == false
                                       select new
                                       {
                                           Id = nv.ID,
                                           Name = nv.Name,
                                           Role = nv.Role,
                                           Phone = nv.Phone,
                                           Salary = nv.BasicSalary,
                                           Allowance = nv.Allowance,
                                           Bonus =nv.Bonus,
                                       };

                    // Gán dữ liệu cho DataGridView dgvSinhVien
                    dgvStaff.DataSource = sinhVienData.Take(1000).ToList();

                // Đổi tên tiêu đề của các cột
                dgvStaff.Columns[0].Width = 85;
                dgvStaff.Columns[1].Width = 150;
                dgvStaff.Columns[2].Width = 125;
                dgvStaff.Columns[3].Width = 98;
                dgvStaff.Columns[4].Width = 100;
                

            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi chi tiết
                MessageBox.Show("Something went wrong while loading Staff Info!!.\n\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public bool ValidateNV()
        {
            if (txtId.Text.Length == 0)
            {
                MessageBox.Show("Id staff is empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (FindDuplicateNV())
            {
                MessageBox.Show("Id staff has existed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool FindDuplicateNV()
        {
            foreach (DataGridViewRow row in dgvStaff.Rows)
            {
                if (row == null) return false;
                else
                {
                    if (row.Cells["Id"].Value != null && row.Cells["Id"].Value.ToString() == txtId.Text)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private void AddDataToDatabase(string id, string name,string dt, string role, int BS, int A, int B)
        {
            // Tạo một đối tượng SINH_VIEN mới
            Staff newStaff = new Staff
            {
                ID = id,
                Name = name,
                Role = role,
                Phone = dt,
                BasicSalary = BS,
                Allowance = A,
                Bonus = B,
                Hide = false,

                // Nếu có thêm các trường khác, hãy thêm vào đây
            };

            // Thêm đối tượng mới vào DbSet và lưu vào cơ sở dữ liệu
            db.Staffs.Add(newStaff);
            db.SaveChanges();
        }
        private void btnAddNV_Click(object sender, EventArgs e)
        {
            if (ValidateNV())
            {
                // Lưu dữ liệu vào cơ sở dữ liệu
                int BS = Int32.Parse(txtBS.Text);
                int A = Int32.Parse(txtA.Text);
                int B = Int32.Parse(txtB.Text);
                AddDataToDatabase(txtId.Text, txtName.Text,txtPhone.Text,cmbRole.SelectedItem.ToString().Trim(),BS,A,B);
                //Load db
                LoadDataToDGV();

                // Xóa các trường dữ liệu
                clearNVFields();
            }
            else
            {
                MessageBox.Show("Fill important information please.");
            }
        }
        private void clearNVFields()
        {
            txtId.Clear();
            txtName.Clear();
            cmbRole.SelectedIndex = -1;
            txtPhone.Clear();
            txtBS.Text = "0";
            txtB.Text = "0";
            txtA.Text = "0";
            txtId.ReadOnly = false;
            btnAdd.Enabled = true;
        }
        private void HRM_Frm_Load(object sender, EventArgs e)
        {
            LoadDataToDGV();
            txtBS.Text = "0";
            txtB.Text = "0";
            txtA.Text = "0";
            AutoCompleteStringCollection sourceName = new AutoCompleteStringCollection();
            var listNames = db.Staffs.Select(x => x.ID).ToList(); 
            foreach (string name in listNames)
            {
                sourceName.Add(name);
                //MessageBox.Show(name.ToString());
            }

            txtId.AutoCompleteCustomSource = sourceName;
            txtId.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtId.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private void dgvStaff_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvStaff.Rows.Count)
            {
                DataGridViewRow selectedRow = dgvStaff.Rows[e.RowIndex];

                // Lấy thông tin từ dòng được chọn
                txtId.Text = selectedRow.Cells["Id"].Value.ToString();
                txtName.Text = selectedRow.Cells["Name"].Value.ToString();
                cmbRole.Text = selectedRow.Cells[2].Value.ToString();
                if (selectedRow.Cells["Phone"].Value == null) { txtPhone.Text = ""; }
                else txtPhone.Text= selectedRow.Cells["Phone"].Value.ToString();
                txtBS.Text = selectedRow.Cells["Salary"].Value.ToString();
                txtA.Text = selectedRow.Cells["Allowance"].Value.ToString();
                txtB.Text = selectedRow.Cells["Bonus"].Value.ToString();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string Id = txtId.Text;
            string name = txtName.Text;
            string role = cmbRole.Text.ToString().Trim();
            string phone = txtPhone.Text;
            int BS = Int32.Parse(txtBS.Text);
            int A = Int32.Parse(txtA.Text);
            int B = Int32.Parse(txtB.Text);

            // Kiểm tra xem MSSV có tồn tại trong cơ sở dữ liệu không
            Staff nhanvienUpdate = db.Staffs.FirstOrDefault(sv => sv.ID == Id);

            if (nhanvienUpdate != null)
            {
                // Cập nhật thông tin sinh viên
                nhanvienUpdate.Name = name;
                nhanvienUpdate.Role = role;
                nhanvienUpdate.Phone = phone;
                nhanvienUpdate.BasicSalary = BS;
                nhanvienUpdate.Allowance = A;
                nhanvienUpdate.Bonus = B;

                // Lưu thay đổi vào cơ sở dữ liệu
                db.SaveChanges();
                clearNVFields();
                MessageBox.Show("Update Staff successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataToDGV();

            }
            else
            {
                MessageBox.Show("Can't find Staff to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void DeleteStaff(string id)
        {
            

                // Lấy danh sách cần xóa
                Staff staffToDelete = db.Staffs.Where(sv => sv.ID == id).FirstOrDefault();
                staffToDelete.Hide = true;
                db.Entry(staffToDelete).State = System.Data.Entity.EntityState.Modified;
                // Lưu thay đổi vào cơ sở dữ liệu
                db.SaveChanges();

                MessageBox.Show("Delete succesfully.", "Announcement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataToDGV();
            
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (dgvStaff.SelectedRows.Count > 0)
                {
                   
                    foreach (DataGridViewRow selectedRow in dgvStaff.SelectedRows)
                    {
                        // Lấy thông tin từ dòng được chọn
                        string id = selectedRow.Cells["Id"].Value.ToString();

                        // Xóa từ cơ sở dữ liệu
                        DeleteStaff(id);
                    }
                    clearNVFields();
                    // Cập nhật lại dgvSinhVien sau khi xóa 
                    LoadDataToDGV();
                }
                else
                {
                    MessageBox.Show("Select staff to delete.", "Announcement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Something went wrong: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            clearNVFields();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            using(SaveFileDialog saveFileDialog = new SaveFileDialog())
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
                            for (int col = 1; col <= dgvStaff.Columns.Count; col++)
                            {
                                worksheet.Cells[1, col].Value = dgvStaff.Columns[col - 1].HeaderText;
                            }

                            // Write data from DataGridView to the worksheet
                            for (int row = 0; row < dgvStaff.Rows.Count; row++)
                            {
                                for (int col = 0; col < dgvStaff.Columns.Count; col++)
                                {
                                    worksheet.Cells[row + 2, col + 1].Value = dgvStaff.Rows[row].Cells[col].Value;
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text) == true)
            {
                if (cmbRole.SelectedIndex ==-1) 
                {
                    LoadDataToDGV();
                    clearNVFields();
                }
                else
                {
                    var sinhVienData = from nv in db.Staffs
                                       where nv.Hide == false && nv.Role == cmbRole.Text
                                       select new
                                       {
                                           Id = nv.ID,
                                           Name = nv.Name,
                                           Role = nv.Role,
                                           Phone = nv.Phone,
                                           Salary = nv.BasicSalary,
                                           Allowance = nv.Allowance,
                                           Bonus = nv.Bonus,
                                       };

                    // Gán dữ liệu cho DataGridView dgvSinhVien
                    dgvStaff.DataSource = sinhVienData.Take(1000).ToList();

                    // Đổi tên tiêu đề của các cột
                    dgvStaff.Columns[0].Width = 85;
                    dgvStaff.Columns[1].Width = 150;
                    dgvStaff.Columns[2].Width = 125;
                    dgvStaff.Columns[3].Width = 98;
                    dgvStaff.Columns[4].Width = 65;
                    clearNVFields();
                }    
            }
            else
            {
                if (cmbRole.SelectedIndex == -1)
                {
                    var sinhVienData = from nv in db.Staffs
                                       where nv.Hide == false && nv.ID == txtId.Text
                                       select new
                                       {
                                           Id = nv.ID,
                                           Name = nv.Name,
                                           Role = nv.Role,
                                           Phone = nv.Phone,
                                           Salary = nv.BasicSalary,
                                           Allowance = nv.Allowance,
                                           Bonus = nv.Bonus,
                                       };

                    // Gán dữ liệu cho DataGridView dgvSinhVien
                    dgvStaff.DataSource = sinhVienData.Take(1000).ToList();

                    // Đổi tên tiêu đề của các cột
                    dgvStaff.Columns[0].Width = 85;
                    dgvStaff.Columns[1].Width = 150;
                    dgvStaff.Columns[2].Width = 125;
                    dgvStaff.Columns[3].Width = 98;
                    dgvStaff.Columns[4].Width = 65;
                    clearNVFields();
                }
                else
                {
                    var sinhVienData = from nv in db.Staffs
                                       where nv.Hide == false && nv.Role == cmbRole.Text && nv.ID.Contains(txtId.Text)
                                       select new 
                                       {
                                           Id = nv.ID,
                                           Name = nv.Name,
                                           Role = nv.Role,
                                           Phone = nv.Phone,
                                           Salary = nv.BasicSalary,
                                           Allowance = nv.Allowance,
                                           Bonus = nv.Bonus,
                                       };

                    // Gán dữ liệu cho DataGridView dgvSinhVien
                    dgvStaff.DataSource = sinhVienData.Take(1000).ToList();

                    // Đổi tên tiêu đề của các cột
                    dgvStaff.Columns[0].Width = 85;
                    dgvStaff.Columns[1].Width = 150;
                    dgvStaff.Columns[2].Width = 125;
                    dgvStaff.Columns[3].Width = 98;
                    dgvStaff.Columns[4].Width = 65;
                    clearNVFields();
                }
            }
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtPhone.MaxLength = 10;
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtBS_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtA_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsLetter(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }
    }
}
