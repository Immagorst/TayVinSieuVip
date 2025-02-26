using System;
using System.Data;
using System.Windows.Forms;
using WindowsFormsApp1.TayVinSieuVipDataSetTableAdapters; // Import TableAdapter

namespace WindowsFormsApp1
{
    public partial class frmNhaCungCap : Form
    {
        private TayVinSieuVipDataSet dataset;
        private NhaCungCapTableAdapter nccAdapter;
        private int selectedMaNCC = -1;

        public frmNhaCungCap()
        {
            InitializeComponent();
        }

        private void frmNhaCungCap_Load(object sender, EventArgs e)
        {
            dataset = new TayVinSieuVipDataSet(); // Khởi tạo DataSet
            nccAdapter = new NhaCungCapTableAdapter(); // Khởi tạo TableAdapter
            LoadData(); // Tải dữ liệu lên DataGridView
        }

        private void LoadData()
        {
            nccAdapter.Fill(dataset.NhaCungCap); // Đổ dữ liệu từ TableAdapter vào DataTable
            dgvNhaCungCap.DataSource = dataset.NhaCungCap; // Gán dữ liệu vào DataGridView

            foreach (DataGridViewColumn col in dgvNhaCungCap.Columns)
            {
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // Căn giữa nội dung
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter; // Căn giữa tiêu đề
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTenNCC.Text))
            {
                DataRow newRow = dataset.NhaCungCap.NewRow();
                newRow["TenNCC"] = txtTenNCC.Text;
                newRow["SDT"] = txtSDT.Text;
                newRow["DiaChi"] = txtDiaChi.Text;

                dataset.NhaCungCap.Rows.Add(newRow);
                nccAdapter.Update(dataset.NhaCungCap);

                MessageBox.Show("Thêm nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ResetForm();
            }
            else
            {
                MessageBox.Show("Vui lòng nhập tên nhà cung cấp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvNhaCungCap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedMaNCC = Convert.ToInt32(dgvNhaCungCap.Rows[e.RowIndex].Cells["MaNCC"].Value);
                txtTenNCC.Text = dgvNhaCungCap.Rows[e.RowIndex].Cells["TenNCC"].Value.ToString();
                txtSDT.Text = dgvNhaCungCap.Rows[e.RowIndex].Cells["SDT"].Value.ToString();
                txtDiaChi.Text = dgvNhaCungCap.Rows[e.RowIndex].Cells["DiaChi"].Value.ToString();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedMaNCC == -1)
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp để cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataRow[] rows = dataset.NhaCungCap.Select($"MaNCC = {selectedMaNCC}");
            if (rows.Length > 0)
            {
                rows[0]["TenNCC"] = txtTenNCC.Text;
                rows[0]["SDT"] = txtSDT.Text;
                rows[0]["DiaChi"] = txtDiaChi.Text;

                nccAdapter.Update(dataset.NhaCungCap);
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ResetForm();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedMaNCC == -1)
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                DataRow[] rows = dataset.NhaCungCap.Select($"MaNCC = {selectedMaNCC}");
                if (rows.Length > 0)
                {
                    rows[0].Delete();
                    nccAdapter.Update(dataset.NhaCungCap);
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ResetForm();
                }
            }
        }

        private void ResetForm()
        {
            txtTenNCC.Text = "";
            txtSDT.Text = "";
            txtDiaChi.Text = "";
            selectedMaNCC = -1;
        }
    }
}
