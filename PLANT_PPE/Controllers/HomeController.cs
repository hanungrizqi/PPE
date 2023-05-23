using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PLANT_PPE.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            return View();
        }
    }
}