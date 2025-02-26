using System;
using System.Data;
using System.Windows.Forms;
using WindowsFormsApp1.TayVinSieuVipDataSetTableAdapters;

namespace WindowsFormsApp1
{
    public partial class frmChiTietHDB : Form
    {
        private TayVinSieuVipDataSet dataset;
        private ChiTietHoaDonBanTableAdapter cthdbAdapter;
        private HoaDonBanHangTableAdapter hdbAdapter;
        private MatHangTableAdapter mhAdapter;
        private int selectedMaCTHDB = -1; // Lưu mã chi tiết hóa đơn đang chọn

        public frmChiTietHDB()
        {
            InitializeComponent();
        }

        private void frmChiTietHDB_Load(object sender, EventArgs e)
        {
            dataset = new TayVinSieuVipDataSet();
            cthdbAdapter = new ChiTietHoaDonBanTableAdapter();
            hdbAdapter = new HoaDonBanHangTableAdapter();
            mhAdapter = new MatHangTableAdapter();

            LoadData();
            LoadComboBox();
        }

        private void LoadData()
        {
            cthdbAdapter.Fill(dataset.ChiTietHoaDonBan);
            dgvChiTietHDB.DataSource = dataset.ChiTietHoaDonBan;
        }

        private void LoadComboBox()
        {
            hdbAdapter.Fill(dataset.HoaDonBanHang);
            mhAdapter.Fill(dataset.MatHang);

            cbHoaDonBan.DataSource = dataset.HoaDonBanHang;
            cbHoaDonBan.DisplayMember = "MaHDB";
            cbHoaDonBan.ValueMember = "MaHDB";

            cbMatHang.DataSource = dataset.MatHang;
            cbMatHang.DisplayMember = "TenMH";
            cbMatHang.ValueMember = "MaMH";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cbHoaDonBan.SelectedValue != null && cbMatHang.SelectedValue != null && !string.IsNullOrWhiteSpace(txtSoLuong.Text) && !string.IsNullOrWhiteSpace(txtDonGia.Text))
            {
                DataRow newRow = dataset.ChiTietHoaDonBan.NewRow();
                newRow["MaHDB"] = cbHoaDonBan.SelectedValue;
                newRow["MaMH"] = cbMatHang.SelectedValue;
                newRow["SoLuong"] = int.Parse(txtSoLuong.Text);
                newRow["DonGia"] = decimal.Parse(txtDonGia.Text);

                dataset.ChiTietHoaDonBan.Rows.Add(newRow);
                cthdbAdapter.Update(dataset.ChiTietHoaDonBan);
                MessageBox.Show("Thêm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvChiTietHDB_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedMaCTHDB = Convert.ToInt32(dgvChiTietHDB.Rows[e.RowIndex].Cells["MaCTHDB"].Value);
                cbHoaDonBan.SelectedValue = dgvChiTietHDB.Rows[e.RowIndex].Cells["MaHDB"].Value;
                cbMatHang.SelectedValue = dgvChiTietHDB.Rows[e.RowIndex].Cells["MaMH"].Value;
                txtSoLuong.Text = dgvChiTietHDB.Rows[e.RowIndex].Cells["SoLuong"].Value.ToString();
                txtDonGia.Text = dgvChiTietHDB.Rows[e.RowIndex].Cells["DonGia"].Value.ToString();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedMaCTHDB == -1)
            {
                MessageBox.Show("Vui lòng chọn chi tiết hóa đơn để cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataRow[] rows = dataset.ChiTietHoaDonBan.Select($"MaCTHDB = {selectedMaCTHDB}");
            if (rows.Length > 0)
            {
                rows[0]["MaHDB"] = cbHoaDonBan.SelectedValue;
                rows[0]["MaMH"] = cbMatHang.SelectedValue;
                rows[0]["SoLuong"] = int.Parse(txtSoLuong.Text);
                rows[0]["DonGia"] = decimal.Parse(txtDonGia.Text);

                cthdbAdapter.Update(dataset.ChiTietHoaDonBan);
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedMaCTHDB == -1)
            {
                MessageBox.Show("Vui lòng chọn chi tiết hóa đơn để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                DataRow[] rows = dataset.ChiTietHoaDonBan.Select($"MaCTHDB = {selectedMaCTHDB}");
                if (rows.Length > 0)
                {
                    rows[0].Delete();
                    cthdbAdapter.Update(dataset.ChiTietHoaDonBan);
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
            }
        }
    }
}
