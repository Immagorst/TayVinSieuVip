using System;
using System.Data;
using System.Windows.Forms;
using WindowsFormsApp1.TayVinSieuVipDataSetTableAdapters;

namespace WindowsFormsApp1
{
    public partial class frmPhong: Form
    {
        private TayVinSieuVipDataSet dataset;
        private PhongTableAdapter phongAdapter;
        private LoaiPhongTableAdapter loaiPhongAdapter;
        private int selectedMaPhong = -1; // Lưu mã phòng đang chọn
        public frmPhong()
        {
            InitializeComponent();
        }

        private void frmPhong_Load(object sender, EventArgs e)
        {
            dataset = new TayVinSieuVipDataSet();
            phongAdapter = new PhongTableAdapter();
            loaiPhongAdapter = new LoaiPhongTableAdapter();

            LoadData();
            LoadComboBox();
        }

        private void LoadData()
        {
            phongAdapter.Fill(dataset.Phong); // Lấy dữ liệu từ SQL Server
            dgvPhong.DataSource = dataset.Phong; // Gán vào DataGridView

            foreach (DataGridViewColumn col in dgvPhong.Columns)
            {
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private void LoadComboBox()
        {
            loaiPhongAdapter.Fill(dataset.LoaiPhong); // Lấy dữ liệu LoaiPhong từ SQL Server

            if (dataset.LoaiPhong.Rows.Count > 0)
            {
                cbLoaiPhong.DataSource = dataset.LoaiPhong;
                cbLoaiPhong.DisplayMember = "TenLoaiPhong"; // Hiển thị tên loại phòng
                cbLoaiPhong.ValueMember = "MaLoaiPhong"; // Giá trị thực là mã loại phòng
                cbLoaiPhong.SelectedIndex = -1;
            }
        }

        private void dgvPhong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedMaPhong = Convert.ToInt32(dgvPhong.Rows[e.RowIndex].Cells["MaPhong"].Value);
                txtTenPhong.Text = dgvPhong.Rows[e.RowIndex].Cells["TenPhong"].Value.ToString();
                cbLoaiPhong.SelectedValue = dgvPhong.Rows[e.RowIndex].Cells["MaLoaiPhong"].Value;
                txtTrangThai.Text = dgvPhong.Rows[e.RowIndex].Cells["TrangThai"].Value.ToString();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTenPhong.Text) && cbLoaiPhong.SelectedValue != null)
            {
                DataRow newRow = dataset.Phong.NewRow();
                newRow["TenPhong"] = txtTenPhong.Text;
                newRow["MaLoaiPhong"] = cbLoaiPhong.SelectedValue;
                newRow["TrangThai"] = txtTrangThai.Text;

                dataset.Phong.Rows.Add(newRow);
                phongAdapter.Update(dataset.Phong); // Lưu vào CSDL

                MessageBox.Show("Thêm phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ResetForm();
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedMaPhong == -1)
            {
                MessageBox.Show("Vui lòng chọn phòng để cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataRow[] rows = dataset.Phong.Select($"MaPhong = {selectedMaPhong}");
            if (rows.Length > 0)
            {
                rows[0]["TenPhong"] = txtTenPhong.Text;
                rows[0]["MaLoaiPhong"] = cbLoaiPhong.SelectedValue;
                rows[0]["TrangThai"] = txtTrangThai.Text;

                phongAdapter.Update(dataset.Phong);
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ResetForm();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedMaPhong == -1)
            {
                MessageBox.Show("Vui lòng chọn phòng để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                DataRow[] rows = dataset.Phong.Select($"MaPhong = {selectedMaPhong}");
                if (rows.Length > 0)
                {
                    rows[0].Delete();
                    phongAdapter.Update(dataset.Phong);
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ResetForm();
                }
            }
        }

        private void ResetForm()
        {
            txtTenPhong.Text = "";
            cbLoaiPhong.SelectedIndex = -1;
            txtTrangThai.Text = "";
            selectedMaPhong = -1;
        }

        private void btnReLoad_Click(object sender, EventArgs e)
        {
            LoadData();
            LoadComboBox();
        }
    }
}
