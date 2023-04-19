using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuanLyKhachSan.Models.Entities;

namespace QuanLyKhachSan.Models.Functions
{
    public class HamDichVu
    {
        private ModelQLKS db;

        public HamDichVu()
        {
            db = new ModelQLKS();
        }

        public IQueryable<DichVu> DichVus
        {
            get { return db.DichVus; }
        }

        public string Insert(DichVu model)
        {
            db.DichVus.Add(model);
            db.SaveChanges();
            return model.MaDichVu;
        }

        public string Update(DichVu model)
        {
            DichVu dbEntry = db.DichVus.Find(model.MaDichVu);
            if (dbEntry == null)
            {
                return null;
            }
            dbEntry.MaDichVu = model.MaDichVu;
            dbEntry.TenDichVu = model.TenDichVu;
            dbEntry.GiaDichVu = model.GiaDichVu;
            db.SaveChanges();
            return model.MaDichVu;
        }

        public string Delete(string MaDichVu)
        {
            DichVu dbEntry = db.DichVus.Find(MaDichVu);
            if (dbEntry == null)
            {
                return null;
            }
            db.DichVus.Remove(dbEntry);
            db.SaveChanges();
            return MaDichVu;
        }
    }
}