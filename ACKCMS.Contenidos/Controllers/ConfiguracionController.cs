using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ACKCMS.Contenidos.Models;

namespace ACKCMS.Contenidos.Controllers
{
    public class ConfiguracionController : Controller
    {
        private FitosanitariasEntities db = new FitosanitariasEntities();

        // GET: Configuracion
        public ActionResult Index()
        {
            var results = db.ConfiguracionSitios.OrderByDescending(x => x.id).ToList();
            ViewBag.Activo = results.Any() ? results.First().id : 0;
            return View(results);
        }

        // GET: Configuracion/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConfiguracionSitio configuracionSitio = db.ConfiguracionSitios.Find(id);
            if (configuracionSitio == null)
            {
                return HttpNotFound();
            }
            return View(configuracionSitio);
        }

        // GET: Configuracion/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Configuracion/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Create([Bind(Include = "id,TituloCongreso,TituloCortoCongreso,DiasDelCongreso,Ciudad")] ConfiguracionSitio configuracionSitio,
            HttpPostedFileBase LogoCongreso,
            HttpPostedFileBase ComprobanteAsistenciaHeader,
            HttpPostedFileBase ComprobanteAsistenciaFooter,
            HttpPostedFileBase ComprobanteOradorHeader,
            HttpPostedFileBase ComprobanteOradorFooter)
        {
            if (string.IsNullOrWhiteSpace(configuracionSitio.TituloCongreso))
                configuracionSitio.TituloCongreso = "<h1>Sin título</h1>";

            if (ModelState.IsValid)
            {
                // LogoCongreso
                if (LogoCongreso != null && LogoCongreso.ContentLength > 0)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(LogoCongreso.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(LogoCongreso.ContentLength);
                    }

                    configuracionSitio.LogoCongreso = imageData;
                }

                // ComprobanteAsistenciaHeader
                if (ComprobanteAsistenciaHeader != null && ComprobanteAsistenciaHeader.ContentLength > 0)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(ComprobanteAsistenciaHeader.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(ComprobanteAsistenciaHeader.ContentLength);
                    }

                    configuracionSitio.ComprobanteAsistenciaHeader = imageData;
                }

                // ComprobanteAsistenciaFooter
                if (ComprobanteAsistenciaFooter != null && ComprobanteAsistenciaFooter.ContentLength > 0)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(ComprobanteAsistenciaFooter.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(ComprobanteAsistenciaFooter.ContentLength);
                    }

                    configuracionSitio.ComprobanteAsistenciaFooter = imageData;
                }

                // ComprobanteOradorHeader
                if (ComprobanteOradorHeader != null && ComprobanteOradorHeader.ContentLength > 0)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(ComprobanteOradorHeader.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(ComprobanteOradorHeader.ContentLength);
                    }

                    configuracionSitio.ComprobanteOradorHeader = imageData;
                }

                // ComprobanteOradorFooter
                if (ComprobanteOradorFooter != null && ComprobanteOradorFooter.ContentLength > 0)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(ComprobanteOradorFooter.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(ComprobanteOradorFooter.ContentLength);
                    }

                    configuracionSitio.ComprobanteOradorFooter = imageData;
                }

                db.ConfiguracionSitios.Add(configuracionSitio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(configuracionSitio);
        }

        // GET: Configuracion/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConfiguracionSitio configuracionSitio = db.ConfiguracionSitios.Find(id);
            if (configuracionSitio == null)
            {
                return HttpNotFound();
            }
            return View(configuracionSitio);
        }

        // POST: Configuracion/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,TituloCongreso,LogoCongreso,ComprobanteAsistenciaHeader,ComprobanteAsistenciaFooter,ComprobanteOradorHeader,ComprobanteOradorFooter")] ConfiguracionSitio configuracionSitio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(configuracionSitio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(configuracionSitio);
        }

        // GET: Configuracion/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConfiguracionSitio configuracionSitio = db.ConfiguracionSitios.Find(id);
            if (configuracionSitio == null)
            {
                return HttpNotFound();
            }
            return View(configuracionSitio);
        }

        // POST: Configuracion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ConfiguracionSitio configuracionSitio = db.ConfiguracionSitios.Find(id);
            db.ConfiguracionSitios.Remove(configuracionSitio);
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
