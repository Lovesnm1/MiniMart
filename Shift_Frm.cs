using DocumentFormat.OpenXml.Wordprocessing;
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
using System.Xml.Linq;
using System.IO;
using OfficeOpenXml;


namespace DesignUI
{
    public partial class Shift_Frm : Form
    {
        public Shift_Frm()
        {
            InitializeComponent();
        }
        MiniMartEntities db = new MiniMartEntities();
        private void LoadDataToDGV()
        {
            try
            {

                // Truy vấn LINQ để lấy dữ liệu từ bảng SINH_VIEN
                var shiftData = from nv in db.Shifts
                                orderby nv.Date
                                select new
                                {
                                    Date = nv.Date,
                                    Shift = nv.Shift1,
                                    Counter = nv.Counter,
                                    Id = nv.ID_Cashier,

                                };

                // Gán dữ liệu cho DataGridView dgvSinhVien
                dgvShift.DataSource = shiftData.Take(1000).ToList();
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi chi tiết
                MessageBox.Show("Something went wrong while loading Shift Info!!.\n\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadCmbId()
        {
            try
            {

                var Id = from k in db.Staffs
                           where k.Role == "Cashier"
                           select new
                           {
                               Id = k.ID,
                           };
                cmbId.DataSource = Id.ToList();
                cmbId.ValueMember = "Id";
                cmbId.SelectedIndex = -1;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Loading Id:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Shift_Frm_Load(object sender, EventArgs e)
        {
            LoadDataToDGV();
            LoadCmbId();
        }
        DateTime dt;
        private void clearFields()
        {
            dtpDate.Value = new DateTime(2025, 12, 30);
            dtpDate.CustomFormat = " ";
            cmbShift.Text = "";
            cmbCounter.Text = "";
            cmbId.Text = "";

        }
        public bool FindDuplicate()
        {
            foreach (DataGridViewRow row in dgvShift.Rows)
            {
                if (row == null) return false;
                else
                {
                    if ( row.Cells["Date"].Value.ToString() == dtpDate.Text
                       && row.Cells["Shift"].Value.ToString() == cmbShift.Text
                        && row.Cells["Counter"].Value.ToString() == cmbCounter.Text)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool ValidateShift()
        {
           
            if (FindDuplicate())
            {
                MessageBox.Show("Id staff has existed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else
            {
                return true;
            }
        }
        private void AddDataToDatabase(string date, string shift, string counter, string id)
        {
            
            // Tạo một đối tượng SINH_VIEN mới
            Shift newShift = new Shift
            {
                Date = Convert.ToDateTime(date),
                Shift1 = shift,
                Counter = counter,
                ID_Cashier = id,

                // Nếu có thêm các trường khác, hãy thêm vào đây
            };

            // Thêm đối tượng mới vào DbSet và lưu vào cơ sở dữ liệu
            db.Shifts.Add(newShift);
            db.SaveChanges();
        }
        private void AddDataToDatabase(string date, string shift, string counter)
        {

            // Tạo một đối tượng SINH_VIEN mới
            Shift newShift = new Shift
            {
                Date = Convert.ToDateTime(date),
                Shift1 = shift,
                Counter = counter,
                

                // Nếu có thêm các trường khác, hãy thêm vào đây
            };

            // Thêm đối tượng mới vào DbSet và lưu vào cơ sở dữ liệu
            db.Shifts.Add(newShift);
            db.SaveChanges();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (dtpDate.Text == " " || cmbShift.Text=="" || cmbCounter.Text == "")
            {
                MessageBox.Show("Need to fill Date,Shift,Counter!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            
            }
            else
            {   if (FindDuplicate())
                {
                    MessageBox.Show("Has existed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else
                {  // Lưu dữ liệu vào cơ sở dữ liệu
                    if (cmbId.SelectedIndex == -1)
                    {
                        dt = Convert.ToDateTime(dtpDate.Text);
                        AddDataToDatabase(dtpDate.Text, cmbShift.SelectedItem.ToString(), cmbCounter.SelectedItem.ToString());
                    }
                    else AddDataToDatabase(dtpDate.Text, cmbShift.SelectedItem.ToString(), cmbCounter.SelectedItem.ToString(), cmbId.SelectedValue.ToString());
                    //Load db
                    LoadDataToDGV();

                    // Xóa các trường dữ liệu
                    clearFields();
                }
            }

        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            dtpDate.CustomFormat = "yyyy-MM-dd";          
        }

        private void dtpDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                dtpDate.CustomFormat = " ";
            }
        }

        private void dgvShift_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cmbId.Text = "";
            if (e.RowIndex >= 0 && e.RowIndex < dgvShift.Rows.Count)
            {
                DataGridViewRow selectedRow = dgvShift.Rows[e.RowIndex];

                // Lấy thông tin từ dòng được chọn
                dtpDate.Text = selectedRow.Cells["Date"].Value.ToString();
                cmbShift.Text = selectedRow.Cells["Shift"].Value.ToString();
                cmbCounter.Text = selectedRow.Cells["Counter"].Value.ToString();
                if (selectedRow.Cells["Id"].Value == null) { }
                else cmbId.Text = selectedRow.Cells["Id"].Value.ToString();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            DateTime date = Convert.ToDateTime(dtpDate.Text);
            string shift = cmbShift.Text;
            string counter = cmbCounter.Text;
            string Id = cmbId.Text;

            Shift ShiftUpdate = db.Shifts.FirstOrDefault(sv => sv.Date == date && sv.Shift1 == shift && sv.Counter==counter );
            if (ShiftUpdate != null)
            {
                dt = Convert.ToDateTime(dtpDate.Text);

                ShiftUpdate.ID_Cashier = Id;

                // Lưu thay đổi vào cơ sở dữ liệu
                db.SaveChanges();
                clearFields();
                MessageBox.Show("Update Shift successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataToDGV();

            }
            else
            {
                MessageBox.Show("Can't find Shift to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void DeleteShift(DateTime date, string shift, string counter)
        {
            // Lấy danh sách cần xóa
            
            Shift shiftToDelete = db.Shifts.Where(sv => sv.Date == date && sv.Shift1 == shift && sv.Counter == counter).FirstOrDefault();
            db.Entry(shiftToDelete).State = System.Data.Entity.EntityState.Modified;
            db.Shifts.Remove(shiftToDelete);
            // Lưu thay đổi vào cơ sở dữ liệu
            db.SaveChanges();

            MessageBox.Show("Delete succesfully.", "Announcement", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadDataToDGV();

        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {

                if (dgvShift.SelectedRows.Count > 0)
                {

                    foreach (DataGridViewRow selectedRow in dgvShift.SelectedRows)
                    {
                        // Lấy thông tin từ dòng được chọn
                        DateTime date = Convert.ToDateTime(dtpDate.Text);
                        string shift = cmbShift.Text;
                        string counter = cmbCounter.Text;
                        // Xóa từ cơ sở dữ liệu
                        DeleteShift(date,shift,counter);
                    }
                    dtpDate.CustomFormat= " ";
                    cmbShift.Text = "";
                    cmbCounter.Text = "";
                    cmbId.Text = "";
                    // Cập nhật lại dgvSinhVien sau khi xóa 
                    LoadDataToDGV();
                    clearFields();
                }
                else
                {
                    MessageBox.Show("Select shift to delete.", "Announcement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Something went wrong: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (dtpDate.Text != " ")
            {
                DateTime now = Convert.ToDateTime(dtpDate.Text);
                var shiftData = from nv in db.Shifts
                                orderby nv.Date,nv.Shift1,nv.Counter
                                where nv.Date == now
                                select new
                                {
                                    Date = nv.Date,
                                    Shift = nv.Shift1,
                                    Counter = nv.Counter,
                                    Id = nv.ID_Cashier,

                                };

                // Gán dữ liệu cho DataGridView dgvSinhVien
                dgvShift.DataSource = shiftData.Take(1000).ToList();
                clearFields();
            }    
            else
            {
                LoadDataToDGV();
                clearFields();
            }    
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dtpDate.Value = new DateTime(2025,12,30);
            dtpDate.CustomFormat = " ";
            cmbShift.Text = "";
            cmbCounter.Text = "";
            cmbId.Text = "";
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
                            for (int col = 1; col <= dgvShift.Columns.Count; col++)
                            {
                                worksheet.Cells[1, col].Value = dgvShift.Columns[col - 1].HeaderText;
                            }

                            // Write data from DataGridView to the worksheet
                            for (int row = 0; row < dgvShift.Rows.Count; row++)
                            {
                                for (int col = 0; col < dgvShift.Columns.Count; col++)
                                {
                                    worksheet.Cells[row + 2, col + 1].Value = dgvShift.Rows[row].Cells[col].Value;
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
