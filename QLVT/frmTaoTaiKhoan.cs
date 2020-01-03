using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT
{
    public partial class frmTaoTaiKhoan : Form
    {
        public frmTaoTaiKhoan()
        {
            InitializeComponent();
        }

        private void FrmTaoTaiKhoan_Load(object sender, EventArgs e)
        {
            if (Program.mGroup == "CONGTY")
            {
                radioChiNhanh.Enabled = radioUser.Enabled = false;
            }
            else if (Program.mGroup == "CHINHANH")
            {
                radioCongTY.Enabled = false;
            }
           


        }

        private void BtnTaoTaiKhoan_Click(object sender, EventArgs e)
        {
            if (!checkValidate(txtLogin, "Login không được trống!")) return;
            if (!checkValidate(txtPass, "Password không được trống!")) return;
            if (!checkValidate(txtUser, "User is not không được trống!")) return;
            if (!(radioCongTY.Checked || radioChiNhanh.Checked || radioUser.Checked))
            {
                MessageBox.Show("Role is not empty!!", "Notification",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtLogin.Text.Contains(" "))
            {
                MessageBox.Show("Login name không được chứa khoảng trắng!", "Notification",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string query = "DECLARE	@result int " +
                           "EXEC @result = SP_CheckID '"+txtUser.Text+"' ," +"'MANV' " +
                           "SELECT 'result' = @result";
            SqlDataReader dataReader = Program.ExecSqlDataReader(query);
            if (dataReader == null) return;
            dataReader.Read();
            int resultCheckMANV = int.Parse(dataReader.GetValue(0).ToString());
            dataReader.Close();
            if (resultCheckMANV != 1)
            {
                MessageBox.Show("Mã NV không tồn tại ở Chi Nhánh hiện tại vui lòng kiểm tra lại!\n", "Notification",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLogin.Focus();
                return;
            }

            query = "DECLARE @result int " +
                           "EXEC @result = SP_CheckID '" + txtLogin.Text + "' ," + "'"+txtUser.Text+"' " +
                           "SELECT 'result' = @result";
            dataReader = Program.ExecSqlDataReader(query);
            if (dataReader == null) return;
            dataReader.Read();
            int resultCheckLogin = int.Parse(dataReader.GetValue(0).ToString());
            dataReader.Close();

            if (resultCheckLogin == 1)
            {
                MessageBox.Show("Login name bị trùng. Vui lòng chọn tên login khác!\n", "Notification",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLogin.Focus();
                return;
            }
            else if (resultCheckLogin == 2)
            {
                MessageBox.Show("User bị trùng. Vui lòng chọn nhân viên khác!\n", "Notification",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUser.Focus();
                return;
            }
            else
            {
                String role = radioCongTY.Checked ? "CONGTY" : (radioChiNhanh.Checked ? "CHINHANH" : "USER");

                query = "DECLARE @result int " +
                           "EXEC @result = sp_TaoTaiKhoan  '"+txtLogin.Text+"' ," +" '"+txtPass.Text+"',"+ " '"+txtUser.Text+"' "+",'"+role+ "' " +
                           "SELECT 'result' = @result";
                
                dataReader = Program.ExecSqlDataReader(query);
                if (dataReader == null) return;
                dataReader.Read();
                int result = int.Parse(dataReader.GetValue(0).ToString());
                dataReader.Close();
                if (result == 0)
                {
                    MessageBox.Show("Tạo tài khoản thành công!", "Notification",
                       MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtLogin.Clear();
                    txtPass.Clear();
                    txtUser.Clear();
                    radioCongTY.Checked = radioChiNhanh.Checked = radioUser.Checked = false;
                    txtLogin.Focus();
                }
            }

        }
        private bool checkValidate(TextBox tb, string str)
        {
            if (tb.Text.Trim().Equals(""))
            {
                MessageBox.Show(str, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tb.Focus();
                return false;
            }
            return true;
        }
    }
}
