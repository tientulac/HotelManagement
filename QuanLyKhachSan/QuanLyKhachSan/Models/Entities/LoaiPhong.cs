namespace QuanLyKhachSan.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LoaiPhong")]
    public partial class LoaiPhong
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LoaiPhong()
        {
            Phongs = new HashSet<Phong>();
        }

        [Key]
        [StringLength(10)]
        [Required(ErrorMessage="Không được để trống Mã Loại")]
        public string MaLoai { get; set; }

        [StringLength(30)]
        [Required(ErrorMessage = "Không được để trống Tên Loại")]
        public string TenLoai { get; set; }

        [StringLength(250)]
        [Required(ErrorMessage = "Không được để trống Ghi Chú")]
        public string GhiChu { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Chưa Chọn Ảnh")]
        public string DuongDanAnh { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Phong> Phongs { get; set; }
    }
}
