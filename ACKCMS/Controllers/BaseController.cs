using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACKCMS.Models;

namespace ACKCMS.Controllers
{
    public class BaseController : Controller
    {
        public FitosanitariasEntities Db { get; set; }

        public BaseController()
        {
            this.Db = new FitosanitariasEntities();

            ViewBag.Menus = (from menu in Db.CMS_MENU.ToList()
                where !menu.ID_MENUPADRE.HasValue
                select new MenuWithChilds()
                {
                    Menu = menu,
                    ChildMenus = Db.CMS_MENU.ToList().Where(x => x.ID_MENUPADRE.Equals(menu.ID_MENU)).ToList()
                }).ToList();

            ViewBag.Articles = Db.CMS_ARTICULO.Where(x => !x.UI_FECHA_BAJA.HasValue && x.OBSERVACIONES.Contains("destacado")).ToList();
            ViewBag.NotMainArticles = Db.CMS_ARTICULO.Where(x => !x.UI_FECHA_BAJA.HasValue && !x.OBSERVACIONES.Contains("destacado")).ToList();

        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ViewImage(int id)
        {
            var item = Db.CMS_IMAGEN.Find(id);
            byte[] buffer = item.BinaryArr;
            return File(buffer, "image/jpg", string.Format("{0}.jpg", id));
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ViewComprobante(int id)
        {
            var item = Db.Accreditation.Find(id);
            byte[] buffer = item.ComprobanteBinaryArr;
            return File(buffer, "image/jpg", string.Format("{0}.jpg", id));
        }
    }

    public class MenuWithChilds
    {
        public CMS_MENU Menu { get; set; }
        public List<CMS_MENU> ChildMenus { get; set; }
    }
}