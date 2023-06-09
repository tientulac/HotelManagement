﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuanLyKhachSan.Models.Entities;

namespace QuanLyKhachSan.Models.Functions
{
    public class HamDatPhong
    {
        private ModelQLKS db;

        public HamDatPhong()
        {
            db = new ModelQLKS();
        }

        public IQueryable<DatPhong> DatPhongs
        {
            get { return db.DatPhongs; }
        }

        public int Insert(DatPhong model)
        {
            model.TrangThai = 0;
            model.PhuongThucThanhToan = 0;
            db.DatPhongs.Add(model);
            db.SaveChanges();
            return model.MaDatPhong;
        }

        public int ThanhToan(DatPhong model)
        {
            DatPhong dbEntry = db.DatPhongs.Find(model.MaDatPhong);
            if (dbEntry == null)
            {
                return -1;
            }
            dbEntry.MaDatPhong = model.MaDatPhong;
            dbEntry.TenTaiKhoan = model.TenTaiKhoan;
            dbEntry.NgayDat = model.NgayDat;
            dbEntry.NgayDen = model.NgayDen;
            dbEntry.NgayTra = model.NgayTra;
            dbEntry.ThanhTien = model.ThanhTien;
            dbEntry.TrangThai = 1;
            dbEntry.PhuongThucThanhToan = 1;
            db.SaveChanges();
            return model.MaDatPhong;
        }

        public int Update(DatPhong model)
        {
            DatPhong dbEntry = db.DatPhongs.Find(model.MaDatPhong);
            if (dbEntry == null)
            {
                return -1;
            }
            dbEntry.MaDatPhong = model.MaDatPhong;
            dbEntry.TenTaiKhoan = model.TenTaiKhoan;
            dbEntry.NgayDat = model.NgayDat;
            dbEntry.NgayDen = model.NgayDen;
            dbEntry.NgayTra = model.NgayTra;
            dbEntry.ThanhTien = model.ThanhTien;
            db.SaveChanges();
            return model.MaDatPhong;
        }

        public int Delete(int MaDatPhong)
        {
            DatPhong dbEntry = db.DatPhongs.Find(MaDatPhong);
            if (dbEntry == null)
            {
                return -1;
            }
            var _phong = db.Phongs.Where(x => x.MaPhong == dbEntry.MaPhong);
            if (_phong != null)
            {
                _phong.FirstOrDefault().ConTrong = true;
                db.SaveChanges();
            }
            dbEntry.TrangThai = 2;
            //db.DatPhongs.Remove(dbEntry);
            db.SaveChanges();

            return MaDatPhong;
        }
    }
}