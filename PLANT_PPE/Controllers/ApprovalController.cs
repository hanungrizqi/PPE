using Newtonsoft.Json;
using PLANT_PPE.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PLANT_PPE.Models;

namespace PLANT_PPE.Controllers
{
    public class ApprovalController : Controller
    {
        DB_PLANT_PPEDataContext db = new DB_PLANT_PPEDataContext();
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
        public ActionResult DetailPPE_PM(int idppe)
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            ViewBag.idppe = idppe;
            return View();
        }
        public ActionResult DetailPPE_ProjectManager(int idppe)
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            ViewBag.idppe = idppe;
            return View();
        }
        public ActionResult DetailPPE_ProjectManager_Penerima(int idppe)
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            ViewBag.idppe = idppe;
            return View();
        }
        public ActionResult DetailPPE_DivHead(int idppe)
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            ViewBag.idppe = idppe;
            return View();
        }
        public ActionResult DetailPPE_DivHead_OPR(int idppe)
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

        public ActionResult PlantADMDevManager()
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

        public ActionResult PrintSectHead()
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            return Redirect("http://10.14.101.181/ReportServer_RPTPROD?/PPE/Rpt_PPE_SecHead");
        }
        
        public async Task<ActionResult> Detail_DeptHead(string ppe)
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            string encodedPpe = HttpUtility.UrlEncode(ppe);
            List<TBL_T_PPE> tbl = new List<TBL_T_PPE>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri((string)Session["Web_Link"]);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/Approval/Get_PPE_EquipmentPart?ppe=" + encodedPpe);

                if (Res.IsSuccessStatusCode)
                {
                    var ApiResponse = Res.Content.ReadAsStringAsync().Result;
                    Cls_PPEDetail data = new Cls_PPEDetail();
                    data = JsonConvert.DeserializeObject<Cls_PPEDetail>(ApiResponse);
                    tbl = (List<TBL_T_PPE>)data.tbl;
                    ViewBag.dataEquipp = tbl;
                    ViewBag.ppe_numberr = ppe;
                }
            }
            return View();
        }
    }
}