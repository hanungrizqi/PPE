using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PLANT_PPE.Controllers
{
    public class ApprovalController : Controller
    {
        public ActionResult SectionHead()
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            return View();
        }
        public ActionResult DetailPPE(int idppe)
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            ViewBag.idppe = idppe;
            return View();
        }
        public ActionResult PlantManager()
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            return View();
        }
        public ActionResult PlantDeptHead()
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            return View();
        }
        public ActionResult ProjectManager()
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            return View();
        }
        public ActionResult ProjectManagerPenerima()
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            return View();
        }
        public ActionResult DivHead()
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            return View();
        }
        public ActionResult DivHeadOPR()
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            return View();
        }
    }
}