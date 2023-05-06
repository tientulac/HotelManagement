using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyKhachSan.Models.ViewModels
{
    public class FilterModel
    {
        public string TenTaiKhoan { get; set; }
        public string MaPhong { get; set; }
        public DateTime? NgayDat { get; set; }
        public DateTime? NgayDen { get; set; }
        public DateTime? NgayTra { get; set; }
        public int? TrangThai { get; set; }
        public int? PhuongThucThanhToan { get; set; }
    }
}