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
            ViewBag.Emp = db.TBL_R_MASTER_KARYAWAN_ALLs.ToList();
            ViewBag.Group = db.TBL_M_ROLEs.ToList();
            return View();
        }
        public ActionResult AccountProfile()
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            ViewBag.DistrictLoc = db.VW_R_DISTRICT_LOCATIONs.ToList();
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

        public ActionResult District()
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            ViewBag.dstrct = db.VW_DISTRICTs.ToList();
            ViewBag.loc = db.VW_LOCATIONs.ToList();
            //ViewBag.acctprofile = db.VW_MSF68Cs.ToList();
            return View();
        }

        public ActionResult MappingApproval()
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            return View();
        }

        public ActionResult UserApprove()
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            ViewBag.Emp = db.TBL_R_MASTER_KARYAWAN_ALLs.ToList();
            /*ViewBag.dstrct = db.VW_DISTRICTs.ToList();
            ViewBag.loc = db.VW_LOCATIONs.ToList();*/
            return View();
        }
    }
}