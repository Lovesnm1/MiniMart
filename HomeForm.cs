using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
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
    public partial class HomeForm : Form
    {
        public HomeForm()
        {
            InitializeComponent();
        }
        MiniMartEntities db = new MiniMartEntities();
        private void LoadDGV()
        {
            try
            {
                DataGridViewImageColumn dgvImage = new DataGridViewImageColumn();
                dgvImage.HeaderText = "Image";
                dgvImage.ImageLayout = DataGridViewImageCellLayout.Stretch;

                dgvGood.Columns.Add(dgvImage);
                dgvGood.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvGood.RowTemplate.Height = 120;
                dgvGood.AllowUserToAddRows = false;
                var GoodsData = from nv in db.Goods
                                where nv.Hide == false
                                select new
                               {
                                   Name = nv.Name,
                                   Catagory = nv.Catagory,
                                   Brand = nv.Brand,
                                   Price = nv.Price,
                                   Inventory = nv.InventoryNumber,
                               };

                dgvGood.DataSource = GoodsData.Take(100).ToList();    
                dgvGood[0,0].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\Toonies.png");
                dgvGood[0,1].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\KhoaiTaySuon.jpg");
                dgvGood[0,2].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\Pepsi.png");
                dgvGood[0, 3].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\C2.jpg");
                dgvGood[0, 4].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\SnackBiDo.png");
                dgvGood[0, 5].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\SnackCaChua.jpg");
                dgvGood[0, 6].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\OLongTea.png"); 

                //foreach ()
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi chi tiết
                MessageBox.Show("Something went wrong while loading Goods Info!!.\n\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadcmbCatogory()
        {
            var catogory = db.Goods.Select(x => x.Catagory);
            cmbCatagory.DataSource = catogory.Distinct().ToList();
            cmbCatagory.SelectedIndex = -1;
        }
        private void LoadcmbBrand()
        {
            var catogory = db.Goods.Select(x => x.Brand);
            cmbBrand.DataSource = catogory.Distinct().ToList();
            cmbBrand.SelectedIndex = -1;
        }
        private void HomeForm_Load(object sender, EventArgs e)
        {
            LoadDGV();
            LoadcmbCatogory();
            LoadcmbBrand();
        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text == "" && cmbCatagory.SelectedIndex == -1 && cmbBrand.SelectedIndex == -1)
            {

            }    
            else if (txtSearch.Text != "" && cmbCatagory.SelectedIndex == -1 && cmbBrand.SelectedIndex == -1)
            {
                string search = txtSearch.Text;
                dgvGood.DataSource = null;
                dgvGood.Rows.Clear();
                dgvGood.Columns.Clear();
                dgvGood.Refresh();
                DataGridViewImageColumn dgvImage = new DataGridViewImageColumn();
                dgvImage.HeaderText = "Image";
                dgvImage.ImageLayout = DataGridViewImageCellLayout.Stretch;

                dgvGood.Columns.Add(dgvImage);
                dgvGood.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvGood.RowTemplate.Height = 120;
                dgvGood.AllowUserToAddRows = false;
                var GoodsData = from nv in db.Goods
                                where nv.Hide == false && nv.Name.Contains(search)
                                select new
                                {
                                    Name = nv.Name,
                                    Catagory = nv.Catagory,
                                    Brand = nv.Brand,
                                    Price = nv.Price,
                                    Inventory = nv.InventoryNumber,
                                };
                dgvGood.DataSource = GoodsData.Take(100).ToList();
                foreach( DataGridViewRow row in dgvGood.Rows)
                {
                    string Name = row.Cells[1].Value.ToString().Trim();
                    if (Name.Contains("Toonies"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\Toonies.png");
                    
                    else if (Name.Contains("Khoai Tây Sườn"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\KhoaiTaySuon.jpg");
                    else if (Name.Contains("Pepsi"))
                         dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\Pepsi.png");
                    else if (Name.Contains("C2"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\C2.jpg");
                    else if (Name.Contains("Snack Bí Đỏ"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\SnackBiDo.png");
                    else if (Name.Contains("Snack Cà Chua"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\SnackCaChua.jpg");
                    else if(Name.Contains("Long TEA +"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\OLongTea.png");
                }
            }
            else if (txtSearch.Text == "" && cmbCatagory.SelectedIndex != -1 && cmbBrand.SelectedIndex == -1)
            {
                //string search = txtSearch.Text;
                string loai = cmbCatagory.Text;
                dgvGood.DataSource = null;
                dgvGood.Rows.Clear();
                dgvGood.Columns.Clear();
                dgvGood.Refresh();
                DataGridViewImageColumn dgvImage = new DataGridViewImageColumn();
                dgvImage.HeaderText = "Image";
                dgvImage.ImageLayout = DataGridViewImageCellLayout.Stretch;

                dgvGood.Columns.Add(dgvImage);
                dgvGood.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvGood.RowTemplate.Height = 120;
                dgvGood.AllowUserToAddRows = false;
                var GoodsData = from nv in db.Goods
                                where nv.Hide == false && nv.Catagory == loai
                                select new
                                {
                                    Name = nv.Name,
                                    Catagory = nv.Catagory,
                                    Brand = nv.Brand,
                                    Price = nv.Price,
                                    Inventory = nv.InventoryNumber,
                                };
                dgvGood.DataSource = GoodsData.Take(100).ToList();
                foreach (DataGridViewRow row in dgvGood.Rows)
                {
                    string Name = row.Cells[1].Value.ToString().Trim();
                    if (Name.Contains("Toonies"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\Toonies.png");

                    else if (Name.Contains("Khoai Tây Sườn"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\KhoaiTaySuon.jpg");
                    else if (Name.Contains("Pepsi"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\Pepsi.png");
                    else if (Name.Contains("C2"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\C2.jpg");
                    else if (Name.Contains("Snack Bí Đỏ"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\SnackBiDo.png");
                    else if (Name.Contains("Snack Cà Chua"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\SnackCaChua.jpg");
                    else if (Name.Contains("Long TEA +"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\OLongTea.png");
                }
            }
            else if (txtSearch.Text == "" && cmbCatagory.SelectedIndex == -1 && cmbBrand.SelectedIndex != -1)
            {
                //string search = txtSearch.Text;
                //string loai = cmbCatagory.Text;
                string brand = cmbBrand.Text;
                dgvGood.DataSource = null;
                dgvGood.Rows.Clear();
                dgvGood.Columns.Clear();
                dgvGood.Refresh();
                DataGridViewImageColumn dgvImage = new DataGridViewImageColumn();
                dgvImage.HeaderText = "Image";
                dgvImage.ImageLayout = DataGridViewImageCellLayout.Stretch;

                dgvGood.Columns.Add(dgvImage);
                dgvGood.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvGood.RowTemplate.Height = 120;
                dgvGood.AllowUserToAddRows = false;
                var GoodsData = from nv in db.Goods
                                where nv.Hide == false && nv.Brand == brand
                                select new
                                {
                                    Name = nv.Name,
                                    Catagory = nv.Catagory,
                                    Brand = nv.Brand,
                                    Price = nv.Price,
                                    Inventory = nv.InventoryNumber,
                                };
                dgvGood.DataSource = GoodsData.Take(100).ToList();
                foreach (DataGridViewRow row in dgvGood.Rows)
                {
                    string Name = row.Cells[1].Value.ToString().Trim();
                    if (Name.Contains("Toonies"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\Toonies.png");

                    else if (Name.Contains("Khoai Tây Sườn"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\KhoaiTaySuon.jpg");
                    else if (Name.Contains("Pepsi"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\Pepsi.png");
                    else if (Name.Contains("C2"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\C2.jpg");
                    else if (Name.Contains("Snack Bí Đỏ"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\SnackBiDo.png");
                    else if (Name.Contains("Snack Cà Chua"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\SnackCaChua.jpg");
                    else if (Name.Contains("Long TEA +"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\OLongTea.png");
                }
            }
            else if (txtSearch.Text != "" && cmbCatagory.SelectedIndex != -1 && cmbBrand.SelectedIndex == -1)
            {
                string search = txtSearch.Text;
                string loai = cmbCatagory.Text;
                //string brand = cmbBrand.Text;
                dgvGood.DataSource = null;
                dgvGood.Rows.Clear();
                dgvGood.Columns.Clear();
                dgvGood.Refresh();
                DataGridViewImageColumn dgvImage = new DataGridViewImageColumn();
                dgvImage.HeaderText = "Image";
                dgvImage.ImageLayout = DataGridViewImageCellLayout.Stretch;

                dgvGood.Columns.Add(dgvImage);
                dgvGood.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvGood.RowTemplate.Height = 120;
                dgvGood.AllowUserToAddRows = false;
                var GoodsData = from nv in db.Goods
                                where nv.Hide == false && nv.Catagory == loai && nv.Name.Contains(search)
                                select new
                                {
                                    Name = nv.Name,
                                    Catagory = nv.Catagory,
                                    Brand = nv.Brand,
                                    Price = nv.Price,
                                    Inventory = nv.InventoryNumber,
                                };
                dgvGood.DataSource = GoodsData.Take(100).ToList();
                foreach (DataGridViewRow row in dgvGood.Rows)
                {
                    string Name = row.Cells[1].Value.ToString().Trim();
                    if (Name.Contains("Toonies"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\Toonies.png");

                    else if (Name.Contains("Khoai Tây Sườn"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\KhoaiTaySuon.jpg");
                    else if (Name.Contains("Pepsi"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\Pepsi.png");
                    else if (Name.Contains("C2"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\C2.jpg");
                    else if (Name.Contains("Snack Bí Đỏ"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\SnackBiDo.png");
                    else if (Name.Contains("Snack Cà Chua"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\SnackCaChua.jpg");
                    else if (Name.Contains("Long TEA +"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\OLongTea.png");
                }
            }
            else if (txtSearch.Text == "" && cmbCatagory.SelectedIndex != -1 && cmbBrand.SelectedIndex != -1)
            {
                //string search = txtSearch.Text;
                string loai = cmbCatagory.Text;
                string brand = cmbBrand.Text;
                dgvGood.DataSource = null;
                dgvGood.Rows.Clear();
                dgvGood.Columns.Clear();
                dgvGood.Refresh();
                DataGridViewImageColumn dgvImage = new DataGridViewImageColumn();
                dgvImage.HeaderText = "Image";
                dgvImage.ImageLayout = DataGridViewImageCellLayout.Stretch;

                dgvGood.Columns.Add(dgvImage);
                dgvGood.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvGood.RowTemplate.Height = 120;
                dgvGood.AllowUserToAddRows = false;
                var GoodsData = from nv in db.Goods
                                where nv.Hide == false && nv.Brand == brand && nv.Catagory == loai
                                select new
                                {
                                    Name = nv.Name,
                                    Catagory = nv.Catagory,
                                    Brand = nv.Brand,
                                    Price = nv.Price,
                                    Inventory = nv.InventoryNumber,
                                };
                dgvGood.DataSource = GoodsData.Take(100).ToList();
                foreach (DataGridViewRow row in dgvGood.Rows)
                {
                    string Name = row.Cells[1].Value.ToString().Trim();
                    if (Name.Contains("Toonies"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\Toonies.png");

                    else if (Name.Contains("Khoai Tây Sườn"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\KhoaiTaySuon.jpg");
                    else if (Name.Contains("Pepsi"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\Pepsi.png");
                    else if (Name.Contains("C2"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\C2.jpg");
                    else if (Name.Contains("Snack Bí Đỏ"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\SnackBiDo.png");
                    else if (Name.Contains("Snack Cà Chua"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\SnackCaChua.jpg");
                    else if (Name.Contains("Long TEA +"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\OLongTea.png");
                }
            }
            else if (txtSearch.Text != "" && cmbCatagory.SelectedIndex == -1 && cmbBrand.SelectedIndex != -1)
            {
                string search = txtSearch.Text;
                //string loai = cmbCatagory.Text;
                string brand = cmbBrand.Text;
                dgvGood.DataSource = null;
                dgvGood.Rows.Clear();
                dgvGood.Columns.Clear();
                dgvGood.Refresh();
                DataGridViewImageColumn dgvImage = new DataGridViewImageColumn();
                dgvImage.HeaderText = "Image";
                dgvImage.ImageLayout = DataGridViewImageCellLayout.Stretch;

                dgvGood.Columns.Add(dgvImage);
                dgvGood.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvGood.RowTemplate.Height = 120;
                dgvGood.AllowUserToAddRows = false;
                var GoodsData = from nv in db.Goods
                                where nv.Hide == false && nv.Brand == brand && nv.Name.Contains(search)
                                select new
                                {
                                    Name = nv.Name,
                                    Catagory = nv.Catagory,
                                    Brand = nv.Brand,
                                    Price = nv.Price,
                                    Inventory = nv.InventoryNumber,
                                };
                dgvGood.DataSource = GoodsData.Take(100).ToList();
                foreach (DataGridViewRow row in dgvGood.Rows)
                {
                    string Name = row.Cells[1].Value.ToString().Trim();
                    if (Name.Contains("Toonies"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\Toonies.png");

                    else if (Name.Contains("Khoai Tây Sườn"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\KhoaiTaySuon.jpg");
                    else if (Name.Contains("Pepsi"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\Pepsi.png");
                    else if (Name.Contains("C2"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\C2.jpg");
                    else if (Name.Contains("Snack Bí Đỏ"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\SnackBiDo.png");
                    else if (Name.Contains("Snack Cà Chua"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\SnackCaChua.jpg");
                    else if (Name.Contains("Long TEA +"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\OLongTea.png");
                }
            }
            else if (txtSearch.Text != "" && cmbCatagory.SelectedIndex != -1 && cmbBrand.SelectedIndex != -1)
            {
                string search = txtSearch.Text;
                string loai = cmbCatagory.Text;
                string brand = cmbBrand.Text;
                dgvGood.DataSource = null;
                dgvGood.Rows.Clear();
                dgvGood.Columns.Clear();
                dgvGood.Refresh();
                DataGridViewImageColumn dgvImage = new DataGridViewImageColumn();
                dgvImage.HeaderText = "Image";
                dgvImage.ImageLayout = DataGridViewImageCellLayout.Stretch;

                dgvGood.Columns.Add(dgvImage);
                dgvGood.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvGood.RowTemplate.Height = 120;
                dgvGood.AllowUserToAddRows = false;
                var GoodsData = from nv in db.Goods
                                where nv.Hide == false && nv.Catagory == loai && nv.Brand == brand && nv.Name.Contains(search)
                                select new
                                {
                                    Name = nv.Name,
                                    Catagory = nv.Catagory,
                                    Brand = nv.Brand,
                                    Price = nv.Price,
                                    Inventory = nv.InventoryNumber,
                                };
                dgvGood.DataSource = GoodsData.Take(100).ToList();
                foreach (DataGridViewRow row in dgvGood.Rows)
                {
                    string Name = row.Cells[1].Value.ToString().Trim();
                    if (Name.Contains("Toonies"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\Toonies.png");

                    else if (Name.Contains("Khoai Tây Sườn"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\KhoaiTaySuon.jpg");
                    else if (Name.Contains("Pepsi"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\Pepsi.png");
                    else if (Name.Contains("C2"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\C2.jpg");
                    else if (Name.Contains("Snack Bí Đỏ"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\SnackBiDo.png");
                    else if (Name.Contains("Snack Cà Chua"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\SnackCaChua.jpg");
                    else if (Name.Contains("Long TEA +"))
                        dgvGood[0, row.Index].Value = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\Documents\\Công nghệ phần mềm\\Products Image\\OLongTea.png");
                }
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            cmbCatagory.SelectedIndex = -1;
            cmbBrand.SelectedIndex = -1;
            LoadDGV();
        }
    }
}
