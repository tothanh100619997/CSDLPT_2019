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
    public partial class SubFormDDH : Form
    {
        public SubFormDDH()
        {
            InitializeComponent();
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

        private void BtnTao_Click(object sender, EventArgs e)
        {

        }

        private void BtnTao_Click_1(object sender, EventArgs e)
        {
            if (!checkValidate(txtMaSoDDH, "Mã DDH is không được trống!")) return;
            if (!checkValidate(txtNCC, "Nhà cung cấp không được trống!")) return;
          
            string query = "DECLARE	@result int " +
                           "EXEC @result = SP_CheckID '"+txtMaSoDDH.Text+"', "+"'MADDH' " +
                           "SELECT 'result' = @result";

            SqlDataReader dataReader = Program.ExecSqlDataReader(query);
           
            dataReader.Read();
            int result = int.Parse(dataReader.GetValue(0).ToString());
            dataReader.Close();

         
           
            //Bỏ qua TH tồn tại ở CN hiện tại khi vị trí MasoDDH đang nhập đúng băng vị trí đang đứng
            if (result == 1)
            {
                MessageBox.Show("Mã DDH đã tồn tại ở Chi Nhánh hiện tại!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (result == 2)
            {
                MessageBox.Show("Mã DDH đã tồn tại ở Chi Nhánh khác!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                DialogResult dr = MessageBox.Show("Bạn có chắc muốn ghi dữ liệu vào Database?", "Thông báo",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    try
                    {
                        Program.frmNV.bdsDDH.EndEdit();
                        Program.frmNV.datHangTableAdapter.Update(Program.frmNV.dS.DatHang);
                       
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ghi dữ liệu thất lại. Vui lòng kiểm tra lại!\n" + ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        
    }
}
