using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuanLyKhachSan.Models.Entities;

namespace QuanLyKhachSan.Models.Functions
{
    public class HamTaiKhoan
    {
        private ModelQLKS db;

        public HamTaiKhoan()
        {
            db = new ModelQLKS();
        }

        public IQueryable<TaiKhoan> TaiKhoans
        {
            get { return db.TaiKhoans; }
        }

        public string Insert(TaiKhoan model)
        {
            db.TaiKhoans.Add(model);
            db.SaveChanges();
            return model.TenTaiKhoan;
        }

        public string Update(TaiKhoan model)
        {
            TaiKhoan dbEntry = db.TaiKhoans.Find(model.TenTaiKhoan);
            if (dbEntry == null)
            {
                return null;
            }
            dbEntry.TenTaiKhoan = model.TenTaiKhoan;
            dbEntry.MatKhau = model.MatKhau;
            dbEntry.HoTen = model.HoTen;
            dbEntry.SoDienThoai = model.SoDienThoai;
            dbEntry.Email = model.Email;
            dbEntry.LaAdmin = model.LaAdmin;
            db.SaveChanges();
            return model.TenTaiKhoan;
        }

        public string Delete(string TenTaiKhoan)
        {
            TaiKhoan dbEntry = db.TaiKhoans.Find(TenTaiKhoan);
            if (dbEntry == null)
            {
                return null;
            }
            db.TaiKhoans.Remove(dbEntry);
            db.SaveChanges();
            return TenTaiKhoan;
        }
    }
}