using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;

namespace QLVT
{
    public partial class frmMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public frmMain()
        {
            InitializeComponent();
            frmNhanVien f = new frmNhanVien();
            f.MdiParent = this;
            f.Show();
        }
        private Form CheckExists(Type ftype)
        {
            foreach (Form f in this.MdiChildren)
                if (f.GetType() == ftype)
                    return f;
            return null;
        }


        private void FrmMain_Load(object sender, EventArgs e)
        {

        }



        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {


            DialogResult dr;
            dr = MessageBox.Show("Bạn có muốn thoát khỏi chương trình không ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {

                Application.Exit();

            }
            else
            {
                e.Cancel = true;

            }
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void Ribbon_Click(object sender, EventArgs e)
        {
            
        }

        private void BtnNhanVIenClick(object sender, ItemClickEventArgs e)
        {
            Form frm = this.CheckExists(typeof(frmNhanVien));
            if (frm != null) frm.Activate();
            else
            {
                frmNhanVien f = new frmNhanVien();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void BtnDangXuat_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Hide();
            Program.frmLogin.Visible = true;

        }









    }
}
