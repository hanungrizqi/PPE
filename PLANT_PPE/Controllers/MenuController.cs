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
                //ViewBag.Sub = db.TBL_R_SUB_MENUs.Where(x => x.Akses.Contains("ALL")).ToList();
                if (Session["PositionID"].ToString() == "KP1PT015")
                {
                    ViewBag.Sub = db.TBL_R_SUB_MENUs.Where(x => x.Akses.Contains("Section Head")).ToList();
                } 
                else if (Session["PositionID"].ToString() == "KP1PT02") {
                    ViewBag.Sub = db.TBL_R_SUB_MENUs.Where(x => x.Akses.Contains("Plant Manager")).ToList();
                }
                else if (Session["PositionID"].ToString() == "SP1D1" || Session["PositionID"].ToString() == "TM1Q1" || Session["PositionID"].ToString() == "RT1D01" || Session["PositionID"].ToString() == "BD1D10" || Session["PositionID"].ToString() == "AS1Q1" || Session["PositionID"].ToString() == "IN1AB1" || Session["PositionID"].ToString() == "TO1Q1" || Session["PositionID"].ToString() == "SR1Q1" || Session["PositionID"].ToString() == "IN1AC1" || Session["PositionID"].ToString() == "AG1Q1")
                {
                    ViewBag.Sub = db.TBL_R_SUB_MENUs.Where(x => x.Akses.Contains("Plant Dept Head")).ToList();
                }
                else if (Session["PositionID"].ToString() == "BD11" || Session["PositionID"].ToString() == "AS11" || Session["PositionID"].ToString() == "IN11" || Session["PositionID"].ToString() == "TO11" || Session["PositionID"].ToString() == "RT11" || Session["PositionID"].ToString() == "SP11")
                {
                    ViewBag.Sub = db.TBL_R_SUB_MENUs.Where(x => x.Akses.Contains("PM Pengirim") || x.Akses.Contains("PM Penerima")).ToList();
                }
                else if (Session["PositionID"].ToString() == "KP0161")
                {
                    ViewBag.Sub = db.TBL_R_SUB_MENUs.Where(x => x.Akses.Contains("Division Head ENG")).ToList();
                }
                else if (Session["PositionID"].ToString() == "KP0111")
                {
                    ViewBag.Sub = db.TBL_R_SUB_MENUs.Where(x => x.Akses.Contains("Division Head OPR")).ToList();
                }
                else
                {
                    ViewBag.Sub = db.TBL_R_SUB_MENUs.Where(x => x.Akses.Contains("ALL")).ToList();
                }
                return PartialView("_Sidebar", menu);
            }

        }
    }
}