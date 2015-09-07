using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ACKCMS.Contenidos.Models;

namespace ACKCMS.Contenidos.Controllers
{
    [Authorize]
    public class CMS_ARTICULOController : Controller
    {
        private FitosanitariasEntities db = new FitosanitariasEntities();

        public JsonResult deleteArticulo(int id)
        {
            var art = db.CMS_ARTICULO.Find(id);
            art.UI_FECHA_BAJA = DateTime.Now;
            db.Entry(art).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { data = "ok" }, JsonRequestBehavior.AllowGet);
        }

        // GET: CMS_ARTICULO
        public async Task<ActionResult> Index()
        {
            return View(await db.CMS_ARTICULO.Where(x => !x.UI_FECHA_BAJA.HasValue).ToListAsync());
        }

        // GET: CMS_ARTICULO/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CMS_ARTICULO cMS_ARTICULO = await db.CMS_ARTICULO.FindAsync(id);
            if (cMS_ARTICULO == null)
            {
                return HttpNotFound();
            }
            return View(cMS_ARTICULO);
        }

        // GET: CMS_ARTICULO/Create
        public ActionResult Create()
        {
            return View(new CMS_ARTICULO());
        }

        // POST: CMS_ARTICULO/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "ID_ARTICULO,UI_FECHA_ALTA,UI_FECHA_MODIFICACION,UI_FECHA_BAJA,UI_USUARIO,UI_INFO,ID_TIPOARTICULO,ID_ESTADOARTICULO,TITULO,ENCABEZADO,CONTENIDO,PATH_CONTENIDOEXT,PATH_PLANCONTENIDOEXT,PATH_CONTENIDOTRANS,RESUMEN,FECHA,FECHA_VENCIMIENTO,ACTIVO,VERSION,OBSERVACIONES")] CMS_ARTICULO cMS_ARTICULO)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.CMS_ARTICULO.Add(cMS_ARTICULO);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    return View(cMS_ARTICULO);
        //}
        //[HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        //public ActionResult Create([Bind(Include = "ID_ARTICULO,UI_FECHA_ALTA,UI_FECHA_MODIFICACION,UI_FECHA_BAJA,UI_USUARIO,UI_INFO,ID_TIPOARTICULO,ID_ESTADOARTICULO,TITULO,ENCABEZADO,CONTENIDO,PATH_CONTENIDOEXT,PATH_PLANCONTENIDOEXT,PATH_CONTENIDOTRANS,RESUMEN,FECHA,FECHA_VENCIMIENTO,ACTIVO,VERSION,OBSERVACIONES,URLIMAGEN")] CMS_ARTICULO cMS_ARTICULO)
        //{
        //    ModelState.Remove("ID_ARTICULO");
        //    ModelState.Remove("UI_FECHA_ALTA");
        //    ModelState.Remove("ID_TIPOARTICULO");
        //    ModelState.Remove("ID_ESTADOARTICULO");

        //    if (string.IsNullOrWhiteSpace(cMS_ARTICULO.TITULO))
        //        ModelState.AddModelError("error1", "Debe ingresar el título");

        //    if (string.IsNullOrWhiteSpace(cMS_ARTICULO.ENCABEZADO))
        //        ModelState.AddModelError("error2", "Debe ingresar el encabezado");

        //    if (string.IsNullOrWhiteSpace(cMS_ARTICULO.CONTENIDO))
        //        ModelState.AddModelError("error3", "Debe ingresar el contenido");

        //    if (string.IsNullOrWhiteSpace(cMS_ARTICULO.RESUMEN))
        //        ModelState.AddModelError("error4", "Debe ingresar el resumen");

        //    if (ModelState.IsValid)
        //    {
        //        cMS_ARTICULO.UI_FECHA_ALTA = DateTime.Now;
        //        db.CMS_ARTICULO.Add(cMS_ARTICULO);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(cMS_ARTICULO);
        //}

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Create(
            [Bind(Include = "ID_ARTICULO,UI_FECHA_ALTA,UI_FECHA_MODIFICACION,UI_FECHA_BAJA,UI_USUARIO,UI_INFO,ID_TIPOARTICULO,ID_ESTADOARTICULO,TITULO,ENCABEZADO,CONTENIDO,PATH_CONTENIDOEXT,PATH_PLANCONTENIDOEXT,PATH_CONTENIDOTRANS,RESUMEN,FECHA,FECHA_VENCIMIENTO,ACTIVO,VERSION,OBSERVACIONES,URLIMAGEN")] CMS_ARTICULO cMS_ARTICULO,
            HttpPostedFileBase[] uploadImages
            )
        {
            ModelState.Remove("ID_ARTICULO");
            ModelState.Remove("UI_FECHA_ALTA");
            ModelState.Remove("ID_TIPOARTICULO");
            ModelState.Remove("ID_ESTADOARTICULO");

            if (string.IsNullOrWhiteSpace(cMS_ARTICULO.TITULO))
                ModelState.AddModelError("error1", "Debe ingresar el título");

            if (string.IsNullOrWhiteSpace(cMS_ARTICULO.CONTENIDO))
                ModelState.AddModelError("error3", "Debe ingresar el contenido");

            if (ModelState.IsValid)
            {
                cMS_ARTICULO.UI_FECHA_ALTA = DateTime.Now;

                db.CMS_ARTICULO.Add(cMS_ARTICULO);
                db.SaveChanges();

                if (uploadImages != null && uploadImages.Any() && uploadImages[0] != null)
                {
                    foreach (var image in uploadImages)
                    {
                        if (image.ContentLength > 0)
                        {
                            byte[] imageData = null;
                            using (var binaryReader = new BinaryReader(image.InputStream))
                            {
                                imageData = binaryReader.ReadBytes(image.ContentLength);
                            }
                            var relatedImage = new CMS_IMAGEN()
                            {
                                BinaryArr = imageData,
                                Nombre = image.FileName,
                                CMS_ARTICULO_ID = cMS_ARTICULO.ID_ARTICULO
                            };
                            db.CMS_IMAGEN.Add(relatedImage);
                            db.SaveChanges();
                        }
                    }
                }

                return RedirectToAction("Index");
            }
            return View(cMS_ARTICULO);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ViewImage(int id)
        {
            var item = db.CMS_IMAGEN.Find(id);
            byte[] buffer = item.BinaryArr;
            return File(buffer, "image/jpg", string.Format("{0}.jpg", id));
        }


        // GET: CMS_ARTICULO/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CMS_ARTICULO cMS_ARTICULO = await db.CMS_ARTICULO.FindAsync(id);
            if (cMS_ARTICULO == null)
            {
                return HttpNotFound();
            }
            return View(cMS_ARTICULO);
        }

        // POST: CMS_ARTICULO/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "ID_ARTICULO,UI_FECHA_ALTA,UI_FECHA_MODIFICACION,UI_FECHA_BAJA,UI_USUARIO,UI_INFO,ID_TIPOARTICULO,ID_ESTADOARTICULO,TITULO,ENCABEZADO,CONTENIDO,PATH_CONTENIDOEXT,PATH_PLANCONTENIDOEXT,PATH_CONTENIDOTRANS,RESUMEN,FECHA,FECHA_VENCIMIENTO,ACTIVO,VERSION,OBSERVACIONES")] CMS_ARTICULO cMS_ARTICULO)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(cMS_ARTICULO).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    return View(cMS_ARTICULO);
        //}

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Edit(
            [Bind(Include = "ID_ARTICULO,UI_FECHA_ALTA,UI_FECHA_MODIFICACION,UI_FECHA_BAJA,UI_USUARIO,UI_INFO,ID_TIPOARTICULO,ID_ESTADOARTICULO,TITULO,ENCABEZADO,CONTENIDO,PATH_CONTENIDOEXT,PATH_PLANCONTENIDOEXT,PATH_CONTENIDOTRANS,RESUMEN,FECHA,FECHA_VENCIMIENTO,ACTIVO,VERSION,OBSERVACIONES,URLIMAGEN")] CMS_ARTICULO cMS_ARTICULO,
            HttpPostedFileBase[] uploadImages
            )
        {
            if (ModelState.IsValid)
            {
                db.Entry(cMS_ARTICULO).State = EntityState.Modified;
                db.SaveChanges();

                if (uploadImages != null && uploadImages.Any() && uploadImages.FirstOrDefault() != null)
                {
                    //cMS_ARTICULO = db.CMS_ARTICULO.Find(cMS_ARTICULO.ID_ARTICULO);
                    //cMS_ARTICULO.CMS_IMAGEN.Clear();
                    //db.Entry(cMS_ARTICULO).State = EntityState.Modified;
                    //db.SaveChanges();

                    var prevImgs = db.CMS_IMAGEN.Where(x => x.CMS_ARTICULO_ID.Equals(cMS_ARTICULO.ID_ARTICULO)).ToList();
                    db.CMS_IMAGEN.RemoveRange(prevImgs);
                    db.SaveChanges();


                    foreach (var image in uploadImages)
                    {
                        if (image.ContentLength > 0)
                        {
                            byte[] imageData = null;
                            using (var binaryReader = new BinaryReader(image.InputStream))
                            {
                                imageData = binaryReader.ReadBytes(image.ContentLength);
                            }
                            var relatedImage = new CMS_IMAGEN()
                            {
                                BinaryArr = imageData,
                                Nombre = image.FileName,
                                CMS_ARTICULO_ID = cMS_ARTICULO.ID_ARTICULO
                            };
                            db.CMS_IMAGEN.Add(relatedImage);
                        }
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("Index");
            }
            return View(cMS_ARTICULO);
        }

        // GET: CMS_ARTICULO/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CMS_ARTICULO cMS_ARTICULO = await db.CMS_ARTICULO.FindAsync(id);
            if (cMS_ARTICULO == null)
            {
                return HttpNotFound();
            }
            return View(cMS_ARTICULO);
        }

        // POST: CMS_ARTICULO/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CMS_ARTICULO cMS_ARTICULO = await db.CMS_ARTICULO.FindAsync(id);
            db.CMS_ARTICULO.Remove(cMS_ARTICULO);
            await db.SaveChangesAsync();
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