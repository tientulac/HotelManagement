﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyKhachSan.Models.ViewModels
{
    public class PhongView
    {
        public string MaPhong { get; set; }
        public string TenPhong { get; set; }
        public string MaLoai { get; set; }
        public int? DienTich { get; set; }
        public int? GiaThue { get; set; }
        public string TenLoai { get; set; }
        public string DuongDanAnh { get; set; }
        public bool ConTrong { get; set; }
    }
}