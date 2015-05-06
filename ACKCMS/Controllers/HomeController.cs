using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACKCMS.Models;

namespace ACKCMS.Controllers
{
    public class HomeController : BaseController
    {
        private const int MaxWorkWords = 500;

        public ActionResult Areas()
        {
            return View(Db.WorkArea.ToList());
        }

        public ActionResult Trabajos()
        {
            return View();
        }

        public ActionResult Trabajo(int id)
        {
            var work = Db.Work.Find(id);
            ViewBag.Areas = Db.WorkArea.ToList();
            return View(work);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Trabajo(int Id, string Title, string Autores, string areaId, string Instituciones, string Body, string finalizado)
        {
            var work = Db.Work.Find(Id);
            work.Title = Title;
            work.Autores = Autores;
            work.Instituciones = Instituciones;
            work.Body = Body;
            work.AreaID = int.Parse(areaId);

            if (!string.IsNullOrWhiteSpace(finalizado) && finalizado.Equals("si"))
                work.EstadoID = 2;

            Db.SaveChanges();

            ViewBag.Notificaciones = new List<string>() { "Se guardaron los cambios en este trabajo" };

            var words = Utils.HtmlRemoval.WordCount(Body);

            if (words > MaxWorkWords)
                ((List<string>)ViewBag.Notificaciones).Add("Ha superado el máximo de " + MaxWorkWords + " palabras permitidas para un trabajo. Se guardó su progreso, pero deberá cumplir con este requisito para poder presentarlo.");
            
            ViewBag.Areas = Db.WorkArea.ToList();

            if (work.EstadoID.Equals(2))
                return RedirectToAction("PrecargaTrabajos", "Home", new { work.Accreditation.DNI });

            return View(work);
        }

        [HttpPost]
        public ActionResult Trabajos(string DNI)
        {
            var ack = Db.Accreditation.FirstOrDefault(x => x.DNI.Trim().ToLower().Equals(DNI.Trim().ToLower()));
            if (ack == null) return RedirectToAction("Ack404", "Home");

            if (!ack.AcreditacionRealizada)
                return RedirectToAction("Ack404", "Home", new { message = "Su acreditación todavía se encuentra sujeta a verificación de pago. Por favor vuelva a intentar luego." });

            return RedirectToAction("PrecargaTrabajos", "Home", new { dni = DNI });
        }

        // Carga de resúmenes
        public ActionResult PrecargaTrabajos(string dni)
        {
            var ack = Db.Accreditation.FirstOrDefault(x => x.DNI.Trim().ToLower().Equals(dni.Trim().ToLower()));
            if (ack == null) return RedirectToAction("Ack404", "Home");

            if (!ack.AcreditacionRealizada)
                return RedirectToAction("Ack404", "Home", new { message = "Su acreditación todavía se encuentra sujeta a verificación de pago. Por favor vuelva a intentar luego." });

            var works = ack.Work.ToList();
            ack.CantTrabajosPresenta = 2;

            if (!works.Any())
            {
                for (int i = 0; i < ack.CantTrabajosPresenta; i++)
                {
                    var newWorkA = new Work
                    {
                        Title = "Sin título",
                        Body = "Sin cuerpo de trabajo",
                        AreaID = 1,
                        AckID = ack.id,
                        NotasAdicionales = "",
                        EstadoID = 1,
                        ComentariosDelEvaluador = "",
                        Puntaje = null,
                        PosicionEnElArea = null,
                        SupervisorUserId = null,
                        Supervisado = false
                    };
                    Db.Work.Add(newWorkA);
                    Db.SaveChanges();
                }
            }

            return View(ack);
        }

        public ActionResult Ack404(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                ViewBag.Message = "La información ingresada no produjo ningún resultado en la base de datos, por favor verifique y vuelva a intentar.";
            else
                ViewBag.Message = message;

            return View();
        }

        public ActionResult Index()
        {
            ViewBag.ShowBigHeader = true;

            return View();
        }

        public ActionResult Art(int id)
        {
            return View(Db.CMS_ARTICULO.Find(id));
        }

        public ActionResult Categoria(int id)
        {
            var categoria = Db.CMS_CATEGORIA.Find(id);
            return View(categoria.CMS_ARTICULO.ToList());
        }

        public ActionResult Acreditacion()
        {
            return View(new Accreditation());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Acreditacion(Accreditation model, string opcionDePago)
        {

            var errores = new List<string>();
            if (string.IsNullOrWhiteSpace(model.Apellido))
                errores.Add("Ingrese el apellido");
            
            if (string.IsNullOrWhiteSpace(model.Nombre))
                errores.Add("Ingrese el nombre");

            if (string.IsNullOrWhiteSpace(model.Pais))
                errores.Add("Ingrese el país");

            if (string.IsNullOrWhiteSpace(model.Provincia))
                errores.Add("Ingrese la provincia");

            if (string.IsNullOrWhiteSpace(model.Localidad))
                errores.Add("Ingrese la localidad");

            if (string.IsNullOrWhiteSpace(model.Direccion))
                errores.Add("Ingrese la dirección");

            if (string.IsNullOrWhiteSpace(model.Email))
                errores.Add("Ingrese el email de contacto");

            if (string.IsNullOrWhiteSpace(model.DNI))
                errores.Add("Ingrese el email de DNI");

            if (errores.Any())
            {
                ViewBag.Notificaciones = errores;
                return View(model);
            }

            if (ModelState.IsValid && !string.IsNullOrWhiteSpace(opcionDePago))
            {
                var cantidadDeTrabajos = int.Parse(opcionDePago);
                model.CantTrabajosPresenta = cantidadDeTrabajos;
                model.FechaAcreditacion = DateTime.Now;
                model.EleccionDePago = opcionDePago;
                Db.Accreditation.Add(model);
                Db.SaveChanges();
            }
            return RedirectToAction("Acreditado", "Home", new { id = model.id });
        }

        public ActionResult Acreditado(int id)
        {
            var ack = Db.Accreditation.Find(id);
            return View(ack);
        }

        public ActionResult ImpresionDeComprobante()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImpresionDeComprobante(string DNI)
        {
            var record = Db.Accreditation.FirstOrDefault(x => x.DNI.Trim().ToLower().Equals(DNI.Trim().ToLower()));

            if (record == null) ViewBag.Result = "No se encontró el DNI ingresado.";
            else
            {
                if (record.AcreditacionRealizada)
                {
                    return RedirectToAction("ImpresionDeComprobanteOK", "Home", new { dni = DNI });

                }
                else
                {
                    ViewBag.Result = string.Format("Sr/a. {0}, {1}; Su acreditación aún no fue completada, por este motivo no puede imprimir el comprobante aun.",
                        record.Apellido, record.Nombre);
                }
            }

            return View();
        }

        public ActionResult ImpresionDeComprobanteOK(string dni)
        {
            var record = Db.Accreditation.FirstOrDefault(x => x.DNI.Trim().ToLower().Equals(dni.Trim().ToLower()));
            return View(record);
        }

        public ActionResult EstadoAcreditacion()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EstadoAcreditacion(string DNI)
        {
            var record = Db.Accreditation.FirstOrDefault(x => x.DNI.Trim().ToLower().Equals(DNI.Trim().ToLower()));

            if (record == null) ViewBag.Result = "No se encontró el DNI ingresado.";
            else
            {
                if (record.AcreditacionRealizada)
                {
                    ViewBag.Result =
                        string.Format("Sr/a. {0}, {1}; Su acreditación ya fue realizada. Número de comprobante: {2}",
                            record.Apellido, record.Nombre, record.AcreditacionComprobanteNro);

                }
                else
                {
                    ViewBag.Result = string.Format("Sr/a. {0}, {1}; Su acreditación aún no fue completada.",
                        record.Apellido, record.Nombre);
                }
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}