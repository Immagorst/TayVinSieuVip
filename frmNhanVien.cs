using System;
using System.Data;
using System.Windows.Forms;
using WindowsFormsApp1.TayVinSieuVipDataSetTableAdapters; // Import TableAdapter

namespace WindowsFormsApp1
{
    public partial class frmNhanVien: Form
    {
        private TayVinSieuVipDataSet dataset;
        private NhanVienTableAdapter nvAdapter;
        private int selectedMaNV = -1;
        public frmNhanVien()
        {
            InitializeComponent();
        }

        private void frmNhanVien_Load(object sender, EventArgs e)
        {
            dataset = new TayVinSieuVipDataSet();
            nvAdapter = new NhanVienTableAdapter();
            LoadData();
        }

        private void LoadData()
        {
            nvAdapter.Fill(dataset.NhanVien);
            dgvNhanVien.DataSource = dataset.NhanVien;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTenNhanVien.Text))
            {
                DataRow newRow = dataset.NhanVien.NewRow();
                newRow["TenNV"] = txtTenNhanVien.Text;
                newRow["ChucVu"] = txtChucVu.Text;
                newRow["SDT"] = txtSDT.Text;
                newRow["DiaChi"] = txtDiaChi.Text;

                dataset.NhanVien.Rows.Add(newRow);
                nvAdapter.Update(dataset.NhanVien);

                MessageBox.Show("Thêm nhân viên thành công!", "Thông báo");
                LoadData();
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedMaNV == -1)
            {
                MessageBox.Show("Vui lòng chọn nhân viên để cập nhật!", "Lỗi");
                return;
            }

            DataRow[] rows = dataset.NhanVien.Select($"MaNV = {selectedMaNV}");
            if (rows.Length > 0)
            {
                rows[0]["TenNV"] = txtTenNhanVien.Text;
                rows[0]["ChucVu"] = txtChucVu.Text;
                rows[0]["SDT"] = txtSDT.Text;
                rows[0]["DiaChi"] = txtDiaChi.Text;

                nvAdapter.Update(dataset.NhanVien);
                MessageBox.Show("Cập nhật thành công!", "Thông báo");
                LoadData();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedMaNV == -1)
            {
                MessageBox.Show("Vui lòng chọn nhân viên để xóa!", "Lỗi");
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                DataRow[] rows = dataset.NhanVien.Select($"MaNV = {selectedMaNV}");
                if (rows.Length > 0)
                {
                    rows[0].Delete();
                    nvAdapter.Update(dataset.NhanVien);
                    MessageBox.Show("Xóa thành công!", "Thông báo");
                    LoadData();
                }
            }
        }

        private void btnReLoad_Click(object sender, EventArgs e)
        {
            LoadData();
            MessageBox.Show("Dữ liệu đã được làm mới!", "Thông báo");
        }

        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedMaNV = Convert.ToInt32(dgvNhanVien.Rows[e.RowIndex].Cells["MaNV"].Value);
                txtTenNhanVien.Text = dgvNhanVien.Rows[e.RowIndex].Cells["TenNV"].Value.ToString();
                txtChucVu.Text = dgvNhanVien.Rows[e.RowIndex].Cells["ChucVu"].Value.ToString();
                txtSDT.Text = dgvNhanVien.Rows[e.RowIndex].Cells["SDT"].Value.ToString();
                txtDiaChi.Text = dgvNhanVien.Rows[e.RowIndex].Cells["DiaChi"].Value.ToString();
            }
        }

       
    }
}
