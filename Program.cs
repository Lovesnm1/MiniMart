using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesignUI
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
           // Application.Run(new MainForm());
            Application.Run(new HomeForm());
            //Application.Run(new HRM_Frm());
            //Application.Run(new Shift_Frm());
            //Application.Run(new StatisticsFollowInvoice());
            //Application.Run(new StatisticsFollowGoods());
            //Application.Run(new Membership_Frm());
            //Application.Run(new Hotline_Frm());
            //Application.Run(new Cashier_Fr_());
            //Application.Run(new Goods_Frm());
            //Application.Run(new ImportGoodsNote());
        }
    }
}
