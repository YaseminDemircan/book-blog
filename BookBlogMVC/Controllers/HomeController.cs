using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookBlogMVC.Models;
using PagedList;
using PagedList.Mvc;

namespace BookBlogMVC.Controllers
{
    public class HomeController : Controller
    {
        BlogMVCdbEntities db = new BlogMVCdbEntities();

        public ActionResult Index(int? page)
        {
            var makale = db.Articles.ToList().ToPagedList(page ?? 1,5);
            return View(makale);
        }

        public PartialViewResult Kategori()
        {
            var kategoriler = db.Kategoris.ToList();
            return PartialView(kategoriler);
        }

        public PartialViewResult CokOkunanlar()
        {
            var populer = db.Articles.OrderBy(x => x.EklenmeTarihi).ToList();
            return PartialView(populer);
        }

        public ActionResult MakaleDetay(int id)
        {
            var makale = db.Articles.Where(x => x.MakaleId == id).SingleOrDefault();
            if (makale == null)
            {
                return HttpNotFound();
            }
            return View(makale);
        }

        public ActionResult KategoriMakale(int id, int? page)
        {
            ViewBag.PageTitle = db.Kategoris.Where(x => x.KategoriId == id).Select(x=>x.Adi).SingleOrDefault();
            ViewBag.Aciklama = db.Kategoris.Where(x => x.KategoriId == id).Select(x => x.Aciklama).SingleOrDefault();
            var kategori = db.Articles.Where(x => x.KategoriID == id).ToList().ToPagedList(page ?? 1, 4);
            return View(kategori);
        }

        public ActionResult YazarMakale(int id, int? page)
        {
            ViewBag.PageTitleAd = db.Yazars.Where(x => x.YazarId == id).Select(x => x.Adi).SingleOrDefault();
            ViewBag.PageTitleSoyad = db.Yazars.Where(x => x.YazarId == id).Select(x => x.Soyadi).SingleOrDefault();
            var yazar = db.Articles.Where(x => x.YazarID == id).ToList().ToPagedList(page ?? 1, 4);
            return View(yazar);
        }

        public ActionResult Ara(int? page, string Ara = null)
        {   
            var aranan = db.Articles.Where(x => (x.Giris.Contains(Ara)) || (x.Baslik.Contains(Ara)) || (x.Gelisme.Contains(Ara)) || (x.Sonuc.Contains(Ara))).ToList().ToPagedList(page ?? 1, 4);
            return View(aranan);
        }

        public JsonResult YorumYap(string yorum, int Makaleid)
        {
            if (yorum != null)
            {
                db.Yorums.Add(new Yorum { MakaleID = Makaleid, Yorum1 = yorum, EklenmeTarihi = DateTime.Now });
                db.SaveChanges();
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        
        public string Begen(int id)
        {
            Article mkl = db.Articles.FirstOrDefault(x => x.MakaleId == id);
            mkl.Begeni++;
            db.SaveChanges();
            return mkl.Begeni.ToString();
        }

        public string Goruntuleme(int Makaleid)
        {
            var mkl = db.Articles.Where(x => x.MakaleId == Makaleid).FirstOrDefault();
            mkl.GoruntulenmeSayisi++;
            db.SaveChanges();
            return mkl.GoruntulenmeSayisi.ToString();
        }
    }
}