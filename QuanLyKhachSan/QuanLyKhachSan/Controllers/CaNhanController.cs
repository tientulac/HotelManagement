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
            try
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
                    DichVu = dp.DichVu,
                    TrangThai = dp.TrangThai,
                    PhuongThucThanhToan = dp.PhuongThucThanhToan
                });
                var listLsView = new List<LichSuView>();
                if (listLichSu.Any())
                {
                    foreach (var m in listLichSu.ToList())
                    {
                        int daysCount = 1;
                        if (m.NgayDen != null && m.NgayTra != null)
                        {
                            DateTime startDate = new DateTime(m.NgayDen.Value.Year, m.NgayDen.Value.Month, m.NgayDen.Value.Day);
                            DateTime endDate = new DateTime(m.NgayTra.Value.Year, m.NgayTra.Value.Month, m.NgayTra.Value.Day);
                            TimeSpan span = endDate - startDate;
                            daysCount = span.Days;
                        }

                        listLsView.Add(new LichSuView
                        {
                            MaDatPhong = m.MaDatPhong,
                            TenPhong = m.TenPhong,
                            NgayDat = m.NgayDat.Value.ToString("dd/MM/yyyy"),
                            NgayDen = m.NgayDen.Value.ToString("dd/MM/yyyy"),
                            NgayTra = m.NgayTra.Value.ToString("dd/MM/yyyy"),
                            DichVu = m.DichVu,
                            ThanhTien = m.ThanhTien,
                            CoTheHuy = m.NgayDen > dateHomNay ? true : false,
                            TongTien = (int)(daysCount * m.ThanhTien),
                            TrangThai = m.TrangThai.GetValueOrDefault(),
                            PhuongThucThanhToan = m.PhuongThucThanhToan.GetValueOrDefault(),
                            PhuongThucThanhToanString = m.PhuongThucThanhToan == 0 ? "" : m.PhuongThucThanhToan == 1 ? "Thanh toán bằng tiền mặt" : "Thanh toán bằng chuyển khoản",
                            TrangThaiString = m.TrangThai == 0 ? "Chờ thanh toán" : m.TrangThai == 1 ? "Đã thanh toán" : "Hủy đặt"
                        });
                    }
                }
                return View(listLsView);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        public ActionResult ThanhToanNgay()
        {
            int maDat = Convert.ToInt16(RouteData.Values["id"].ToString());
            var pDat = db.DatPhongs.Where(x => x.MaDatPhong == maDat).FirstOrDefault();
            var HamDP = new HamDatPhong();
            HamDP.ThanhToan(pDat);
            TempData["ThanhToan"] = 1;
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
        public ActionResult CaNhan(TaiKhoanDangKyView tk)
        {
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

        [HttpPost]
        public ActionResult LichSu(FilterModel req)
        {
            try
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
                    DichVu = dp.DichVu,
                    TrangThai = dp.TrangThai,
                    PhuongThucThanhToan = dp.PhuongThucThanhToan
                });
                var listLsView = new List<LichSuView>();
                if (listLichSu.Any())
                {
                    foreach (var m in listLichSu.ToList())
                    {
                        int daysCount = 1;
                        if (m.NgayDen != null && m.NgayTra != null)
                        {
                            DateTime startDate = new DateTime(m.NgayDen.Value.Year, m.NgayDen.Value.Month, m.NgayDen.Value.Day);
                            DateTime endDate = new DateTime(m.NgayTra.Value.Year, m.NgayTra.Value.Month, m.NgayTra.Value.Day);
                            TimeSpan span = endDate - startDate;
                            daysCount = span.Days;
                        }

                        listLsView.Add(new LichSuView
                        {
                            MaDatPhong = m.MaDatPhong,
                            TenPhong = m.TenPhong,
                            NgayDat = m.NgayDat.Value.ToString("dd/MM/yyyy"),
                            NgayDen = m.NgayDen.Value.ToString("dd/MM/yyyy"),
                            NgayTra = m.NgayTra.Value.ToString("dd/MM/yyyy"),
                            DichVu = m.DichVu,
                            ThanhTien = m.ThanhTien,
                            CoTheHuy = m.NgayDen > dateHomNay ? true : false,
                            TongTien = (int)(daysCount * m.ThanhTien),
                            TrangThai = m.TrangThai.GetValueOrDefault(),
                            NgayDatFilter = m.NgayDat,
                            NgayDenFilter = m.NgayDen,
                            NgayTraFilter = m.NgayTra,
                            PhuongThucThanhToan = m.PhuongThucThanhToan.GetValueOrDefault(),
                            PhuongThucThanhToanString = m.PhuongThucThanhToan == 0 ? "" : m.PhuongThucThanhToan == 1 ? "Thanh toán bằng tiền mặt" : "Thanh toán bằng chuyển khoản",
                            TrangThaiString = m.TrangThai == 0 ? "Chờ thanh toán" : m.TrangThai == 1 ? "Đã thanh toán" : "Hủy đặt"
                        });
                    }
                }
                if (!String.IsNullOrEmpty(req.TenPhong))
                {
                    listLsView = listLsView.Where(x => x.TenPhong.ToLower().Contains(req.TenPhong.ToLower())).ToList();
                }
                if (req.NgayDat != null)
                {
                    listLsView = listLsView.Where(x => x.NgayDatFilter == req.NgayDat).ToList();
                }
                if (req.NgayDen != null)
                {
                    listLsView = listLsView.Where(x => x.NgayDenFilter == req.NgayDen).ToList();
                }
                if (req.NgayTra != null)
                {
                    listLsView = listLsView.Where(x => x.NgayTraFilter == req.NgayTra).ToList();
                }
                if (req.TrangThai != null)
                {
                    listLsView = listLsView.Where(x => x.TrangThai == req.TrangThai).ToList();
                }
                if (req.PhuongThucThanhToan != null)
                {
                    listLsView = listLsView.Where(x => x.PhuongThucThanhToan == req.PhuongThucThanhToan).ToList();
                }
                return Json(new { success = true, data = listLsView }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
