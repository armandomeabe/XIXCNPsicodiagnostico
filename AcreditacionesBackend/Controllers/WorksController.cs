using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AcreditacionesBackend.Models;
using Microsoft.AspNet.Identity;

namespace AcreditacionesBackend.Controllers
{
    public class WorksController : Controller
    {
        private Entities db = new Entities();

        public ActionResult ComprobanteOrador(int id)
        {
            var work = db.Works.Find(id);

            var configuracion = db.ConfiguracionSitios.OrderByDescending(x => x.id).First();

            ViewBag.NombreCorto = configuracion.TituloCortoCongreso;
            ViewBag.Dias = configuracion.DiasDelCongreso;

            return View(work);
        }

        public ActionResult requestReview(int id)
        {
            var work = db.Works.Find(id);
            work.EstadoID = 1; // revisión
            work.TrabajoAprobado = false;
            db.Entry(work).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", "Works");
        }

        public ActionResult resumeOk(int id, bool ok)
        {
            var work = db.Works.Find(id);
            if (ok)
                work.EstadoID = 3; // aprobado
            else
                work.EstadoID = 2; // revisión

            work.TrabajoAprobado = ok;
            db.Entry(work).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", "Works");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetAttachedFile(int id)
        {
            var item = db.WorkDocuments.Find(id);
            byte[] buffer = item.DocumentFile;
            return File(buffer, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", item.Nombre);
        }


        [Authorize]
        public async Task<ActionResult> Export(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Work work = await db.Works.FindAsync(id);
            if (work == null)
            {
                return HttpNotFound();
            }
            return View(work);
        }

        [Authorize]
        public async Task<ActionResult> Assigned()
        {
            var userId = User.Identity.GetUserId();
            var works = await db.Works.Where(x => x.SupervisorUserId.Equals(userId) && !x.Supervisado.Value).ToListAsync();
            return View(works);
        }

        public async Task<ActionResult> Review(int id)
        {
            var work = await db.Works.FindAsync(id);
            ViewBag.EstadoId = new SelectList(db.WorkStatus, "id", "Nombre", work.EstadoID);
            return View(work);
        }

        [Authorize]
        [HttpPost, ValidateInput(false)]
        public async Task<ActionResult> Review(int Id, int? Puntaje, int EstadoId, string ComentariosDelEvaluador)
        {
            if (!Puntaje.HasValue) Puntaje = 0;

            var work = await db.Works.FindAsync(Id);

            work.EstadoID = EstadoId;
            work.Puntaje = Puntaje;
            work.ComentariosDelEvaluador = ComentariosDelEvaluador;
            db.Entry(work).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return RedirectToAction("Assigned");
        }

        [Authorize]
        public async Task<ActionResult> Finalize(int id)
        {
            var work = await db.Works.FindAsync(id);

            if (work.Puntaje.HasValue)
            {
                if (work.EstadoID.Equals(3))
                    work.TrabajoAprobado = true;

                work.Supervisado = true;
                db.Entry(work).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }

            return RedirectToAction("Assigned");
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> GenerateURL(int id)
        {
            var work = await db.Works.FindAsync(id);
            ViewBag.SecretCode = Utils.Base64String.Base64Encode(Utils.Base64String.Base64Encode(Utils.Base64String.Base64Encode(Utils.Base64String.Base64Encode(id.ToString()))));
            return View(work);
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> AssignWorkToSupervisor(int id)
        {
            var work = await db.Works.FindAsync(id);
            ViewBag.SupervisorUserId = new SelectList(db.AspNetUsers, "Id", "Email", work.SupervisorUserId);
            return View(work);
        }

        [Authorize(Roles = "admin")]
        [HttpPost, ValidateInput(false)]
        public async Task<ActionResult> AssignWorkToSupervisor(int Id, string SupervisorUserId, string NotasAdicionales)
        {
            var work = await db.Works.FindAsync(Id);
            work.Supervisado = false;
            work.SupervisorUserId = SupervisorUserId;
            work.NotasAdicionales = NotasAdicionales;

            work.Supervisado = false;
            work.TrabajoAprobado = null;

            db.Entry(work).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        //[Authorize(Roles = "admin")]
        //public async Task<ActionResult> Index()
        //{
        //    return View(await db.Works.Where(x => x.EstadoID != 6).ToListAsync());
        //}

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Index(bool? notFinished, int? areaId, int? scrollTo)
        {
            ViewBag.scrollTo = scrollTo.GetValueOrDefault(0);
            ViewBag.NotFinished = notFinished.GetValueOrDefault(false);
            ViewBag.areaId = areaId.GetValueOrDefault(-1);

            if (areaId.GetValueOrDefault(-1).Equals(-1))
            {
                if (notFinished.GetValueOrDefault(false))
                {
                    return View(db.Works.Where(x => x.EstadoID == 1).OrderBy(x => x.Id).ToList());
                }
                return View(db.Works.Where(x => x.EstadoID != 1 && x.EstadoID != 6).OrderBy(x => x.Id).ToList());
            }
            else
            {
                if (notFinished.GetValueOrDefault(false))
                {
                    return View(db.Works.Where(x => x.EstadoID == 1 && x.AreaID.Equals(areaId.Value)).OrderBy(x => x.Id).ToList());
                }
                return View(db.Works.Where(x => x.EstadoID != 1 && x.EstadoID != 6 && x.AreaID.Equals(areaId.Value)).OrderBy(x => x.Id).ToList());
            }
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Work work = await db.Works.FindAsync(id);
            if (work == null)
            {
                return HttpNotFound();
            }
            return View(work);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Works/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Body,AreaID,AckID,NotasAdicionales,EstadoID,ComentariosDelEvaluador,Puntaje")] Work work)
        {
            if (ModelState.IsValid)
            {
                db.Works.Add(work);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(work);
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Work work = await db.Works.FindAsync(id);
            if (work == null)
            {
                return HttpNotFound();
            }
            ViewBag.Area = (await db.WorkAreas.FindAsync(work.AreaID)).Nombre;
            return View(work);
        }

        [Authorize(Roles = "admin"), HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Body,AreaID,AckID,NotasAdicionales,EstadoID,ComentariosDelEvaluador,Puntaje,Autores,Coordinadores")] Work work)
        {
            if (ModelState.IsValid)
            {
                db.Entry(work).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(work);
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Work work = await db.Works.FindAsync(id);
            if (work == null)
            {
                return HttpNotFound();
            }
            return View(work);
        }

        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Work work = await db.Works.FindAsync(id);
            work.EstadoID = 6; // 6 = Eliminado
            //db.Works.Remove(work);
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
