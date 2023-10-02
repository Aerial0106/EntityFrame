using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        NVContextDB ContextDB = new NVContextDB();
        public Form1()
        {
            InitializeComponent();
        }

        private void BindGrid(List<NhanVien> nhanVienList)
        {
            dgvNhanVien.Rows.Clear();
            foreach (var item in nhanVienList)
            {
                int index = dgvNhanVien.Rows.Add();
                dgvNhanVien.Rows[index].Cells[0].Value = item.MaNV;
                dgvNhanVien.Rows[index].Cells[1].Value = item.TenNV;
                dgvNhanVien.Rows[index].Cells[2].Value = item.Ngaysinh;
                dgvNhanVien.Rows[index].Cells[3].Value = item.Phongban.TenPB;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            List<NhanVien> nhanVienList = ContextDB.NhanViens.ToList();
            try
            {
                BindGrid(nhanVienList);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            List<Phongban> phongbans = ContextDB.Phongbans.ToList();
            foreach (var item in phongbans)
            {
                cmbPhongBan.Items.Add(item.TenPB.ToString());
            }
            cmbPhongBan.SelectedItem = "Công nghệ thông tin";
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<NhanVien> listStudent = ContextDB.NhanViens.ToList();
            DialogResult dl = MessageBox.Show("Bạn có muốn xóa sinh viên", "thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dl == DialogResult.Yes)
            {
                NhanVien dbDeltete = ContextDB.NhanViens.FirstOrDefault(nv => nv.MaNV == txtMaNV.Text);
                if (dbDeltete != null)
                {
                    ContextDB.NhanViens.Remove(dbDeltete);
                    ContextDB.SaveChanges();
                    List<NhanVien> listNewNhanvien = ContextDB.NhanViens.ToList();
                    dgvNhanVien.DataSource = null;
                    BindGrid(listNewNhanvien);
                    txtMaNV.Text = "";
                    txtTenNV.Text = "";
                    MessageBox.Show("Xóa nhân viên thành công", "Thông báo", MessageBoxButtons.OK);
                }
            }
        }

        private void dgvNhanVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMaNV.Text == "" || txtTenNV.Text == "")
                {
                    throw new Exception("Hãy điền đủ thông tin !!!");
                }
                List<NhanVien> listStudent = ContextDB.NhanViens.ToList();
                NhanVien checkID = ContextDB.NhanViens.FirstOrDefault(nv => nv.MaNV == txtMaNV.Text);
                if (checkID != null)
                {
                    throw new Exception("Mã số nhân viên đã tồn tại!!!");
                }

                string selectedPhongBan = cmbPhongBan.Text;
                Phongban selectedPhongBanObj = ContextDB.Phongbans.FirstOrDefault(NV => NV.TenPB == selectedPhongBan);
                string maPb = selectedPhongBanObj.MaPB;

                NhanVien nhanVien = new NhanVien() { MaNV = txtMaNV.Text, TenNV = txtTenNV.Text, Ngaysinh = Convert.ToDateTime(dateTimePicker1.Text), MaPB = maPb };
                ContextDB.NhanViens.Add(nhanVien);
                ContextDB.SaveChanges();

                List<NhanVien> listNewNhanVien = ContextDB.NhanViens.ToList();
                dgvNhanVien.DataSource = null;
                BindGrid(listNewNhanVien);
                txtMaNV.Text = "";
                txtTenNV.Text = "";
                throw new Exception("Thêm nhân viên thành công !!!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = dgvNhanVien.CurrentRow.Index;
            txtMaNV.Text = dgvNhanVien.Rows[i].Cells[0].Value.ToString();
            txtTenNV.Text = dgvNhanVien.Rows[i].Cells[1].Value.ToString();
            dateTimePicker1.Text = dgvNhanVien.Rows[i].Cells[2].Value.ToString();
            cmbPhongBan.Text = dgvNhanVien.Rows[i].Cells[3].Value.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string selectedPhongBan = cmbPhongBan.Text;
            Phongban selectedPhongBanObj = ContextDB.Phongbans.FirstOrDefault(NV => NV.TenPB == selectedPhongBan);
            string maPb = selectedPhongBanObj.MaPB;
            try
            {            
                NhanVien dbUpdate = ContextDB.NhanViens.FirstOrDefault(nv => nv.MaNV == txtMaNV.Text);
                if (dbUpdate != null)
                {
                    dbUpdate.TenNV = txtTenNV.Text;
                    dbUpdate.Ngaysinh = Convert.ToDateTime(dateTimePicker1.Text);
                    dbUpdate.MaPB = maPb;
                    ContextDB.SaveChanges();
                    List<NhanVien> listnewStudent = ContextDB.NhanViens.ToList();
                    dgvNhanVien.DataSource = null;
                    BindGrid(listnewStudent);
                    throw new Exception("Cập nhật thành công !!!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
