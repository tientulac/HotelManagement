using QuanLyKhachSan.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyKhachSan.Models.ViewModels
{
    public class DichVuPhongModel
    {
        public int IdDichVuPhong { get; set; }
        public string MaDichVu { get; set; }
        public string TenDichVu { get; set; }
        public int? GiaDichVu { get; set; }
        public string MaPhong { get; set; }
        public bool ConTrong { get; set; }
    }
}