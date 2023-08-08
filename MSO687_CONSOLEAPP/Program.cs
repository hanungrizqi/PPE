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
            Console.ReadLine(); // Keep the console application running
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

            //penambahan if currentDay
            DateTime today = DateTime.Today;
            //DateTime yesterday = today.AddDays(-1);
            int currentDay = today.Day;
            int currentMonth = today.Month;
            if (currentDay == 27)
            {
                //var dataEquipment = db.TBL_T_PPEs.Where(item => item.DATE_RECEIVED_SM.ToString() == "2023-07-18" && item.FLAG == 0).ToList();
                //var dataEquipment = db.TBL_T_PPEs.Where(item => item.DATE_RECEIVED_SM.ToString() == DateTime.Today.ToString("yyyy-MM-dd") && item.FLAG == 0).ToList();
                //var dataEquipment = db.TBL_T_PPEs.Where(item => item.DATE_RECEIVED_SM.HasValue &&
                //           item.DATE_RECEIVED_SM.Value.Year == today.Year &&
                //           item.DATE_RECEIVED_SM.Value.Month == currentMonth &&
                //           item.DATE_RECEIVED_SM.Value.Day >= 15 &&
                //           item.DATE_RECEIVED_SM.Value.Day <= 26 &&
                //           item.FLAG == 0).ToList();
                var dataEquipment = db.VW_T_MSF687_TRASNFER_ASSETs.ToList();

                foreach (var item in dataEquipment)
                {
                    try
                    {
                        var dataList = db.VW_MSF685s.Where(a => a.ASSET_NO == item.EQUIP_NO).GroupBy(a => a.SUB_ASSET_NO).Select(g => g.First()).ToList();

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
                        }
                        else
                        {
                            dbs.Refresh(RefreshMode.OverwriteCurrentValues, cekmanserv);
                            cekmanserv.POST_DATETIME = DateTime.Now;
                            cekmanserv.JOB_STATUS = 1;
                            cekmanserv.JOB_REMARK = "Login Ellipse";
                            dbs.SubmitChanges();
                        }

                        screen_DTO = service.executeScreen(context, "MSO687");

                        while (screen_DTO.mapName != "MSM687A")
                        {
                            screen_request.screenFields = null;
                            screen_request.screenKey = "4";
                            screen_DTO = service.submit(context, screen_request);
                        }

                        foreach (var data in dataList)
                        {
                            //var dataAcctSub = db.VW_R_ACCT_PROFILEs.Where(a => a.DSTRCT_CODE == item.DISTRICT_FROM && a.EQUIP_LOCATION == item.LOC_FROM && a.SUB_ASSET_NO == data.SUB_ASSET_NO).FirstOrDefault(); //sesuai unit yang dimutasi
                            var dataAcctSub = db.VW_R_ACCT_PROFILEs.Where(a => a.EQUIP_NO == data.ASSET_NO && a.SUB_ASSET_NO == data.SUB_ASSET_NO).FirstOrDefault();

                            //deklarasi variabel input
                            List<ScreenNameValueDTO> listInsert = new List<ScreenNameValueDTO>();
                            ScreenNameValueDTO fieldDTO0 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO1 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO2 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO3 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO4 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO5 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO6 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO7 = new ScreenNameValueDTO();

                            //input dengan parameter fieldname dan value
                            fieldDTO0.fieldName = "DSTRCT_CODE_FR1I";
                            fieldDTO0.value = item.DISTRICT_FROM; //fieldDTO0.value = "RANT";
                            fieldDTO1.fieldName = "DSTRCT_CODE_TO1I";
                            fieldDTO1.value = item.DISTRICT_TO; //fieldDTO1.value = "MASS";
                            fieldDTO2.fieldName = "EQP_ASSET_REFA1I";
                            fieldDTO2.value = "E";
                            fieldDTO3.fieldName = "EQP_ASSET_REFB1I";
                            fieldDTO3.value = "E";
                            fieldDTO4.fieldName = "EQP_AS_REF_FR1I";
                            fieldDTO4.value = item.EQUIP_NO; //fieldDTO4.value = "DT4011";
                            fieldDTO5.fieldName = "EQP_AS_REF_TO1I";
                            fieldDTO5.value = item.EQUIP_NO; //fieldDTO5.value = "DT4011";
                            fieldDTO6.fieldName = "SUB_ASSET_FROM1I";
                            fieldDTO6.value = data.SUB_ASSET_NO; //fieldDTO6.value = "000001";
                            fieldDTO7.fieldName = "SUB_ASSET_TO1I";
                            fieldDTO7.value = data.SUB_ASSET_NO; //fieldDTO7.value = "000001";

                            //disatukan dalam list
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

                            Console.WriteLine("EXEC-MSM685A");

                            //Remove list
                            listInsert.Remove(fieldDTO0);
                            listInsert.Remove(fieldDTO1);
                            listInsert.Remove(fieldDTO2);
                            listInsert.Remove(fieldDTO3);
                            listInsert.Remove(fieldDTO4);
                            listInsert.Remove(fieldDTO5);
                            listInsert.Remove(fieldDTO6);
                            listInsert.Remove(fieldDTO7);

                            ScreenNameValueDTO fieldDTO8 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO9 = new ScreenNameValueDTO();

                            //input dengan parameter fieldname dan value
                            fieldDTO8.fieldName = "ACCT_PROFILE2I"; //fieldDTO8.value = "1";
                            fieldDTO8.value = dataAcctSub.ACCT_PROFILE;
                            fieldDTO9.fieldName = "DEPR_EXP_CODE2I"; //fieldDTO9.value = "1";
                            fieldDTO9.value = dataAcctSub.ACCT_PROFILE;

                            //disatukan dalam list
                            listInsert.Add(fieldDTO8);
                            listInsert.Add(fieldDTO9);

                            //submit data
                            screen_request.screenFields = listInsert.ToArray();
                            screen_request.screenKey = "1";
                            screen_DTO = service.submit(context, screen_request);

                            Console.WriteLine("EXEC-MSM685B");

                            if (screen_DTO.programName != "MSM685C")
                            {
                                screen_request.screenFields = null;
                                screen_request.screenKey = "1";
                                screen_DTO = service.submit(context, screen_request);
                            }

                            //Remove list
                            listInsert.Remove(fieldDTO8);
                            listInsert.Remove(fieldDTO9);

                            //ScreenNameValueDTO fieldDTO10 = new ScreenNameValueDTO();
                            //ScreenNameValueDTO fieldDTO11 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO12 = new ScreenNameValueDTO();

                            //input dengan parameter fieldname dan value
                            //fieldDTO10.fieldName = "DEPR_METHOD3I";
                            //fieldDTO10.value = "L";
                            //fieldDTO11.fieldName = "DEPR_RATE3I";
                            //fieldDTO11.value = "";
                            fieldDTO12.fieldName = "EST_MM_LIFE3I";
                            fieldDTO12.value = "12";

                            //disatukan dalam list
                            //listInsert.Add(fieldDTO10);
                            //listInsert.Add(fieldDTO11);
                            listInsert.Add(fieldDTO12);

                            //submit data
                            screen_request.screenFields = listInsert.ToArray();
                            //screen_request.screenFields = null;
                            screen_request.screenKey = "1";
                            screen_DTO = service.submit(context, screen_request);

                            //submit data
                            screen_request.screenFields = null;
                            screen_request.screenKey = "1";
                            screen_DTO = service.submit(context, screen_request);

                            Console.WriteLine("EXEC-MSM685C");

                            //Remove list
                            listInsert.Remove(fieldDTO12);

                            ScreenNameValueDTO fieldDTO13 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO14 = new ScreenNameValueDTO();

                            //input dengan parameter fieldname dan value
                            DateTime datereceivedsm = (DateTime)item.DATE_RECEIVED_SM;
                            fieldDTO13.fieldName = "XFER_DATE1I";
                            fieldDTO13.value = datereceivedsm.ToString("yyyyMMdd"); //"20220730";
                            fieldDTO14.fieldName = "XFER_PERCENT1I";
                            fieldDTO14.value = "100.00";

                            //disatukan dalam list
                            listInsert.Add(fieldDTO13);
                            listInsert.Add(fieldDTO14);

                            //submit data
                            screen_request.screenFields = listInsert.ToArray();
                            screen_request.screenKey = "1";
                            screen_DTO = service.submit(context, screen_request);

                            screen_request.screenFields = null;
                            screen_request.screenKey = "1";
                            screen_DTO = service.submit(context, screen_request);

                            Console.WriteLine("MSO687 SUCCESS");

                            // Kosongkan listInsert untuk penggunaan selanjutnya
                            listInsert.Clear();

                        }

                        //Console.WriteLine(item.ID); // Print the "Name" property of each object
                        //Console.WriteLine(item.EQUIP_NO); // Print the "Name" property of each object
                        //ubah flag menjadi 1
                        var cek = dbs.TBL_T_UPLOAD_JOB_MASTERs.FirstOrDefault(a => a.JOB_ID == item.JOB_MASTER_ID);
                        if (cek != null)
                        {
                            dbs.Refresh(RefreshMode.OverwriteCurrentValues, cek);
                            cek.POST_DATETIME = DateTime.Now;
                            cek.JOB_STATUS = 4; // 4 = Success
                            cek.JOB_REMARK = "Success Created MSO687 " + item.EQUIP_NO;
                            dbs.SubmitChanges();
                        }

                        var cek2 = db.TBL_T_PPEs.FirstOrDefault(a => a.EQUIP_NO == item.EQUIP_NO);
                        if (cek2 != null)
                        {
                            // 1 = Success
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
                            dbs.Refresh(RefreshMode.OverwriteCurrentValues, cek);
                            cek.POST_DATETIME = DateTime.Now;
                            cek.JOB_STATUS = 6; // 6 = Failed
                            cek.JOB_REMARK = "Failed Created " + item.EQUIP_NO + " - " + exMessage;
                            dbs.SubmitChanges();
                        }

                        var cek2 = db.TBL_T_PPEs.FirstOrDefault(a => a.EQUIP_NO == item.EQUIP_NO);
                        if (cek2 != null)
                        {
                            // 2 = Error
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
                //var dataEquipment = db.TBL_T_PPEs.Where(item => item.DATE_RECEIVED_SM.ToString() == "2023-07-18" && item.FLAG == 0).ToList();
                //var dataEquipment = db.TBL_T_PPEs.Where(item => item.DATE_RECEIVED_SM.ToString() == DateTime.Today.ToString("yyyy-MM-dd") && item.FLAG == 0).ToList();
                var dataEquipment = db.VW_T_MSF687_TRASNFER_ASSETs.ToList();

                foreach (var item in dataEquipment)
                {
                    try
                    {
                        var dataList = db.VW_MSF685s.Where(a => a.ASSET_NO == item.EQUIP_NO).GroupBy(a => a.SUB_ASSET_NO).Select(g => g.First()).ToList();

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
                        }
                        else
                        {
                            dbs.Refresh(RefreshMode.OverwriteCurrentValues, cekmanserv);
                            cekmanserv.POST_DATETIME = DateTime.Now;
                            cekmanserv.JOB_STATUS = 1;
                            cekmanserv.JOB_REMARK = "Login Ellipse";
                            dbs.SubmitChanges();
                        }

                        screen_DTO = service.executeScreen(context, "MSO687");

                        while (screen_DTO.mapName != "MSM687A")
                        {
                            screen_request.screenFields = null;
                            screen_request.screenKey = "4";
                            screen_DTO = service.submit(context, screen_request);
                        }

                        foreach (var data in dataList)
                        {
                            //var dataAcctSub = db.VW_R_ACCT_PROFILEs.Where(a => a.DSTRCT_CODE == item.DISTRICT_FROM && a.EQUIP_LOCATION == item.LOC_FROM && a.SUB_ASSET_NO == data.SUB_ASSET_NO).FirstOrDefault(); //sesuai unit yang dimutasi
                            var dataAcctSub = db.VW_R_ACCT_PROFILEs.Where(a => a.EQUIP_NO == data.ASSET_NO && a.SUB_ASSET_NO == data.SUB_ASSET_NO).FirstOrDefault();

                            //deklarasi variabel input
                            List<ScreenNameValueDTO> listInsert = new List<ScreenNameValueDTO>();
                            ScreenNameValueDTO fieldDTO0 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO1 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO2 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO3 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO4 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO5 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO6 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO7 = new ScreenNameValueDTO();

                            //input dengan parameter fieldname dan value
                            fieldDTO0.fieldName = "DSTRCT_CODE_FR1I";
                            fieldDTO0.value = item.DISTRICT_FROM; //fieldDTO0.value = "RANT";
                            fieldDTO1.fieldName = "DSTRCT_CODE_TO1I";
                            fieldDTO1.value = item.DISTRICT_TO; //fieldDTO1.value = "MASS";
                            fieldDTO2.fieldName = "EQP_ASSET_REFA1I";
                            fieldDTO2.value = "E";
                            fieldDTO3.fieldName = "EQP_ASSET_REFB1I";
                            fieldDTO3.value = "E";
                            fieldDTO4.fieldName = "EQP_AS_REF_FR1I";
                            fieldDTO4.value = item.EQUIP_NO; //fieldDTO4.value = "DT4011";
                            fieldDTO5.fieldName = "EQP_AS_REF_TO1I";
                            fieldDTO5.value = item.EQUIP_NO; //fieldDTO5.value = "DT4011";
                            fieldDTO6.fieldName = "SUB_ASSET_FROM1I";
                            fieldDTO6.value = data.SUB_ASSET_NO; //fieldDTO6.value = "000001";
                            fieldDTO7.fieldName = "SUB_ASSET_TO1I";
                            fieldDTO7.value = data.SUB_ASSET_NO; //fieldDTO7.value = "000001";

                            //disatukan dalam list
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

                            Console.WriteLine("EXEC-MSM685A");

                            //Remove list
                            listInsert.Remove(fieldDTO0);
                            listInsert.Remove(fieldDTO1);
                            listInsert.Remove(fieldDTO2);
                            listInsert.Remove(fieldDTO3);
                            listInsert.Remove(fieldDTO4);
                            listInsert.Remove(fieldDTO5);
                            listInsert.Remove(fieldDTO6);
                            listInsert.Remove(fieldDTO7);

                            ScreenNameValueDTO fieldDTO8 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO9 = new ScreenNameValueDTO();

                            //input dengan parameter fieldname dan value
                            fieldDTO8.fieldName = "ACCT_PROFILE2I"; //fieldDTO8.value = "1";
                            fieldDTO8.value = dataAcctSub.ACCT_PROFILE;
                            fieldDTO9.fieldName = "DEPR_EXP_CODE2I"; //fieldDTO9.value = "1";
                            fieldDTO9.value = dataAcctSub.ACCT_PROFILE;

                            //disatukan dalam list
                            listInsert.Add(fieldDTO8);
                            listInsert.Add(fieldDTO9);

                            //submit data
                            screen_request.screenFields = listInsert.ToArray();
                            screen_request.screenKey = "1";
                            screen_DTO = service.submit(context, screen_request);

                            Console.WriteLine("EXEC-MSM685B");

                            if (screen_DTO.programName != "MSM685C")
                            {
                                screen_request.screenFields = null;
                                screen_request.screenKey = "1";
                                screen_DTO = service.submit(context, screen_request);
                            }

                            //Remove list
                            listInsert.Remove(fieldDTO8);
                            listInsert.Remove(fieldDTO9);

                            //ScreenNameValueDTO fieldDTO10 = new ScreenNameValueDTO();
                            //ScreenNameValueDTO fieldDTO11 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO12 = new ScreenNameValueDTO();

                            //input dengan parameter fieldname dan value
                            //fieldDTO10.fieldName = "DEPR_METHOD3I";
                            //fieldDTO10.value = "L";
                            //fieldDTO11.fieldName = "DEPR_RATE3I";
                            //fieldDTO11.value = "";
                            fieldDTO12.fieldName = "EST_MM_LIFE3I";
                            fieldDTO12.value = "12";

                            //disatukan dalam list
                            //listInsert.Add(fieldDTO10);
                            //listInsert.Add(fieldDTO11);
                            listInsert.Add(fieldDTO12);

                            //submit data
                            screen_request.screenFields = listInsert.ToArray();
                            //screen_request.screenFields = null;
                            screen_request.screenKey = "1";
                            screen_DTO = service.submit(context, screen_request);

                            //submit data
                            screen_request.screenFields = null;
                            screen_request.screenKey = "1";
                            screen_DTO = service.submit(context, screen_request);

                            Console.WriteLine("EXEC-MSM685C");

                            //Remove list
                            listInsert.Remove(fieldDTO12);

                            ScreenNameValueDTO fieldDTO13 = new ScreenNameValueDTO();
                            ScreenNameValueDTO fieldDTO14 = new ScreenNameValueDTO();

                            //input dengan parameter fieldname dan value
                            DateTime datereceivedsm = (DateTime)item.DATE_RECEIVED_SM;
                            fieldDTO13.fieldName = "XFER_DATE1I";
                            fieldDTO13.value = datereceivedsm.ToString("yyyyMMdd"); //"20220730";
                            fieldDTO14.fieldName = "XFER_PERCENT1I";
                            fieldDTO14.value = "100.00";

                            //disatukan dalam list
                            listInsert.Add(fieldDTO13);
                            listInsert.Add(fieldDTO14);

                            //submit data
                            screen_request.screenFields = listInsert.ToArray();
                            screen_request.screenKey = "1";
                            screen_DTO = service.submit(context, screen_request);

                            screen_request.screenFields = null;
                            screen_request.screenKey = "1";
                            screen_DTO = service.submit(context, screen_request);

                            Console.WriteLine("MSO687 SUCCESS");

                            // Kosongkan listInsert untuk penggunaan selanjutnya
                            listInsert.Clear();

                        }

                        //Console.WriteLine(item.ID); // Print the "Name" property of each object
                        //Console.WriteLine(item.EQUIP_NO); // Print the "Name" property of each object
                        //ubah flag menjadi 1
                        var cek = dbs.TBL_T_UPLOAD_JOB_MASTERs.FirstOrDefault(a => a.JOB_ID == item.JOB_MASTER_ID);
                        if (cek != null)
                        {
                            dbs.Refresh(RefreshMode.OverwriteCurrentValues, cek);
                            cek.POST_DATETIME = DateTime.Now;
                            cek.JOB_STATUS = 4; //4 = Success
                            cek.JOB_REMARK = "Success Created MSO687 " + item.EQUIP_NO;
                            dbs.SubmitChanges();
                        }

                        var cek2 = db.TBL_T_PPEs.FirstOrDefault(a => a.EQUIP_NO == item.EQUIP_NO);
                        if (cek2 != null)
                        {
                            // 1 = Success
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
                            dbs.Refresh(RefreshMode.OverwriteCurrentValues, cek);
                            cek.POST_DATETIME = DateTime.Now;
                            cek.JOB_STATUS = 6; //6 = Failed
                            cek.JOB_REMARK = "Failed Created " + item.EQUIP_NO + " - " + exMessage;
                            dbs.SubmitChanges();
                        }

                        var cek2 = db.TBL_T_PPEs.FirstOrDefault(a => a.EQUIP_NO == item.EQUIP_NO);
                        if (cek2 != null)
                        {
                            // 2 = Error
                            cek2.FLAG = 2;
                            db.SubmitChanges();
                        }

                        // You can also log the error here if needed
                        Console.WriteLine("Error occurred: " + exMessage);
                    }
                }
                //Console.WriteLine("Not the 27th of the month. Skipping execution.");
                //return;
            }

        }
        private static string AcakHurufBesarKecil(string input)
        {
            // Konversi string ke array karakter agar dapat diubah
            char[] characters = input.ToCharArray();

            // Menggunakan waktu saat ini sebagai seed untuk Random
            Random random = new Random(DateTime.Now.Millisecond);

            // Loop untuk mengacak setiap karakter
            for (int i = 0; i < characters.Length; i++)
            {
                // Jika karakter merupakan huruf (bukan angka atau simbol), maka acak besar atau kecilnya
                if (char.IsLetter(characters[i]))
                {
                    // Jika angka acak lebih besar dari 0.5, maka huruf menjadi besar
                    // Jika angka acak lebih kecil atau sama dengan 0.5, maka huruf menjadi kecil
                    characters[i] = random.NextDouble() > 0.5 ? char.ToUpper(characters[i]) : char.ToLower(characters[i]);
                }
            }

            // Kembalikan string hasil pengacakan
            return new string(characters);
        }
    }
}
