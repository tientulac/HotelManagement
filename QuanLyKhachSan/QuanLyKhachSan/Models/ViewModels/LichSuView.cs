using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyKhachSan.Models.ViewModels
{
    public class LichSuView
    {
        public int MaDatPhong { get; set; }
        public string TenPhong { get; set; }
        public string NgayDat { get; set; }
        public string NgayDen { get; set; }
        public string NgayTra { get; set; }
        public string DichVu { get; set; }
        public int? ThanhTien { get; set; }
        public bool CoTheHuy { get; set; }

    }
}