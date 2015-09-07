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
    public class WorkAreasController : Controller
    {
        private Entities db = new Entities();

        // GET: WorkAreas
        public async Task<ActionResult> Index()
        {
            return View(await db.WorkAreas.ToListAsync());
        }

        // GET: WorkAreas/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkArea workArea = await db.WorkAreas.FindAsync(id);
            if (workArea == null)
            {
                return HttpNotFound();
            }
            return View(workArea);
        }

        // GET: WorkAreas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WorkAreas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public async Task<ActionResult> Create([Bind(Include = "Nombre,Coordinadores")] WorkArea workArea)
        {
            if (ModelState.IsValid)
            {
                db.WorkAreas.Add(workArea);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(workArea);
        }

        // GET: WorkAreas/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkArea workArea = await db.WorkAreas.FindAsync(id);
            if (workArea == null)
            {
                return HttpNotFound();
            }
            return View(workArea);
        }

        // POST: WorkAreas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nombre,Coordinadores")] WorkArea workArea)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workArea).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(workArea);
        }

        // GET: WorkAreas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkArea workArea = await db.WorkAreas.FindAsync(id);
            if (workArea == null)
            {
                return HttpNotFound();
            }
            return View(workArea);
        }

        // POST: WorkAreas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            WorkArea workArea = await db.WorkAreas.FindAsync(id);
            db.WorkAreas.Remove(workArea);
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
