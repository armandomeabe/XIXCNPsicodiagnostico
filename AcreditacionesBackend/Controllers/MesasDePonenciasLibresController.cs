using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AcreditacionesBackend.Models;

namespace AcreditacionesBackend.Controllers
{
    public class MesasDePonenciasLibresController : Controller
    {
        private Entities db = new Entities();

        public async Task<ActionResult> AddOrRemoveWorkToTable(int id, int idWork)
        {
            var model = await db.MesasDePonenciasLibres.FindAsync(id);
            var work = await db.Works.FindAsync(idWork);

            if (!work.MesasDePonenciasLibres.Any())
                work.MesasDePonenciasLibres.Add(model);
            else
                work.MesasDePonenciasLibres.Clear();

            await db.SaveChangesAsync();

            return RedirectToAction("PonenciasEnMesas", "MesasDePonenciasLibres", new { id = id });
        }


        public async Task<ActionResult> PonenciasEnMesas(int id)
        {
            var model = await db.MesasDePonenciasLibres.FindAsync(id);

            var ponencias = await db.Works.Where(x => x.AreaID.Equals(6)
            &&
            (!x.MesasDePonenciasLibres.Any() || x.MesasDePonenciasLibres.FirstOrDefault().id.Equals(id))
            ).ToListAsync();
            ViewBag.Ponencias = ponencias;

            return View(model);
        }

        // GET: MesasDePonenciasLibres
        public async Task<ActionResult> Index()
        {
            return View(await db.MesasDePonenciasLibres.ToListAsync());
        }

        // GET: MesasDePonenciasLibres/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MesasDePonenciasLibre mesasDePonenciasLibre = await db.MesasDePonenciasLibres.FindAsync(id);
            if (mesasDePonenciasLibre == null)
            {
                return HttpNotFound();
            }
            return View(mesasDePonenciasLibre);
        }

        // GET: MesasDePonenciasLibres/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MesasDePonenciasLibres/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public async Task<ActionResult> Create([Bind(Include = "id,Titulo,Coordinadores,DescripcionOpcional")] MesasDePonenciasLibre mesasDePonenciasLibre)
        {
            if (ModelState.IsValid)
            {
                db.MesasDePonenciasLibres.Add(mesasDePonenciasLibre);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(mesasDePonenciasLibre);
        }

        // GET: MesasDePonenciasLibres/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MesasDePonenciasLibre mesasDePonenciasLibre = await db.MesasDePonenciasLibres.FindAsync(id);
            if (mesasDePonenciasLibre == null)
            {
                return HttpNotFound();
            }
            return View(mesasDePonenciasLibre);
        }

        // POST: MesasDePonenciasLibres/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public async Task<ActionResult> Edit([Bind(Include = "id,Titulo,Coordinadores,DescripcionOpcional")] MesasDePonenciasLibre mesasDePonenciasLibre)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mesasDePonenciasLibre).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(mesasDePonenciasLibre);
        }

        // GET: MesasDePonenciasLibres/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MesasDePonenciasLibre mesasDePonenciasLibre = await db.MesasDePonenciasLibres.FindAsync(id);
            if (mesasDePonenciasLibre == null)
            {
                return HttpNotFound();
            }
            return View(mesasDePonenciasLibre);
        }

        // POST: MesasDePonenciasLibres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            MesasDePonenciasLibre mesasDePonenciasLibre = await db.MesasDePonenciasLibres.FindAsync(id);
            db.MesasDePonenciasLibres.Remove(mesasDePonenciasLibre);
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
