using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DesignUI
{
    public partial class MainForm : Form
    {
        private Size frmSize;
        private int borderSize = 2;
        public MainForm()
        {
            InitializeComponent();
            this.Padding = new Padding(borderSize);
          
        }
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private static extern void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private static extern void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void btnMenu_Click(object sender, EventArgs e)
        {
            CollapseMenu();
        }
        private void CollapseMenu()
        {
            if (this.panelMenu.Width > 200) //Collapse menu
            {
                panelMenu.Width = 100;
                pictureBox1.Visible = false;
                btnMenu.Dock = DockStyle.Top;
                foreach (Button menuButton in panelMenu.Controls.OfType<Button>())
                {
                    menuButton.Text = "";
                    menuButton.ImageAlign = ContentAlignment.MiddleCenter;
                    menuButton.Padding = new Padding(0);
                }
            }
            else
            { //Expand menu
                panelMenu.Width = 230;
                pictureBox1.Visible = true;
                btnMenu.Dock = DockStyle.None;
                foreach (Button menuButton in panelMenu.Controls.OfType<Button>())
                {
                    menuButton.Text = "   " + menuButton.Tag.ToString();
                    menuButton.ImageAlign = ContentAlignment.MiddleLeft;
                    menuButton.Padding = new Padding(10, 0, 0, 0);
                }
            }
        }
        private void btnMaximize_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                frmSize = this.ClientSize;
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                this.Size = frmSize;
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        // Dieu khien vi tri Form 
        private void panelTitle_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            frmSize = this.ClientSize;
            frm.Dock = DockStyle.Fill;
            frm.TopLevel = false;
            frm.AutoScroll = true;
            panel1.Controls.Add(frm);
            frm.Show();
        }
        // Khoa thanh cong cu 
        /*protected override void WndProc(ref Message m)
        {

            const int WM_NCCALCSIZE = 0x0083;
            const int WM_NCHITTEST = 0x0084;
            if (m.Msg == WM_NCCALCSIZE)
            {
                return;
            }
            const int resizeAreaSize = 10;
            const int HTCLIENT = 1; //Represents the client area of the window
            const int HTLEFT = 10;  //Left border of a window, allows resize horizontally to the left
            const int HTRIGHT = 11; //Right border of a window, allows resize horizontally to the right
            const int HTTOP = 12;   //Upper-horizontal border of a window, allows resize vertically up
            const int HTTOPLEFT = 13;//Upper-left corner of a window border, allows resize diagonally to the left
            const int HTTOPRIGHT = 14;//Upper-right corner of a window border, allows resize diagonally to the right
            const int HTBOTTOM = 15; //Lower-horizontal border of a window, allows resize vertically down
            const int HTBOTTOMLEFT = 16;//Lower-left corner of a window border, allows resize diagonally to the left
            const int HTBOTTOMRIGHT = 17;//Lower-right corner of a window border, allows resize diagonally to the right
            if (m.Msg == WM_NCHITTEST)
            { //If the windows m is WM_NCHITTEST
                base.WndProc(ref m);
                if (this.WindowState == FormWindowState.Normal)//Resize the form if it is in normal state
                {
                    if ((int)m.Result == HTCLIENT)//If the result of the m (mouse pointer) is in the client area of the window
                    {
                        Point screenPoint = new Point(m.LParam.ToInt32()); //Gets screen point coordinates(X and Y coordinate of the pointer)                           
                        Point clientPoint = this.PointToClient(screenPoint); //Computes the location of the screen point into client coordinates                          
                        if (clientPoint.Y <= resizeAreaSize)//If the pointer is at the top of the form (within the resize area- X coordinate)
                        {
                            if (clientPoint.X <= resizeAreaSize) //If the pointer is at the coordinate X=0 or less than the resizing area(X=10) in 
                                m.Result = (IntPtr)HTTOPLEFT; //Resize diagonally to the left
                            else if (clientPoint.X < (this.Size.Width - resizeAreaSize))//If the pointer is at the coordinate X=11 or less than the width of the form(X=Form.Width-resizeArea)
                                m.Result = (IntPtr)HTTOP; //Resize vertically up
                            else //Resize diagonally to the right
                                m.Result = (IntPtr)HTTOPRIGHT;
                        }
                        else if (clientPoint.Y <= (this.Size.Height - resizeAreaSize)) //If the pointer is inside the form at the Y coordinate(discounting the resize area size)
                        {
                            if (clientPoint.X <= resizeAreaSize)//Resize horizontally to the left
                                m.Result = (IntPtr)HTLEFT;
                            else if (clientPoint.X > (this.Width - resizeAreaSize))//Resize horizontally to the right
                                m.Result = (IntPtr)HTRIGHT;
                        }
                        else
                        {
                            if (clientPoint.X <= resizeAreaSize)//Resize diagonally to the left
                                m.Result = (IntPtr)HTBOTTOMLEFT;
                            else if (clientPoint.X < (this.Size.Width - resizeAreaSize)) //Resize vertically down
                                m.Result = (IntPtr)HTBOTTOM;
                            else //Resize diagonally to the right
                                m.Result = (IntPtr)HTBOTTOMRIGHT;
                        }
                    }
                }
                return;
            }
            base.WndProc(ref m);
        }*/

        private void Form1_Resize(object sender, EventArgs e)
        {
            //AdjustForm();
        }
        HomeForm frm = new HomeForm();
        //EventForm frm1 = new EventForm();
        //StudentInfo frm2 = new StudentInfo();
        HRM_Frm HRM = new HRM_Frm();
        Shift_Frm SHIFT = new Shift_Frm();
        public bool check = false;
        private void btnHome_Click(object sender, EventArgs e)
        {
            //frm1.Hide();
            // frm2.Hide();
            //frm3.Hide();
            frm.Hide();
            HRM.Hide();
            SHIFT.Hide();
            staGoods.Hide();
            staInvoice.Hide();
            staFrm.Hide();
            member.Hide();
            hl.Hide();
            CashFrm.Hide();
            ign.Hide();
            GF.Hide();
            txtTitle.Text = "Home";
            frm.Dock = DockStyle.Fill;
            frm.TopLevel = false;
            frm.AutoScroll = true;
            panel1.Controls.Add(frm);
            frm.Show();
        }

        private void btnEvent_Click(object sender, EventArgs e)
        {
            //frm.Hide();
            //frm2.Hide();
            //frm3.Hide();
            //txtTitle.Text = "Event";
            //frm1.Dock = DockStyle.Fill;
            //frm1.TopLevel = false;
            //frm1.AutoScroll = true;
           // panel1.Controls.Add(frm1);
            //frm1.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnTeacher_Click(object sender, EventArgs e)
        {
            
        }

        private void btnHRM_Click(object sender, EventArgs e)
        {
            rjDropdownMenu1.Show(btnHRM, btnHRM.Width, 0);         
        }
        private void hIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm.Hide();
            HRM.Hide();
            SHIFT.Hide();
            staGoods.Hide();
            staInvoice.Hide();
            staFrm.Hide();
            member.Hide();
            hl.Hide();
            CashFrm.Hide();
            ign.Hide();
            GF.Hide();
            txtTitle.Text = "HR Management";
            HRM.Dock = DockStyle.Fill;
            HRM.TopLevel = false;
            HRM.AutoScroll = true;
            panel1.Controls.Add(HRM);
            HRM.Show();
        }

        private void hEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm.Hide();
            HRM.Hide();
            SHIFT.Hide();
            staGoods.Hide();
            staInvoice.Hide();
            staFrm.Hide();
            member.Hide();
            hl.Hide();
            CashFrm.Hide();
            ign.Hide();
            GF.Hide();
            txtTitle.Text = "HR Management";
            SHIFT.Dock = DockStyle.Fill;
            SHIFT.TopLevel = false;
            SHIFT.AutoScroll = true;
            panel1.Controls.Add(SHIFT);
            SHIFT.Show();
        }
        StatisticsFollowGoods staGoods = new StatisticsFollowGoods();
        StatisticsFollowInvoice staInvoice = new StatisticsFollowInvoice();
        InventorFrm staFrm = new InventorFrm();
        private void statisticsGoodsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm.Hide();
            HRM.Hide();
            SHIFT.Hide();
            staGoods.Hide();
            staInvoice.Hide();
            staFrm.Hide();
            member.Hide();
            hl.Hide();
            CashFrm.Hide();
            ign.Hide();
            GF.Hide();
            txtTitle.Text = "Accountant";
            staGoods.Dock = DockStyle.Fill;
            staGoods.TopLevel = false;
            staGoods.AutoScroll = true;
            panel1.Controls.Add(staGoods);
            staGoods.Show();
        }

        private void statisticsInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm.Hide();
            HRM.Hide();
            SHIFT.Hide();
            staGoods.Hide();
            staInvoice.Hide();
            staFrm.Hide();
            member.Hide();
            hl.Hide();
            CashFrm.Hide();
            ign.Hide();
            GF.Hide();
            txtTitle.Text = "Accountant";
            staInvoice.Dock = DockStyle.Fill;
            staInvoice.TopLevel = false;
            staInvoice.AutoScroll = true;
            panel1.Controls.Add(staInvoice);
            staInvoice.Show();
        }

        private void importExportInventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm.Hide();
            HRM.Hide();
            SHIFT.Hide();
            staGoods.Hide();
            staInvoice.Hide();
            staFrm.Hide();
            member.Hide();
            hl.Hide();
            CashFrm.Hide();
            ign.Hide();
            GF.Hide();
            txtTitle.Text = "Accountant";
            staFrm.Dock = DockStyle.Fill;
            staFrm.TopLevel = false;
            staFrm.AutoScroll = true;
            panel1.Controls.Add(staFrm);
            staFrm.Show();
        }

        private void btnAccountant_Click(object sender, EventArgs e)
        {
            rjDropdownMenu2.Show(btnAccountant, btnAccountant.Width, 0);
        }

        private void rjDropdownMenu4_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void btnCSS_Click(object sender, EventArgs e)
        {
            rjDropdownMenu3.Show(btnCSS, btnCSS.Width, 0);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        Membership_Frm member = new Membership_Frm();
        Hotline_Frm hl = new Hotline_Frm();
        private void membershipManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm.Hide();
            HRM.Hide();
            SHIFT.Hide();
            staGoods.Hide();
            staInvoice.Hide();
            staFrm.Hide();
            member.Hide();
            hl.Hide();
            CashFrm.Hide();
            ign.Hide();
            GF.Hide();
            txtTitle.Text = "Customer Service Staff";
            member.Dock = DockStyle.Fill;
            member.TopLevel = false;
            member.AutoScroll = true;
            panel1.Controls.Add(member);
            member.Show();
        }

        private void hotlineProblemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm.Hide();
            HRM.Hide();
            SHIFT.Hide();
            staGoods.Hide();
            staInvoice.Hide();
            staFrm.Hide();
            member.Hide();
            hl.Hide();
            CashFrm.Hide();
            ign.Hide();
            GF.Hide();
            txtTitle.Text = "Customer Service Staff";
            hl.Dock = DockStyle.Fill;
            hl.TopLevel = false;
            hl.AutoScroll = true;
            panel1.Controls.Add(hl);
            hl.Show();
        }
        Cashier_Fr_ CashFrm = new Cashier_Fr_();
        private void btnCashier_Click(object sender, EventArgs e)
        {
            frm.Hide();
            HRM.Hide();
            SHIFT.Hide();
            staGoods.Hide();
            staInvoice.Hide();
            staFrm.Hide();
            member.Hide();
            hl.Hide();
            CashFrm.Hide();
            ign.Hide();
            GF.Hide();
            txtTitle.Text = "Cashier";
            CashFrm.Dock = DockStyle.Fill;
            CashFrm.TopLevel = false;
            CashFrm.AutoScroll = true;
            panel1.Controls.Add(CashFrm);
            CashFrm.Show();
        }

        ImportGoodsNote ign = new ImportGoodsNote();
        Goods_Frm GF = new Goods_Frm();
        private void btnStorageM_Click(object sender, EventArgs e)
        {
            rjDropdownMenu4.Show(btnStorageM, btnAccountant.Width, 0);
        }

        private void importProductNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm.Hide();
            HRM.Hide();
            SHIFT.Hide();
            staGoods.Hide();
            staInvoice.Hide();
            staFrm.Hide();
            member.Hide();
            hl.Hide();
            CashFrm.Hide();
            ign.Hide();
            GF.Hide();
            txtTitle.Text = "Storage Management";
            ign.Dock = DockStyle.Fill;
            ign.TopLevel = false;
            ign.AutoScroll = true;
            panel1.Controls.Add(ign);
            ign.Show();
        }

        private void productManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm.Hide();
            HRM.Hide();
            SHIFT.Hide();
            staGoods.Hide();
            staInvoice.Hide();
            staFrm.Hide();
            member.Hide();
            hl.Hide();
            CashFrm.Hide();
            ign.Hide();
            GF.Hide();
            txtTitle.Text = "Storage Management";
            GF.Dock = DockStyle.Fill;
            GF.TopLevel = false;
            GF.AutoScroll = true;
            panel1.Controls.Add(GF);
            GF.Show();
        }
    }
}
