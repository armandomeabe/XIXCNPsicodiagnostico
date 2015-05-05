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
            return View();
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
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Create([Bind(Include = "ID_ARTICULO,UI_FECHA_ALTA,UI_FECHA_MODIFICACION,UI_FECHA_BAJA,UI_USUARIO,UI_INFO,ID_TIPOARTICULO,ID_ESTADOARTICULO,TITULO,ENCABEZADO,CONTENIDO,PATH_CONTENIDOEXT,PATH_PLANCONTENIDOEXT,PATH_CONTENIDOTRANS,RESUMEN,FECHA,FECHA_VENCIMIENTO,ACTIVO,VERSION,OBSERVACIONES,URLIMAGEN")] CMS_ARTICULO cMS_ARTICULO)
        {
            ModelState.Remove("ID_ARTICULO");
            ModelState.Remove("UI_FECHA_ALTA");
            ModelState.Remove("ID_TIPOARTICULO");
            ModelState.Remove("ID_ESTADOARTICULO");

            if (string.IsNullOrWhiteSpace(cMS_ARTICULO.TITULO))
                ModelState.AddModelError("error1", "Debe ingresar el título");

            if (string.IsNullOrWhiteSpace(cMS_ARTICULO.ENCABEZADO))
                ModelState.AddModelError("error2", "Debe ingresar el encabezado");

            if (string.IsNullOrWhiteSpace(cMS_ARTICULO.CONTENIDO))
                ModelState.AddModelError("error3", "Debe ingresar el contenido");

            if (string.IsNullOrWhiteSpace(cMS_ARTICULO.RESUMEN))
                ModelState.AddModelError("error4", "Debe ingresar el resumen");

            if (ModelState.IsValid)
            {
                cMS_ARTICULO.UI_FECHA_ALTA = DateTime.Now;
                db.CMS_ARTICULO.Add(cMS_ARTICULO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cMS_ARTICULO);
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
            HttpPostedFileBase pictureMain,
            HttpPostedFileBase pictureMid,
            HttpPostedFileBase pictureThumb
            )
        {
            if (ModelState.IsValid)
            {
                db.Entry(cMS_ARTICULO).State = EntityState.Modified;
                db.SaveChanges();

                if (pictureMain != null && pictureMain.ContentLength > 0)
                {
                    var articuloEdit = db.CMS_ARTICULO.Find(cMS_ARTICULO.ID_ARTICULO);

                    var fileName = Path.GetFileName(cMS_ARTICULO.ID_ARTICULO + "_MAIN_" + pictureMain.FileName);
                    if (fileName != null)
                    {
                        var path = Path.Combine(Server.MapPath("~/Uploads/ArchivosArticulos/"), fileName);
                        pictureMain.SaveAs(path);

                        articuloEdit.CMS_ARCHIVO.Add(new CMS_ARCHIVO()
                        {
                            ID_TIPO = 1, //1 = IMAGEN
                            Nombre = fileName,
                            RelativePath = path
                        });
                        db.SaveChanges();
                    }
                }

                if (pictureMid != null && pictureMid.ContentLength > 0)
                {
                    var articuloEdit = db.CMS_ARTICULO.Find(cMS_ARTICULO.ID_ARTICULO);

                    var fileName = Path.GetFileName(cMS_ARTICULO.ID_ARTICULO + "_MID_" + pictureMid.FileName);
                    if (fileName != null)
                    {
                        var path = Path.Combine(Server.MapPath("~/Uploads/ArchivosArticulos/"), fileName);
                        pictureMid.SaveAs(path);

                        articuloEdit.CMS_ARCHIVO.Add(new CMS_ARCHIVO()
                        {
                            ID_TIPO = 1, //1 = IMAGEN
                            Nombre = fileName,
                            RelativePath = path
                        });
                        db.SaveChanges();
                    }
                }

                if (pictureThumb != null && pictureThumb.ContentLength > 0)
                {
                    var articuloEdit = db.CMS_ARTICULO.Find(cMS_ARTICULO.ID_ARTICULO);

                    var fileName = Path.GetFileName(cMS_ARTICULO.ID_ARTICULO + "_THUMB_" + pictureThumb.FileName);
                    if (fileName != null)
                    {
                        var path = Path.Combine(Server.MapPath("~/Uploads/ArchivosArticulos/"), fileName);
                        pictureThumb.SaveAs(path);

                        articuloEdit.CMS_ARCHIVO.Add(new CMS_ARCHIVO()
                        {
                            ID_TIPO = 1, //1 = IMAGEN
                            Nombre = fileName,
                            RelativePath = path
                        });
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