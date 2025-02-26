using System;
using System.Data;
using System.Windows.Forms;
using WindowsFormsApp1.TayVinSieuVipDataSetTableAdapters;

namespace WindowsFormsApp1
{
    public partial class frmHoaDonBan : Form
    {
        private TayVinSieuVipDataSet dataset;
        private HoaDonBanHangTableAdapter hdbAdapter;
        private NhanVienTableAdapter nvAdapter;
        private PhongTableAdapter phongAdapter;
        private int selectedMaHDB = -1;

        public frmHoaDonBan()
        {
            InitializeComponent();
        }

        private void frmHoaDonBan_Load(object sender, EventArgs e)
        {
            dataset = new TayVinSieuVipDataSet();
            hdbAdapter = new HoaDonBanHangTableAdapter();
            nvAdapter = new NhanVienTableAdapter();
            phongAdapter = new PhongTableAdapter();

            LoadData();
            LoadComboBox();
        }

        private void LoadData()
        {
            hdbAdapter.Fill(dataset.HoaDonBanHang);
            dgvHoaDonBan.DataSource = dataset.HoaDonBanHang;
        }

        private void LoadComboBox()
        {
            nvAdapter.Fill(dataset.NhanVien);
            phongAdapter.Fill(dataset.Phong);

            cbNhanVien.DataSource = dataset.NhanVien;
            cbNhanVien.DisplayMember = "TenNV";
            cbNhanVien.ValueMember = "MaNV";

            cbPhong.DataSource = dataset.Phong;
            cbPhong.DisplayMember = "TenPhong";
            cbPhong.ValueMember = "MaPhong";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cbNhanVien.SelectedValue != null && cbPhong.SelectedValue != null)
            {
                DataRow newRow = dataset.HoaDonBanHang.NewRow();
                newRow["MaNV"] = cbNhanVien.SelectedValue;
                newRow["NgayBan"] = dtpNgayBan.Value;
                newRow["MaPhong"] = cbPhong.SelectedValue;

                dataset.HoaDonBanHang.Rows.Add(newRow);
                hdbAdapter.Update(dataset.HoaDonBanHang);
                MessageBox.Show("Thêm hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
        }

        private void dgvHoaDonBan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedMaHDB = Convert.ToInt32(dgvHoaDonBan.Rows[e.RowIndex].Cells["MaHDB"].Value);
                cbNhanVien.SelectedValue = dgvHoaDonBan.Rows[e.RowIndex].Cells["MaNV"].Value;
                cbPhong.SelectedValue = dgvHoaDonBan.Rows[e.RowIndex].Cells["MaPhong"].Value;
                dtpNgayBan.Value = Convert.ToDateTime(dgvHoaDonBan.Rows[e.RowIndex].Cells["NgayBan"].Value);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedMaHDB == -1) return;

            DataRow[] rows = dataset.HoaDonBanHang.Select($"MaHDB = {selectedMaHDB}");
            if (rows.Length > 0)
            {
                rows[0]["MaNV"] = cbNhanVien.SelectedValue;
                rows[0]["NgayBan"] = dtpNgayBan.Value;
                rows[0]["MaPhong"] = cbPhong.SelectedValue;

                hdbAdapter.Update(dataset.HoaDonBanHang);
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedMaHDB == -1) return;

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                DataRow[] rows = dataset.HoaDonBanHang.Select($"MaHDB = {selectedMaHDB}");
                if (rows.Length > 0)
                {
                    rows[0].Delete();
                    hdbAdapter.Update(dataset.HoaDonBanHang);
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
            }
        }
    }
}
