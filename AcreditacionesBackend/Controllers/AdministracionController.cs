using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using AcreditacionesBackend.Models;

namespace AcreditacionesBackend.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdministracionController : Controller
    {
        private readonly Entities db = new Entities();

        public ActionResult ExportarEmailsPosters()
        {
            return GenericExporter(db.Accreditations.Where(x => x.Works.Any()).ToList());
        }


        private class WorkForExportDto
        {
            public string Autor { get; set; }
            public string Titulo { get; set; }
        }
        public ActionResult ExportarTitulosYAutores()
        {
            var result = new List<WorkForExportDto>();
            foreach (var work in db.Works.Where(x => x.Authors.Any()).ToList())
            {
                result.Add(new WorkForExportDto()
                {
                    Autor = work.Authors.First().Nombre,
                    Titulo = Regex.Replace(work.Title, @"<[^>]*>", String.Empty)
                });
            }

            return GenericExporter(result);
        }

        public ActionResult GenericExporter(object dataSource)
        {
            var gv = new GridView { DataSource = dataSource };
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Exportados.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("Index");
        }

        public ActionResult ExportData()
        {
            GridView gv = new GridView();
            gv.DataSource = db.Accreditations.ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Exportados.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("Index");
        }

        public ActionResult Acreditar(int id)
        {
            return View(db.Accreditations.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Acreditar([Bind(Include = "id,Apellido,Nombre,Profesion,Pais,Provincia,Localidad,Direccion,Telefono,Movil,NumeroMatricula,NumeroSocio,EleccionDePago,AcreditacionRealizada,AcreditacionComprobanteNro,Email,DNI")] Accreditation accreditation)
        {
            if (string.IsNullOrWhiteSpace(accreditation.AcreditacionComprobanteNro))
            {
                ModelState.AddModelError("Numero", "Ingrese el número de comprobante de depósito");
            }
            var model = db.Accreditations.Find(accreditation.id);

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(accreditation.AcreditacionComprobanteNro))
                    model.AcreditacionRealizada = true;

                model.AcreditacionComprobanteNro = accreditation.AcreditacionComprobanteNro;

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }


            return View(model);
        }

        // GET: Administracion
        public ActionResult Index(bool? onlyPendingAcks, string desde, string hasta, string orderby)
        {
            try
            {
                string dateString, format;
                DateTime result;
                CultureInfo provider = CultureInfo.InvariantCulture;
                format = "dd/MM/yyyy";

                var acks = db.Accreditations.OrderByDescending(x => x.FechaAcreditacion).ToList();

                if (!string.IsNullOrWhiteSpace(desde))
                {
                    var dateDesde = DateTime.ParseExact(desde, format, provider);
                    acks = acks.Where(x => x.FechaAcreditacion >= dateDesde).ToList();
                }
                if (!string.IsNullOrWhiteSpace(hasta))
                {
                    var dateHasta = DateTime.ParseExact(hasta, format, provider);
                    acks = acks.Where(x => x.FechaAcreditacion <= dateHasta).ToList();
                }

                if (onlyPendingAcks.HasValue && onlyPendingAcks.Value)
                {
                    acks = acks.Where(x => !x.AcreditacionRealizada).ToList();
                }

                if (!string.IsNullOrWhiteSpace(orderby))
                {
                    if (orderby.Equals("name"))
                        acks = acks.OrderBy(x => x.Apellido).ToList();

                    if (orderby.Equals("date"))
                        acks = acks.OrderBy(x => x.FechaAcreditacion).ToList();
                }

                return View(acks);
            }
            catch (Exception)
            {
                if (onlyPendingAcks.HasValue && onlyPendingAcks.Value)
                {
                    return View(db.Accreditations.Where(x => !x.AcreditacionRealizada).ToList());
                }
                return View(db.Accreditations.OrderByDescending(x => x.FechaAcreditacion).ToList());
            }
        }

        // GET: Administracion/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accreditation accreditation = db.Accreditations.Find(id);
            if (accreditation == null)
            {
                return HttpNotFound();
            }
            return View(accreditation);
        }

        // GET: Administracion/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Administracion/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Apellido,Nombre,Profesion,Pais,Provincia,Localidad,Direccion,Telefono,Movil,NumeroMatricula,NumeroSocio,EleccionDePago,AcreditacionRealizada,AcreditacionComprobanteNro,Email,DNI")] Accreditation accreditation)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(accreditation.AcreditacionComprobanteNro))
                    accreditation.AcreditacionRealizada = true;

                db.Accreditations.Add(accreditation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(accreditation);
        }

        // GET: Administracion/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accreditation accreditation = db.Accreditations.Find(id);
            if (accreditation == null)
            {
                return HttpNotFound();
            }
            return View(accreditation);
        }

        // POST: Administracion/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Apellido,Nombre,Profesion,Pais,Provincia,Localidad,Direccion,Telefono,Movil,NumeroMatricula,NumeroSocio,EleccionDePago,AcreditacionRealizada,AcreditacionComprobanteNro,Email,DNI")] Accreditation accreditation)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(accreditation.AcreditacionComprobanteNro))
                    accreditation.AcreditacionRealizada = true;

                db.Entry(accreditation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(accreditation);
        }

        // GET: Administracion/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accreditation accreditation = db.Accreditations.Find(id);
            if (accreditation == null)
            {
                return HttpNotFound();
            }
            return View(accreditation);
        }

        // POST: Administracion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Accreditation accreditation = db.Accreditations.Find(id);
            db.Accreditations.Remove(accreditation);
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
