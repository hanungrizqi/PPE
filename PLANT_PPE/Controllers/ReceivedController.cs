using Newtonsoft.Json;
using PLANT_PPE.Models;
using PLANT_PPE.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using System.IO;

namespace PLANT_PPE.Controllers
{
    public class ReceivedController : Controller
    {
        DB_PLANT_PPEDataContext db = new DB_PLANT_PPEDataContext();
        // GET: Received
        public ActionResult Index()
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            return View();
        }
        public async Task<ActionResult> DetailPPE_EQP(string ppe)
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            string encodedPpe = HttpUtility.UrlEncode(ppe);
            List<TBL_T_PPE> tbl = new List<TBL_T_PPE>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri((string)Session["Web_Link"]);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/SM/Get_PPE_EquipmentPart?ppe=" + encodedPpe);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var ApiResponse = Res.Content.ReadAsStringAsync().Result;
                    Cls_PPEDetail data = new Cls_PPEDetail();
                    data = JsonConvert.DeserializeObject<Cls_PPEDetail>(ApiResponse);
                    tbl = (List<TBL_T_PPE>)data.tbl;
                    ViewBag.dataEquip = tbl;
                    ViewBag.ppe_number = ppe;
                }
            }
            return View();
        }
    }
}