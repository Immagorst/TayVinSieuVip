using System;
using System.Data;
using System.Windows.Forms;
using WindowsFormsApp1.TayVinSieuVipDataSetTableAdapters;

namespace WindowsFormsApp1
{
    public partial class frmMat_Hang : Form
    {
        private TayVinSieuVipDataSet dataset;
        private MatHangTableAdapter mhAdapter;
        private DonViTinhTableAdapter dvtAdapter;
        private NhaCungCapTableAdapter nccAdapter;
        private int selectedMaMH = -1; // Lưu mã mặt hàng đang chọn

        public frmMat_Hang()
        {
            InitializeComponent();
        }

        private void frmMat_Hang_Load(object sender, EventArgs e)
        {
            dataset = new TayVinSieuVipDataSet(); // Khởi tạo DataSet
            mhAdapter = new MatHangTableAdapter(); // Khởi tạo TableAdapter
            dvtAdapter = new DonViTinhTableAdapter();
            nccAdapter = new NhaCungCapTableAdapter();
            LoadData(); // Tải dữ liệu lên DataGridView
            LoadComboBox(); // Tải dữ liệu cho ComboBox
        }

        private void LoadData()
        {
            mhAdapter.Fill(dataset.MatHang); // Đổ dữ liệu từ TableAdapter vào DataTable
            dgvMatHang.DataSource = dataset.MatHang; // Gán dữ liệu vào DataGridView

            foreach (DataGridViewColumn col in dgvMatHang.Columns)
            {
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private void LoadComboBox()
        {
            try
            {
                // Khởi tạo dataset và TableAdapter
                dataset = new TayVinSieuVipDataSet();
                dvtAdapter = new DonViTinhTableAdapter();
                dvtAdapter.Fill(dataset.DonViTinh); // Lấy dữ liệu từ SQL Server

                // Kiểm tra xem có dữ liệu không
                if (dataset.DonViTinh.Rows.Count == 0)
                {
                    MessageBox.Show("Không có đơn vị tính trong cơ sở dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Gán dữ liệu vào ComboBox
                cbDonViTinh.DataSource = dataset.DonViTinh;
                cbDonViTinh.DisplayMember = "TenDVT"; // Hiển thị tên đơn vị tính
                cbDonViTinh.ValueMember = "MaDVT";   // Lấy mã đơn vị tính khi chọn
                cbDonViTinh.SelectedIndex = -1;      // Không chọn mặc định
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu đơn vị tính: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTenMH.Text) && cbDonViTinh.SelectedValue != null && cbNhaCungCap.SelectedValue != null)
            {
                DataRow newRow = dataset.MatHang.NewRow();
                newRow["TenMH"] = txtTenMH.Text;
                newRow["MaDVT"] = cbDonViTinh.SelectedValue;
                newRow["MaNCC"] = cbNhaCungCap.SelectedValue;
                newRow["DonGia"] = nudDonGia.Value;

                dataset.MatHang.Rows.Add(newRow);
                mhAdapter.Update(dataset.MatHang); // Cập nhật CSDL

                MessageBox.Show("Thêm mặt hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ResetForm();
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvMatHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedMaMH = Convert.ToInt32(dgvMatHang.Rows[e.RowIndex].Cells["MaMH"].Value);
                txtTenMH.Text = dgvMatHang.Rows[e.RowIndex].Cells["TenMH"].Value.ToString();
                cbDonViTinh.SelectedValue = dgvMatHang.Rows[e.RowIndex].Cells["MaDVT"].Value;
                cbNhaCungCap.SelectedValue = dgvMatHang.Rows[e.RowIndex].Cells["MaNCC"].Value;
                nudDonGia.Value = Convert.ToDecimal(dgvMatHang.Rows[e.RowIndex].Cells["DonGia"].Value);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedMaMH == -1)
            {
                MessageBox.Show("Vui lòng chọn mặt hàng để cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataRow[] rows = dataset.MatHang.Select($"MaMH = {selectedMaMH}");
            if (rows.Length > 0)
            {
                rows[0]["TenMH"] = txtTenMH.Text;
                rows[0]["MaDVT"] = cbDonViTinh.SelectedValue;
                rows[0]["MaNCC"] = cbNhaCungCap.SelectedValue;
                rows[0]["DonGia"] = nudDonGia.Value;

                mhAdapter.Update(dataset.MatHang);
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ResetForm();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedMaMH == -1)
            {
                MessageBox.Show("Vui lòng chọn mặt hàng để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                DataRow[] rows = dataset.MatHang.Select($"MaMH = {selectedMaMH}");
                if (rows.Length > 0)
                {
                    rows[0].Delete();
                    mhAdapter.Update(dataset.MatHang);
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ResetForm();
                }
            }
        }

        private void ResetForm()
        {
            txtTenMH.Text = "";
            cbDonViTinh.SelectedIndex = -1;
            cbNhaCungCap.SelectedIndex = -1;
            nudDonGia.Value = 0;
            selectedMaMH = -1;
        }
    }
}
