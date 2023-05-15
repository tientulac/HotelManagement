using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyKhachSan.Models.ViewModels
{
    public class DatPhongViewModel
    {
        public int MaDatPhong { get; set; }
        public string TenTaiKhoan { get; set; }
        public string MaPhong { get; set; }
        public DateTime? NgayDat { get; set; }
        public DateTime? NgayDen { get; set; }
        public DateTime? NgayTra { get; set; }
        public string DichVu { get; set; }
        public int? ThanhTien { get; set; }
        public int? TrangThai { get; set; }
        public int? PhuongThucThanhToan { get; set; }
        public string PhuongThucThanhToanString { get; set; }
        public string TrangThaiString { get; set; }
        public string NgayDatString { get; set; }
        public string NgayDenString { get; set; }
        public string NgayTraString { get; set; }
        public string TenPhong { get; set; }
        public int? GiaThue { get; set; }
        public string HoTen { get; set; }
    }
}