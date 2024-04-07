using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static ClosedXML.Excel.XLPredefinedFormat;

namespace DesignUI
{
    public partial class Cashier_Fr_ : Form
    {
        public Cashier_Fr_()
        {
            InitializeComponent();
        }
        MiniMartEntities db = new MiniMartEntities();
        int BillID;
        private void Cashier_Fr__Load(object sender, EventArgs e)
        {
            var list = db.Invoices.OrderByDescending(x => x.ID).Take(1).Select(x => x.ID);
            foreach (var a in list)
            {
                BillID = Int32.Parse(a.ToString());
            }
            BillID++;
            //MessageBox.Show(BillID.ToString());
            AutoCompleteStringCollection sourceName = new AutoCompleteStringCollection();
            var listNames = db.Goods.Where(s => s.Hide == false).Select(x => x.ID.Trim() + "-" + x.Name + "-" + x.Price).ToList();
            foreach (var name in listNames)
            {
                sourceName.Add(name.ToString());
                //MessageBox.Show(name.ToString());
            }
            txtSearch.AutoCompleteCustomSource = sourceName;
            txtSearch.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtSearch.AutoCompleteSource = AutoCompleteSource.CustomSource;

            AutoCompleteStringCollection sourceId = new AutoCompleteStringCollection();
            var listIds = db.Memberships.Where(s => s.Hide == false).Select(x => x.ID.Trim()+"-"+x.Name).ToList();
            foreach (var name in listIds)
            {
                sourceId.Add(name.ToString());
                //MessageBox.Show(name.ToString());
            }
            cmbId.AutoCompleteCustomSource = sourceId;
            cmbId.AutoCompleteMode = AutoCompleteMode.Suggest;
            cmbId.AutoCompleteSource = AutoCompleteSource.CustomSource;

        }
        int SubTotal;
        private void LoadDataToDGV()
        {
            try
            {
                int sl = Int32.Parse(nudQuantity.Value.ToString());
                string txt = txtIdProDuct.Text;
                var id = db.Goods.Where(s=>s.ID == txt ).Select(x => x.ID).ToList();
                var Name = db.Goods.Where(s => s.ID == txt).Select(x => x.Name).ToList();
                var price = db.Goods.Where(s => s.ID == txt).Select(x => x.Price).ToList();
                int total = sl * Int32.Parse(price[0].Value.ToString());
                dgvCashier.Rows.Add(id[0], Name[0], price[0],sl,total);
                SubTotal = SubTotal + total;
                string money = SubTotal.ToString();
                lblTotal.Text = money;
                lblCustomer.Text = money;
                txtIdProDuct.Clear();

            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi chi tiết
                MessageBox.Show("Something went wrong while adding Products Info!!.\n\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvCashier_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCashier.CurrentCell.OwningColumn.Name == "dgvDel")
            {
                dgvCashier.Rows.Remove(dgvCashier.SelectedRows[0]);
            }    
            else if (dgvCashier.CurrentCell.OwningColumn.Name == "dgvQuantity")
            {
                nudQuantity.Value = Int32.Parse(dgvCashier.SelectedRows[0].Cells[3].Value.ToString());
                txtIdProDuct.Text = dgvCashier.SelectedRows[0].Cells[0].Value.ToString();
                co = true;
            }    
            
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {             
        }
        bool co = false;

        private void btnCash_Click(object sender, EventArgs e)
        {
            txtNumber.Focus();
            txtNumber.Text = "1000";
        }

        private void nudQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (co == false)
            {
                if (e.KeyChar == Convert.ToChar(Keys.Return))
                {
                    if (txtSearch.Text == "" && nudQuantity.Text == "0")
                    {
                        MessageBox.Show("Please choose Products and Quantity", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (txtSearch.Text != "" && nudQuantity.Text == "0")
                    {
                        MessageBox.Show("Quantity must be above 0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (txtSearch.Text == "" && nudQuantity.Text != "0")
                    {
                        MessageBox.Show("Please choose Products", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        bool k = false;
                        foreach (DataGridViewRow row in dgvCashier.Rows)
                        {
                            string[] str = txtSearch.Text.Split('-');
                            string IdRow = row.Cells[0].Value.ToString().Trim();
                            if (IdRow == str[0].Trim())
                            {
                                k = true;
                                
                                break;

                            }
                        }
                        if (k == true)
                        {
                            MessageBox.Show("The product is already on the bill. You can only change the quantity!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            string[] str = txtSearch.Text.Split('-');
                            txtIdProDuct.Text = str[0];
                            LoadDataToDGV();
                            txtSearch.Text = "";
                            nudQuantity.Value = 1;
                            k = false;
                        }
                    }
                }
            }
            else
            {
                if (e.KeyChar == Convert.ToChar(Keys.Return))
                {
                    //MessageBox.Show("1");
                    if (nudQuantity.Text == "0")
                    {
                        MessageBox.Show("Quantity must be above 0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        foreach (DataGridViewRow row in dgvCashier.Rows)
                        {
                            string IdRow = row.Cells[0].Value.ToString().Trim();
                            if ( IdRow== txtIdProDuct.Text.Trim())
                            {
                                int sl = Int32.Parse(nudQuantity.Value.ToString());
                                row.Cells[3].Value = sl;
                                nudQuantity.Value = 1;
                                co = false;
                                break;
                            }    
                        }    
                    }
                }    
            } 
                
        }

       

        private void txtNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Return))
            {
                int cash = Int32.Parse(txtNumber.Text);
                int Customer = Int32.Parse(lblCustomer.Text);
                if (cash < Customer)
                {
                    MessageBox.Show("Not enough money to pay", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                }    
                else
                {
                    lblCash.Text = cash.ToString();
                    lblCharge.Text = (cash - Customer).ToString();
                }    
            }
        }

        private void btnEsc_Click(object sender, EventArgs e)
        {
            txtNumber.Text = "1000";
            btnName_ID.Text = cmbId.Text;
        }
        private void cmbId_TextChanged(object sender, EventArgs e)
        {
            btnName_ID.Text = cmbId.Text;
            if (string.IsNullOrEmpty(cmbId.Text))
            {
                btnName_ID.Text = "ID - Name";
            }    
        }
        private void btnInvoice_Click(object sender, EventArgs e)
        {
            if (dgvCashier.RowCount == 0 )
            {
                MessageBox.Show("Please choose at least 1 Products!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (lblCash.Text == "0000000")
                {
                    MessageBox.Show("Please enter the cash!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }    
                else
                {
                    if (btnName_ID.Text == "ID - Name")
                    {
                        System.DateTime today = System.DateTime.Today;
                        Invoice newInvoice = new Invoice
                        {
                            ID = BillID.ToString(),
                            CreatedDate = today,
                            Hide = false,
                        };
                        db.Invoices.Add(newInvoice);
                        db.SaveChanges();
                        foreach (DataGridViewRow row in dgvCashier.Rows)
                        {

                            InvoiceDetail newInvoiceDetails = new InvoiceDetail
                            {
                                ID_Invoice = BillID.ToString(),
                                ID_Goods = (row.Cells[0].Value.ToString().Trim()),
                                Quantity = Int32.Parse(row.Cells[3].Value.ToString()),
                                Total = Int32.Parse(row.Cells[4].Value.ToString()),
                            };
                            db.InvoiceDetails.Add(newInvoiceDetails);
                            db.SaveChanges();

                            string id = row.Cells[0].Value.ToString().Trim();
                            Good QuantityUpdate = db.Goods.FirstOrDefault(sv => sv.ID == id);
                            if (QuantityUpdate != null)
                            {
                                if (QuantityUpdate.ShipmentNumber == null) QuantityUpdate.ShipmentNumber = Int32.Parse(row.Cells[3].Value.ToString());
                                else QuantityUpdate.ShipmentNumber = QuantityUpdate.ShipmentNumber+ Int32.Parse(row.Cells[3].Value.ToString());
                                QuantityUpdate.InventoryNumber = QuantityUpdate.InventoryNumber - Int32.Parse(row.Cells[3].Value.ToString());
                            }
                            db.SaveChanges();

                        }
                        MessageBox.Show("Successfully!!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearAll();

                    }    
                    else
                    {
                        string[] checkId = cmbId.Text.Split('-');
                        string ckID = checkId[0];
                        int total =Int32.Parse( lblTotal.Text);
                        Membership thanhvien = db.Memberships.FirstOrDefault(sv => sv.ID == ckID.Trim());
                        if (thanhvien != null)
                        {
                            System.DateTime today = System.DateTime.Today;
                            Invoice newInvoice = new Invoice
                            {
                                ID = BillID.ToString(),
                                CreatedDate = today,
                                Hide = false,
                                ID_Membership = ckID.Trim(),
                            };
                            db.Invoices.Add(newInvoice);
                            db.SaveChanges();

                            Membership memberUpdate = db.Memberships.FirstOrDefault(sv => sv.ID == ckID);
                            memberUpdate.Points = memberUpdate.Points + total;
                            foreach (DataGridViewRow row in dgvCashier.Rows)
                            {

                                InvoiceDetail newInvoiceDetails = new InvoiceDetail
                                {
                                    ID_Invoice = BillID.ToString(),
                                    ID_Goods = (row.Cells[0].Value.ToString().Trim()),
                                    Quantity = Int32.Parse(row.Cells[3].Value.ToString()),
                                    Total = Int32.Parse(row.Cells[4].Value.ToString()),
                                    
                                };
                                db.InvoiceDetails.Add(newInvoiceDetails);
                                db.SaveChanges();
                                string id = row.Cells[0].Value.ToString().Trim();
                                Good QuantityUpdate = db.Goods.FirstOrDefault(sv => sv.ID == id);
                                if (QuantityUpdate != null)
                                {
                                    if (QuantityUpdate.ShipmentNumber == null) QuantityUpdate.ShipmentNumber = Int32.Parse(row.Cells[3].Value.ToString());
                                    else QuantityUpdate.ShipmentNumber = QuantityUpdate.ShipmentNumber + Int32.Parse(row.Cells[3].Value.ToString());
                                    QuantityUpdate.InventoryNumber = QuantityUpdate.InventoryNumber - Int32.Parse(row.Cells[3].Value.ToString());
                                }
                                db.SaveChanges();
                            }
                            MessageBox.Show("Successfully!!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearAll();
                        }
                        else
                        {
                            MessageBox.Show("Please correct the Membeship _ If not please Delete!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }    
            } 
                
        }
        private void ClearAll()
        {
            txtSearch.Text = "";
            nudQuantity.Value = 1;
            lblTotal.Text = "0000000";
            lblCash.Text = "0000000";
            lblCustomer.Text = "0000000";
            lblCharge.Text = "0000000";
            txtNumber.Text = "";
            cmbId.SelectedIndex = -1;
            cmbId.Text = "";
            btnName_ID.Text = "Id - Name";
            dgvCashier.Rows.Clear();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            string Id = "A123";
            Good QuantityUpdate = db.Goods.FirstOrDefault(sv => sv.ID == Id);
            if (QuantityUpdate != null)
            {
                QuantityUpdate.ShipmentNumber = Int32.Parse(dgvCashier.Rows[0].Cells[3].Value.ToString());
                QuantityUpdate.InventoryNumber = QuantityUpdate.InventoryNumber - Int32.Parse(dgvCashier.Rows[0].Cells[3].Value.ToString());
            }
            db.SaveChanges();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {

        }
    }
}
