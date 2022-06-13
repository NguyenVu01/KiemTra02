using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KiemTra02
{
    public partial class frmNhanVien : Form
    {
        DataTable tblNV = new DataTable();
        public frmNhanVien()
        {
            InitializeComponent();
            DAO.Connect();
        }
        private void Load_dataGridView()
        {
            string sql;
            sql = "SELECT * FROM tblNhanVien";
            tblNV = DAO.LoadDataToTable(sql);
            dataGridView.DataSource = tblNV;
            dataGridView.Columns[0].HeaderText = "Mã nhân viên";
            dataGridView.Columns[1].HeaderText = "Họ và tên";
            dataGridView.Columns[2].HeaderText = "Quê quán";
            dataGridView.AllowUserToAddRows = false;
            dataGridView.EditMode = DataGridViewEditMode.EditProgrammatically;
        }
        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaNV.Text = dataGridView.CurrentRow.Cells[0].Value.ToString();
            txtHoTen.Text = dataGridView.CurrentRow.Cells[1].Value.ToString();
            txtQueQuan.Text = dataGridView.CurrentRow.Cells[2].Value.ToString();
        }
        private void frmNhanVien_Load(object sender, EventArgs e)
        {
            txtMaNV.Enabled = false;
            btnLuu.Enabled = false;
            btnHuy.Enabled = false;
            Load_dataGridView();
        }
        private void btnThoat_Click(object sender, EventArgs e)
        {
            DAO.Close();
            this.Close();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            ResetValues();
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnThem.Enabled = false;
            btnHuy.Enabled = true;
            btnLuu.Enabled = true;
            txtMaNV.Enabled = true;
            txtMaNV.Focus();
        }
        private void ResetValues()
        {
            txtMaNV.Text = "";
            txtHoTen.Text = "";
            txtQueQuan.Text = "";
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (tblNV.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMaNV.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtHoTen.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return;
            }
            if (txtQueQuan.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập quê quán", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQueQuan.Focus();
                return;
            }
            try
            {
                string sql = "UPDATE tblNhanVien SET  HoTen=N'" + txtHoTen.Text.Trim().ToString() +
                             "',QueQuan= N'" + txtQueQuan.Text.Trim().ToString() +
                             "' WHERE MaNV= '" + txtMaNV.Text + "'";
                DAO.RunSql(sql);
                Load_dataGridView();
                ResetValues();
                btnHuy.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã có lỗi gì đó! " + ex.Message, "Báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string sql;
            if (tblNV.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMaNV.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                sql = "DELETE tblNhanVien WHERE MaNV = '" + txtMaNV.Text + "'";
                DAO.RunSqlDel(sql);
                Load_dataGridView();
                ResetValues();
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;
            if (txtMaNV.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaNV.Focus();
                return;
            }
            if (txtHoTen.Text.Trim().Length == 0)
            {
                MessageBox.Show("Tên nhân viên sẽ được tự động đặt theo tên bạn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                txtHoTen.Text = "Trịnh Trung Nguyên";
            }
            if (txtQueQuan.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập quê quán", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQueQuan.Focus();
                return;
            }
            sql = "SELECT MaNV FROM tblNhanVien WHERE MaNV ='" + txtMaNV.Text.Trim() + "'";
            if (DAO.CheckKey(sql))
            {
                MessageBox.Show("Mã nhân viên này đã có, bạn phải nhập mã khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaNV.Focus();
                txtMaNV.Text = "";
                return;
            }
            sql = "INSERT INTO tblNhanVien(MaNV, HoTen, QueQuan) VALUES('" + txtMaNV.Text + "',N'" + txtHoTen.Text + "',N'" + txtQueQuan.Text + "')";
            try
            {
                DAO.RunSql(sql);
                Load_dataGridView();
                ResetValues();
                btnXoa.Enabled = true;
                btnThem.Enabled = true;
                btnSua.Enabled = true;
                btnHuy.Enabled = false;
                btnLuu.Enabled = false;
                txtMaNV.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã có lỗi gì đó! " + ex.Message, "Báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            ResetValues();
            btnHuy.Enabled = false;
            btnThem.Enabled = true;
            btnXoa.Enabled = true;
            btnSua.Enabled = true;
            btnLuu.Enabled = false;
            txtMaNV.Enabled = false;
        }
    }
}
