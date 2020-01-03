using DevExpress.Utils.Menu;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QLVT
{
    public partial class frmNhanVien : Form
    {
        private int position = 0;
        private string maChiNhanh = "";
        public frmNhanVien()
        {
            InitializeComponent();
            
        }

        private void NhanVienBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsNV.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dS);

        }

        private void FrmNhanVien_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dS.CTDDH' table. You can move, or remove it, as needed.
            this.cTDDHTableAdapter.Fill(this.dS.CTDDH);
            // TODO: This line of code loads data into the 'dS.DatHang' table. You can move, or remove it, as needed.
            this.datHangTableAdapter.Fill(this.dS.DatHang);
            // TODO: This line of code loads data into the 'dS.CTDDH' table. You can move, or remove it, as needed.

            // TODO: This line of code loads data into the 'dS.DatHang' table. You can move, or remove it, as needed.

            // TODO: This line of code loads data into the 'qL_VATUDataSet.V_DS_PHANMANH' table. You can move, or remove it, as needed.
            this.v_DS_PHANMANHTableAdapter.Fill(this.qL_VATUDataSet.V_DS_PHANMANH);
            // TODO: This line of code loads data into the 'dS.NhanVien' table. You can move, or remove it, as needed.
            this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
            this.nhanVienTableAdapter.Fill(this.dS.NhanVien);
            //Tu dong chon chi nhanh
            this.cmbPhanManh.SelectedIndex = Program.mChinhanh;
            maChiNhanh = (((DataRowView)bdsNV[0])["MACN"].ToString());

            
            if (Program.mGroup == "CONGTY")
            {
                this.btnThem.Enabled = this.btnXoa.Enabled  = false;
                this.btnPhucHoi.Enabled = this.btnGhi.Enabled = this.btnRefresh.Enabled = this.btnChuyenChiNhanh.Enabled = false;
                this.groupBox1.Enabled = this.btnSua.Enabled = false;

            }else
            {
                this.cmbPhanManh.Enabled = false;
            }
           
                this.btnPhucHoi.Enabled = false;
                this.groupBox1.Enabled = false;
            switchPanel("Thông tin nhân viên", groupBox1);

        }
        private void GroupBox1_Enter(object sender, EventArgs e)
        {

        }
        private void HOLabel_Click(object sender, EventArgs e)
        {

        }
        private void TxtHO_TextChanged(object sender, EventArgs e)
        {

        }
        private void TENLabel_Click(object sender, EventArgs e)
        {

        }
        private void TxtTEN_TextChanged(object sender, EventArgs e)
        {

        }        
        private void MACNLabel_Click_1(object sender, EventArgs e)
        {

        }
        private void BtnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            switchPanel("Thông tin nhân viên", groupBox1);
            position = bdsNV.Count;
            this.bdsNV.AddNew();
            //Giá trị mặc định khi Thêm NV
            txtMANV.Text = newMaNV().ToString();
            txtLUONG.Text = 4000000.ToString();
            ((DataRowView)bdsNV[bdsNV.Position])["LUONG"] = 4000000;
            txtMACHINHANH.Text = maChiNhanh;
            cbTRANGTHAIXOA.Checked = cbTRANGTHAIXOA.Enabled = false;
            btnThem.Enabled = gcNV.Enabled = btnXoa.Enabled = btnSua.Enabled = false; ;

            btnChuyenChiNhanh.Enabled = btnRefresh.Enabled  =  false;
            btnPhucHoi.Enabled = groupBox1.Enabled = btnGhi.Enabled = true;
           
          
           
        }

        private void CmbPhanManh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPhanManh.SelectedValue != null)
            {

                    
                
                    if (Program.servername != cmbPhanManh.SelectedValue.ToString())
                    {
                        Program.servername = cmbPhanManh.SelectedValue.ToString();
                    }
                    if (cmbPhanManh.SelectedIndex != Program.mChinhanh)
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
                this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
                this.nhanVienTableAdapter.Fill(this.dS.NhanVien);
            }
        }

        private void BtnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
        public static int newMaNV()
        {
            string query = "SELECT *FROM V_ListMANV";           
            SqlDataReader dataReader = Program.ExecSqlDataReader(query);
            List<int> list = new List<int>();
            try
            {
                while (dataReader.Read())
                {
                    list.Add(dataReader.GetInt32(0));
                    
                }
                dataReader.Close();

                for (int i = 1; i <= list[list.Count - 1]; i++)
                {
                    if (list.BinarySearch(i) < 0) return i; //Tìm thấy số âm tức số đó chưa tồn tại. Nhận số đó là ID mới
                }
                return list[(list.Count - 1)] + 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm Mã Nhân Viên mới! \n" + ex.Message, "Notification", MessageBoxButtons.OK);
                dataReader.Close();
            }
            return -1;  //Không tìm thấy trả -1 đánh dấu Dừng chương trình
        }

        private void TxtLUONG_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
            (e.KeyChar != '.'))
            {

                e.Handled = true;

            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
                
            }
        }

        private void BtnSua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            switchPanel("Thông tin nhân viên", groupBox1);
            position = bdsNV.Position;
             gcNV.Enabled = btnThem.Enabled = btnSua.Enabled = false;
            btnXoa.Enabled = btnRefresh.Enabled = btnChuyenChiNhanh.Enabled = false;
            groupBox1.Enabled = btnPhucHoi.Enabled = btnGhi.Enabled = cbTRANGTHAIXOA.Enabled = true;
        }

        private void BtnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
            if (!checkValidate(txtMANV, "Mã NV không được trống!")) return;
            if (!checkValidate(txtHO, "Họ không được trống!")) return;
            if (!checkValidate(txtTEN, "Tên tên không được trống!")) return;
            if (!checkValidate(txtDIACHI, "Địa chỉ không được trống!")) return;

            if (!dtpNGAYSINH.Checked)
            {
                    MessageBox.Show("Vui lòng chọn ngày sinh", "Notification",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dtpNGAYSINH.Focus();
                    return;
            }

            try
                {
                   
                    btnThem.Enabled = btnXoa.Enabled = gcNV.Enabled = true;
                    btnChuyenChiNhanh.Enabled = btnRefresh.Enabled = true;
                    btnPhucHoi.Enabled = groupBox1.Enabled = btnGhi.Enabled = false;
                    this.bdsNV.EndEdit();
                    this.nhanVienTableAdapter.Update(this.dS.NhanVien);
                    bdsNV.Position = position;
                }
                catch (Exception ex)
                {
                    //Khi Update database lỗi thì xóa record vừa thêm trong bds
                    bdsNV.RemoveCurrent();
                    MessageBox.Show("Ghi dữ liệu thất lại. Vui lòng kiểm tra lại!\n" + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            
        }

        private void BtnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
            int positionNV = 0;
            positionNV = bdsNV.Position;
            string nameNV = (((DataRowView)bdsNV[positionNV])["TEN"].ToString());
            DialogResult dr1 = MessageBox.Show("Bạn có thực sự muốn xóa nhân viên ##  "+nameNV+" ## không?", "Xác nhận",
                                                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr1 != DialogResult.OK) return;

            if (this.cbTRANGTHAIXOA.Checked) 
            {
                MessageBox.Show("Nhân Viên này đã bị xóa hoặc chuyển chi nhánh. Vui lòng chọn nhân viên khác!\n", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (int.Parse(txtMANV.Text) == int.Parse(Program.username))
            {
                MessageBox.Show("Tài khoản Nhân Viên đang được đăng nhập không thể xóa. Vui lòng chọn nhân viên khác!\n", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Update trạng thái và xóa login, user(nếu có)
            string query = "DECLARE @result int " +
                  "EXEC @result = SP_DeleteNV "+int.Parse(txtMANV.Text)+" " +
                  "SELECT 'result' = @result";
            SqlDataReader dataReader = Program.ExecSqlDataReader(query);
            if (dataReader == null) return;
            dataReader.Read();
           
            int result = int.Parse(dataReader.GetValue(0).ToString());
            dataReader.Close();
            if (result == 1)
            {
                MessageBox.Show("Xóa Login không thành công. Vui lòng liên hệ Quản trị viên!", "Thông báo",
                     MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (result == 2)
            {
                MessageBox.Show("Xóa User không thành công. Vui lòng liên hệ Quản trị viên!", "Thông báo",
                     MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
           
            this.nhanVienTableAdapter.Fill(this.dS.NhanVien);

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

        private void BtnPhucHoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btnThem.Enabled = btnXoa.Enabled = gcNV.Enabled = true;
            btnRefresh.Enabled = btnChuyenChiNhanh.Enabled   = true;
            btnPhucHoi.Enabled = groupBox1.Enabled  = false;   
            bdsNV.CancelEdit();
            bdsNV.Position = position;
        }

        private void BtnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.readload();
        }
        public void readload()
        {
            this.v_DS_PHANMANHTableAdapter.Fill(this.qL_VATUDataSet.V_DS_PHANMANH);
            // TODO: This line of code loads data into the 'dS.NhanVien' table. You can move, or remove it, as needed.
            this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
            this.nhanVienTableAdapter.Fill(this.dS.NhanVien);
        }

        private void BtnChuyenChiNhanh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int positionNV = bdsNV.Find("MANV", int.Parse(txtMANV.Text));
            int trangthaixoaNV = int.Parse(((DataRowView)bdsNV[positionNV])["TrangThaiXoa"].ToString());
            if (trangthaixoaNV == 1)
            {
                MessageBox.Show("Nhân Viên này đã bị xóa hoặc chuyển chi nhánh. Vui lòng chọn nhân viên khác!\n", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (int.Parse(txtMANV.Text) == int.Parse(Program.username))
            {
                MessageBox.Show("Tài khoản Nhân Viên đang được đăng nhập không thể chuyển chi nhánh. Vui lòng chọn nhân viên khác!\n", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DialogResult resultDR = MessageBox.Show("Bạn có chắc muốn chuyển nhân viên này?", "Xác nhận",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (resultDR == DialogResult.OK)
            {
                position = bdsNV.Position;
                int MANVCurrent;
                int MANVNew;
                try
                {
                    MANVCurrent = int.Parse(((DataRowView)bdsNV[bdsNV.Position])["MANV"].ToString());
                    MANVNew = newMaNV();
                    if (MANVNew == -1) return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi Parse Int!\n" + ex.Message, "Notification",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string query = "DECLARE	@result int " +
                               "EXEC @result = SP_ChuyenCN @p1, @p2 " +
                               "SELECT 'result' = @result";
                SqlCommand sqlCommand = new SqlCommand(query, Program.conn);
                sqlCommand.Parameters.AddWithValue("@p1", MANVCurrent);
                sqlCommand.Parameters.AddWithValue("@p2", MANVNew);
                SqlDataReader dataReader = null;
                try
                {
                    dataReader = sqlCommand.ExecuteReader();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thực thi Database!\n" + ex.Message, "Notification",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //dataReader.Close();
                    return;
                }
                dataReader.Read();
                int result = int.Parse(dataReader.GetValue(0).ToString());
                dataReader.Close();
                
                if (result == -2)         //Trường hợp 1: Kiểm tra thấy bên chi nhánh kia có ít nhất 1 NV giống nhau về họ tên, ngày sinh
                {
                    Program.maNVChuyenCN = MANVCurrent;
                    Program.popupFormNVTrung = new PopupFormNVTrung();
                    Program.popupFormNVTrung.Show();
                    Program.frmChinh.Enabled = false;
                }
                else if (result == -1)    //Trường hợp thất bại
                {
                    MessageBox.Show("Chuyển Chi nhánh thất bại! Dữ liệu đã được Roolback!", "Notification",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (result == -3)
                {
                    MessageBox.Show("Xóa Login không thành công. Vui lòng liên hệ Quản trị viên!", "Notification",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (result == -4)
                {
                    MessageBox.Show("Xóa User không thành công. Vui lòng liên hệ Quản trị viên!", "Notification",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (result == 0)     //Trường hợp thành công khi bên Chi nhánh kia nhân viên chưa từng chuyển chi nhánh
                {
                    MessageBox.Show("Chuyển chi nhánh thành công. Với Mã Nhân Viên mới là: " + MANVNew, "Notification",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.nhanVienTableAdapter.Fill(this.dS.NhanVien);  //Cập nhật xong Refresh lại 
              
            }
        }
        //==================================== group đơn đặc hàng===================
        private void BtnDH_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            switchPanel("Đơn đặc hàng", gbDDH);
            
            bdsNV.Position = bdsNV.Find("MANV", Program.username);
        }
        private void switchPanel(string caption, GroupBox groupBox)
        {
            btnThongTinNhanVien.Links[0].Caption = caption;
           
            groupBox1.Visible = false;
            gbDDH.Visible = false;
           /* gbPhieuNhap.Visible = false;
            gbPhieuXuat.Visible = false;*/
            groupBox.Visible = true;
        }

        private void BtnInfoNV_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            switchPanel("Thông tin nhân viên", groupBox1);

            bdsNV.Position = bdsNV.Find("MANV", Program.username);
        }
        private DXMenuItem createMenuItem(string caption)
        {
            DXMenuItem menuItem = new DXMenuItem();         
            menuItem.Caption = caption;
            return menuItem;
        }
        private void menuAddDDH_Click(object sender, EventArgs e)   //MenuItem của PopupMenu
        {
            Program.SubFormDDH = new SubFormDDH();
            Program.SubFormDDH.Show();
            Program.frmChinh.Enabled = false;
            bdsNV.Position = bdsNV.Find("MANV", Program.username);
        }
        private void menuAddChiTietDDH_Click(object sender, EventArgs e)//MenuItem của PopupMenu
        {
            Program.SubFormCTDDH = new SubFormCTDDH();
            Program.SubFormCTDDH.Show();
            Program.frmChinh.Enabled = false;
        }
        private void menuAddPN_Click(object sender, EventArgs e)
        {
          /*  if (phieuNhapBDS.Count > 0)
            {
                MessageBox.Show("Đơn đặt hàng này đã được lập Phiếu Nhập!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }*/
            Program.SubFormPhieuNhap = new SubFormPN();
            Program.SubFormPhieuNhap.Show();
            Program.frmChinh.Enabled = false;
        }
      
        private void GridView2_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            Console.WriteLine("Da vao");
            if (Program.mGroup == "CHINHANH" || Program.mGroup == "USER")
            {
                int maNVLapDDH = 0;
                if (gridView2.GetRowCellValue(bdsDDH.Position, "MANV") != null)
                {
                    maNVLapDDH = int.Parse(gridView2.GetRowCellValue(bdsDDH.Position, "MANV").ToString().Trim());
                }
                if (e.MenuType == GridMenuType.Row)
                {
                    GridViewMenu menu = e.Menu;
                    DXMenuItem menuAddDDH = createMenuItem("Thêm DDH");
                    menuAddDDH.Click += new EventHandler(menuAddDDH_Click);
                    menu.Items.Add(menuAddDDH);

                    if (maNVLapDDH == int.Parse(Program.username))
                    {
                        DXMenuItem menuAddCTDDH = createMenuItem("Thêm chi tiết DDH");
                        menuAddCTDDH.Click += new EventHandler(menuAddChiTietDDH_Click);
                        menu.Items.Add(menuAddCTDDH);
                    }
                    DXMenuItem menuAddPN = createMenuItem("Thêm Phiếu Nhập");
                    menuAddPN.Click += new EventHandler(menuAddPN_Click);
                    menu.Items.Add(menuAddPN);
                }
            }
        }
    }
}
