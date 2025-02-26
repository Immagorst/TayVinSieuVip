using System;
using System.Data;
using System.Windows.Forms;
using WindowsFormsApp1.TayVinSieuVipDataSetTableAdapters;

namespace WindowsFormsApp1
{
    public partial class frmChiTietHoaDonNhap : Form
    {
        private TayVinSieuVipDataSet dataset;
        private ChiTietHoaDonNhapTableAdapter cthdnAdapter;
        private HoaDonNhapTableAdapter hdnAdapter;
        private MatHangTableAdapter mhAdapter;
        private int selectedMaCTHDN = -1;

        public frmChiTietHoaDonNhap()
        {
            InitializeComponent();
        }

        private void frmChiTietHoaDonNhap_Load(object sender, EventArgs e)
        {
            dataset = new TayVinSieuVipDataSet();
            cthdnAdapter = new ChiTietHoaDonNhapTableAdapter();
            hdnAdapter = new HoaDonNhapTableAdapter();
            mhAdapter = new MatHangTableAdapter();

            LoadData();
            LoadComboBox();
        }

        private void LoadData()
        {
            cthdnAdapter.Fill(dataset.ChiTietHoaDonNhap);
            dgvChiTietHDN.DataSource = dataset.ChiTietHoaDonNhap;
        }

        private void LoadComboBox()
        {
            hdnAdapter.Fill(dataset.HoaDonNhap);
            cbHoaDonNhap.DataSource = dataset.HoaDonNhap;
            cbHoaDonNhap.DisplayMember = "MaHDN";
            cbHoaDonNhap.ValueMember = "MaHDN";

            mhAdapter.Fill(dataset.MatHang);
            cbMatHang.DataSource = dataset.MatHang;
            cbMatHang.DisplayMember = "TenMH";
            cbMatHang.ValueMember = "MaMH";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cbHoaDonNhap.SelectedValue != null && cbMatHang.SelectedValue != null && !string.IsNullOrWhiteSpace(txtSoLuong.Text) && !string.IsNullOrWhiteSpace(txtDonGia.Text))
            {
                DataRow newRow = dataset.ChiTietHoaDonNhap.NewRow();
                newRow["MaHDN"] = cbHoaDonNhap.SelectedValue;
                newRow["MaMH"] = cbMatHang.SelectedValue;
                newRow["SoLuong"] = int.Parse(txtSoLuong.Text);
                newRow["DonGia"] = decimal.Parse(txtDonGia.Text);

                dataset.ChiTietHoaDonNhap.Rows.Add(newRow);
                cthdnAdapter.Update(dataset.ChiTietHoaDonNhap);
                MessageBox.Show("Thêm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvChiTietHDN_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedMaCTHDN = Convert.ToInt32(dgvChiTietHDN.Rows[e.RowIndex].Cells["MaCTHDN"].Value);
                cbHoaDonNhap.SelectedValue = dgvChiTietHDN.Rows[e.RowIndex].Cells["MaHDN"].Value;
                cbMatHang.SelectedValue = dgvChiTietHDN.Rows[e.RowIndex].Cells["MaMH"].Value;
                txtSoLuong.Text = dgvChiTietHDN.Rows[e.RowIndex].Cells["SoLuong"].Value.ToString();
                txtDonGia.Text = dgvChiTietHDN.Rows[e.RowIndex].Cells["DonGia"].Value.ToString();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedMaCTHDN == -1)
            {
                MessageBox.Show("Vui lòng chọn chi tiết hóa đơn để cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataRow[] rows = dataset.ChiTietHoaDonNhap.Select($"MaCTHDN = {selectedMaCTHDN}");
            if (rows.Length > 0)
            {
                rows[0]["MaHDN"] = cbHoaDonNhap.SelectedValue;
                rows[0]["MaMH"] = cbMatHang.SelectedValue;
                rows[0]["SoLuong"] = int.Parse(txtSoLuong.Text);
                rows[0]["DonGia"] = decimal.Parse(txtDonGia.Text);

                cthdnAdapter.Update(dataset.ChiTietHoaDonNhap);
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedMaCTHDN == -1)
            {
                MessageBox.Show("Vui lòng chọn chi tiết hóa đơn để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                DataRow[] rows = dataset.ChiTietHoaDonNhap.Select($"MaCTHDN = {selectedMaCTHDN}");
                if (rows.Length > 0)
                {
                    rows[0].Delete();
                    cthdnAdapter.Update(dataset.ChiTietHoaDonNhap);
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
            }
        }
    }
}