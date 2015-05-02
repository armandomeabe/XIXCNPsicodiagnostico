using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACKCMS.Models;

namespace ACKCMS.Controllers
{
    public class BaseController : Controller
    {
        public FitosanitariasEntities Db { get; set; }

        public BaseController()
        {
            this.Db = new FitosanitariasEntities();

            ViewBag.Menus = (from menu in Db.CMS_MENU.ToList()
                where !menu.ID_MENUPADRE.HasValue
                select new MenuWithChilds()
                {
                    Menu = menu,
                    ChildMenus = Db.CMS_MENU.ToList().Where(x => x.ID_MENUPADRE.Equals(menu.ID_MENU)).ToList()
                }).ToList();

        }
    }

    public class MenuWithChilds
    {
        public CMS_MENU Menu { get; set; }
        public List<CMS_MENU> ChildMenus { get; set; }
    }
}