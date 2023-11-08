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

                var dataMap = db.TBL_M_USER_APPROVALs.Where(a => a.Position_id == Session["PositionID"].ToString()).ToList();
                var poslist = dataMap.Select(b => new { b.sub_menu }).Distinct().ToList();

                if (poslist.Count == 1)
                {
                    ViewBag.Sub = db.TBL_R_SUB_MENUs.Where(x => x.Akses.Contains(poslist[0].sub_menu)).ToList();
                }
                else if (poslist.Count == 2) {
                    ViewBag.Sub = db.TBL_R_SUB_MENUs.Where(x => x.Akses.Contains(poslist[0].sub_menu) || x.Akses.Contains(poslist[1].sub_menu)).ToList();
                }
                else if (poslist.Count == 3)
                {
                    ViewBag.Sub = db.TBL_R_SUB_MENUs.Where(x => x.Akses.Contains(poslist[0].sub_menu) || x.Akses.Contains(poslist[1].sub_menu) || x.Akses.Contains(poslist[2].sub_menu)).ToList();
                }
                else if (poslist.Count == 4)
                {
                    ViewBag.Sub = db.TBL_R_SUB_MENUs.Where(x => x.Akses.Contains(poslist[0].sub_menu) || x.Akses.Contains(poslist[1].sub_menu) || x.Akses.Contains(poslist[2].sub_menu) || x.Akses.Contains(poslist[3].sub_menu)).ToList();
                }
                else if (poslist.Count == 5)
                {
                    ViewBag.Sub = db.TBL_R_SUB_MENUs.Where(x => x.Akses.Contains(poslist[0].sub_menu) || x.Akses.Contains(poslist[1].sub_menu) || x.Akses.Contains(poslist[2].sub_menu) || x.Akses.Contains(poslist[3].sub_menu) || x.Akses.Contains(poslist[4].sub_menu)).ToList();
                }
                else if (poslist.Count == 6)
                {
                    ViewBag.Sub = db.TBL_R_SUB_MENUs.Where(x => x.Akses.Contains(poslist[0].sub_menu) || x.Akses.Contains(poslist[1].sub_menu) || x.Akses.Contains(poslist[2].sub_menu) || x.Akses.Contains(poslist[3].sub_menu) || x.Akses.Contains(poslist[4].sub_menu) || x.Akses.Contains(poslist[5].sub_menu)).ToList();
                }
                else if (poslist.Count == 7)
                {
                    ViewBag.Sub = db.TBL_R_SUB_MENUs.Where(x => x.Akses.Contains(poslist[0].sub_menu) || x.Akses.Contains(poslist[1].sub_menu) || x.Akses.Contains(poslist[2].sub_menu) || x.Akses.Contains(poslist[3].sub_menu) || x.Akses.Contains(poslist[4].sub_menu) || x.Akses.Contains(poslist[5].sub_menu) || x.Akses.Contains(poslist[6].sub_menu)).ToList();
                }
                else if (poslist.Count == 8)
                {
                    ViewBag.Sub = db.TBL_R_SUB_MENUs.Where(x => x.Akses.Contains(poslist[0].sub_menu) || x.Akses.Contains(poslist[1].sub_menu) || x.Akses.Contains(poslist[2].sub_menu) || x.Akses.Contains(poslist[3].sub_menu) || x.Akses.Contains(poslist[4].sub_menu) || x.Akses.Contains(poslist[5].sub_menu) || x.Akses.Contains(poslist[6].sub_menu) || x.Akses.Contains(poslist[7].sub_menu)).ToList();
                }
                else
                {
                    if (Session["PositionID"].ToString() == "KP2AC0133")
                    {
                        ViewBag.Sub = db.TBL_R_SUB_MENUs.Where(x => x.Akses == "Accounting").ToList();
                    }
                    else
                    {
                        ViewBag.Sub = db.TBL_R_SUB_MENUs.Where(x => x.Akses.Contains("ALL")).ToList();
                    }
                }



                /*for (int i = 0; i < poslist.Count; i++)
                {
                    if (poslist[i].POSITION_CODE == "SH")
                    {
                        ViewBag.Sub = db.TBL_R_SUB_MENUs.Where(x => x.Akses.Contains("Section Head")).ToList();
                    }
                    else if (poslist[i].POSITION_CODE == "PM")
                    {
                        ViewBag.Sub = db.TBL_R_SUB_MENUs.Where(x => x.Akses.Contains("Plant Manager")).ToList();
                    }
                    else if (poslist[i].POSITION_CODE == "DH")
                    {
                        ViewBag.Sub = db.TBL_R_SUB_MENUs.Where(x => x.Akses.Contains("Plant Dept Head")).ToList();
                    }
                    else if (poslist[i].POSITION_CODE == "PMP")
                    {
                        ViewBag.Sub = db.TBL_R_SUB_MENUs.Where(x => x.Akses.Contains("PM Pengirim") || x.Akses.Contains("PM Penerima")).ToList();
                    }
                    else if (poslist[i].POSITION_CODE == "DHE")
                    {
                        ViewBag.Sub = db.TBL_R_SUB_MENUs.Where(x => x.Akses.Contains("Division Head ENG")).ToList();
                    }
                    else if (poslist[i].POSITION_CODE == "DHO")
                    {
                        ViewBag.Sub = db.TBL_R_SUB_MENUs.Where(x => x.Akses.Contains("Division Head OPR")).ToList();
                    }
                    else
                    {
                        ViewBag.Sub = db.TBL_R_SUB_MENUs.Where(x => x.Akses.Contains("ALL")).ToList();
                    }
                }*/

                //ViewBag.Sub = db.TBL_R_SUB_MENUs.Where(x => x.Akses.Contains("ALL")).ToList();


                /*if (Session["PositionID"].ToString() == "KP1PT015")
                {
                    ViewBag.Sub = db.TBL_R_SUB_MENUs.Where(x => x.Akses.Contains("Section Head") || x.Akses.Contains("lain")).ToList();
                }
                else if (Session["PositionID"].ToString() == "KP1PT02")
                {
                    ViewBag.Sub = db.TBL_R_SUB_MENUs.Where(x => x.Akses.Contains("Plant Manager")).ToList();
                }
                else if (Session["PositionID"].ToString() == "SP1D1" || Session["PositionID"].ToString() == "TM1Q1" || Session["PositionID"].ToString() == "RT1D01" || Session["PositionID"].ToString() == "BD1D1" || Session["PositionID"].ToString() == "AS1Q1" || Session["PositionID"].ToString() == "IN1AB1" || Session["PositionID"].ToString() == "TO1Q1" || Session["PositionID"].ToString() == "SR1Q1" || Session["PositionID"].ToString() == "IN1AC1" || Session["PositionID"].ToString() == "AG1Q1")
                {
                    ViewBag.Sub = db.TBL_R_SUB_MENUs.Where(x => x.Akses.Contains("Plant Dept Head")).ToList();
                }
                else if (Session["PositionID"].ToString() == "BD11" || Session["PositionID"].ToString() == "AS11" || Session["PositionID"].ToString() == "IN11" || Session["PositionID"].ToString() == "TO11" || Session["PositionID"].ToString() == "RT11" || Session["PositionID"].ToString() == "SP11" || Session["PositionID"].ToString() == "SR11" || Session["PositionID"].ToString() == "PE11")
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
                }*/
                return PartialView("_Sidebar", menu);
            }

        }
    }
}