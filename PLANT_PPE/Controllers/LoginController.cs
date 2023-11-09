using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PLANT_PPE.Models;

namespace PLANT_PPE.Controllers
{
    public class LoginController : Controller
    {
        DB_PLANT_PPEDataContext db = new DB_PLANT_PPEDataContext();
        // GET: Login
        public ActionResult Index()
        {
            Session["Web_Link"] = System.Configuration.ConfigurationManager.AppSettings["WebApp_Link"].ToString();
            return View();
        }

        public JsonResult MakeSession(string NRP, string Roled)
        {
            string nrp = "";

            if (NRP.Count() > 7)
            {
                nrp = NRP.Substring(NRP.Length - 7);
            }
            else
            {
                nrp = NRP;
            }
            var dataUser = db.VW_KARYAWAN_ALLs.Where(a => a.EMPLOYEE_ID == nrp).FirstOrDefault();
            var dataRole = db.TBL_M_USERs.Where(a => a.Username == nrp).FirstOrDefault();

            if (dataRole != null)
            {
                var dataRoledakun = db.TBL_M_ROLEs.Where(a => a.ID == dataRole.ID_Role).FirstOrDefault();
                var dataApprovalid = db.TBL_M_MAPPING_APPROVALSSes.Where(a => a.NEXT_POSITION_ID == dataUser.POSITION_ID).FirstOrDefault();
                if (Roled == null || Roled == "" || Roled != dataRoledakun.RoleName)
                {
                    return new JsonResult() { Data = new { Remarks = false, Message = "Role tidak sesuai" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }

                Session["Web_Link"] = System.Configuration.ConfigurationManager.AppSettings["WebApp_Link"].ToString();
                Session["Nrp"] = nrp;
                Session["ID_Role"] = dataRole.ID_Role;
                Session["Name"] = dataUser.NAME;
                Session["Site"] = dataUser.DSTRCT_CODE;
                Session["Role"] = dataRoledakun.RoleName;
                Session["PositionID"] = dataUser.POSITION_ID.Trim();
                ViewBag.POSID = dataUser.POSITION_ID;
                return new JsonResult() { Data = new { Remarks = true }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                return new JsonResult() { Data = new { Remarks = false, Message = "Maaf anda tidak memiliki akses ke NEW PPE" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }
        
        public ActionResult Logout()
        {
            Session.RemoveAll();

            return RedirectToAction("Index", "Login");
        }
    }
}