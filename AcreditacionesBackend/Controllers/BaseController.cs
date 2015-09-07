using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AcreditacionesBackend.Models;

namespace AcreditacionesBackend.Controllers
{
    public class BaseController : Controller
    {
        Entities db { get; set; }
        public BaseController()
        {
            db = new Entities();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ViewConfigLogo()
        {
            var item = db.ConfiguracionSitios.OrderByDescending(x => x.id).First();

            if (item.LogoCongreso == null)
                return null;

            byte[] buffer = item.LogoCongreso;
            return File(buffer, "image/jpg", string.Format("{0}.jpg", item.id));
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ViewComprobanteAsistenciaHeader()
        {
            var item = db.ConfiguracionSitios.OrderByDescending(x => x.id).First();

            if (item.ComprobanteAsistenciaHeader == null)
                return null;

            byte[] buffer = item.ComprobanteAsistenciaHeader;
            return File(buffer, "image/jpg", string.Format("{0}.jpg", item.id));
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ViewComprobanteAsistenciaFooter()
        {
            var item = db.ConfiguracionSitios.OrderByDescending(x => x.id).First();

            if (item.ComprobanteAsistenciaFooter == null)
                return null;

            byte[] buffer = item.ComprobanteAsistenciaFooter;
            return File(buffer, "image/jpg", string.Format("{0}.jpg", item.id));
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ViewComprobanteOradorHeader()
        {
            var item = db.ConfiguracionSitios.OrderByDescending(x => x.id).First();

            if (item.ComprobanteOradorHeader == null)
                return null;

            byte[] buffer = item.ComprobanteOradorHeader;
            return File(buffer, "image/jpg", string.Format("{0}.jpg", item.id));
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ViewComprobanteOradorFooter()
        {
            var item = db.ConfiguracionSitios.OrderByDescending(x => x.id).First();

            if (item.ComprobanteOradorFooter == null)
                return null;

            byte[] buffer = item.ComprobanteOradorFooter;
            return File(buffer, "image/jpg", string.Format("{0}.jpg", item.id));
        }
    }
}