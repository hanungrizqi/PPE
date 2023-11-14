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
using System.Security.Cryptography;
using System.Data.SqlClient;

namespace MSO687_CONSOLEAPP
{
    class Program
    {
        static void Main(string[] args)
        {
            Execute();
            //Console.ReadLine(); // Keep the console application running
        }

        static void Execute()
        {

            Console.WriteLine("EXEC-MSO687");

            DB_PLANT_PPE_CONSOLEDataContext db = new DB_PLANT_PPE_CONSOLEDataContext();
            DB_MANAGEMENT_SERVICESDataContext dbs = new DB_MANAGEMENT_SERVICESDataContext();
            
            DateTime today = DateTime.Today;
            int currentDay = today.Day;
            int currentMonth = today.Month;

            if (currentDay == 26)
            {
                //var dataEquipment = db.VW_T_MSF687_TRASNFER_ASSETs.ToList();
                var dataEquipment = db.VW_T_MSF687_TRASNFER_ASSETs.Where(a => a.DATE_RECEIVED_SM.HasValue && a.DATE_RECEIVED_SM.Value.Day >= 16 && a.DATE_RECEIVED_SM.Value.Day <= 26).ToList();
                foreach (var item in dataEquipment)
                {
                    try
                    {
                        var dataList = db.VW_R_SUB_ASSET_BALANCE_SHEETs.Where(a => a.EQUIP_NO == item.EQUIP_NO).ToList();
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
                                var cekmanserv = dbs.TBL_T_UPLOAD_JOB_MASTERs.Where(x => x.JOB_ID.Contains(item.JOB_MASTER_ID)).FirstOrDefault();
                                if (cekmanserv == null)
                                {
                                    SqlConnection CONNECT = new SqlConnection(Properties.Settings.Default.DB_MANAGEMENT_SERVICES_KPTConnectionString);

                                    CONNECT.Open();
                                    var Query = "INSERT INTO TBL_T_UPLOAD_JOB_MASTER (JOB_ID, SERVER_ID, SCRIPT_CODE, ELLIPSE_USERNAME, ELLIPSE_PASSWORD, ELLIPSE_POSITION, ELLIPSE_DISTRICT, POST_DATETIME, JOB_STATUS, JOB_REMARK, DB_SERVER_NAME, DATABASE_NAME) " +
                                "VALUES ('NEWPPE-MSO687-BY-TEAM-KPP-2023', 'PRODUCTION', 'NEWPPE_MSO687', '" + str_username.ToString() + "', '" + str_password.ToString() + "', '" + str_posisi.ToString() + "', '" + item.DISTRICT_FROM + "', GETDATE(), 1, 'Login Ellipse', '10.14.101.181\\DMSPROD', 'DB_PLANT_PPE_NEW_KPP')";
                                    SqlCommand COMMAND = new SqlCommand(Query, CONNECT);
                                    COMMAND.ExecuteNonQuery();
                                    CONNECT.Close();
                                    conditionMet = true;
                                }
                                else
                                {
                                    SqlConnection CONNECT = new SqlConnection(Properties.Settings.Default.DB_MANAGEMENT_SERVICES_KPTConnectionString);

                                    CONNECT.Open();
                                    var Query = "Update TBL_T_UPLOAD_JOB_MASTER Set JOB_STATUS = 1, JOB_REMARK = 'Loggin Ellipse', POST_DATETIME = GETDATE() Where JOB_ID = '" + cekmanserv.JOB_ID.ToString() + "'";
                                    SqlCommand COMMAND = new SqlCommand(Query.ToString(), CONNECT);
                                    COMMAND.ExecuteNonQuery();
                                    CONNECT.Close();
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
                            //datereceivedsm = new DateTime(2022, datereceivedsm.Month, datereceivedsm.Day);
                            datereceivedsm = new DateTime(datereceivedsm.Year, datereceivedsm.Month, datereceivedsm.Day);

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
                            SqlConnection CONNECT = new SqlConnection(Properties.Settings.Default.DB_MANAGEMENT_SERVICES_KPTConnectionString);

                            CONNECT.Open();
                            var equipNo = item.EQUIP_NO;
                            var jobRemark = "Success Created MSO687 - " + equipNo;
                            var Query = "Update TBL_T_UPLOAD_JOB_MASTER Set JOB_STATUS = 4, JOB_REMARK = @JobRemark, POST_DATETIME = GETDATE() Where JOB_ID = '" + cek.JOB_ID.ToString() + "'";
                            SqlCommand COMMAND = new SqlCommand(Query.ToString(), CONNECT);
                            COMMAND.Parameters.AddWithValue("@JobRemark", jobRemark);
                            COMMAND.ExecuteNonQuery();
                            CONNECT.Close();

                        }

                        var cek2 = db.TBL_T_PPEs.FirstOrDefault(a => a.EQUIP_NO == item.EQUIP_NO);
                        if (cek2 != null)
                        {
                            SqlConnection CONNECT = new SqlConnection(Properties.Settings.Default.DB_PLANT_PPE_NEW_KPTConnectionString);

                            CONNECT.Open();
                            //var Query = "Update TBL_T_PPE Set FLAG = 1 Where EQUIP_NO = '" + item.EQUIP_NO.ToString() + "' AND PPE_NO = '" + item.PPE_NO.ToString() + "'";
                            var Query = "Update TBL_T_PPE Set FLAG = 1, FLAG_REMARKS = 'Success' Where EQUIP_NO = '" + item.EQUIP_NO.ToString() + "' AND PPE_NO = '" + item.PPE_NO.ToString() + "'";
                            SqlCommand COMMAND = new SqlCommand(Query.ToString(), CONNECT);
                            COMMAND.ExecuteNonQuery();
                            CONNECT.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        var exMessage = ex.Message;
                        var cek = dbs.TBL_T_UPLOAD_JOB_MASTERs.FirstOrDefault(a => a.JOB_ID == item.JOB_MASTER_ID);
                        if (cek != null)
                        {
                            SqlConnection CONNECT = new SqlConnection(Properties.Settings.Default.DB_MANAGEMENT_SERVICES_KPTConnectionString);

                            CONNECT.Open();
                            var equipNo = item.EQUIP_NO;
                            //var jobRemark = "Failed Created " + equipNo + " - " + exMessage;
                            var jobRemark = "Failed Created " + equipNo;
                            var Query = "Update TBL_T_UPLOAD_JOB_MASTER Set JOB_STATUS = 6, JOB_REMARK = @JobRemark, POST_DATETIME = GETDATE() Where JOB_ID = '" + cek.JOB_ID.ToString() + "'";
                            SqlCommand COMMAND = new SqlCommand(Query.ToString(), CONNECT);
                            COMMAND.Parameters.AddWithValue("@JobRemark", jobRemark);
                            COMMAND.ExecuteNonQuery();
                            CONNECT.Close();

                        }

                        var cek2 = db.TBL_T_PPEs.FirstOrDefault(a => a.EQUIP_NO == item.EQUIP_NO);
                        if (cek2 != null)
                        {
                            SqlConnection CONNECT = new SqlConnection(Properties.Settings.Default.DB_PLANT_PPE_NEW_KPTConnectionString);

                            CONNECT.Open();
                            var exs = "Failed : " + exMessage;
                            //var Query = "Update TBL_T_PPE Set FLAG = 2 Where EQUIP_NO = '" + item.EQUIP_NO.ToString() + "' AND PPE_NO = '" + item.PPE_NO.ToString() + "'";
                            var Query = "Update TBL_T_PPE Set FLAG = 2, FLAG_REMARKS = @exs Where EQUIP_NO = '" + item.EQUIP_NO.ToString() + "' AND PPE_NO = '" + item.PPE_NO.ToString() + "'";
                            SqlCommand COMMAND = new SqlCommand(Query.ToString(), CONNECT);
                            COMMAND.Parameters.AddWithValue("@exs", exs);
                            COMMAND.ExecuteNonQuery();
                            CONNECT.Close();
                        }
                        // You can also log the error here if needed
                        Console.WriteLine("Error occurred: " + exMessage);
                    }
                }
                //db.SubmitChanges();
            }
            else if (currentDay >= 16 && currentDay <= 25)
            {
                //do nothing cause data will execute on 26

                #region code lama yang langsung execute tanpa memperdulikan currentDay
                //var dataEquipment = db.VW_T_MSF687_TRASNFER_ASSETs.ToList();
                //foreach (var item in dataEquipment)
                //{
                //    try
                //    {
                //        var dataList = db.VW_R_SUB_ASSET_BALANCE_SHEETs.Where(a => a.EQUIP_NO == item.EQUIP_NO).ToList();
                //        foreach (var data in dataList)
                //        {

                //            ScreenService.ScreenService service = new ScreenService.ScreenService();
                //            ScreenService.OperationContext context = new ScreenService.OperationContext();
                //            ScreenService.ScreenDTO screen_DTO = new ScreenService.ScreenDTO();
                //            ScreenService.ScreenSubmitRequestDTO screen_request = new ScreenService.ScreenSubmitRequestDTO();

                //            string str_username = ConfigurationManager.AppSettings["username"].ToString();
                //            string str_password = ConfigurationManager.AppSettings["password"].ToString();
                //            string str_posisi = ConfigurationManager.AppSettings["pos_id"].ToString();

                //            string acak = AcakHurufBesarKecil(str_username);

                //            //login ellipse
                //            context.district = item.DISTRICT_FROM;
                //            context.position = str_posisi;
                //            ClientConversation.authenticate(acak, str_password);

                //            bool conditionMet = false;
                //            while (!conditionMet)
                //            {
                //                var cekmanserv = dbs.TBL_T_UPLOAD_JOB_MASTERs.Where(x => x.JOB_ID.Contains(item.JOB_MASTER_ID)).FirstOrDefault();
                //                if (cekmanserv == null)
                //                {
                //                    SqlConnection CONNECT = new SqlConnection(Properties.Settings.Default.DB_MANAGEMENT_SERVICES_KPTConnectionString);

                //                    CONNECT.Open();
                //                    var Query = "INSERT INTO TBL_T_UPLOAD_JOB_MASTER (JOB_ID, SERVER_ID, SCRIPT_CODE, ELLIPSE_USERNAME, ELLIPSE_PASSWORD, ELLIPSE_POSITION, ELLIPSE_DISTRICT, POST_DATETIME, JOB_STATUS, JOB_REMARK, DB_SERVER_NAME, DATABASE_NAME) " +
                //                "VALUES ('NEWPPE-MSO687-BY-TEAM-KPP-2023', 'TESTING', 'NEWPPE_MSO687', '" + str_username.ToString() + "', '" + str_password.ToString() + "', '" + str_posisi.ToString() + "', '" + item.DISTRICT_FROM + "', GETDATE(), 1, 'Login Ellipse', 'kphosq101\\shpol', 'DB_PLANT_PPE_NEW_KPT')";
                //                    SqlCommand COMMAND = new SqlCommand(Query, CONNECT);
                //                    COMMAND.ExecuteNonQuery();
                //                    CONNECT.Close();
                //                    conditionMet = true;
                //                }
                //                else
                //                {
                //                    SqlConnection CONNECT = new SqlConnection(Properties.Settings.Default.DB_MANAGEMENT_SERVICES_KPTConnectionString);

                //                    CONNECT.Open();
                //                    var Query = "Update TBL_T_UPLOAD_JOB_MASTER Set JOB_STATUS = 1, JOB_REMARK = 'Loggin Ellipse', POST_DATETIME = GETDATE() Where JOB_ID = '" + cekmanserv.JOB_ID.ToString() + "'";
                //                    SqlCommand COMMAND = new SqlCommand(Query.ToString(), CONNECT);
                //                    COMMAND.ExecuteNonQuery();
                //                    CONNECT.Close();
                //                    conditionMet = true;
                //                }
                //            }

                //            screen_DTO = service.executeScreen(context, "MSO687");

                //            while (screen_DTO.mapName != "MSM687A")
                //            {
                //                screen_request.screenFields = null;
                //                screen_request.screenKey = "4";
                //                screen_DTO = service.submit(context, screen_request);
                //            }

                //            List<ScreenNameValueDTO> listInsert = new List<ScreenNameValueDTO>();
                //            ScreenNameValueDTO fieldDTO0 = new ScreenNameValueDTO();
                //            ScreenNameValueDTO fieldDTO1 = new ScreenNameValueDTO();
                //            ScreenNameValueDTO fieldDTO2 = new ScreenNameValueDTO();
                //            ScreenNameValueDTO fieldDTO3 = new ScreenNameValueDTO();
                //            ScreenNameValueDTO fieldDTO4 = new ScreenNameValueDTO();
                //            ScreenNameValueDTO fieldDTO5 = new ScreenNameValueDTO();
                //            ScreenNameValueDTO fieldDTO6 = new ScreenNameValueDTO();
                //            ScreenNameValueDTO fieldDTO7 = new ScreenNameValueDTO();

                //            fieldDTO0.fieldName = "DSTRCT_CODE_FR1I";
                //            fieldDTO0.value = item.DISTRICT_FROM;
                //            fieldDTO1.fieldName = "DSTRCT_CODE_TO1I";
                //            fieldDTO1.value = item.DISTRICT_TO;
                //            fieldDTO2.fieldName = "EQP_ASSET_REFA1I";
                //            fieldDTO2.value = "E";
                //            fieldDTO3.fieldName = "EQP_ASSET_REFB1I";
                //            fieldDTO3.value = "E";
                //            fieldDTO4.fieldName = "EQP_AS_REF_FR1I";
                //            fieldDTO4.value = item.EQUIP_NO;
                //            fieldDTO5.fieldName = "EQP_AS_REF_TO1I";
                //            fieldDTO5.value = item.EQUIP_NO;
                //            fieldDTO6.fieldName = "SUB_ASSET_FROM1I";
                //            fieldDTO6.value = data.SUB_ASSET_NO;
                //            fieldDTO7.fieldName = "SUB_ASSET_TO1I";
                //            fieldDTO7.value = data.SUB_ASSET_NO;

                //            listInsert.Add(fieldDTO0);
                //            listInsert.Add(fieldDTO1);
                //            listInsert.Add(fieldDTO2);
                //            listInsert.Add(fieldDTO3);
                //            listInsert.Add(fieldDTO4);
                //            listInsert.Add(fieldDTO5);
                //            listInsert.Add(fieldDTO6);
                //            listInsert.Add(fieldDTO7);

                //            screen_request.screenFields = listInsert.ToArray();
                //            screen_request.screenKey = "1";
                //            screen_DTO = service.submit(context, screen_request);

                //            listInsert.Remove(fieldDTO0);
                //            listInsert.Remove(fieldDTO1);
                //            listInsert.Remove(fieldDTO2);
                //            listInsert.Remove(fieldDTO3);
                //            listInsert.Remove(fieldDTO4);
                //            listInsert.Remove(fieldDTO5);
                //            listInsert.Remove(fieldDTO6);
                //            listInsert.Remove(fieldDTO7);
                //            Console.WriteLine("MSM685A Success");

                //            var assetLocation = db.TBL_R_ASSET_LOCATIONs.Where(a => a.DSTRCT_CODE == item.DISTRICT_TO && a.EQUIPMENT_LOCATION == item.LOC_TO).FirstOrDefault();

                //            ScreenNameValueDTO fieldDTO8 = new ScreenNameValueDTO();
                //            ScreenNameValueDTO fieldDTO9 = new ScreenNameValueDTO();

                //            fieldDTO8.fieldName = "ACCT_PROFILE2I";
                //            fieldDTO8.value = data.ACCT_PROFILE;

                //            string profitloss = "DEPR_EXP_CODE2I";
                //            string values = "";
                //            switch (data.ACCT_PROFILE)
                //            {
                //                case "0001":
                //                    values = assetLocation.PRODUCTION_EQUIPMENT;
                //                    break;
                //                case "0003":
                //                    values = assetLocation.SUPPORT_EQUIPMENT;
                //                    break;
                //                case "0007":
                //                    values = assetLocation.WORKSHOP_EQUIPMENT;
                //                    break;
                //                default:
                //                    // Default value or action if none of the cases match
                //                    break;
                //            }

                //            fieldDTO9.fieldName = profitloss;
                //            fieldDTO9.value = values;

                //            listInsert.Add(fieldDTO8);
                //            listInsert.Add(fieldDTO9);

                //            screen_request.screenFields = listInsert.ToArray();
                //            screen_request.screenKey = "1";
                //            screen_DTO = service.submit(context, screen_request);

                //            while (screen_DTO.mapName != "MSM685C")
                //            {
                //                screen_request.screenFields = null;
                //                screen_request.screenKey = "1";
                //                screen_DTO = service.submit(context, screen_request);
                //            }

                //            listInsert.Remove(fieldDTO8);
                //            listInsert.Remove(fieldDTO9);
                //            Console.WriteLine("MSM685B Success");

                //            screen_request.screenFields = null;
                //            screen_request.screenKey = "1";
                //            screen_DTO = service.submit(context, screen_request);

                //            while (screen_DTO.mapName != "MSM687A")
                //            {
                //                screen_request.screenFields = null;
                //                screen_request.screenKey = "1";
                //                screen_DTO = service.submit(context, screen_request);
                //            }

                //            Console.WriteLine("MSM685C Success");

                //            ScreenNameValueDTO fieldDTO13 = new ScreenNameValueDTO();
                //            ScreenNameValueDTO fieldDTO14 = new ScreenNameValueDTO();

                //            DateTime datereceivedsm = (DateTime)item.DATE_RECEIVED_SM;
                //            //datereceivedsm = new DateTime(2022, datereceivedsm.Month, datereceivedsm.Day);
                //            datereceivedsm = new DateTime(datereceivedsm.Year, datereceivedsm.Month, datereceivedsm.Day);

                //            fieldDTO13.fieldName = "XFER_DATE1I";
                //            fieldDTO13.value = datereceivedsm.ToString("yyyyMMdd");
                //            fieldDTO14.fieldName = "XFER_PERCENT1I";
                //            fieldDTO14.value = "100.00";

                //            listInsert.Add(fieldDTO13);
                //            listInsert.Add(fieldDTO14);

                //            screen_request.screenFields = listInsert.ToArray();
                //            screen_request.screenKey = "1";
                //            screen_DTO = service.submit(context, screen_request);

                //            screen_request.screenFields = null;
                //            screen_request.screenKey = "1";
                //            screen_DTO = service.submit(context, screen_request);

                //            Console.WriteLine("MSO687 Done");

                //            // Kosongkan listInsert untuk penggunaan selanjutnya
                //            listInsert.Clear();

                //        }

                //        var cek = dbs.TBL_T_UPLOAD_JOB_MASTERs.FirstOrDefault(a => a.JOB_ID == item.JOB_MASTER_ID);
                //        if (cek != null)
                //        {
                //            SqlConnection CONNECT = new SqlConnection(Properties.Settings.Default.DB_MANAGEMENT_SERVICES_KPTConnectionString);

                //            CONNECT.Open();
                //            var equipNo = item.EQUIP_NO;
                //            var jobRemark = "Success Created MSO687 - " + equipNo;
                //            var Query = "Update TBL_T_UPLOAD_JOB_MASTER Set JOB_STATUS = 4, JOB_REMARK = @JobRemark, POST_DATETIME = GETDATE() Where JOB_ID = '" + cek.JOB_ID.ToString() + "'";
                //            SqlCommand COMMAND = new SqlCommand(Query.ToString(), CONNECT);
                //            COMMAND.Parameters.AddWithValue("@JobRemark", jobRemark);
                //            COMMAND.ExecuteNonQuery();
                //            CONNECT.Close();

                //        }

                //        var cek2 = db.TBL_T_PPEs.FirstOrDefault(a => a.EQUIP_NO == item.EQUIP_NO);
                //        if (cek2 != null)
                //        {
                //            SqlConnection CONNECT = new SqlConnection(Properties.Settings.Default.DB_PLANT_PPE_NEW_KPTConnectionString);

                //            CONNECT.Open();
                //            var Query = "Update TBL_T_PPE Set FLAG = 1 Where EQUIP_NO = '" + item.EQUIP_NO.ToString() + "' AND PPE_NO = '" + item.PPE_NO.ToString() + "'";
                //            SqlCommand COMMAND = new SqlCommand(Query.ToString(), CONNECT);
                //            COMMAND.ExecuteNonQuery();
                //            CONNECT.Close();
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        var exMessage = ex.Message;
                //        var cek = dbs.TBL_T_UPLOAD_JOB_MASTERs.FirstOrDefault(a => a.JOB_ID == item.JOB_MASTER_ID);
                //        if (cek != null)
                //        {
                //            SqlConnection CONNECT = new SqlConnection(Properties.Settings.Default.DB_MANAGEMENT_SERVICES_KPTConnectionString);

                //            CONNECT.Open();
                //            var equipNo = item.EQUIP_NO;
                //            //var jobRemark = "Failed Created " + equipNo + " - " + exMessage;
                //            var jobRemark = "Failed Created " + equipNo;
                //            var Query = "Update TBL_T_UPLOAD_JOB_MASTER Set JOB_STATUS = 6, JOB_REMARK = @JobRemark, POST_DATETIME = GETDATE() Where JOB_ID = '" + cek.JOB_ID.ToString() + "'";
                //            SqlCommand COMMAND = new SqlCommand(Query.ToString(), CONNECT);
                //            COMMAND.Parameters.AddWithValue("@JobRemark", jobRemark);
                //            COMMAND.ExecuteNonQuery();
                //            CONNECT.Close();

                //        }

                //        var cek2 = db.TBL_T_PPEs.FirstOrDefault(a => a.EQUIP_NO == item.EQUIP_NO);
                //        if (cek2 != null)
                //        {
                //            SqlConnection CONNECT = new SqlConnection(Properties.Settings.Default.DB_PLANT_PPE_NEW_KPTConnectionString);

                //            CONNECT.Open();
                //            var Query = "Update TBL_T_PPE Set FLAG = 2 Where EQUIP_NO = '" + item.EQUIP_NO.ToString() + "' AND PPE_NO = '" + item.PPE_NO.ToString() + "'";
                //            SqlCommand COMMAND = new SqlCommand(Query.ToString(), CONNECT);
                //            COMMAND.ExecuteNonQuery();
                //            CONNECT.Close();
                //        }
                //        // You can also log the error here if needed
                //        Console.WriteLine("Error occurred: " + exMessage);
                //    }
                //}
                ////db.SubmitChanges();
                #endregion
            }
            else
            {
                var dataEquipment = db.VW_T_MSF687_TRASNFER_ASSETs.ToList();
                foreach (var item in dataEquipment)
                {
                    try
                    {
                        var dataList = db.VW_R_SUB_ASSET_BALANCE_SHEETs.Where(a => a.EQUIP_NO == item.EQUIP_NO).ToList();
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
                                var cekmanserv = dbs.TBL_T_UPLOAD_JOB_MASTERs.Where(x => x.JOB_ID.Contains(item.JOB_MASTER_ID)).FirstOrDefault();
                                if (cekmanserv == null)
                                {
                                    SqlConnection CONNECT = new SqlConnection(Properties.Settings.Default.DB_MANAGEMENT_SERVICES_KPTConnectionString);

                                    CONNECT.Open();
                                    var Query = "INSERT INTO TBL_T_UPLOAD_JOB_MASTER (JOB_ID, SERVER_ID, SCRIPT_CODE, ELLIPSE_USERNAME, ELLIPSE_PASSWORD, ELLIPSE_POSITION, ELLIPSE_DISTRICT, POST_DATETIME, JOB_STATUS, JOB_REMARK, DB_SERVER_NAME, DATABASE_NAME) " +
                                "VALUES ('NEWPPE-MSO687-BY-TEAM-KPP-2023', 'PRODUCTION', 'NEWPPE_MSO687', '" + str_username.ToString() + "', '" + str_password.ToString() + "', '" + str_posisi.ToString() + "', '" + item.DISTRICT_FROM + "', GETDATE(), 1, 'Login Ellipse', '10.14.101.181\\DMSPROD', 'DB_PLANT_PPE_NEW_KPP')";
                                    SqlCommand COMMAND = new SqlCommand(Query, CONNECT);
                                    COMMAND.ExecuteNonQuery();
                                    CONNECT.Close();
                                    conditionMet = true;
                                }
                                else
                                {
                                    SqlConnection CONNECT = new SqlConnection(Properties.Settings.Default.DB_MANAGEMENT_SERVICES_KPTConnectionString);
                                    
                                    CONNECT.Open();
                                    var Query = "Update TBL_T_UPLOAD_JOB_MASTER Set JOB_STATUS = 1, JOB_REMARK = 'Loggin Ellipse', POST_DATETIME = GETDATE() Where JOB_ID = '" + cekmanserv.JOB_ID.ToString() + "'";
                                    SqlCommand COMMAND = new SqlCommand(Query.ToString(), CONNECT);
                                    COMMAND.ExecuteNonQuery();
                                    CONNECT.Close();
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
                            //datereceivedsm = new DateTime(2022, datereceivedsm.Month, datereceivedsm.Day);
                            datereceivedsm = new DateTime(datereceivedsm.Year, datereceivedsm.Month, datereceivedsm.Day);

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
                            SqlConnection CONNECT = new SqlConnection(Properties.Settings.Default.DB_MANAGEMENT_SERVICES_KPTConnectionString);

                            CONNECT.Open();
                            var equipNo = item.EQUIP_NO;
                            var jobRemark = "Success Created MSO687 - " + equipNo;
                            var Query = "Update TBL_T_UPLOAD_JOB_MASTER Set JOB_STATUS = 4, JOB_REMARK = @JobRemark, POST_DATETIME = GETDATE() Where JOB_ID = '" + cek.JOB_ID.ToString() + "'";
                            SqlCommand COMMAND = new SqlCommand(Query.ToString(), CONNECT);
                            COMMAND.Parameters.AddWithValue("@JobRemark", jobRemark);
                            COMMAND.ExecuteNonQuery();
                            CONNECT.Close();
                            
                        }

                        var cek2 = db.TBL_T_PPEs.FirstOrDefault(a => a.EQUIP_NO == item.EQUIP_NO);
                        if (cek2 != null)
                        {
                            SqlConnection CONNECT = new SqlConnection(Properties.Settings.Default.DB_PLANT_PPE_NEW_KPTConnectionString);

                            CONNECT.Open();
                            //var Query = "Update TBL_T_PPE Set FLAG = 1 Where EQUIP_NO = '" + item.EQUIP_NO.ToString() + "' AND PPE_NO = '" + item.PPE_NO.ToString() + "'";
                            var Query = "Update TBL_T_PPE Set FLAG = 1, FLAG_REMARKS = 'Success' Where EQUIP_NO = '" + item.EQUIP_NO.ToString() + "' AND PPE_NO = '" + item.PPE_NO.ToString() + "'";
                            SqlCommand COMMAND = new SqlCommand(Query.ToString(), CONNECT);
                            COMMAND.ExecuteNonQuery();
                            CONNECT.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        var exMessage = ex.Message;
                        var cek = dbs.TBL_T_UPLOAD_JOB_MASTERs.FirstOrDefault(a => a.JOB_ID == item.JOB_MASTER_ID);
                        if (cek != null)
                        {
                            SqlConnection CONNECT = new SqlConnection(Properties.Settings.Default.DB_MANAGEMENT_SERVICES_KPTConnectionString);

                            CONNECT.Open();
                            var equipNo = item.EQUIP_NO;
                            //var jobRemark = "Failed Created " + equipNo + " - " + exMessage;
                            var jobRemark = "Failed Created " + equipNo;
                            var Query = "Update TBL_T_UPLOAD_JOB_MASTER Set JOB_STATUS = 6, JOB_REMARK = @JobRemark, POST_DATETIME = GETDATE() Where JOB_ID = '" + cek.JOB_ID.ToString() + "'";
                            SqlCommand COMMAND = new SqlCommand(Query.ToString(), CONNECT);
                            COMMAND.Parameters.AddWithValue("@JobRemark", jobRemark);
                            COMMAND.ExecuteNonQuery();
                            CONNECT.Close();
                            
                        }

                        var cek2 = db.TBL_T_PPEs.FirstOrDefault(a => a.EQUIP_NO == item.EQUIP_NO);
                        if (cek2 != null)
                        {
                            SqlConnection CONNECT = new SqlConnection(Properties.Settings.Default.DB_PLANT_PPE_NEW_KPTConnectionString);

                            CONNECT.Open();
                            var exs = "Failed : " + exMessage;
                            //var Query = "Update TBL_T_PPE Set FLAG = 2 Where EQUIP_NO = '" + item.EQUIP_NO.ToString() + "' AND PPE_NO = '" + item.PPE_NO.ToString() + "'";
                            var Query = "Update TBL_T_PPE Set FLAG = 2, FLAG_REMARKS = @exs Where EQUIP_NO = '" + item.EQUIP_NO.ToString() + "' AND PPE_NO = '" + item.PPE_NO.ToString() + "'";
                            SqlCommand COMMAND = new SqlCommand(Query.ToString(), CONNECT);
                            COMMAND.Parameters.AddWithValue("@exs", exs);
                            COMMAND.ExecuteNonQuery();
                            CONNECT.Close();
                        }
                        // You can also log the error here if needed
                        Console.WriteLine("Error occurred: " + exMessage);
                    }
                }
                db.SubmitChanges();
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
