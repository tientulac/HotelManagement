namespace QuanLyKhachSan.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TaiKhoan")]
    public partial class TaiKhoan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TaiKhoan()
        {
            DatPhongs = new HashSet<DatPhong>();
        }

        [Key]
        [StringLength(20)]
        [Required(ErrorMessage = "Không được để trống Tên Tài Khoản")]
        public string TenTaiKhoan { get; set; }

        [StringLength(20)]
        [Required(ErrorMessage = "Không được để trống Mật Khẩu")]
        public string MatKhau { get; set; }

        [StringLength(30)]
        [Required(ErrorMessage = "Không được để trống Họ Tên")]
        public string HoTen { get; set; }

        [StringLength(20)]
        [Required(ErrorMessage = "Không được để trống Số Điện Thoại")]
        public string SoDienThoai { get; set; }

        [StringLength(35)]
        [Required(ErrorMessage = "Không được để trống Email")]
        public string Email { get; set; }

        public bool LaAdmin { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DatPhong> DatPhongs { get; set; }
    }
}
