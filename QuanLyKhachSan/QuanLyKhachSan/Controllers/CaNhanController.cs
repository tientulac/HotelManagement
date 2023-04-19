using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyKhachSan.Models.Entities;
using QuanLyKhachSan.Models.ViewModels;
using QuanLyKhachSan.Models.Functions;
using System.Web.Security;

namespace QuanLyKhachSan.Controllers
{
    public class CaNhanController : Controller
    {
        private ModelQLKS db = new ModelQLKS();

        // GET

        public ActionResult DangNhap()
        {
            if (Session["TaiKhoan"] != null) return RedirectToAction("TrangChu", "TrangChu");
            return View(new TaiKhoanDangNhapView());
        }

        public ActionResult DangKy()
        {
            if (Session["TaiKhoan"] != null) return RedirectToAction("TrangChu", "TrangChu");
            return View(new TaiKhoanDangKyView());
        }

        public ActionResult CaNhan()
        {
            if (Session["TaiKhoan"] == null)
            {
                Session["TrangTruoc"] = Request.RawUrl;
                return Redirect("DangNhap");
            }
            var TaiKhoan = (TaiKhoan)Session["TaiKhoan"];
            var TaiKhoanCaNhan = new TaiKhoanDangKyView
            {
                TenTaiKhoan = TaiKhoan.TenTaiKhoan,
                MatKhau = TaiKhoan.MatKhau,
                XacNhanMatKhau = TaiKhoan.MatKhau,
                HoTen = TaiKhoan.HoTen,
                SoDienThoai = TaiKhoan.SoDienThoai,
                Email = TaiKhoan.Email
            };
            return View(TaiKhoanCaNhan);
        }

        public ActionResult LichSu()
        {
            if (Session["TaiKhoan"] == null) return Redirect("DangNhap");
            TaiKhoan taiKhoan = (TaiKhoan)Session["TaiKhoan"];
            DateTime dateHomNay = DateTime.Now.AddDays(-1);
            var listLichSu = db.DatPhongs.Where(dp => dp.TenTaiKhoan == taiKhoan.TenTaiKhoan).Join(db.Phongs, dp => dp.MaPhong, p => p.MaPhong, (dp, p) => new
            {
                MaDatPhong = dp.MaDatPhong,
                TenPhong = p.TenPhong,
                NgayDat = dp.NgayDat,
                NgayDen = dp.NgayDen,
                NgayTra = dp.NgayTra,
                ThanhTien = dp.ThanhTien,
                DichVu = dp.DichVu
            }).AsEnumerable().Select(m =>
                new LichSuView()
                {
                    MaDatPhong = m.MaDatPhong,
                    TenPhong = m.TenPhong,
                    NgayDat = m.NgayDat.Value.ToString("dd/MM/yyyy"),
                    NgayDen = m.NgayDen.Value.ToString("dd/MM/yyyy"),
                    NgayTra = m.NgayTra.Value.ToString("dd/MM/yyyy"),
                    DichVu = m.DichVu,
                    ThanhTien = m.ThanhTien,
                    CoTheHuy = m.NgayDen > dateHomNay ? true : false
                }).ToList();
            return View(listLichSu);
        }

        public ActionResult DangKyThanhCong()
        {
            return View(new TaiKhoan());
        }

        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            HttpCookie ckTaiKhoan = new HttpCookie("TenTaiKhoan"), ckMatKhau = new HttpCookie("MatKhau");
            ckTaiKhoan.Expires = DateTime.Now.AddDays(-1);
            ckMatKhau.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(ckTaiKhoan);
            Response.Cookies.Add(ckMatKhau);
            return RedirectToAction("TrangChu", "TrangChu");
        }

        public ActionResult XoaDatPhong()
        {
            int MaHuy = Convert.ToInt16(RouteData.Values["id"].ToString());
            var HamDP = new HamDatPhong();
            HamDP.Delete(MaHuy);
            TempData["HuyDat"] = 1;
            return RedirectToAction("LichSu", "CaNhan");
        }

        // POST

        [HttpPost]
        public ActionResult DangNhap(TaiKhoanDangNhapView tk)
        {
            if (ModelState.IsValid)
            {
                var list = db.TaiKhoans.Where(m => m.TenTaiKhoan == tk.TenTaiKhoan && m.MatKhau == tk.MatKhau).ToList();
                if (list.Count == 0)
                {
                    ModelState.AddModelError("TenTaiKhoan", "Tài Khoản hoặc Mật Khẩu không chính xác");
                    return View(tk);
                }
                TaiKhoan taiKhoan = list.First();
                if (taiKhoan.LaAdmin)
                {
                    FormsAuthentication.SetAuthCookie(taiKhoan.HoTen, tk.TuDongDangNhap);
                    return RedirectToAction("DSTaiKhoan", "Admin");
                }
                Session["TaiKhoan"] = taiKhoan;
                if (tk.TuDongDangNhap)
                {
                    HttpCookie ckTaiKhoan = new HttpCookie("TenTaiKhoan"), ckMatKhau = new HttpCookie("MatKhau");
                    ckTaiKhoan.Value = taiKhoan.TenTaiKhoan; ckMatKhau.Value = taiKhoan.MatKhau;
                    ckTaiKhoan.Expires = DateTime.Now.AddDays(15);
                    ckMatKhau.Expires = DateTime.Now.AddDays(15);
                    Response.Cookies.Add(ckTaiKhoan);
                    Response.Cookies.Add(ckMatKhau);
                }
                if (Session["TrangTruoc"] != null)
                {
                    return Redirect(Session["TrangTruoc"].ToString());
                }
                return RedirectToAction("TrangChu", "TrangChu");
            }
            return View(tk);
        }

        [HttpPost]
        public ActionResult DangKy(TaiKhoanDangKyView tk)
        {
            if (ModelState.IsValid)
            {
                var taiKhoan = db.TaiKhoans.Find(tk.TenTaiKhoan);
                if (taiKhoan != null)
                {
                    ModelState.AddModelError("TenTaiKhoan", "Tài Khoản đã tồn tại");
                    return View(tk);
                }
                taiKhoan = new TaiKhoan()
                {
                    TenTaiKhoan = tk.TenTaiKhoan,
                    MatKhau = tk.MatKhau,
                    HoTen = tk.HoTen,
                    SoDienThoai = tk.SoDienThoai,
                    Email = tk.Email,
                    LaAdmin = false
                };
                var hTK = new HamTaiKhoan();
                hTK.Insert(taiKhoan);
                return View("DangKyThanhCong", taiKhoan);
            }
            return View(tk);
        }

        [HttpPost]
        public ActionResult CaNhan(TaiKhoanDangKyView tk){
            if (ModelState.IsValid)
            {
                var taiKhoan = new TaiKhoan()
                {
                    TenTaiKhoan = tk.TenTaiKhoan,
                    MatKhau = tk.MatKhau,
                    HoTen = tk.HoTen,
                    SoDienThoai = tk.SoDienThoai,
                    Email = tk.Email,
                    LaAdmin = false
                };
                Session["TaiKhoan"] = taiKhoan;
                var HamTK = new HamTaiKhoan();
                HamTK.Update(taiKhoan);
                ViewBag.Success = 1;
            }
            return View(tk);
        }

    }
}
