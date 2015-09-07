using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACKCMS.Contenidos.Models;

namespace ACKCMS.Contenidos.Controllers
{
    public class BaseController : Controller
    {
        FitosanitariasEntities db { get; set; }
        public BaseController()
        {
            db = new FitosanitariasEntities();
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult ViewConfigLogo(int id)
        {
            var item = db.ConfiguracionSitios.Find(id);

            if (item.LogoCongreso == null)
                return null;

            byte[] buffer = item.LogoCongreso;
            return File(buffer, "image/jpg", string.Format("{0}.jpg", id));
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ViewComprobanteAsistenciaHeader(int id)
        {
            var item = db.ConfiguracionSitios.Find(id);

            if (item.ComprobanteAsistenciaHeader == null)
                return null;

            byte[] buffer = item.ComprobanteAsistenciaHeader;
            return File(buffer, "image/jpg", string.Format("{0}.jpg", id));
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ViewComprobanteAsistenciaFooter(int id)
        {
            var item = db.ConfiguracionSitios.Find(id);

            if (item.ComprobanteAsistenciaFooter == null)
                return null;

            byte[] buffer = item.ComprobanteAsistenciaFooter;
            return File(buffer, "image/jpg", string.Format("{0}.jpg", id));
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ViewComprobanteOradorHeader(int id)
        {
            var item = db.ConfiguracionSitios.Find(id);

            if (item.ComprobanteOradorHeader == null)
                return null;

            byte[] buffer = item.ComprobanteOradorHeader;
            return File(buffer, "image/jpg", string.Format("{0}.jpg", id));
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ViewComprobanteOradorFooter(int id)
        {
            var item = db.ConfiguracionSitios.Find(id);

            if (item.ComprobanteOradorFooter == null)
                return null;

            byte[] buffer = item.ComprobanteOradorFooter;
            return File(buffer, "image/jpg", string.Format("{0}.jpg", id));
        }
    }
}