using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace KiemTra02
{
    public class DAO
    {
        //Khai báo biến chứa chuỗi kết nối
        public static string ConString = "Data Source=NGUYENCHOI\\SQLEXPRESS;" +
                                     "Initial Catalog=QuanLyNhanVien;" +
                                     "Integrated Security=True";
        public static SqlConnection con = new SqlConnection(ConString); //Khai báo đối tượng kết nối


        public static void Connect()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = ConString;
                    con.Open();
                    MessageBox.Show("Kết nối CSDL thành công!", "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void Close()
        {
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                    con.Dispose();
                    MessageBox.Show("Đã ngắt kết nối CSDL!", "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable LoadDataToTable(string sql)
        {
            //The hien so 2 cua SqlDataAdapter(<Tham so 1>, <Tham so 2>) 
            SqlDataAdapter Myadapter = new SqlDataAdapter(sql, con);    // Khai báo
            DataTable table = new DataTable();    // Khai báo DataTable nhận dữ liệu trả về
            Myadapter.Fill(table); 	//Thực hiện câu lệnh SELECT và đổ dữ liệu vào bảng table
            return table;
        }

        public static void RunSql(string sql)
        {
            SqlCommand cmd;                     // Khai báo đối tượng SqlCommand
            cmd = new SqlCommand();          // Khởi tạo đối tượng
            cmd.Connection = con;     // Gán kết nối
            cmd.CommandText = sql;            // Gán câu lệnh SQL
            try
            {
                cmd.ExecuteNonQuery();        // Thực hiện câu lệnh SQL update
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            cmd.Dispose(); //Giai phong
            cmd = null;
        }

        public static void RunSqlDel(string sql)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = DAO.con;
            cmd.CommandText = sql;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (System.Exception)
            {
                MessageBox.Show("Dữ liệu đang được dùng, không thể xóa...", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            cmd.Dispose();
            cmd = null;
        }
        public static bool CheckKey(string sql)
        {
            SqlDataAdapter Mydata = new SqlDataAdapter(sql, con);
            DataTable table = new DataTable();
            Mydata.Fill(table);
            if (table.Rows.Count > 0)
                return true;
            else
                return false;
        }
    }
}
