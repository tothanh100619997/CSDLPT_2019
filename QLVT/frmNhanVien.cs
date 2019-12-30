using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT
{
    public partial class frmNhanVien : Form
    {
        private int position;
        private string maCN = "";
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
            // TODO: This line of code loads data into the 'dS.NhanVien' table. You can move, or remove it, as needed.
            this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
            this.nhanVienTableAdapter.Fill(this.dS.NhanVien);

        }

        private void GroupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void TrangThaiXoaComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void TrangThaiXoaLabel_Click(object sender, EventArgs e)
        {

        }

        private void MANVLabel_Click(object sender, EventArgs e)
        {

        }

        private void TxtMANV_TextChanged(object sender, EventArgs e)
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

        private void CmbMACN_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void MACNLabel_Click(object sender, EventArgs e)
        {

        }

        private void GcNV_Click(object sender, EventArgs e)
        {

        }

        private void MACNLabel_Click_1(object sender, EventArgs e)
        {

        }

        private void BtnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }
    }
}
