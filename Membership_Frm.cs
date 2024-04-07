using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.VariantTypes;
using Microsoft.Office.Interop.Excel;
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
using System.Windows.Forms;

namespace DesignUI
{
    public partial class Membership_Frm : Form
    {
        public Membership_Frm()
        {
            InitializeComponent();
        }
        MiniMartEntities db = new MiniMartEntities();
        private void LoadDataToDGV()
        {
            try
            {

                // Truy vấn LINQ để lấy dữ liệu từ bảng SINH_VIEN
                var shiftData = from nv in db.Memberships
                                orderby nv.ID
                                where nv.Hide == false
                                select new
                                {
                                    ID = nv.ID,
                                    Name = nv.Name,
                                    Address = nv.Address,
                                    Phone = nv.Phone,
                                    Points = nv.Points,

                                };

                // Gán dữ liệu cho DataGridView dgvSinhVien
                dgvMembershift.DataSource = shiftData.Take(1000).ToList();
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi chi tiết
                MessageBox.Show("Something went wrong while loading Membership Info!!.\n\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Membership_Frm_Load(object sender, EventArgs e)
        {
            LoadDataToDGV();
            txtPoints.Text = "0";
        }
        private void AddDataToDatabase(string id, string name, string address, string phone, int points)
        {

            // Tạo một đối tượng SINH_VIEN mới
            Membership newMember = new Membership
            {
                ID = id,
                Name = name,
                Address = address,
                Phone = phone,
                Points = points,
                Hide = false,
                // Nếu có thêm các trường khác, hãy thêm vào đây
            };

            // Thêm đối tượng mới vào DbSet và lưu vào cơ sở dữ liệu
            db.Memberships.Add(newMember);
            db.SaveChanges();
            LoadDataToDGV();
        }
        private void AddDataToDatabase(string id, string name, string phone, int points)
        {

            // Tạo một đối tượng SINH_VIEN mới
            Membership newMember = new Membership
            {
                ID = id,
                Name = name,
                Phone = phone,
                Points = points,
                Hide = false,
                // Nếu có thêm các trường khác, hãy thêm vào đây
            };

            // Thêm đối tượng mới vào DbSet và lưu vào cơ sở dữ liệu
            db.Memberships.Add(newMember);
            db.SaveChanges();
            LoadDataToDGV();
        }
        public bool FindDuplicate()
        {
            Membership check = db.Memberships.FirstOrDefault(sv => sv.ID == txtId.Text || sv.Phone == txtPhone.Text);
            if (check != null) { return true; }       
            return false;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtId.Text == "" || txtPhone.Text == "" || txtName.Text == "")
            {
                MessageBox.Show("Need to fill Id,Phone,Name!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (FindDuplicate())
                {
                    MessageBox.Show("Has existed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (string.IsNullOrEmpty(txtAddress.Text) == true)
                    {
                        AddDataToDatabase(txtId.Text, txtName.Text, txtPhone.Text, Int32.Parse(txtPoints.Text));
                    }
                    else AddDataToDatabase(txtId.Text, txtName.Text,txtAddress.Text, txtPhone.Text, Int32.Parse(txtPoints.Text));
                    MessageBox.Show("Add Member successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }    
            }    
        }

        private void dgvMembershift_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvMembershift.Rows.Count)
            {
                DataGridViewRow selectedRow = dgvMembershift.Rows[e.RowIndex];

                // Lấy thông tin từ dòng được chọn
                txtId.ReadOnly = true;
                txtPhone.ReadOnly = true;
                txtId.Text = selectedRow.Cells["ID"].Value.ToString();
                txtName.Text = selectedRow.Cells["Name"].Value.ToString();
                if (selectedRow.Cells["Address"].Value == null) { txtAddress.Text = ""; }              
                else txtAddress.Text = selectedRow.Cells["Address"].Value.ToString();
                txtPhone.Text = selectedRow.Cells["Phone"].Value.ToString();
                txtPoints.Text = selectedRow.Cells["Points"].Value.ToString();
            }
        }
        private void clearNVFields()
        {
            txtId.Clear();
            txtName.Clear();
            txtAddress.Clear();
            txtPhone.Clear();
            txtPoints.Text = "0";
            txtId.ReadOnly = false;
            btnAdd.Enabled = true;
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string Id = txtId.Text;
            string name = txtName.Text;
            string Address = txtAddress.Text.Trim();
            string phone = txtPhone.Text;
            int points = Int32.Parse(txtPoints.Text);

            // Kiểm tra xem MSSV có tồn tại trong cơ sở dữ liệu không
            Membership nhanvienUpdate = db.Memberships.FirstOrDefault(sv => sv.ID == Id);

            if (nhanvienUpdate != null)
            {
                // Cập nhật thông tin sinh viên
                nhanvienUpdate.Name = name;
                nhanvienUpdate.Address = Address;
                nhanvienUpdate.Points = points;

                // Lưu thay đổi vào cơ sở dữ liệu
                db.SaveChanges();
                clearNVFields();
                MessageBox.Show("Update Member successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataToDGV();

            }
            else
            {
                MessageBox.Show("Can't find Member to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void DeleteStaff(string id)
        {


            // Lấy danh sách cần xóa
            Membership staffToDelete = db.Memberships.Where(sv => sv.ID == id).FirstOrDefault();
            staffToDelete.Hide = true;
            db.Entry(staffToDelete).State = System.Data.Entity.EntityState.Modified;
            // Lưu thay đổi vào cơ sở dữ liệu
            db.SaveChanges();
            MessageBox.Show("Delete succesfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadDataToDGV();

        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {

                if (dgvMembershift.SelectedRows.Count > 0)
                {

                    foreach (DataGridViewRow selectedRow in dgvMembershift.SelectedRows)
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
                    MessageBox.Show("Select Member to delete.", "Announcement", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            LoadDataToDGV();
        }

        private void btnExcel_Click(object sender, EventArgs e)
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
                            for (int col = 1; col <= dgvMembershift.Columns.Count; col++)
                            {
                                worksheet.Cells[1, col].Value = dgvMembershift.Columns[col - 1].HeaderText;
                            }

                            // Write data from DataGridView to the worksheet
                            for (int row = 0; row < dgvMembershift.Rows.Count; row++)
                            {
                                for (int col = 0; col < dgvMembershift.Columns.Count; col++)
                                {
                                    worksheet.Cells[row + 2, col + 1].Value = dgvMembershift.Rows[row].Cells[col].Value;
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
                if (string.IsNullOrEmpty(txtPhone.Text) == true)
                {
                    LoadDataToDGV();
                    clearNVFields();
                }
                else
                {
                    var shiftData = from nv in db.Memberships
                                    orderby nv.ID
                                    where nv.Hide == false && nv.Phone.Contains(txtPhone.Text)
                                    select new
                                    {
                                        ID = nv.ID,
                                        Name = nv.Name,
                                        Address = nv.Address,
                                        Phone = nv.Phone,
                                        Points = nv.Points,

                                    };
                    dgvMembershift.DataSource = shiftData.Take(1000).ToList();
                    clearNVFields();
                }
            }
            else
            {
                if (string.IsNullOrEmpty(txtPhone.Text) == true)
                {
                    var shiftData = from nv in db.Memberships
                                    orderby nv.ID
                                    where nv.Hide == false && nv.ID.Contains(txtId.Text)
                                    select new
                                    {
                                        ID = nv.ID,
                                        Name = nv.Name,
                                        Address = nv.Address,
                                        Phone = nv.Phone,
                                        Points = nv.Points,

                                    };
                    dgvMembershift.DataSource = shiftData.Take(1000).ToList();
                    clearNVFields();
                }
                else
                {
                    var shiftData = from nv in db.Memberships
                                    orderby nv.ID
                                    where nv.Hide == false && nv.Phone.Contains(txtPhone.Text) && nv.ID.Contains(txtId.Text)
                                    select new
                                    {
                                        ID = nv.ID,
                                        Name = nv.Name,
                                        Address = nv.Address,
                                        Phone = nv.Phone,
                                        Points = nv.Points,

                                    };
                    dgvMembershift.DataSource = shiftData.Take(1000).ToList();
                    clearNVFields();
                }
            }
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtPoints_KeyPress(object sender, KeyPressEventArgs e)
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
