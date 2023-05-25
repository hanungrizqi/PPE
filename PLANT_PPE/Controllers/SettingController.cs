using PLANT_PPE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PLANT_PPE.Controllers
{
    public class SettingController : Controller
    {
        DB_PLANT_PPEDataContext db = new DB_PLANT_PPEDataContext();
        public ActionResult Users()
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            ViewBag.Emp = db.VW_KARYAWAN_ALLs.ToList();
            ViewBag.Group = db.TBL_M_ROLEs.ToList();
            return View();
        }

        public ActionResult Menu()
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            ViewBag.Group = db.TBL_M_ROLEs.ToList();
            return View();
        }

        public ActionResult Agreement()
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            return View();
        }
    }
}