using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QuanLyKhachSan.Models.Entities
{
    [Table("DichVuPhong")]
    public partial class DichVuPhong
    {
        [Key]
        public int IdDichVuPhong { get; set; }

        public string MaDichVu { get; set; }

        public string MaPhong { get; set; }
    }
}