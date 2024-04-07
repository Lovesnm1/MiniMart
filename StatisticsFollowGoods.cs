using DocumentFormat.OpenXml.Wordprocessing;
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
    public partial class StatisticsFollowGoods : Form
    {
        public StatisticsFollowGoods()
        {
            InitializeComponent();
        }
        MiniMartEntities db = new MiniMartEntities();
        private void LoadDataToDGV()
        {
            try
            {

                // Truy vấn LINQ để lấy dữ liệu 
                var GoodsData = (from line in db.InvoiceDetails
                                 group line by line.ID_Goods into g
                                 from s in g
                                 join a in db.Goods on s.ID_Goods equals a.ID
                                 orderby a.ID
                                 where a.Hide == false
                                 select new
                                 {
                                     Id = s.ID_Goods,
                                     Name = a.Name,
                                     Catagory = a.Catagory,
                                     Price = a.Price,
                                     SellNumber = g.Sum(pc => pc.Quantity).ToString(),
                                     Total = a.Price * g.Sum(pc => pc.Quantity),
                                 }).Distinct();

                dgvGoods.DataSource = GoodsData.ToList();
                SumGoods();
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi chi tiết
                MessageBox.Show("Something went wrong while loading Staff Info!!.\n\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadcmbCatogory()
        {
            var catogory = db.Goods.Select(x => x.Catagory);
            cmbCatagory.DataSource = catogory.Distinct().ToList();
            cmbCatagory.SelectedIndex = -1;
        }
        private void StatisticsFollowGoods_Load(object sender, EventArgs e)
        {
            LoadDataToDGV();
            LoadcmbCatogory();
            btnLoc.Enabled = false;
        }

        private void cmbCatagory_SelectedValueChanged(object sender, EventArgs e)
        {
            btnLoc.Enabled = true;
        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            if (cmbCatagory.SelectedIndex != -1)
            {
                if (string.IsNullOrEmpty(txtSearch.Text) == true && dtpBD.Text == " " && dtpKT.Text == " ")
                {
                    var GoodsData = (from line in db.InvoiceDetails
                                     group line by line.ID_Goods into g
                                     from s in g
                                     join a in db.Goods on s.ID_Goods equals a.ID
                                     orderby a.ID
                                     where a.Catagory == cmbCatagory.Text && a.Hide == false
                                     select new
                                     {
                                         Id = s.ID_Goods,
                                         Name = a.Name,
                                         Catagory = a.Catagory,
                                         Price = a.Price,
                                         SellNumber = g.Sum(pc => pc.Quantity).ToString(),
                                         Total = a.Price * g.Sum(pc => pc.Quantity),
                                         //CreatedDate = a.CreatedDate,
                                     }).Distinct();
                    dgvGoods.DataSource = GoodsData.ToList();
                    SumGoods();
                }
                else if (string.IsNullOrEmpty(txtSearch.Text) != true && dtpBD.Text == " " && dtpKT.Text == " ")
                {
                    string name = txtSearch.Text;
                    var GoodsData = (from line in db.InvoiceDetails
                                     group line by line.ID_Goods into g
                                     from s in g
                                     join a in db.Goods on s.ID_Goods equals a.ID
                                     orderby a.ID
                                     where a.Catagory == cmbCatagory.Text && a.Name.Contains(name) && a.Hide == false
                                     select new
                                     {
                                         Id = s.ID_Goods,
                                         Name = a.Name,
                                         Catagory = a.Catagory,
                                         Price = a.Price,
                                         SellNumber = g.Sum(pc => pc.Quantity).ToString(),
                                         Total = a.Price * g.Sum(pc => pc.Quantity),
                                         //CreatedDate = a.CreatedDate,
                                     }).Distinct();
                    dgvGoods.DataSource = GoodsData.ToList();
                    SumGoods();
                }
                else if (string.IsNullOrEmpty(txtSearch.Text) == true && dtpBD.Text != " " && dtpKT.Text == " ")
                {
                    //string name = txtSearch.Text;
                    DateTime BD = Convert.ToDateTime(dtpBD.Text);
                    var GoodsData = (from s in db.InvoiceDetails
                                     join a in db.Goods on s.ID_Goods equals a.ID
                                     join c in db.Invoices on s.ID_Invoice equals c.ID
                                     orderby a.ID
                                     where a.Catagory == cmbCatagory.Text && c.CreatedDate >= BD && a.Hide == false
                                     select new
                                     {
                                         Id = s.ID_Goods,
                                         Name = a.Name,
                                         Catagory = a.Catagory,
                                         Price = a.Price,
                                         Quantity = s.Quantity,
                                        // Total = a.Price * g.Sum(pc => pc.Quantity),
                                         CreatedDate = c.CreatedDate,
                                     }
                                     );
                    var GoodsGroupBy = (from s in GoodsData
                                       group s by s.Id into g
                                       from b in g
                                       select new
                                       {
                                           Id = b.Id,
                                           Name = b.Name,
                                           Catagory = b.Catagory,
                                           Price = b.Price,
                                           SellNumber = g.Sum(pc => pc.Quantity).ToString(),
                                           Total = b.Price * g.Sum(pc => pc.Quantity),
                                       }).Distinct();

                    dgvGoods.DataSource = GoodsGroupBy.ToList();
                    SumGoods();
                }
                else if (string.IsNullOrEmpty(txtSearch.Text) == true && dtpBD.Text == " " && dtpKT.Text != " ")
                {
                    //string name = txtSearch.Text;
                    DateTime KT = Convert.ToDateTime(dtpKT.Text);
                    var GoodsData = (from s in db.InvoiceDetails
                                     join a in db.Goods on s.ID_Goods equals a.ID
                                     join c in db.Invoices on s.ID_Invoice equals c.ID
                                     orderby a.ID
                                     where a.Catagory == cmbCatagory.Text && c.CreatedDate <= KT && a.Hide == false
                                     select new
                                     {
                                         Id = s.ID_Goods,
                                         Name = a.Name,
                                         Catagory = a.Catagory,
                                         Price = a.Price,
                                         Quantity = s.Quantity,
                                         // Total = a.Price * g.Sum(pc => pc.Quantity),
                                         CreatedDate = c.CreatedDate,
                                     }
                                     );
                    var GoodsGroupBy = (from s in GoodsData
                                        group s by s.Id into g
                                        from b in g
                                        select new
                                        {
                                            Id = b.Id,
                                            Name = b.Name,
                                            Catagory = b.Catagory,
                                            Price = b.Price,
                                            SellNumber = g.Sum(pc => pc.Quantity).ToString(),
                                            Total = b.Price * g.Sum(pc => pc.Quantity),
                                        }).Distinct();

                    dgvGoods.DataSource = GoodsGroupBy.ToList();
                    SumGoods();
                }   
                else if (string.IsNullOrEmpty(txtSearch.Text) != true && dtpBD.Text == " " && dtpKT.Text != " ")
                {
                    string name = txtSearch.Text;
                    DateTime KT = Convert.ToDateTime(dtpKT.Text);
                    var GoodsData = (from s in db.InvoiceDetails
                                     join a in db.Goods on s.ID_Goods equals a.ID
                                     join c in db.Invoices on s.ID_Invoice equals c.ID
                                     orderby a.ID
                                     where a.Catagory == cmbCatagory.Text && c.CreatedDate <= KT && a.Name.Contains(name) && a.Hide == false
                                     select new
                                     {
                                         Id = s.ID_Goods,
                                         Name = a.Name,
                                         Catagory = a.Catagory,
                                         Price = a.Price,
                                         Quantity = s.Quantity,
                                         // Total = a.Price * g.Sum(pc => pc.Quantity),
                                         CreatedDate = c.CreatedDate,
                                     }
                                     );
                    var GoodsGroupBy = (from s in GoodsData
                                        group s by s.Id into g
                                        from b in g
                                        select new
                                        {
                                            Id = b.Id,
                                            Name = b.Name,
                                            Catagory = b.Catagory,
                                            Price = b.Price,
                                            SellNumber = g.Sum(pc => pc.Quantity).ToString(),
                                            Total = b.Price * g.Sum(pc => pc.Quantity),
                                        }).Distinct();

                    dgvGoods.DataSource = GoodsGroupBy.ToList();
                    SumGoods();
                }    
                else if (string.IsNullOrEmpty(txtSearch.Text) != true && dtpBD.Text != " " && dtpKT.Text == " ")
                {
                    string name = txtSearch.Text;
                    DateTime BD = Convert.ToDateTime(dtpBD.Text);
                    var GoodsData = (from s in db.InvoiceDetails
                                     join a in db.Goods on s.ID_Goods equals a.ID
                                     join c in db.Invoices on s.ID_Invoice equals c.ID
                                     orderby a.ID
                                     where a.Catagory == cmbCatagory.Text && c.CreatedDate >=BD && a.Name.Contains(name) && a.Hide == false
                                     select new
                                     {
                                         Id = s.ID_Goods,
                                         Name = a.Name,
                                         Catagory = a.Catagory,
                                         Price = a.Price,
                                         Quantity = s.Quantity,
                                         // Total = a.Price * g.Sum(pc => pc.Quantity),
                                         CreatedDate = c.CreatedDate,
                                     }
                                     );
                    var GoodsGroupBy = (from s in GoodsData
                                        group s by s.Id into g
                                        from b in g
                                        select new
                                        {
                                            Id = b.Id,
                                            Name = b.Name,
                                            Catagory = b.Catagory,
                                            Price = b.Price,
                                            SellNumber = g.Sum(pc => pc.Quantity).ToString(),
                                            Total = b.Price * g.Sum(pc => pc.Quantity),
                                        }).Distinct();

                    dgvGoods.DataSource = GoodsGroupBy.ToList();
                    SumGoods();
                }
                else if (string.IsNullOrEmpty(txtSearch.Text) == true && dtpBD.Text != " " && dtpKT.Text != " ")
                {
                    //string name = txtSearch.Text;
                    DateTime BD = Convert.ToDateTime(dtpBD.Text);
                    DateTime KT = Convert.ToDateTime(dtpKT.Text);
                    var GoodsData = (from s in db.InvoiceDetails
                                     join a in db.Goods on s.ID_Goods equals a.ID
                                     join c in db.Invoices on s.ID_Invoice equals c.ID
                                     orderby a.ID
                                     where a.Catagory == cmbCatagory.Text && c.CreatedDate >= BD && c.CreatedDate <= KT && a.Hide == false
                                     select new
                                     {
                                         Id = s.ID_Goods,
                                         Name = a.Name,
                                         Catagory = a.Catagory,
                                         Price = a.Price,
                                         Quantity = s.Quantity,
                                         // Total = a.Price * g.Sum(pc => pc.Quantity),
                                         CreatedDate = c.CreatedDate,
                                     }
                                     );
                    var GoodsGroupBy = (from s in GoodsData
                                        group s by s.Id into g
                                        from b in g
                                        select new
                                        {
                                            Id = b.Id,
                                            Name = b.Name,
                                            Catagory = b.Catagory,
                                            Price = b.Price,
                                            SellNumber = g.Sum(pc => pc.Quantity).ToString(),
                                            Total = b.Price * g.Sum(pc => pc.Quantity),
                                        }).Distinct();

                    dgvGoods.DataSource = GoodsGroupBy.ToList();
                    SumGoods();
                }
                else if (string.IsNullOrEmpty(txtSearch.Text) != true && dtpBD.Text != " " && dtpKT.Text != " ")
                {
                    string name = txtSearch.Text;
                    DateTime BD = Convert.ToDateTime(dtpBD.Text);
                    DateTime KT = Convert.ToDateTime(dtpKT.Text);
                    var GoodsData = (from s in db.InvoiceDetails
                                     join a in db.Goods on s.ID_Goods equals a.ID
                                     join c in db.Invoices on s.ID_Invoice equals c.ID
                                     orderby a.ID
                                     where a.Hide == false && a.Catagory == cmbCatagory.Text && c.CreatedDate >= BD && c.CreatedDate <= KT && a.Name.Contains(name)
                                     select new
                                     {
                                         Id = s.ID_Goods,
                                         Name = a.Name,
                                         Catagory = a.Catagory,
                                         Price = a.Price,
                                         Quantity = s.Quantity,
                                         // Total = a.Price * g.Sum(pc => pc.Quantity),
                                         CreatedDate = c.CreatedDate,
                                     }
                                     );
                    var GoodsGroupBy = (from s in GoodsData
                                        group s by s.Id into g
                                        from b in g
                                        select new
                                        {
                                            Id = b.Id,
                                            Name = b.Name,
                                            Catagory = b.Catagory,
                                            Price = b.Price,
                                            SellNumber = g.Sum(pc => pc.Quantity).ToString(),
                                            Total = b.Price * g.Sum(pc => pc.Quantity),
                                        }).Distinct();

                    dgvGoods.DataSource = GoodsGroupBy.ToList();
                    SumGoods();
                }
            }
            else
            {
                if (string.IsNullOrEmpty(txtSearch.Text) == true && dtpBD.Text == " " && dtpKT.Text == " ")
                {
                    var GoodsData = (from line in db.InvoiceDetails
                                     group line by line.ID_Goods into g
                                     from s in g
                                     join a in db.Goods on s.ID_Goods equals a.ID
                                     orderby a.ID
                                     where a.Hide == false
                                     select new
                                     {
                                         Id = s.ID_Goods,
                                         Name = a.Name,
                                         Catagory = a.Catagory,
                                         Price = a.Price,
                                         SellNumber = g.Sum(pc => pc.Quantity).ToString(),
                                         Total = a.Price * g.Sum(pc => pc.Quantity),
                                         //CreatedDate = a.CreatedDate,
                                     }).Distinct();
                    dgvGoods.DataSource = GoodsData.ToList();
                    SumGoods();
                }
                else if (string.IsNullOrEmpty(txtSearch.Text) != true && dtpBD.Text == " " && dtpKT.Text == " ")
                {
                    string name = txtSearch.Text;
                    var GoodsData = (from line in db.InvoiceDetails
                                     group line by line.ID_Goods into g
                                     from s in g
                                     join a in db.Goods on s.ID_Goods equals a.ID
                                     orderby a.ID
                                     where  a.Name.Contains(name) && a.Hide == false
                                     select new
                                     {
                                         Id = s.ID_Goods,
                                         Name = a.Name,
                                         Catagory = a.Catagory,
                                         Price = a.Price,
                                         SellNumber = g.Sum(pc => pc.Quantity).ToString(),
                                         Total = a.Price * g.Sum(pc => pc.Quantity),
                                         //CreatedDate = a.CreatedDate,
                                     }).Distinct();
                    dgvGoods.DataSource = GoodsData.ToList();
                    SumGoods();
                }
                else if (string.IsNullOrEmpty(txtSearch.Text) == true && dtpBD.Text != " " && dtpKT.Text == " ")
                {
                    //string name = txtSearch.Text;
                    DateTime BD = Convert.ToDateTime(dtpBD.Text);
                    var GoodsData = (from s in db.InvoiceDetails
                                     join a in db.Goods on s.ID_Goods equals a.ID
                                     join c in db.Invoices on s.ID_Invoice equals c.ID
                                     orderby a.ID
                                     where  c.CreatedDate >= BD && a.Hide == false
                                     select new
                                     {
                                         Id = s.ID_Goods,
                                         Name = a.Name,
                                         Catagory = a.Catagory,
                                         Price = a.Price,
                                         Quantity = s.Quantity,
                                         // Total = a.Price * g.Sum(pc => pc.Quantity),
                                         CreatedDate = c.CreatedDate,
                                     }
                                     );
                    var GoodsGroupBy = (from s in GoodsData
                                        group s by s.Id into g
                                        from b in g
                                        select new
                                        {
                                            Id = b.Id,
                                            Name = b.Name,
                                            Catagory = b.Catagory,
                                            Price = b.Price,
                                            SellNumber = g.Sum(pc => pc.Quantity).ToString(),
                                            Total = b.Price * g.Sum(pc => pc.Quantity),
                                        }).Distinct();

                    dgvGoods.DataSource = GoodsGroupBy.ToList();
                    SumGoods();
                }
                else if (string.IsNullOrEmpty(txtSearch.Text) == true && dtpBD.Text == " " && dtpKT.Text != " ")
                {
                    //string name = txtSearch.Text;
                    DateTime KT = Convert.ToDateTime(dtpKT.Text);
                    var GoodsData = (from s in db.InvoiceDetails
                                     join a in db.Goods on s.ID_Goods equals a.ID
                                     join c in db.Invoices on s.ID_Invoice equals c.ID
                                     orderby a.ID
                                     where  c.CreatedDate <= KT && a.Hide == false
                                     select new
                                     {
                                         Id = s.ID_Goods,
                                         Name = a.Name,
                                         Catagory = a.Catagory,
                                         Price = a.Price,
                                         Quantity = s.Quantity,
                                         // Total = a.Price * g.Sum(pc => pc.Quantity),
                                         CreatedDate = c.CreatedDate,
                                     }
                                     );
                    var GoodsGroupBy = (from s in GoodsData
                                        group s by s.Id into g
                                        from b in g
                                        select new
                                        {
                                            Id = b.Id,
                                            Name = b.Name,
                                            Catagory = b.Catagory,
                                            Price = b.Price,
                                            SellNumber = g.Sum(pc => pc.Quantity).ToString(),
                                            Total = b.Price * g.Sum(pc => pc.Quantity),
                                        }).Distinct();

                    dgvGoods.DataSource = GoodsGroupBy.ToList();
                    SumGoods();
                }
                else if (string.IsNullOrEmpty(txtSearch.Text) != true && dtpBD.Text == " " && dtpKT.Text != " ")
                {
                    string name = txtSearch.Text;
                    DateTime KT = Convert.ToDateTime(dtpKT.Text);
                    var GoodsData = (from s in db.InvoiceDetails
                                     join a in db.Goods on s.ID_Goods equals a.ID
                                     join c in db.Invoices on s.ID_Invoice equals c.ID
                                     orderby a.ID
                                     where  c.CreatedDate <= KT && a.Name.Contains(name) && a.Hide == false
                                     select new
                                     {
                                         Id = s.ID_Goods,
                                         Name = a.Name,
                                         Catagory = a.Catagory,
                                         Price = a.Price,
                                         Quantity = s.Quantity,
                                         // Total = a.Price * g.Sum(pc => pc.Quantity),
                                         CreatedDate = c.CreatedDate,
                                     }
                                     );
                    var GoodsGroupBy = (from s in GoodsData
                                        group s by s.Id into g
                                        from b in g
                                        select new
                                        {
                                            Id = b.Id,
                                            Name = b.Name,
                                            Catagory = b.Catagory,
                                            Price = b.Price,
                                            SellNumber = g.Sum(pc => pc.Quantity).ToString(),
                                            Total = b.Price * g.Sum(pc => pc.Quantity),
                                        }).Distinct();

                    dgvGoods.DataSource = GoodsGroupBy.ToList();
                    SumGoods();
                }
                else if (string.IsNullOrEmpty(txtSearch.Text) != true && dtpBD.Text != " " && dtpKT.Text == " ")
                {
                    string name = txtSearch.Text;
                    DateTime BD = Convert.ToDateTime(dtpBD.Text);
                    var GoodsData = (from s in db.InvoiceDetails
                                     join a in db.Goods on s.ID_Goods equals a.ID
                                     join c in db.Invoices on s.ID_Invoice equals c.ID
                                     orderby a.ID
                                     where  c.CreatedDate >= BD && a.Name.Contains(name) && a.Hide == false
                                     select new
                                     {
                                         Id = s.ID_Goods,
                                         Name = a.Name,
                                         Catagory = a.Catagory,
                                         Price = a.Price,
                                         Quantity = s.Quantity,
                                         // Total = a.Price * g.Sum(pc => pc.Quantity),
                                         CreatedDate = c.CreatedDate,
                                     }
                                     );
                    var GoodsGroupBy = (from s in GoodsData
                                        group s by s.Id into g
                                        from b in g
                                        select new
                                        {
                                            Id = b.Id,
                                            Name = b.Name,
                                            Catagory = b.Catagory,
                                            Price = b.Price,
                                            SellNumber = g.Sum(pc => pc.Quantity).ToString(),
                                            Total = b.Price * g.Sum(pc => pc.Quantity),
                                        }).Distinct();

                    dgvGoods.DataSource = GoodsGroupBy.ToList();
                    SumGoods();
                }
                else if (string.IsNullOrEmpty(txtSearch.Text) == true && dtpBD.Text != " " && dtpKT.Text != " ")
                {
                    //string name = txtSearch.Text;
                    DateTime BD = Convert.ToDateTime(dtpBD.Text);
                    DateTime KT = Convert.ToDateTime(dtpKT.Text);
                    var GoodsData = (from s in db.InvoiceDetails
                                     join a in db.Goods on s.ID_Goods equals a.ID
                                     join c in db.Invoices on s.ID_Invoice equals c.ID
                                     orderby a.ID
                                     where  c.CreatedDate >= BD && c.CreatedDate <= KT && a.Hide == false
                                     select new
                                     {
                                         Id = s.ID_Goods,
                                         Name = a.Name,
                                         Catagory = a.Catagory,
                                         Price = a.Price,
                                         Quantity = s.Quantity,
                                         // Total = a.Price * g.Sum(pc => pc.Quantity),
                                         CreatedDate = c.CreatedDate,
                                     }
                                     );
                    var GoodsGroupBy = (from s in GoodsData
                                        group s by s.Id into g
                                        from b in g
                                        select new
                                        {
                                            Id = b.Id,
                                            Name = b.Name,
                                            Catagory = b.Catagory,
                                            Price = b.Price,
                                            SellNumber = g.Sum(pc => pc.Quantity).ToString(),
                                            Total = b.Price * g.Sum(pc => pc.Quantity),
                                        }).Distinct();

                    dgvGoods.DataSource = GoodsGroupBy.ToList();
                    SumGoods();
                }
                else if (string.IsNullOrEmpty(txtSearch.Text) != true && dtpBD.Text != " " && dtpKT.Text != " ")
                {
                    string name = txtSearch.Text;
                    DateTime BD = Convert.ToDateTime(dtpBD.Text);
                    DateTime KT = Convert.ToDateTime(dtpKT.Text);
                    var GoodsData = (from s in db.InvoiceDetails
                                     join a in db.Goods on s.ID_Goods equals a.ID
                                     join c in db.Invoices on s.ID_Invoice equals c.ID
                                     orderby a.ID
                                     where c.CreatedDate >= BD && c.CreatedDate <= KT && a.Name.Contains(name) && a.Hide == false
                                     select new
                                     {
                                         Id = s.ID_Goods,
                                         Name = a.Name,
                                         Catagory = a.Catagory,
                                         Price = a.Price,
                                         Quantity = s.Quantity,
                                         // Total = a.Price * g.Sum(pc => pc.Quantity),
                                         CreatedDate = c.CreatedDate,
                                     }
                                     );
                    var GoodsGroupBy = (from s in GoodsData
                                        group s by s.Id into g
                                        from b in g
                                        select new
                                        {
                                            Id = b.Id,
                                            Name = b.Name,
                                            Catagory = b.Catagory,
                                            Price = b.Price,
                                            SellNumber = g.Sum(pc => pc.Quantity).ToString(),
                                            Total = b.Price * g.Sum(pc => pc.Quantity),
                                        }).Distinct();

                    dgvGoods.DataSource = GoodsGroupBy.ToList();
                    SumGoods();
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            btnLoc.Enabled = true;
        }

        private void dtpNgayBD_ValueChanged(object sender, EventArgs e)
        {
            dtpBD.CustomFormat = "yyyy-MM-dd";
            btnLoc.Enabled = true;
        }

        private void dtpNgayBD_KeyDown(object sender, KeyEventArgs e)
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
        private void SumGoods()
        {
            int sum = 0;
            foreach (DataGridViewRow row in dgvGoods.Rows)
            {
                sum = sum + Int32.Parse(row.Cells["Total"].Value.ToString());
            }
            txtAmount.Text = sum.ToString();
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            dtpBD.CustomFormat = " ";
            dtpKT.CustomFormat = " ";
            cmbCatagory.SelectedIndex = -1;
            LoadDataToDGV();
            btnLoc.Enabled = false;
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
                            worksheet.Cells[dgvGoods.Rows.Count + 3, dgvGoods.Columns.Count + 1].Value = "TotalAmount";
                            worksheet.Cells[dgvGoods.Rows.Count + 3, dgvGoods.Columns.Count + 2].Value = txtAmount.Text;
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
