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

                if (Session["ID_Role"].ToString() == "5")
                {
                    if (Session["PositionID"].ToString() != "KP2AC0133")
                    {
                        var settingMenu = menu.Where(x => x.Name_Menu == "Setting").FirstOrDefault();
                        menu.Remove(settingMenu);
                    }
                }

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
                    if (Session["ID_Role"].ToString() == "5")
                    {
                        if (Session["PositionID"].ToString() == "KP2AC0133")
                        {
                            ViewBag.Sub = db.TBL_R_SUB_MENUs.Where(x => x.Akses == "Accounting").ToList();
                        }
                    }
                    else
                    {
                        ViewBag.Sub = db.TBL_R_SUB_MENUs.Where(x => x.Akses.Contains("ALL")).ToList();
                    }
                }

                return PartialView("_Sidebar", menu);
            }

        }
    }
}