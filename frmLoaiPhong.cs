using System;
using System.Data;
using System.Windows.Forms;
using WindowsFormsApp1.TayVinSieuVipDataSetTableAdapters;
namespace WindowsFormsApp1
{
    public partial class frmLoaiPhong: Form
    {
        private TayVinSieuVipDataSet dataset;
        private LoaiPhongTableAdapter loaiPhongAdapter;
        private int selectedMaLoaiPhong = -1; // Lưu mã loại phòng đang chọn
        public frmLoaiPhong()
        {
            InitializeComponent();
        }

        private void frmLoaiPhong_Load(object sender, EventArgs e)
        {
            dataset = new TayVinSieuVipDataSet(); // Khởi tạo dataset
            loaiPhongAdapter = new LoaiPhongTableAdapter(); // Khởi tạo TableAdapter
            LoadData(); // Tải dữ liệu lên DataGridView
        }

        private void LoadData()
        {
            loaiPhongAdapter.Fill(dataset.LoaiPhong); // Lấy dữ liệu từ CSDL
            dgvLoaiPhong.DataSource = dataset.LoaiPhong; // Gán dữ liệu vào DataGridView
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTenLoaiPhong.Text))
            {
                DataRow newRow = dataset.LoaiPhong.NewRow();
                newRow["TenLoaiPhong"] = txtTenLoaiPhong.Text;
                newRow["MoTa"] = txtMoTa.Text;

                dataset.LoaiPhong.Rows.Add(newRow);
                loaiPhongAdapter.Update(dataset.LoaiPhong); // Cập nhật CSDL

                MessageBox.Show("Thêm loại phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ResetForm();
            }
            else
            {
                MessageBox.Show("Vui lòng nhập tên loại phòng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedMaLoaiPhong == -1)
            {
                MessageBox.Show("Vui lòng chọn loại phòng để cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataRow[] rows = dataset.LoaiPhong.Select($"MaLoaiPhong = {selectedMaLoaiPhong}");
            if (rows.Length > 0)
            {
                rows[0]["TenLoaiPhong"] = txtTenLoaiPhong.Text;
                rows[0]["MoTa"] = txtMoTa.Text;

                loaiPhongAdapter.Update(dataset.LoaiPhong);
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ResetForm();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedMaLoaiPhong == -1)
            {
                MessageBox.Show("Vui lòng chọn loại phòng để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                DataRow[] rows = dataset.LoaiPhong.Select($"MaLoaiPhong = {selectedMaLoaiPhong}");
                if (rows.Length > 0)
                {
                    rows[0].Delete();
                    loaiPhongAdapter.Update(dataset.LoaiPhong);
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ResetForm();
                }
            }
        }

        private void btnReLoad_Click(object sender, EventArgs e)
        {
            LoadData();
            ResetForm();
        }

        private void ResetForm()
        {
            txtTenLoaiPhong.Text = "";
            txtMoTa.Text = "";
            selectedMaLoaiPhong = -1;
        }
        private void dgvLoaiPhong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedMaLoaiPhong = Convert.ToInt32(dgvLoaiPhong.Rows[e.RowIndex].Cells["MaLoaiPhong"].Value);
                txtTenLoaiPhong.Text = dgvLoaiPhong.Rows[e.RowIndex].Cells["TenLoaiPhong"].Value.ToString();
                txtMoTa.Text = dgvLoaiPhong.Rows[e.RowIndex].Cells["MoTa"].Value.ToString();
            }
        }

       
    }
}
