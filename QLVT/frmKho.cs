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
using DevExpress.XtraEditors;

namespace QLVT
{
    public partial class frmKho : Form
    {
        private string maChiNhanh = "";
        private int position = 0;
        public frmKho()
        {
            InitializeComponent();
        }

        private void FrmKho_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dS.Kho' table. You can move, or remove it, as needed.

            // TODO: This line of code loads data into the 'qL_VATUDataSet.V_DS_PHANMANH' table. You can move, or remove it, as needed.
            this.khoTableAdapter.Connection.ConnectionString = Program.connstr;
            this.v_DS_PHANMANHTableAdapter.Fill(this.qL_VATUDataSet.V_DS_PHANMANH);
            this.khoTableAdapter.Fill(this.dS.Kho);
            //Tu dong chon chi nhanh
            this.cmbChiNhanh.SelectedIndex = Program.mChinhanh;
            maChiNhanh = (((DataRowView)bdsKho[0])["MACN"].ToString());
            if (Program.mGroup == "CONGTY")
            {
                this.btnThem.Enabled = this.btnXoa.Enabled = false;
                this.btnPhucHoi.Enabled = this.btnGhi.Enabled = this.btnRefresh.Enabled  = false;
                this.groupBox1.Enabled = this.btnSua.Enabled = false;

            }
            else
            {
                this.cmbChiNhanh.Enabled = false;
            }

            this.btnPhucHoi.Enabled = false;
            this.groupBox1.Enabled = false;
        }

        private void BtnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            position = bdsKho.Count;
            this.bdsKho.AddNew();
            //Giá trị mặc định khi Thêm 
            txtMaChiNhanh.Text = maChiNhanh;   
            
            btnThem.Enabled = gcKho.Enabled = btnXoa.Enabled = btnSua.Enabled = false; 
            btnRefresh.Enabled = false;
            btnPhucHoi.Enabled = groupBox1.Enabled = btnGhi.Enabled = true;
        }

        private void CmbChiNhanh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbChiNhanh.SelectedValue != null)
            {



                if (Program.servername != cmbChiNhanh.SelectedValue.ToString())
                {
                    Program.servername = cmbChiNhanh.SelectedValue.ToString();
                }
                if (cmbChiNhanh.SelectedIndex != Program.mChinhanh)
                {
                    Program.mloginDN = Program.remotelogin;
                    Program.passwordDN = Program.remotepassword;
                }
                else
                {
                    Program.mloginDN = Program.mlogin;
                    Program.passwordDN = Program.password;
                }

                if (Program.KetNoi(Program.mloginDN, Program.passwordDN) != 1)
                {
                    return;
                }
                this.khoTableAdapter.Connection.ConnectionString = Program.connstr;
                this.khoTableAdapter.Fill(this.dS.Kho);
            }
        }

        private void BtnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string makho = "";
            if (bdsKho.Position != -1)
            {
                makho = ((DataRowView)bdsKho[bdsKho.Position])["MAKHO"].ToString();
            }
            else
            {
                MessageBox.Show("Không có dữ liệu!", "Notification",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DialogResult dr = MessageBox.Show("Bạn có thực sự muốn xóa nhân viên này?", "Xác nhận",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if(dr == DialogResult.OK)
            {
                bdsKho.RemoveCurrent();
                this.khoTableAdapter.Update(this.dS.Kho);
            }
        }

        private void BtnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void BtnSua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            position = bdsKho.Position;
            gcKho.Enabled = btnThem.Enabled = btnSua.Enabled = false;
            btnXoa.Enabled = btnRefresh.Enabled = false;
            groupBox1.Enabled = btnPhucHoi.Enabled = btnGhi.Enabled = true;
        }
       
        private bool checkValidate(TextEdit txtMaKho, string v)
        {
            if (txtMaKho.Text.Trim().Equals(""))
            {
                MessageBox.Show(v, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaKho.Focus();
                return false;
            }
            return true;
        }

        private void BtnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!checkValidate(txtMaKho, "Mã Kho không được trống!")) return;
            if (!checkValidate(txtTenKho, "Tên Kho không được trống!")) return;
            if (!checkValidate(txtDiaChi, "Địa chỉ không được trống!")) return;
            if (txtMaKho.Text.Trim().Length > 4)
            {
                MessageBox.Show("Mã KHO không được quá 4 kí tự!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
           
            string query = "DECLARE	@result int " +
                           "EXEC @result = SP_CheckID '"+txtMaKho.Text+"' ,"+ "'MAKHO' " +
                           "SELECT 'result' = @result";

            SqlDataReader dataReader = Program.ExecSqlDataReader(query);

            if (dataReader == null) return;
            dataReader.Read();
            int resultMAKHO = int.Parse(dataReader.GetValue(0).ToString());
            dataReader.Close();

            query = "DECLARE @result int " +
                           "EXEC @result = SP_CheckID '" + txtTenKho.Text + "' ," + "'TENKHO' " +
                           "SELECT 'result' = @result";
            dataReader = Program.ExecSqlDataReader(query);
            if (dataReader == null) return;
            dataReader.Read();
            int resultTENKHO = int.Parse(dataReader.GetValue(0).ToString());
            dataReader.Close();

          
           
            //Bỏ qua TH tồn tại ở CN hiện tại khi vị trí MANV đang nhập đúng băng vị trí đang đứng
            if (resultMAKHO == 1)
            {
                MessageBox.Show("Mã KHO đã tồn tại ở Chi Nhánh hiện tại!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (resultTENKHO == 1 )
            {
                MessageBox.Show("Tên Kho đã tồn tại ở Chi Nhánh hiện tại!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (resultMAKHO == 2)
            {
                MessageBox.Show("Mã KHO đã tồn tại ở Chi Nhánh khác!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (resultTENKHO == 2)
            {
                MessageBox.Show("Tên Kho đã tồn tại ở Chi Nhánh khác!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DialogResult dr = MessageBox.Show("Bạn có chắc muốn ghi dữ liệu vào Database?", "Thông báo",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    try
                    {
                        btnThem.Enabled = btnXoa.Enabled = gcKho.Enabled = true;
                        btnRefresh.Enabled = true;
                        btnPhucHoi.Enabled = groupBox1.Enabled = btnGhi.Enabled = false;
                        this.bdsKho.EndEdit();
                        this.khoTableAdapter.Update(this.dS.Kho);
                        bdsKho.Position = position;
                    }
                    catch (Exception ex)
                    {
                        //Khi Update database lỗi thì xóa record vừa thêm trong bds
                        bdsKho.RemoveCurrent();
                        MessageBox.Show("Ghi dữ liệu thất lại. Vui lòng kiểm tra lại!\n" + ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

       

        private void BtnPhucHoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btnThem.Enabled = btnXoa.Enabled = gcKho.Enabled = true;
            btnRefresh.Enabled = true;
            btnPhucHoi.Enabled = groupBox1.Enabled = false;
            bdsKho.CancelEdit();
            bdsKho.Position = position;
        }

        private void BtnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            this.khoTableAdapter.Connection.ConnectionString = Program.connstr;
            this.v_DS_PHANMANHTableAdapter.Fill(this.qL_VATUDataSet.V_DS_PHANMANH);
            this.khoTableAdapter.Fill(this.dS.Kho);
        }
    }
}
