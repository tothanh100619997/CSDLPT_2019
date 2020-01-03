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
    public partial class frmVatTu : Form
    {
        private int position = 0;
        public frmVatTu()
        {
            InitializeComponent();
        }

        private void VattuBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsVatTu.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dS);

        }

        private void FrmVatTu_Load(object sender, EventArgs e)
        {
            
            // TODO: This line of code loads data into the 'dS.vattu' table. You can move, or remove it, as needed.
            this.vattuTableAdapter.Connection.ConnectionString = Program.connstr;
            this.vattuTableAdapter.Fill(this.dS.Vattu);

            if (Program.mGroup == "CONGTY")
            {
                this.btnThem.Enabled = this.btnXoa.Enabled = false;
                this.btnPhucHoi.Enabled = this.btnGhi.Enabled = this.btnRefresh.Enabled = false;
                this.groupBox1.Enabled = this.btnSua.Enabled = false;

            }
           
            this.btnPhucHoi.Enabled = false;
            this.groupBox1.Enabled = false;
            bdsVatTu.Position = 0;

        }

        private void DVTLabel_Click(object sender, EventArgs e)
        {

        }

        private void BtnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            position = bdsVatTu.Count;
            this.bdsVatTu.AddNew();
            //Giá trị mặc định khi Thêm NV           
            txtSLT.Text = 0.ToString();
            txtDonVi.Text = "Cái";    
            btnThem.Enabled = gcVT.Enabled = false;
            btnRefresh.Enabled = btnSua.Enabled = btnXoa.Enabled= false;
            btnPhucHoi.Enabled = groupBox1.Enabled = btnGhi.Enabled = true;
        }

        private void BtnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
           
            string mavt = "";
            if (bdsVatTu.Position != -1)
            {
                mavt = ((DataRowView)bdsVatTu[bdsVatTu.Position])["MAVT"].ToString();
            }
            else
            {
                MessageBox.Show("Không có dữ liệu!", "Notification",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DialogResult dr = MessageBox.Show("Bạn có thực sự muốn xóa vật tư này?", "Xác nhận",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                try
                {
                    //Kiểm tra MAVT có tồn tại trong các Phiếu
                    string query = "DECLARE	@result int " +
                          "EXEC @result = SP_CheckID '"+txtMAVT.Text+"', "+ "'MAVT_EXIST'" +
                          "SELECT 'result' = @result";
                    SqlDataReader dataReader = Program.ExecSqlDataReader(query);
                   
                    dataReader.Read();
                    int result = int.Parse(dataReader.GetValue(0).ToString());
                    dataReader.Close();
                    if (result == 1)
                    {
                        MessageBox.Show("Vật tư này đã tồn tại trong các Phiếu, không thể xóa. Vui lòng kiểm tra lại!\n", "Notification",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    bdsVatTu.RemoveCurrent();
                    this.vattuTableAdapter.Update(this.dS.Vattu);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa vật tư hãy xóa lại! \n" + ex.Message, "Thông báo lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.vattuTableAdapter.Fill(this.dS.Vattu);
                    bdsVatTu.Position = bdsVatTu.Find("MAVT", mavt);
                    return;
                }
            }
            if (bdsVatTu.Count == 0) btnXoa.Enabled = false;
        }

        private void BtnSua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            position = bdsVatTu.Position;
            btnSua.Enabled = gcVT.Enabled = btnThem.Enabled = false;
            btnXoa.Enabled = btnRefresh.Enabled = false;
            groupBox1.Enabled = btnPhucHoi.Enabled = btnGhi.Enabled = true;
            txtMAVT.Enabled = false;
            txtSLT.Enabled = false;
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

        private void BtnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Console.WriteLine(Program.mChinhanh);
            Console.WriteLine(Program.servername);
            if (!checkValidate(txtMAVT, "Mã vật tư không được để trống!")) return;
            if (!checkValidate(txtTen, "Tên vật tư không được để trống!")) return;
            if (!checkValidate(txtDonVi, "Đơn vị tính không được trống!")) return;
            if (txtMAVT.Text.Trim().Length > 4)
            {
                MessageBox.Show("Mã VT không được quá 4 kí tự!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }           
          
            int findPositionTENVT = bdsVatTu.Find("TENVT", txtMAVT.Text);
            if (findPositionTENVT != -1 && (findPositionTENVT != bdsVatTu.Position))
            {
                MessageBox.Show("Tên Vật tư trùng. Vui lòng chọn tên Vật tư khác!", "Notification",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string query = "DECLARE	@result int " +
                           "EXEC @result = SP_CheckID '" + txtMAVT.Text + "', " + "'MAVT'" +
                           "SELECT 'result' = @result";
            SqlDataReader dataReader = Program.ExecSqlDataReader(query);
            if (dataReader == null) return;
          
          
            dataReader.Read();
            int result = int.Parse(dataReader.GetValue(0).ToString());
            dataReader.Close();
            int positionMAVT = bdsVatTu.Find("MAVT", txtMAVT.Text);
            int postionCurrent = bdsVatTu.Position;
            //Bỏ qua TH tồn tại ở CN hiện tại khi vị trí MANV đang nhập đúng băng vị trí đang đứng
            if (result == 1 && (positionMAVT != postionCurrent))
            {
                MessageBox.Show("Mã VT đã tồn tại!", "Thông báo",
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
                        
                        btnThem.Enabled = btnXoa.Enabled = gcVT.Enabled = btnRefresh.Enabled = btnSua.Enabled = true;
                        btnPhucHoi.Enabled  = groupBox1.Enabled = false;
                        this.bdsVatTu.EndEdit();
                        this.vattuTableAdapter.Update(this.dS.Vattu);
                        bdsVatTu.Position = position;
                    }
                    catch (Exception ex)
                    {
                        //Khi Update database lỗi thì xóa record vừa thêm trong bds
                        bdsVatTu.RemoveCurrent();
                        MessageBox.Show("Ghi dữ liệu thất lại. Vui lòng kiểm tra lại!\n" + ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnPhucHoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btnThem.Enabled = btnXoa.Enabled = gcVT.Enabled = btnRefresh.Enabled = btnSua.Enabled = true;
            btnPhucHoi.Enabled = groupBox1.Enabled = false;       
            bdsVatTu.CancelEdit();
            bdsVatTu.Position = position;
        }

        private void BtnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            groupBox1.Enabled = false;
            gcVT.Enabled = true;
            int position = bdsVatTu.Position;
            this.vattuTableAdapter.Fill(this.dS.Vattu);
            bdsVatTu.Position = position;

        }

        private void BtnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
    }
}
