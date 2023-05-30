using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PLANT_PPE.Models;
using PLANT_PPE.ViewModel;

namespace PLANT_PPE.Controllers
{
    public class PPEController : Controller
    {
        //public ActionResult Register()
        //{
        //    if (Session["nrp"] == null)
        //    {
        //        return RedirectToAction("index", "login");
        //    }
        //    return View();
        //}
        private static string RomanMonth(int month)
        {
            switch (month)
            {
                case 1: return "I";
                case 2: return "II";
                case 3: return "III";
                case 4: return "IV";
                case 5: return "V";
                case 6: return "VI";
                case 7: return "VII";
                case 8: return "VIII";
                case 9: return "IX";
                case 10: return "X";
                case 11: return "XI";
                case 12: return "XII";
                default: return "";
            }
        }
        public async Task<ActionResult> Register()
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }

            TBL_T_PPE tbl = new TBL_T_PPE();
            string NoPPE = "";

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri((string)Session["Web_Link"]);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/PPE/Get_LastNoPPE/");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var ApiResponse = Res.Content.ReadAsStringAsync().Result;
                    Cls_PPE data = new Cls_PPE();
                    data = JsonConvert.DeserializeObject<Cls_PPE>(ApiResponse);

                    tbl = data.tbl;
                    if (tbl == null)
                    {
                        string romanMonth = RomanMonth(DateTime.Now.Month);
                        NoPPE = "001" + "/PPE/" + romanMonth + "/" + DateTime.Now.ToString("yyyy");
                    }
                    else
                    {
                        //Penambahan kondisi jumlah roman
                        string romanMonth = RomanMonth(DateTime.Now.Month);
                        //string romanMonth = "VI";
                        if (romanMonth == "I" || romanMonth == "V" || romanMonth == "X")
                        {
                            string month = tbl.PPE_NO.Substring(8, 1);
                            string year = tbl.PPE_NO.Substring(10, 4);
                            string thisMonth = RomanMonth(DateTime.Now.Month);
                            string thisYear = DateTime.Now.ToString("yyyy");
                            if (month == thisMonth && year == thisYear)
                            {
                                int setNo = Convert.ToInt32(tbl.PPE_NO.Substring(0, 3)) + 1;
                                NoPPE = setNo.ToString().PadLeft(3, '0') + "/PPE/" + thisMonth + "/" + thisYear;
                            }
                            else
                            {
                                NoPPE = "001" + "/PPE/" + thisMonth + "/" + thisYear;
                            }
                        } else if (romanMonth == "II" || romanMonth == "IV" || romanMonth == "VI" || romanMonth == "IX" || romanMonth == "XI")
                        {
                            string month = tbl.PPE_NO.Substring(8, 2);
                            string year = tbl.PPE_NO.Substring(11, 4);
                            string thisMonth = RomanMonth(DateTime.Now.Month);
                            //string thisMonth = "VII";
                            string thisYear = DateTime.Now.ToString("yyyy");
                            if (month == thisMonth && year == thisYear)
                            {
                                int setNo = Convert.ToInt32(tbl.PPE_NO.Substring(0, 3)) + 1;
                                NoPPE = setNo.ToString().PadLeft(3, '0') + "/PPE/" + thisMonth + "/" + thisYear;
                            }
                            else
                            {
                                NoPPE = "001" + "/PPE/" + thisMonth + "/" + thisYear;
                            }
                        } else if (romanMonth == "III" || romanMonth == "VII" || romanMonth == "XII")
                        {
                            string month = tbl.PPE_NO.Substring(8, 3);
                            string year = tbl.PPE_NO.Substring(12, 4);
                            string thisMonth = RomanMonth(DateTime.Now.Month);
                            string thisYear = DateTime.Now.ToString("yyyy");
                            if (month == thisMonth && year == thisYear)
                            {
                                int setNo = Convert.ToInt32(tbl.PPE_NO.Substring(0, 3)) + 1;
                                NoPPE = setNo.ToString().PadLeft(3, '0') + "/PPE/" + thisMonth + "/" + thisYear;
                            }
                            else
                            {
                                NoPPE = "001" + "/PPE/" + thisMonth + "/" + thisYear;
                            }
                        } else if (romanMonth == "VIII")
                        {
                            string month = tbl.PPE_NO.Substring(8, 4);
                            string year = tbl.PPE_NO.Substring(13, 4);
                            string thisMonth = RomanMonth(DateTime.Now.Month);
                            string thisYear = DateTime.Now.ToString("yyyy");
                            if (month == thisMonth && year == thisYear)
                            {
                                int setNo = Convert.ToInt32(tbl.PPE_NO.Substring(0, 3)) + 1;
                                NoPPE = setNo.ToString().PadLeft(3, '0') + "/PPE/" + thisMonth + "/" + thisYear;
                            }
                            else
                            {
                                NoPPE = "001" + "/PPE/" + thisMonth + "/" + thisYear;
                            }
                        }

                        //string month = tbl.PPE_NO.Substring(8, 1);
                        //string year = tbl.PPE_NO.Substring(10, 4);
                        //string thisMonth = RomanMonth(DateTime.Now.Month);
                        //string thisYear = DateTime.Now.ToString("yyyy");
                        //if (month == thisMonth && year == thisYear)
                        //{
                        //    int setNo = Convert.ToInt32(tbl.PPE_NO.Substring(0, 3)) + 1;
                        //    NoPPE = setNo.ToString().PadLeft(3, '0') + "/PPE/" + thisMonth + "/" + thisYear;
                        //}
                        //else
                        //{
                        //    NoPPE = "001" + "/PPE/" + thisMonth + "/" + thisYear;
                        //}
                    }

                    ViewBag.NoPPE = NoPPE;

                }
                return View();
            }
        }
    }
}