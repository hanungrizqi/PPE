using PLANT_PPE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PLANT_PPE.Controllers
{
    public class MenuController : Controller
    {
        DB_PLANT_PPEDataContext db = new DB_PLANT_PPEDataContext();

        [ChildActionOnly]
        public ActionResult Menu()
        {
            if (Session["Nrp"] == null)
            {
                var menu = "";

                return PartialView("SideBar", menu);
            }
            else
            {
                var menu = db.VW_R_MENUs.Where(x => x.ID == Convert.ToInt16(Session["ID_Role"].ToString())).OrderBy(x => x.Order).ToList();
                ViewBag.Sub = db.TBL_R_SUB_MENUs.Where(x => x.Akses.Contains("ALL")).ToList();

                return PartialView("_Sidebar", menu);
            }

        }
    }
}