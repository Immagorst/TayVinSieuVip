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

namespace WindowsFormsApp1
{
    public partial class frmDonViTinh : Form
    {
        private TayVinSieuVipDataSet dataset;
        private DonViTinhTableAdapter dvtAdapter;
        private string nhanvien = "admin"; // Mặc định admin, sau sẽ truyền từ đăng nhập

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
    }
}