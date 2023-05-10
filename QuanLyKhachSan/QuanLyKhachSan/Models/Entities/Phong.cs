namespace QuanLyKhachSan.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Phong")]
    public partial class Phong
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Phong()
        {
            DatPhongs = new HashSet<DatPhong>();
        }

        [Key]
        [StringLength(10)]
        [Required(ErrorMessage = "Không được để trống Mã Phòng")]
        public string MaPhong { get; set; }

        [StringLength(10)]
        [Required(ErrorMessage = "Không được để trống Tên Phòng")]
        public string TenPhong { get; set; }

        [StringLength(10)]
        public string MaLoai { get; set; }

        [Required(ErrorMessage = "Không được để trống Diện Tích")]
        public int? DienTich { get; set; }

        [Required(ErrorMessage = "Không được để trống Giá Thuê")]
        public int? GiaThue { get; set; }

        public bool ConTrong { get; set; }

        public DateTime? NgayTao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DatPhong> DatPhongs { get; set; }

        public virtual LoaiPhong LoaiPhong { get; set; }
    }
}
