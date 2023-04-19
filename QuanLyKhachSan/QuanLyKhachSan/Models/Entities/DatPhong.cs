namespace QuanLyKhachSan.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DatPhong")]
    public partial class DatPhong
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaDatPhong { get; set; }

        [StringLength(20)]
        public string TenTaiKhoan { get; set; }

        [StringLength(10)]
        public string MaPhong { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayDat { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayDen { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayTra { get; set; }

        [StringLength(100)]
        public string DichVu { get; set; }

        public int? ThanhTien { get; set; }

        public virtual Phong Phong { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }
    }
}
