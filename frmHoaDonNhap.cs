using System;
using System.Data;
using System.Windows.Forms;
using WindowsFormsApp1.TayVinSieuVipDataSetTableAdapters;

namespace WindowsFormsApp1
{
    public partial class frmHoaDonNhap : Form
    {
        private TayVinSieuVipDataSet dataset;
        private HoaDonNhapTableAdapter hdnAdapter;
        private NhanVienTableAdapter nvAdapter;
        private NhaCungCapTableAdapter nccAdapter;
        private int selectedMaHDN = -1;

        public frmHoaDonNhap()
        {
            InitializeComponent();
        }

        private void frmHoaDonNhap_Load(object sender, EventArgs e)
        {
            dataset = new TayVinSieuVipDataSet();
            hdnAdapter = new HoaDonNhapTableAdapter();
            nvAdapter = new NhanVienTableAdapter();
            nccAdapter = new NhaCungCapTableAdapter();

            LoadData();
            LoadComboBox();
        }

        private void LoadData()
        {
            hdnAdapter.Fill(dataset.HoaDonNhap);
            dgvHoaDonNhap.DataSource = dataset.HoaDonNhap;
        }

        private void LoadComboBox()
        {
            nvAdapter.Fill(dataset.NhanVien);
            cbNhanVien.DataSource = dataset.NhanVien;
            cbNhanVien.DisplayMember = "TenNV";
            cbNhanVien.ValueMember = "MaNV";

            nccAdapter.Fill(dataset.NhaCungCap);
            cbNhaCungCap.DataSource = dataset.NhaCungCap;
            cbNhaCungCap.DisplayMember = "TenNCC";
            cbNhaCungCap.ValueMember = "MaNCC";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cbNhanVien.SelectedValue != null && cbNhaCungCap.SelectedValue != null)
            {
                DataRow newRow = dataset.HoaDonNhap.NewRow();
                newRow["MaNV"] = cbNhanVien.SelectedValue;
                newRow["MaNCC"] = cbNhaCungCap.SelectedValue;
                newRow["NgayNhap"] = dtNgayNhap.Value;

                dataset.HoaDonNhap.Rows.Add(newRow);
                hdnAdapter.Update(dataset.HoaDonNhap);
                MessageBox.Show("Thêm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvHoaDonNhap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedMaHDN = Convert.ToInt32(dgvHoaDonNhap.Rows[e.RowIndex].Cells["MaHDN"].Value);
                cbNhanVien.SelectedValue = dgvHoaDonNhap.Rows[e.RowIndex].Cells["MaNV"].Value;
                cbNhaCungCap.SelectedValue = dgvHoaDonNhap.Rows[e.RowIndex].Cells["MaNCC"].Value;
                dtNgayNhap.Value = Convert.ToDateTime(dgvHoaDonNhap.Rows[e.RowIndex].Cells["NgayNhap"].Value);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedMaHDN == -1)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn để cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataRow[] rows = dataset.HoaDonNhap.Select($"MaHDN = {selectedMaHDN}");
            if (rows.Length > 0)
            {
                rows[0]["MaNV"] = cbNhanVien.SelectedValue;
                rows[0]["MaNCC"] = cbNhaCungCap.SelectedValue;
                rows[0]["NgayNhap"] = dtNgayNhap.Value;

                hdnAdapter.Update(dataset.HoaDonNhap);
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedMaHDN == -1)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                DataRow[] rows = dataset.HoaDonNhap.Select($"MaHDN = {selectedMaHDN}");
                if (rows.Length > 0)
                {
                    rows[0].Delete();
                    hdnAdapter.Update(dataset.HoaDonNhap);
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
            }
        }
    }
}
