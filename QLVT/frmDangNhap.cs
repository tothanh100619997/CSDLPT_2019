using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace QLVT
{
    public partial class frmDangNhap : Form
    {
        public frmDangNhap()
        {
            InitializeComponent();
        }

        private void GroupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void FrmDangNhap_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'qL_VATUDataSet.V_DS_PHANMANH' table. You can move, or remove it, as needed.
            this.v_DS_PHANMANHTableAdapter.Fill(this.qL_VATUDataSet.V_DS_PHANMANH);
            cbTenChiNhanh.SelectedIndex = 1;
            cbTenChiNhanh.SelectedIndex = 0;
           
        }

        private void CbTenChiNhanh_SelectedIndexChanged(object sender, EventArgs e)
        {   if(cbTenChiNhanh.SelectedValue != null)
            {
                Program.servername = cbTenChiNhanh.SelectedValue.ToString();

            }
        
        }

        private void BtnDangNhap_Click(object sender, EventArgs e)
        {
            if (txtLogin.Text.Trim() == "")
            {
                MessageBox.Show("Tài khoản đăng nhập không được rỗng !","Báo lỗi đăng nhập",MessageBoxButtons.OK);
                txtLogin.Focus();
                return;
            }
            Program.mlogin = txtLogin.Text;
            Program.password = txtPass.Text;
            if (Program.KetNoi()!=1)
            {
                return;
            }

            Program.mChinhanh = cbTenChiNhanh.SelectedIndex;

            Program.mloginDN = Program.mlogin;
            Program.passwordDN = Program.password;
            string strLenh = "EXEC sp_DangNhap '" + Program.mlogin + "'";

            Program.myReader = Program.ExecSqlDataReader(strLenh);
            if (Program.myReader == null) return;
            Program.myReader.Read();


            Program.username = Program.myReader.GetString(0);     // Lay user name
            if (Convert.IsDBNull(Program.username))
            {
                MessageBox.Show("Login bạn nhập không có quyền truy cập dữ liệu\n Bạn xem lại username, password", "", MessageBoxButtons.OK);
                return;
            }
            Program.mHoten = Program.myReader.GetString(1);
            Program.mGroup = Program.myReader.GetString(2);
            Program.myReader.Close();
            Program.conn.Close();
            //MessageBox.Show("Nhan vien - Nhom : " + Program.mHoten + " - " + Program.mGroup, "", MessageBoxButtons.OK);
            Program.frmChinh.MANV.Text  = "Mã nhân viên: " + Program.username;
            Program.frmChinh.HOTEN.Text = "Họ tên: " + Program.mHoten;
            Program.frmChinh.NHOM.Text  = "Nhóm: " + Program.mGroup;

            Program.frmChinh.Activate();
            Program.frmChinh.Show();
            this.Visible = false;

        }

        private void BtnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmDangNhap_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void TxtLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) txtPass.Focus();
        }

        private void TxtPass_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) BtnDangNhap_Click(sender, e);
        }
    }
}
