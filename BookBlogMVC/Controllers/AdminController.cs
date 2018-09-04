using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookBlogMVC.Models;
using System.Drawing;

namespace BookBlogMVC.Controllers
{
    public class AdminController : Controller
    {
        BlogMVCdbEntities db = new BlogMVCdbEntities();
        // GET: Admin
        public ActionResult Index()
        {
            var makaleler = db.Articles.ToList();
            return View(makaleler);
        }

        // GET: Admin/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            ViewBag.KategoriID = new SelectList(db.Kategoris, "KategoriId", "Adi");
            ViewBag.YazarID = new SelectList(db.Yazars, "YazarId", "Adi");
            ViewBag.YazarID = new SelectList(db.Yazars, "YazarId", "Soyadi");
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        public ActionResult Create(Article makale,  HttpPostedFileBase resim)
        {
            try
            {
                Image img = Image.FromStream(resim.InputStream);
                Bitmap Resim = new Bitmap(img);
                Resim.Save(Server.MapPath("/Images/" + resim.FileName));

                Resim rsm = new Resim();
                rsm.BuyukBoyut = "/Images/" + resim.FileName;
                
                db.Resims.Add(rsm);
                db.SaveChanges();

                makale.ResimID = rsm.ResimId;
                makale.GoruntulenmeSayisi = 0;
                makale.Begeni = 0;
                db.Articles.Add(makale);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            var makale = db.Articles.Where(x => x.MakaleId == id).SingleOrDefault();
            if (makale == null)
            {
                return HttpNotFound();
            }

            ViewBag.KategoriID = new SelectList(db.Kategoris, "KategoriId", "Adi", makale.KategoriID);
            ViewBag.YazarID = new SelectList(db.Yazars, "YazarId", "Adi", makale.YazarID);
            return View(makale);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Article mkl, HttpPostedFileBase resim)
        {
            try
            {
                var makale = db.Articles.Where(x => x.MakaleId == id).SingleOrDefault();
                if (resim != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(makale.Resim.BuyukBoyut)))
                    {
                        System.IO.File.Delete(Server.MapPath(makale.Resim.BuyukBoyut));
                    }
                    Image img = Image.FromStream(resim.InputStream);
                    Bitmap Resim = new Bitmap(img);
                    Resim.Save(Server.MapPath("/Images/" + resim.FileName));

                    Resim rsm = new Resim();
                    rsm.BuyukBoyut = "/Images/" + resim.FileName;

                    db.Resims.Add(rsm);
                    db.SaveChanges();

                    makale.ResimID = rsm.ResimId;
                }
                makale.Baslik = mkl.Baslik;
                makale.Gelisme = mkl.Gelisme;
                makale.Giris = mkl.Giris;
                makale.Sonuc = mkl.Sonuc;
                makale.KategoriID = mkl.KategoriID;
                makale.EklenmeTarihi = mkl.EklenmeTarihi;
                makale.GoruntulenmeSayisi = mkl.GoruntulenmeSayisi;
                makale.Begeni = mkl.Begeni;
                makale.YazarID = mkl.YazarID;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            var makale = db.Articles.Where(x => x.MakaleId == id).SingleOrDefault();
            if (makale == null)
            {
                return HttpNotFound();
            }
            return View(makale);
        }

        // POST: Admin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var makale = db.Articles.Where(x => x.MakaleId == id).SingleOrDefault();
                if (makale == null)
                {
                    return HttpNotFound();
                }
                if (System.IO.File.Exists(Server.MapPath(makale.Resim.BuyukBoyut)))
                {
                    System.IO.File.Delete(Server.MapPath(makale.Resim.BuyukBoyut));
                }
                foreach (var y in makale.Yorums.ToList())
                {
                    db.Yorums.Remove(y);
                }
                db.Articles.Remove(makale);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
