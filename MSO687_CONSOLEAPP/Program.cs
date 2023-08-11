using MSO687_CONSOLEAPP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSO687_CONSOLEAPP.ScreenService;
using System.Configuration;
using EllipseWebServicesClient;
using System.CodeDom.Compiler;
using System.Data.Linq;

namespace MSO687_CONSOLEAPP
{
    class Program
    {
        static void Main(string[] args)
        {
            //ScheduleExecute();
            Execute();
            //Console.ReadLine(); // Keep the console application running
        }

        //static void ScheduleExecute()
        //{
        //    // Get the current time
        //    DateTime currentTime = DateTime.Now;

        //    // Set the desired execution time to 14:50 today
        //    DateTime scheduledTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 06, 01, 0);

        //    // If the scheduled time has already passed today, schedule it for tomorrow
        //    if (currentTime > scheduledTime)
        //    {
        //        scheduledTime = scheduledTime.AddDays(1);
        //    }

        //    // Calculate the time remaining until the scheduled time
        //    TimeSpan timeUntilScheduledTime = scheduledTime - currentTime;

        //    // Create a Timer to execute the Execute method at the scheduled time
        //    Timer timer = new Timer(Execute, null, timeUntilScheduledTime, Timeout.InfiniteTimeSpan);
        //}

        //private static void Execute(object state)
        static void Execute()
        {

            Console.WriteLine("EXEC-MSO687");

            DB_PLANT_PPE_CONSOLEDataContext db = new DB_PLANT_PPE_CONSOLEDataContext();
            DB_MNGMT_SRVCDataContext dbs = new DB_MNGMT_SRVCDataContext();
            
            DateTime today = DateTime.Today;
            int currentDay = today.Day;
            int currentMonth = today.Month;
            
            if (currentDay == 27)
            {
                var dataEquipment = db.VW_T_MSF687_TRASNFER_ASSETs.ToList();
                foreach (var item in dataEquipment)
                {
                    try
                    {
                        var dataList = db.VW_R_SUB_ASSET_BALANCE_SHEETs.Where(a => a.EQUIP_NO == item.EQUIP_NO).GroupBy(a => a.SUB_ASSET_NO).Select(g => g.First()).ToList();
                        foreach (var data in dataList)
                        {
                        
                            ScreenService.ScreenService service = new ScreenService.ScreenService();
                            ScreenService.OperationContext context = new ScreenService.OperationContext();
                            ScreenService.ScreenDTO screen_DTO = new ScreenService.ScreenDTO();
                            ScreenService.ScreenSubmitRequestDTO screen_request = new ScreenService.ScreenSubmitRequestDTO();

                            string str_username = ConfigurationManager.AppSettings["username"].ToString();
                            string str_password = ConfigurationManager.AppSettings["password"].ToString();
                            string str_posisi = ConfigurationManager.AppSettings["pos_id"].ToString();

                            string acak = AcakHurufBesarKecil(str_username);

                            //login ellipse
                            context.district = item.DISTRICT_FROM;
                            context.position = str_posisi;
                            ClientConversation.authenticate(acak, str_password);

                            bool conditionMet = false;
                            while (!conditionMet)
                            {
                                var cekmanserv = dbs.TBL_T_UPLOAD_JOB_MASTERs.FirstOrDefault(a => a.JOB_ID == item.JOB_MASTER_ID);
                                if (cekmanserv == null)
                                {
                                    TBL_T_UPLOAD_JOB_MASTER manserv = new TBL_T_UPLOAD_JOB_MASTER();
                                    manserv.JOB_ID = item.JOB_MASTER_ID; //"NEWPPE-MSO687-BY-TEAM-KPP-2023";
                                    manserv.SERVER_ID = "TESTING";
                                    manserv.SCRIPT_CODE = "NEWPPE_MSO687";
                                    manserv.ELLIPSE_USERNAME = str_username;
                                    manserv.ELLIPSE_PASSWORD = str_password;
                                    manserv.ELLIPSE_POSITION = str_posisi;
                                    manserv.ELLIPSE_DISTRICT = item.DISTRICT_FROM;
                                    manserv.POST_DATETIME = DateTime.Now;
                                    manserv.JOB_STATUS = 1; // 1 = LOGIN, 4 = SUCCESS, 6 = FAILED
                                    manserv.JOB_REMARK = "Login Ellipse";
                                    manserv.DB_SERVER_NAME = "kphosq101\\shpol";
                                    manserv.DATABASE_NAME = "DB_PLANT_PPE_NEW_KPT";

                                    dbs.TBL_T_UPLOAD_JOB_MASTERs.InsertOnSubmit(manserv);
                                    dbs.SubmitChanges();
                                    conditionMet = true;
                                }
                                else
                                {
                                    cekmanserv.POST_DATETIME = DateTime.Parse(DateTime.Now.ToString());
                                    cekmanserv.JOB_STATUS = 1;
                                    cekmanserv.JOB_REMARK = "Login Ellipse";
                                    dbs.SubmitChanges();
                                    conditionMet = true;
                                }
                            }

                            screen_DTO = service.executeScreen(context, "MSO687");
                            
                            while (screen_DTO.mapName != "MSM687A")
                            {
                                screen_request.screenFields = null;
                                screen_request.screenKey = "4";
                                screen_DTO = service.submit(context, screen_request);
                            }
                            
                            List<ScreenNameValueDTO> listInsert = new List<ScreenNameValueDTO>();
                            ScreenNameValueDTO fieldDTO0 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO1 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO2 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO3 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO4 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO5 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO6 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO7 = new ScreenNameValueDTO();
                            
                            fieldDTO0.fieldName = "DSTRCT_CODE_FR1I";
                            fieldDTO0.value = item.DISTRICT_FROM;
                            fieldDTO1.fieldName = "DSTRCT_CODE_TO1I";
                            fieldDTO1.value = item.DISTRICT_TO;
                            fieldDTO2.fieldName = "EQP_ASSET_REFA1I";
                            fieldDTO2.value = "E";
                            fieldDTO3.fieldName = "EQP_ASSET_REFB1I";
                            fieldDTO3.value = "E";
                            fieldDTO4.fieldName = "EQP_AS_REF_FR1I";
                            fieldDTO4.value = item.EQUIP_NO;
                            fieldDTO5.fieldName = "EQP_AS_REF_TO1I";
                            fieldDTO5.value = item.EQUIP_NO;
                            fieldDTO6.fieldName = "SUB_ASSET_FROM1I";
                            fieldDTO6.value = data.SUB_ASSET_NO;
                            fieldDTO7.fieldName = "SUB_ASSET_TO1I";
                            fieldDTO7.value = data.SUB_ASSET_NO;
                            
                            listInsert.Add(fieldDTO0);
                            listInsert.Add(fieldDTO1);
                            listInsert.Add(fieldDTO2);
                            listInsert.Add(fieldDTO3);
                            listInsert.Add(fieldDTO4);
                            listInsert.Add(fieldDTO5);
                            listInsert.Add(fieldDTO6);
                            listInsert.Add(fieldDTO7);
                            
                            screen_request.screenFields = listInsert.ToArray();
                            screen_request.screenKey = "1";
                            screen_DTO = service.submit(context, screen_request);
                            
                            listInsert.Remove(fieldDTO0);
                            listInsert.Remove(fieldDTO1);
                            listInsert.Remove(fieldDTO2);
                            listInsert.Remove(fieldDTO3);
                            listInsert.Remove(fieldDTO4);
                            listInsert.Remove(fieldDTO5);
                            listInsert.Remove(fieldDTO6);
                            listInsert.Remove(fieldDTO7);
                            Console.WriteLine("MSM685A Success");

                            var assetLocation = db.TBL_R_ASSET_LOCATIONs.Where(a => a.DSTRCT_CODE == item.DISTRICT_TO && a.EQUIPMENT_LOCATION == item.LOC_TO).FirstOrDefault();
                            
                            ScreenNameValueDTO fieldDTO8 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO9 = new ScreenNameValueDTO();
                            
                            fieldDTO8.fieldName = "ACCT_PROFILE2I";
                            fieldDTO8.value = data.ACCT_PROFILE;
                            
                            string profitloss = "DEPR_EXP_CODE2I";
                            string values = "";
                            switch (data.ACCT_PROFILE)
                            {
                                case "0001":
                                    values = assetLocation.PRODUCTION_EQUIPMENT;
                                    break;
                                case "0003":
                                    values = assetLocation.SUPPORT_EQUIPMENT;
                                    break;
                                case "0007":
                                    values = assetLocation.WORKSHOP_EQUIPMENT;
                                    break;
                                default:
                                    // Default value or action if none of the cases match
                                    break;
                            }
                            
                            fieldDTO9.fieldName = profitloss;
                            fieldDTO9.value = values;
                            
                            listInsert.Add(fieldDTO8);
                            listInsert.Add(fieldDTO9);
                            
                            screen_request.screenFields = listInsert.ToArray();
                            screen_request.screenKey = "1";
                            screen_DTO = service.submit(context, screen_request);
                            
                            while (screen_DTO.mapName != "MSM685C")
                            {
                                screen_request.screenFields = null;
                                screen_request.screenKey = "1";
                                screen_DTO = service.submit(context, screen_request);
                            }
                            
                            listInsert.Remove(fieldDTO8);
                            listInsert.Remove(fieldDTO9);
                            Console.WriteLine("MSM685B Success");
                            
                            screen_request.screenFields = null;
                            screen_request.screenKey = "1";
                            screen_DTO = service.submit(context, screen_request);
                            
                            while (screen_DTO.mapName != "MSM687A")
                            {
                                screen_request.screenFields = null;
                                screen_request.screenKey = "1";
                                screen_DTO = service.submit(context, screen_request);
                            }
                            
                            Console.WriteLine("MSM685C Success");

                            ScreenNameValueDTO fieldDTO13 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO14 = new ScreenNameValueDTO();
                            
                            DateTime datereceivedsm = (DateTime)item.DATE_RECEIVED_SM;
                            fieldDTO13.fieldName = "XFER_DATE1I";
                            fieldDTO13.value = datereceivedsm.ToString("yyyyMMdd");
                            fieldDTO14.fieldName = "XFER_PERCENT1I";
                            fieldDTO14.value = "100.00";
                            
                            listInsert.Add(fieldDTO13);
                            listInsert.Add(fieldDTO14);
                            
                            screen_request.screenFields = listInsert.ToArray();
                            screen_request.screenKey = "1";
                            screen_DTO = service.submit(context, screen_request);

                            screen_request.screenFields = null;
                            screen_request.screenKey = "1";
                            screen_DTO = service.submit(context, screen_request);

                            Console.WriteLine("MSO687 Done");

                            // Kosongkan listInsert untuk penggunaan selanjutnya
                            listInsert.Clear();

                        }
                        
                        var cek = dbs.TBL_T_UPLOAD_JOB_MASTERs.FirstOrDefault(a => a.JOB_ID == item.JOB_MASTER_ID);
                        if (cek != null)
                        {
                            cek.POST_DATETIME = DateTime.Parse(DateTime.Now.ToString());
                            cek.JOB_STATUS = 4;
                            cek.JOB_REMARK = "Success Created MSO687 " + item.EQUIP_NO;
                            dbs.SubmitChanges();
                        }

                        var cek2 = db.TBL_T_PPEs.FirstOrDefault(a => a.EQUIP_NO == item.EQUIP_NO);
                        if (cek2 != null)
                        {
                            cek2.FLAG = 1;
                            db.SubmitChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        var exMessage = ex.Message;
                        var cek = dbs.TBL_T_UPLOAD_JOB_MASTERs.FirstOrDefault(a => a.JOB_ID == item.JOB_MASTER_ID);
                        if (cek != null)
                        {
                            cek.POST_DATETIME = DateTime.Parse(DateTime.Now.ToString());
                            cek.JOB_STATUS = 6;
                            cek.JOB_REMARK = "Failed Created " + item.EQUIP_NO + " - " + exMessage;
                            dbs.SubmitChanges();
                        }

                        var cek2 = db.TBL_T_PPEs.FirstOrDefault(a => a.EQUIP_NO == item.EQUIP_NO);
                        if (cek2 != null)
                        {
                            cek2.FLAG = 2;
                            db.SubmitChanges();
                        }
                        // You can also log the error here if needed
                        Console.WriteLine("Error occurred: " + exMessage);
                    }
                }
            }
            if (currentDay == 15 || currentDay == 16 || currentDay == 17 || currentDay == 18 || currentDay == 19 || currentDay == 20 || currentDay == 21 || currentDay == 22 || currentDay == 23 || currentDay == 24 || currentDay == 25 || currentDay == 26)
            {
                Console.WriteLine("Nothing to execution on 15 until 26");
                return;
            }
            else
            {
                var dataEquipment = db.VW_T_MSF687_TRASNFER_ASSETs.ToList();
                foreach (var item in dataEquipment)
                {
                    try
                    {
                        var dataList = db.VW_R_SUB_ASSET_BALANCE_SHEETs.Where(a => a.EQUIP_NO == item.EQUIP_NO).GroupBy(a => a.SUB_ASSET_NO).Select(g => g.First()).ToList();
                        foreach (var data in dataList)
                        {

                            ScreenService.ScreenService service = new ScreenService.ScreenService();
                            ScreenService.OperationContext context = new ScreenService.OperationContext();
                            ScreenService.ScreenDTO screen_DTO = new ScreenService.ScreenDTO();
                            ScreenService.ScreenSubmitRequestDTO screen_request = new ScreenService.ScreenSubmitRequestDTO();

                            string str_username = ConfigurationManager.AppSettings["username"].ToString();
                            string str_password = ConfigurationManager.AppSettings["password"].ToString();
                            string str_posisi = ConfigurationManager.AppSettings["pos_id"].ToString();

                            string acak = AcakHurufBesarKecil(str_username);

                            //login ellipse
                            context.district = item.DISTRICT_FROM;
                            context.position = str_posisi;
                            ClientConversation.authenticate(acak, str_password);

                            bool conditionMet = false;
                            while (!conditionMet)
                            {
                                var cekmanserv = dbs.TBL_T_UPLOAD_JOB_MASTERs.FirstOrDefault(a => a.JOB_ID == item.JOB_MASTER_ID);
                                if (cekmanserv == null)
                                {
                                    TBL_T_UPLOAD_JOB_MASTER manserv = new TBL_T_UPLOAD_JOB_MASTER();
                                    manserv.JOB_ID = item.JOB_MASTER_ID; //"NEWPPE-MSO687-BY-TEAM-KPP-2023";
                                    manserv.SERVER_ID = "TESTING";
                                    manserv.SCRIPT_CODE = "NEWPPE_MSO687";
                                    manserv.ELLIPSE_USERNAME = str_username;
                                    manserv.ELLIPSE_PASSWORD = str_password;
                                    manserv.ELLIPSE_POSITION = str_posisi;
                                    manserv.ELLIPSE_DISTRICT = item.DISTRICT_FROM;
                                    manserv.POST_DATETIME = DateTime.Now;
                                    manserv.JOB_STATUS = 1; // 1 = LOGIN, 4 = SUCCESS, 6 = FAILED
                                    manserv.JOB_REMARK = "Login Ellipse";
                                    manserv.DB_SERVER_NAME = "kphosq101\\shpol";
                                    manserv.DATABASE_NAME = "DB_PLANT_PPE_NEW_KPT";

                                    dbs.TBL_T_UPLOAD_JOB_MASTERs.InsertOnSubmit(manserv);
                                    dbs.SubmitChanges();
                                    conditionMet = true;
                                }
                                else
                                {
                                    cekmanserv.POST_DATETIME = DateTime.Parse(DateTime.Now.ToString());
                                    cekmanserv.JOB_STATUS = 1;
                                    cekmanserv.JOB_REMARK = "Login Ellipse";
                                    dbs.SubmitChanges();
                                    conditionMet = true;
                                }
                            }

                            screen_DTO = service.executeScreen(context, "MSO687");

                            while (screen_DTO.mapName != "MSM687A")
                            {
                                screen_request.screenFields = null;
                                screen_request.screenKey = "4";
                                screen_DTO = service.submit(context, screen_request);
                            }

                            List<ScreenNameValueDTO> listInsert = new List<ScreenNameValueDTO>();
                            ScreenNameValueDTO fieldDTO0 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO1 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO2 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO3 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO4 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO5 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO6 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO7 = new ScreenNameValueDTO();

                            fieldDTO0.fieldName = "DSTRCT_CODE_FR1I";
                            fieldDTO0.value = item.DISTRICT_FROM;
                            fieldDTO1.fieldName = "DSTRCT_CODE_TO1I";
                            fieldDTO1.value = item.DISTRICT_TO;
                            fieldDTO2.fieldName = "EQP_ASSET_REFA1I";
                            fieldDTO2.value = "E";
                            fieldDTO3.fieldName = "EQP_ASSET_REFB1I";
                            fieldDTO3.value = "E";
                            fieldDTO4.fieldName = "EQP_AS_REF_FR1I";
                            fieldDTO4.value = item.EQUIP_NO;
                            fieldDTO5.fieldName = "EQP_AS_REF_TO1I";
                            fieldDTO5.value = item.EQUIP_NO;
                            fieldDTO6.fieldName = "SUB_ASSET_FROM1I";
                            fieldDTO6.value = data.SUB_ASSET_NO;
                            fieldDTO7.fieldName = "SUB_ASSET_TO1I";
                            fieldDTO7.value = data.SUB_ASSET_NO;

                            listInsert.Add(fieldDTO0);
                            listInsert.Add(fieldDTO1);
                            listInsert.Add(fieldDTO2);
                            listInsert.Add(fieldDTO3);
                            listInsert.Add(fieldDTO4);
                            listInsert.Add(fieldDTO5);
                            listInsert.Add(fieldDTO6);
                            listInsert.Add(fieldDTO7);

                            screen_request.screenFields = listInsert.ToArray();
                            screen_request.screenKey = "1";
                            screen_DTO = service.submit(context, screen_request);

                            listInsert.Remove(fieldDTO0);
                            listInsert.Remove(fieldDTO1);
                            listInsert.Remove(fieldDTO2);
                            listInsert.Remove(fieldDTO3);
                            listInsert.Remove(fieldDTO4);
                            listInsert.Remove(fieldDTO5);
                            listInsert.Remove(fieldDTO6);
                            listInsert.Remove(fieldDTO7);
                            Console.WriteLine("MSM685A Success");

                            var assetLocation = db.TBL_R_ASSET_LOCATIONs.Where(a => a.DSTRCT_CODE == item.DISTRICT_TO && a.EQUIPMENT_LOCATION == item.LOC_TO).FirstOrDefault();

                            ScreenNameValueDTO fieldDTO8 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO9 = new ScreenNameValueDTO();

                            fieldDTO8.fieldName = "ACCT_PROFILE2I";
                            fieldDTO8.value = data.ACCT_PROFILE;

                            string profitloss = "DEPR_EXP_CODE2I";
                            string values = "";
                            switch (data.ACCT_PROFILE)
                            {
                                case "0001":
                                    values = assetLocation.PRODUCTION_EQUIPMENT;
                                    break;
                                case "0003":
                                    values = assetLocation.SUPPORT_EQUIPMENT;
                                    break;
                                case "0007":
                                    values = assetLocation.WORKSHOP_EQUIPMENT;
                                    break;
                                default:
                                    // Default value or action if none of the cases match
                                    break;
                            }

                            fieldDTO9.fieldName = profitloss;
                            fieldDTO9.value = values;

                            listInsert.Add(fieldDTO8);
                            listInsert.Add(fieldDTO9);

                            screen_request.screenFields = listInsert.ToArray();
                            screen_request.screenKey = "1";
                            screen_DTO = service.submit(context, screen_request);

                            while (screen_DTO.mapName != "MSM685C")
                            {
                                screen_request.screenFields = null;
                                screen_request.screenKey = "1";
                                screen_DTO = service.submit(context, screen_request);
                            }

                            listInsert.Remove(fieldDTO8);
                            listInsert.Remove(fieldDTO9);
                            Console.WriteLine("MSM685B Success");

                            screen_request.screenFields = null;
                            screen_request.screenKey = "1";
                            screen_DTO = service.submit(context, screen_request);

                            while (screen_DTO.mapName != "MSM687A")
                            {
                                screen_request.screenFields = null;
                                screen_request.screenKey = "1";
                                screen_DTO = service.submit(context, screen_request);
                            }

                            Console.WriteLine("MSM685C Success");

                            ScreenNameValueDTO fieldDTO13 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO14 = new ScreenNameValueDTO();

                            DateTime datereceivedsm = (DateTime)item.DATE_RECEIVED_SM;
                            fieldDTO13.fieldName = "XFER_DATE1I";
                            fieldDTO13.value = datereceivedsm.ToString("yyyyMMdd");
                            fieldDTO14.fieldName = "XFER_PERCENT1I";
                            fieldDTO14.value = "100.00";

                            listInsert.Add(fieldDTO13);
                            listInsert.Add(fieldDTO14);

                            screen_request.screenFields = listInsert.ToArray();
                            screen_request.screenKey = "1";
                            screen_DTO = service.submit(context, screen_request);

                            screen_request.screenFields = null;
                            screen_request.screenKey = "1";
                            screen_DTO = service.submit(context, screen_request);

                            Console.WriteLine("MSO687 Done");

                            // Kosongkan listInsert untuk penggunaan selanjutnya
                            listInsert.Clear();

                        }

                        var cek = dbs.TBL_T_UPLOAD_JOB_MASTERs.FirstOrDefault(a => a.JOB_ID == item.JOB_MASTER_ID);
                        if (cek != null)
                        {
                            cek.POST_DATETIME = DateTime.Parse(DateTime.Now.ToString());
                            cek.JOB_STATUS = 4;
                            cek.JOB_REMARK = "Success Created MSO687 " + item.EQUIP_NO;
                            dbs.SubmitChanges();
                        }

                        var cek2 = db.TBL_T_PPEs.FirstOrDefault(a => a.EQUIP_NO == item.EQUIP_NO);
                        if (cek2 != null)
                        {
                            cek2.FLAG = 1;
                            db.SubmitChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        var exMessage = ex.Message;
                        var cek = dbs.TBL_T_UPLOAD_JOB_MASTERs.FirstOrDefault(a => a.JOB_ID == item.JOB_MASTER_ID);
                        if (cek != null)
                        {
                            cek.POST_DATETIME = DateTime.Parse(DateTime.Now.ToString());
                            cek.JOB_STATUS = 6;
                            cek.JOB_REMARK = "Failed Created " + item.EQUIP_NO + " - " + exMessage;
                            dbs.SubmitChanges();
                        }

                        var cek2 = db.TBL_T_PPEs.FirstOrDefault(a => a.EQUIP_NO == item.EQUIP_NO);
                        if (cek2 != null)
                        {
                            cek2.FLAG = 2;
                            db.SubmitChanges();
                        }
                        // You can also log the error here if needed
                        Console.WriteLine("Error occurred: " + exMessage);
                    }
                }
            }
        }
        
        private static string AcakHurufBesarKecil(string input)
        {
            char[] characters = input.ToCharArray();
            Random random = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < characters.Length; i++)
            {
                if (char.IsLetter(characters[i]))
                {
                    characters[i] = random.NextDouble() > 0.5 ? char.ToUpper(characters[i]) : char.ToLower(characters[i]);
                }
            }
            return new string(characters);
        }
    }
}
