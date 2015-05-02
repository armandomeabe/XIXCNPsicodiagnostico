using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ACKCMS.Contenidos.Models;

namespace ACKCMS.Contenidos.Controllers
{
    [Authorize]
    public class CMS_MENUController : Controller
    {
        private FitosanitariasEntities db = new FitosanitariasEntities();

        // GET: CMS_MENU
        public ActionResult Index()
        {
            var cMS_MENU = db.CMS_MENU.Include(c => c.CMS_ARTICULO);
            return View(cMS_MENU.ToList());
        }

        // GET: CMS_MENU/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CMS_MENU cMS_MENU = db.CMS_MENU.Find(id);
            if (cMS_MENU == null)
            {
                return HttpNotFound();
            }
            return View(cMS_MENU);
        }

        // GET: CMS_MENU/Create
        public ActionResult Create()
        {
            ViewBag.ID_ARTICULO = new SelectList(db.CMS_ARTICULO, "ID_ARTICULO", "TITULO");
            return View();
        }

        // POST: CMS_MENU/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_MENU,UI_FECHA_ALTA,UI_FECHA_MODIFICACION,UI_FECHA_BAJA,UI_USUARIO,UI_INFO,ID_MENUPADRE,NOMBRE,OBSERVACIONES,ID_ARTICULO")] CMS_MENU cMS_MENU)
        {
            ModelState.Remove("UI_FECHA_ALTA");
            ModelState.Remove("UI_FECHA_BAJA");
            ModelState.Remove("UI_FECHA_MODIFICACION");
            ModelState.Remove("UI_INFO");
            ModelState.Remove("UI_USUARIO");

            if (ModelState.IsValid)
            {
                cMS_MENU.UI_FECHA_ALTA = DateTime.Now;
                
                db.CMS_MENU.Add(cMS_MENU);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_ARTICULO = new SelectList(db.CMS_ARTICULO, "ID_ARTICULO", "UI_USUARIO", cMS_MENU.ID_ARTICULO);
            return View(cMS_MENU);
        }

        // GET: CMS_MENU/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CMS_MENU cMS_MENU = db.CMS_MENU.Find(id);
            if (cMS_MENU == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_ARTICULO = new SelectList(db.CMS_ARTICULO, "ID_ARTICULO", "TITULO", cMS_MENU.ID_ARTICULO);
            return View(cMS_MENU);
        }

        // POST: CMS_MENU/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_MENU,UI_FECHA_ALTA,UI_FECHA_MODIFICACION,UI_FECHA_BAJA,UI_USUARIO,UI_INFO,ID_MENUPADRE,NOMBRE,OBSERVACIONES,ID_ARTICULO")] CMS_MENU cMS_MENU)
        {
            ModelState.Remove("UI_FECHA_ALTA");
            ModelState.Remove("UI_FECHA_BAJA");
            ModelState.Remove("UI_FECHA_MODIFICACION");
            ModelState.Remove("UI_INFO");
            ModelState.Remove("UI_USUARIO");

            if (ModelState.IsValid)
            {
                db.Entry(cMS_MENU).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_ARTICULO = new SelectList(db.CMS_ARTICULO, "ID_ARTICULO", "UI_USUARIO", cMS_MENU.ID_ARTICULO);
            return View(cMS_MENU);
        }

        // GET: CMS_MENU/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CMS_MENU cMS_MENU = db.CMS_MENU.Find(id);
            if (cMS_MENU == null)
            {
                return HttpNotFound();
            }
            return View(cMS_MENU);
        }

        // POST: CMS_MENU/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CMS_MENU cMS_MENU = db.CMS_MENU.Find(id);
            db.CMS_MENU.Remove(cMS_MENU);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
