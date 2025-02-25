using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.TayVinSieuVipDataSetTableAdapters;
using System.Data.SqlClient;


namespace WindowsFormsApp1
{
    public partial class frmDonViTinh : Form
    {
        private TayVinSieuVipDataSet dataset;
        private DonViTinhTableAdapter dvtAdapter;
        private string nhanvien = "admin"; // Mặc định admin, sau sẽ truyền từ đăng nhập
        private int selectedMaDVT = -1;
        public frmDonViTinh()
        {
            InitializeComponent();
        }

        private void frmDonViTinh_Load(object sender, EventArgs e)
        {
            dataset = new TayVinSieuVipDataSet(); // Khởi tạo dataset
            dvtAdapter = new DonViTinhTableAdapter(); // Khởi tạo adapter
            LoadData(); // Tải dữ liệu từ CSDL
        }

        private void LoadData()
        {
            dvtAdapter.Fill(dataset.DonViTinh); // Đổ dữ liệu từ TableAdapter vào DataTable
            dgvDVT.DataSource = dataset.DonViTinh; // Hiển thị dữ liệu lên DataGridView
            foreach (DataGridViewColumn col in dgvDVT.Columns)
            {
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // Căn giữa nội dung
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter; // Căn giữa tiêu đề
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtDVT.Text)) // Nếu TextBox không rỗng
            {
                // Thêm dòng mới vào DataTable
                DataRow newRow = dataset.DonViTinh.NewRow();
                newRow["TenDVT"] = txtDVT.Text; // Thêm tên đơn vị tính

                dataset.DonViTinh.Rows.Add(newRow); // Thêm vào DataTable
                dvtAdapter.Update(dataset.DonViTinh); // Cập nhật CSDL

                MessageBox.Show("Đã thêm đơn vị tính thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadData(); // Tải lại dữ liệu để cập nhật DataGridView
                txtDVT.Text = ""; // Reset lại textbox sau khi thêm thành công
                txtDVT.Focus(); // Đưa con trỏ về TextBox để nhập tiếp
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đơn vị tính", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void dgvDVT_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Kiểm tra xem có chọn dòng hợp lệ không
            {
                DataGridViewRow row = dgvDVT.Rows[e.RowIndex];
                selectedMaDVT = Convert.ToInt32(row.Cells["MaDVT"].Value); // Lưu ID đơn vị tính
                txtDVT.Text = row.Cells["TenDVT"].Value.ToString(); // Hiển thị tên vào TextBox
            }
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (selectedMaDVT == -1 || string.IsNullOrEmpty(txtDVT.Text))
            {
                MessageBox.Show("Vui lòng chọn đơn vị tính cần sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Cập nhật dữ liệu trong dataset
            foreach (DataRow row in dataset.DonViTinh.Rows)
            {
                if (Convert.ToInt32(row["MaDVT"]) == selectedMaDVT) // Tìm dòng cần sửa
                {
                    row["TenDVT"] = txtDVT.Text; // Cập nhật dữ liệu
                    break;
                }
            }

            // Lưu thay đổi vào CSDL
            dvtAdapter.Update(dataset.DonViTinh);
            MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoadData(); // Refresh dữ liệu
            txtDVT.Text = ""; // Reset TextBox
            selectedMaDVT = -1; // Reset ID
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedMaDVT == -1)
            {
                MessageBox.Show("Vui lòng chọn đơn vị tính cần xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                // Xóa khỏi dataset
                DataRow rowToDelete = dataset.DonViTinh.Rows.Find(selectedMaDVT);
                if (rowToDelete != null)
                {
                    rowToDelete.Delete(); // Đánh dấu xóa
                    dvtAdapter.Update(dataset.DonViTinh); // Cập nhật vào CSDL
                }

                // Kiểm tra nếu bảng trống thì reset ID về 1
                if (dataset.DonViTinh.Rows.Count == 0)
                {
                    ResetID();
                }

                MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadData(); // Refresh dữ liệu
                txtDVT.Text = ""; // Reset TextBox
                selectedMaDVT = -1; // Reset ID
            }
        }

        // Hàm Reset ID về 1 nếu bảng trống
        private void ResetID()
        {
            string connString = "Server=Immagorst\\MSSQLSERVER01;Database=TayVinSieuVip;Integrated Security=True;";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DBCC CHECKIDENT ('DonViTinh', RESEED, 0);", conn);
                cmd.ExecuteNonQuery();
            }
        }





    }
}