using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.VariantTypes;
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
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Windows.Interop;
using static ClosedXML.Excel.XLPredefinedFormat;

namespace DesignUI
{
    public partial class Goods_Frm : Form
    {
        public Goods_Frm()
        {
            InitializeComponent();
        }
        MiniMartEntities db = new MiniMartEntities();
        private void LoadDataToDGV()
        {
            try
            {

                // Truy vấn LINQ để lấy dữ liệu từ bảng SINH_VIEN
                var GoodsData = from nv in db.Goods
                                orderby nv.ID
                                where nv.Hide == false
                                select new
                                {
                                    ID = nv.ID,
                                    Name = nv.Name,
                                    Catagory = nv.Catagory,
                                    Brand = nv.Brand,
                                    Price = nv.Price,
                                    Unit = nv.Unit,
                                    MFG = nv.MFG,
                                    EXP = nv.EXP,
                                };

                // Gán dữ liệu cho DataGridView dgvSinhVien
                dgvGoods.DataSource = GoodsData.Take(1000).ToList();
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi chi tiết
                MessageBox.Show("Something went wrong while loading Shift Info!!.\n\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Goods_Frm_Load(object sender, EventArgs e)
        {
            LoadDataToDGV();
            AutoCompleteStringCollection sourceCatagory = new AutoCompleteStringCollection();
            var listCatagory = db.Goods.Select(x => x.Catagory).ToList().Distinct();
            foreach (string name in listCatagory)
            {
                if (name != null && name != "UpdateLater")
                    sourceCatagory.Add(name);
            }
            txtCatagory.AutoCompleteCustomSource = sourceCatagory;
            txtCatagory.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtCatagory.AutoCompleteSource = AutoCompleteSource.CustomSource;

            AutoCompleteStringCollection sourceBrand = new AutoCompleteStringCollection();
            var listBrand = db.Goods.Select(x => x.Brand).ToList().Distinct();
            foreach (string name in listBrand)
            {
                if (name != null && name != "UpdateLater")
                    sourceBrand.Add(name);
            }
            txtBrand.AutoCompleteCustomSource = sourceBrand;
            txtBrand.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtBrand.AutoCompleteSource = AutoCompleteSource.CustomSource;

            AutoCompleteStringCollection sourceUnit = new AutoCompleteStringCollection();
            var listunit = db.Goods.Select(x => x.Unit).ToList().Distinct();
            foreach (string name in listunit)
            {
                if (name != null && name !="UpdateLater")
                    sourceUnit.Add(name);
            }
            txtUnit.AutoCompleteCustomSource = sourceUnit;
            txtUnit.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtUnit.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtPrice.Text = "0";

        }
        public bool FindDuplicate()
        {
            Good check = db.Goods.FirstOrDefault(sv => sv.ID == txtId.Text);
            if (check != null) { return true; }
            return false;
        }
        private void AddDataToDatabase(string id, string name, string catagory, string brand, string unit, int price, string MFG,string EXP)
        {

            // Tạo một đối tượng SINH_VIEN mới
            System.DateTime dtMFG = Convert.ToDateTime(MFG);
            System.DateTime dtEXP = Convert.ToDateTime(EXP);
            Good newGood = new Good
            {
                ID = id,
                Name = name,
                Catagory = catagory,
                Brand = brand,
                Unit = unit,
                Price = price,
                MFG = dtMFG,
                EXP = dtEXP,
                Hide = false,
                // Nếu có thêm các trường khác, hãy thêm vào đây
            };
            // Thêm đối tượng mới vào DbSet và lưu vào cơ sở dữ liệu
            db.Goods.Add(newGood);
            db.SaveChanges();
            LoadDataToDGV();
        }
        private void AddDataToDatabase(string id, string name, string catagory, string brand, string unit, int price)
        {

            // Tạo một đối tượng SINH_VIEN mới
            Good newGood = new Good
            {
                ID = id,
                Name = name,
                Catagory = catagory,
                Brand = brand,
                Unit = unit,
                Price = price,
                Hide = false,
                // Nếu có thêm các trường khác, hãy thêm vào đây
            };
            // Thêm đối tượng mới vào DbSet và lưu vào cơ sở dữ liệu
            db.Goods.Add(newGood);
            db.SaveChanges();
            LoadDataToDGV();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtId.Text == "")
            {
                MessageBox.Show("Id goods is empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (FindDuplicate() == true)
                {
                    MessageBox.Show("Goods has existed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (dtpEXP.Text == " " && dtpMFG.Text == " ")
                    {
                        string ten = txtName.Text;
                        string nhanhieu =txtBrand.Text;
                        string donvi = txtUnit.Text;
                        string loai = txtCatagory.Text;
                        if (txtName.Text == "") ten = "UpdateLater";
                        if (txtBrand.Text == "") nhanhieu = "UpdateLater";
                        if (txtUnit.Text == "") donvi = "UpdateLater";
                        if (txtCatagory.Text == "") loai = "UpdateLater";
                        AddDataToDatabase(txtId.Text, ten
                            , loai, nhanhieu, donvi, Int32.Parse(txtPrice.Text));
                        clearNVFields();
                        MessageBox.Show("Add product successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        string ten = txtName.Text;
                        string nhanhieu = txtBrand.Text;
                        string donvi = txtUnit.Text;
                        string loai = txtCatagory.Text;
                        if (txtName.Text == "") ten = "UpdateLater";
                        if (txtBrand.Text == "") nhanhieu = "UpdateLater";
                        if (txtUnit.Text == "") donvi = "UpdateLater";
                        if (txtCatagory.Text == "") loai = "UpdateLater";
                        AddDataToDatabase(txtId.Text, ten
                            , loai, nhanhieu, donvi, Int32.Parse(txtPrice.Text),dtpMFG.Text,dtpEXP.Text);
                        clearNVFields();
                        MessageBox.Show("Add product successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                   
                }
            }    
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void dgvGoods_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvGoods.Rows.Count)
            {
                DataGridViewRow selectedRow = dgvGoods.Rows[e.RowIndex];

                // Lấy thông tin từ dòng được chọn
                txtId.ReadOnly = true;
                txtId.Text = selectedRow.Cells["ID"].Value.ToString();
                txtName.Text = selectedRow.Cells["Name"].Value.ToString();
                txtCatagory.Text = selectedRow.Cells["Catagory"].Value.ToString();
                txtBrand.Text = selectedRow.Cells["Brand"].Value.ToString();
                txtUnit.Text = selectedRow.Cells["Unit"].Value.ToString();
                txtPrice.Text = selectedRow.Cells["Price"].Value.ToString();
                if (selectedRow.Cells["MFG"].Value == null)
                {
                    dtpMFG.CustomFormat = " ";
                }
                else
                {
                    dtpMFG.CustomFormat = "yyyy-MM-dd";
                    dtpMFG.Text = selectedRow.Cells["MFG"].Value.ToString();
                }
                if (selectedRow.Cells["EXP"].Value == null) dtpEXP.CustomFormat = " ";
                else
                {
                    dtpEXP.CustomFormat = "yyyy-MM-dd";
                    dtpEXP.Text = selectedRow.Cells["EXP"].Value.ToString();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string Id = txtId.Text;
            string name = txtName.Text;
            string catagory = txtCatagory.Text.Trim();
            string Brand = txtBrand.Text;
            int Price = Int32.Parse(txtPrice.Text);
            System.DateTime dtMFG = Convert.ToDateTime(dtpMFG.Text);
            System.DateTime dtEXP = Convert.ToDateTime(dtpEXP.Text);
            Good nhanvienUpdate = db.Goods.FirstOrDefault(sv => sv.ID == Id);
            if (nhanvienUpdate != null)
            {

                nhanvienUpdate.Name = name;
                nhanvienUpdate.Catagory = catagory;
                nhanvienUpdate.Brand = Brand;
                nhanvienUpdate.Price = Price;
                nhanvienUpdate.MFG = dtMFG;
                nhanvienUpdate.EXP = dtEXP;

                // Lưu thay đổi vào cơ sở dữ liệu
                db.SaveChanges();
                clearNVFields();
                MessageBox.Show("Update Product successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataToDGV();

            }
            else
            {
                MessageBox.Show("Can't find Product to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void clearNVFields()
        {
            txtId.Clear();
            txtName.Clear();
            txtCatagory.Clear();
            txtBrand.Clear();
            txtPrice.Text = "0";
            dtpMFG.CustomFormat = " ";
            dtpEXP.CustomFormat = " ";
            txtId.ReadOnly = false;
        }

        private void dtpMFG_ValueChanged(object sender, EventArgs e)
        {
            dtpMFG.CustomFormat = "yyyy-MM-dd";
        }

        private void dtpEXP_ValueChanged(object sender, EventArgs e)
        {
            dtpEXP.CustomFormat = "yyyy-MM-dd";
        }
        private void DeleteGoods(string id)
        {
            // Lấy danh sách cần xóa

            Good goodToDelete = db.Goods.Where(sv => sv.ID == id).FirstOrDefault();
            goodToDelete.Hide = true;
            db.Entry(goodToDelete).State = System.Data.Entity.EntityState.Modified;
            // Lưu thay đổi vào cơ sở dữ liệu
            db.SaveChanges();
            MessageBox.Show("Delete succesfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadDataToDGV();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {

                if (dgvGoods.SelectedRows.Count > 0)
                {

                    foreach (DataGridViewRow selectedRow in dgvGoods.SelectedRows)
                    {
                        // Lấy thông tin từ dòng được chọn
                        string id = selectedRow.Cells["Id"].Value.ToString();
                        // Xóa từ cơ sở dữ liệu
                        DeleteGoods(id);
                    }
                    clearNVFields();
                    // Cập nhật lại dgvSinhVien sau khi xóa 
                    LoadDataToDGV();
                }
                else
                {
                    MessageBox.Show("Select Goods to delete.", "Announcement", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                            for (int col = 1; col <= dgvGoods.Columns.Count; col++)
                            {
                                worksheet.Cells[1, col].Value = dgvGoods.Columns[col - 1].HeaderText;
                            }

                            // Write data from DataGridView to the worksheet
                            for (int row = 0; row < dgvGoods.Rows.Count; row++)
                            {
                                for (int col = 0; col < dgvGoods.Columns.Count; col++)
                                {
                                    worksheet.Cells[row + 2, col + 1].Value = dgvGoods.Rows[row].Cells[col].Value;
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
