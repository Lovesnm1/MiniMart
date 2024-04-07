using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace DesignUI
{
    public partial class ImportGoodsNote : Form
    {
        public ImportGoodsNote()
        {
            InitializeComponent();
        }
        MiniMartEntities db = new MiniMartEntities();
        int CodeID;
        private void ImportGoodsNote_Load(object sender, EventArgs e)
        {
            var list = db.GoodsReceiveds.OrderByDescending(x => x.Code).Take(1).Select(x => x.Code);
            foreach (var a in list)
            {
                CodeID = Int32.Parse(a.ToString());
            }
            CodeID++;
            txtPrice.Text = "0";
            txtQuantity.Text = "1";
            dgvImport.Rows.Clear();
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
            if (e.KeyChar == Convert.ToChar(Keys.Return))
            {
                int gia = Int32.Parse(txtPrice.Text);
                if (gia < 1000)
                {
                    MessageBox.Show("Price must be above 1000", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }    
            }    
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtPhone.MaxLength = 10;
            if(!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
            if (e.KeyChar == Convert.ToChar(Keys.Return))
            { 
                if (txtQuantity.Text == "0")
                {
                    MessageBox.Show("Quantity must be above 0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private bool checkId()
        {
            Good check = db.Goods.FirstOrDefault(sv => sv.ID == txtID.Text);
            if (check == null)
            {
                DialogResult d = MessageBox.Show("The product is not available yet, do you want to add it? ", "Announcement", MessageBoxButtons.OK);
                if (d == DialogResult.OK)
                {
                    Good newP = new Good
                    {
                        ID = txtID.Text,
                        Name = "UpdateLater",
                        Catagory = "UpdateLater",
                        Brand = "UpdateLater",
                        Price = 0,
                        Unit = "UpdateLater",
                        Hide = false,

                        // Nếu có thêm các trường khác, hãy thêm vào đây
                    };

                    // Thêm đối tượng mới vào DbSet và lưu vào cơ sở dữ liệu
                    db.Goods.Add(newP);
                    db.SaveChanges();
                    return true;
                }
            }

            return false;
        }
        int total = 0;
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "" || txtPrice.Text == "" || txtUnit.Text == "" || txtQuantity.Text == "" || txtName.Text == "" 
                || txtAddress.Text == "" || txtPhone.Text == "")
            {
                MessageBox.Show("Must fill all - Note can be blanked", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }    
            else
            {
                int gia = Int32.Parse(txtPrice.Text);
                int sl = Int32.Parse(txtQuantity.Text);
                if (checkId() == true)
                {
                    
                    if (txtNote.Text == "")
                    {
                        dgvImport.Rows.Add(txtID.Text, txtPrice.Text, txtUnit.Text, txtQuantity.Text, txtName.Text, txtAddress.Text, txtPhone.Text, "");
                        total = total + (gia * sl);
                    }
                    else
                    {
                        total = total + (gia * sl);
                        dgvImport.Rows.Add(txtID.Text, txtPrice.Text, txtUnit.Text, txtQuantity.Text, txtName.Text, txtAddress.Text, txtPhone.Text, txtNote.Text);
                    }
                }
                else
                {
                    if (txtNote.Text == "")
                    {
                        total = total + (gia * sl);
                        dgvImport.Rows.Add(txtID.Text, txtPrice.Text, txtUnit.Text, txtQuantity.Text, txtName.Text, txtAddress.Text, txtPhone.Text, "");
                    }
                    else
                    {
                        total = total + (gia * sl);
                        dgvImport.Rows.Add(txtID.Text, txtPrice.Text, txtUnit.Text, txtQuantity.Text, txtName.Text, txtAddress.Text, txtPhone.Text, txtNote.Text);
                    }
                }
                lblTotal.Text = total.ToString();
                ClearFiels();
            }    
        }

        private void dgvImport_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvImport.Rows.Count)
            {
                DataGridViewRow selectedRow = dgvImport.Rows[e.RowIndex];

                // Lấy thông tin từ dòng được chọn
                txtID.ReadOnly = true;
                txtID.Text = selectedRow.Cells[0].Value.ToString();
                txtPrice.Text = selectedRow.Cells[1].Value.ToString();
                txtUnit.Text = selectedRow.Cells[2].Value.ToString();
                txtQuantity.Text = selectedRow.Cells[3].Value.ToString();
                txtName.Text = selectedRow.Cells[4].Value.ToString();
                txtAddress.Text = selectedRow.Cells[5].Value.ToString();
                txtPhone.Text = selectedRow.Cells[6].Value.ToString();
                txtNote.Text = selectedRow.Cells[7].Value.ToString();

            }
        }
        private void ClearFiels()
        {
            txtID.Text = "";
            txtPrice.Text = "0";
            txtQuantity.Text = "1";
            txtName.Text = "";
            txtAddress.Text = "";
            txtPhone.Text = "";
            txtNote.Text = "";
            txtID.ReadOnly = false;
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string Id = txtID.Text;
            if (Id == "")
            {
                MessageBox.Show("Nothing to Update", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                foreach (DataGridViewRow row in dgvImport.Rows)
                {
                    string IdRow = row.Cells[0].Value.ToString().Trim();
                    if (IdRow ==Id.Trim())
                    {
                        int gia = Int32.Parse(txtPrice.Text);
                        int sl = Int32.Parse(txtQuantity.Text);
                         row.Cells[1].Value = gia;
                        row.Cells[2].Value = txtUnit.Text;
                        row.Cells[3].Value = sl;
                        row.Cells[4].Value = txtName.Text;
                        row.Cells[5].Value = txtAddress.Text  ;
                        row.Cells[6].Value = txtPhone.Text;
                        row.Cells[7].Value = txtNote.Text;
                        ClearFiels();
                        break;
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string Id = txtID.Text;
            if (Id == "")
            {
                MessageBox.Show("Nothing to Delete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                foreach (DataGridViewRow row in dgvImport.Rows)
                {
                    string IdRow = row.Cells[0].Value.ToString().Trim();
                    if (IdRow == Id.Trim())
                    {
                        dgvImport.Rows.Remove(row);
                        ClearFiels();
                        break;
                    }
                }

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvImport.Rows.Count == 0) 
            {
                MessageBox.Show("Please fill all the information", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                foreach (DataGridViewRow row in dgvImport.Rows)
                {
                    string note = "";
                    if (row.Cells[7].Value == null) note = "";
                    MessageBox.Show(CodeID.ToString());
                    GoodsReceived newGoodR = new GoodsReceived
                    {
                        Code = CodeID.ToString(),
                        ID_Goods = row.Cells[0].Value.ToString().Trim(),
                        Price = Int32.Parse(row.Cells[1].Value.ToString()),
                        Unit = (row.Cells[2].Value.ToString()),
                        Quantity = Int32.Parse(row.Cells[3].Value.ToString()),
                        Seller = row.Cells[4].Value.ToString(),
                        Address = row.Cells[5].Value.ToString(),
                        Phone = Int32.Parse(row.Cells[6].Value.ToString()),
                        Note = note,
                        Hide = false,
                        // Nếu có thêm các trường khác, hãy thêm vào đây
                    };
                    // Thêm đối tượng mới vào DbSet và lưu vào cơ sở dữ liệu
                    string id = row.Cells[0].Value.ToString().Trim();
                    db.GoodsReceiveds.Add(newGoodR);
                    db.SaveChanges();
                    Good QuantityUpdate = db.Goods.FirstOrDefault(sv => sv.ID == id);
                    if (QuantityUpdate != null)
                    {
                        if (QuantityUpdate.InventoryNumber == null) QuantityUpdate.InventoryNumber = Int32.Parse(row.Cells[3].Value.ToString());
                        else QuantityUpdate.InventoryNumber = QuantityUpdate.InventoryNumber + Int32.Parse(row.Cells[3].Value.ToString());
                    }
                    db.SaveChanges();
                }
                MessageBox.Show("Save succesfully!!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }    
        }
    }
}
