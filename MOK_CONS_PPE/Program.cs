using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Configuration;
using System.Runtime.ConstrainedExecution;
using MOK_CONS_PPE.EquipmentService;
using EllipseWebServicesClient;
using System.Data;

namespace MOK_CONS_PPE
{
    class Program
    {
        static void Main(string[] args)
        {
            string errormsg = "";
            #region Define Logging
            string NamaFile = DateTime.Now.ToString("yyyyMMddhhmmss");
            FileStream fs = new FileStream(Properties.Settings.Default.LogDir + NamaFile + ".txt", FileMode.Create);
            TextWriter tmp = Console.Out;
            StreamWriter sw = new StreamWriter(fs);
            #endregion

            Console.SetOut(sw);
            Console.WriteLine("Start Process At " + DateTime.Now.ToString());

            #region Get Job
            Console.WriteLine(DateTime.Now.ToString() + " : Get Job");
            SqlConnection DBJOB = new SqlConnection(Properties.Settings.Default.DB_DCTM_Integration);
            string Qry = "";
            string[] DataProcess = new string[9];
            try
            {
                DBJOB.Open();
                //Qry = "SELECT TOP 1 id, r_workflow_id, work_id, case when action = 0 then 1 else 0 end as action, task_name, doc_no, note, app_name, performer_login  FROM DCTM_Task_Integrator Where status = 8001 and app_name = 'Persetujuan Pemindahan Equipment'";
                Qry = "SELECT TOP 1 id, r_workflow_id, work_id, action, task_name, doc_no, note, app_name, performer_login  FROM DCTM_Task_Integrator Where status = 8001 and app_name = 'Persetujuan Pemindahan Equipment'";
                SqlCommand cmd = new SqlCommand(Qry.ToString(), DBJOB);
                SqlDataReader readerGetJob = cmd.ExecuteReader();
                while (readerGetJob.Read())
                {
                    DataProcess[0] = readerGetJob["id"].ToString();
                    DataProcess[1] = readerGetJob["r_workflow_id"].ToString();
                    DataProcess[2] = readerGetJob["work_id"].ToString();
                    DataProcess[3] = readerGetJob["action"].ToString();
                    DataProcess[4] = readerGetJob["task_name"].ToString();
                    DataProcess[5] = readerGetJob["doc_no"].ToString();
                    DataProcess[6] = readerGetJob["note"].ToString();
                    DataProcess[7] = readerGetJob["app_name"].ToString();
                    DataProcess[8] = readerGetJob["performer_login"].ToString();

                }
                DBJOB.Close();
                Console.WriteLine(DateTime.Now.ToString() + " : ID Job " + DataProcess[0].ToString());
                readerGetJob.Close();
            }
            catch (Exception e)
            {
                errormsg = "Chek Job Error : " + e.ToString();
                Console.WriteLine(DateTime.Now.ToString() + errormsg.ToString());
            }
            #endregion

            #region Update Status 8002
            try
            {
                DBJOB.Open();
                Qry = "Update DCTM_Task_Integrator Set status = 8002 where id = '" + DataProcess[0].ToString() + "'";
                SqlCommand cmd = new SqlCommand(Qry.ToString(), DBJOB);
                cmd.ExecuteNonQuery();
                DBJOB.Close();
                Console.WriteLine(DateTime.Now.ToString() + " : Update Status 8002");

            }
            catch (Exception e)
            {
                errormsg = "Error Update Status 8002 : " + e.ToString();
                Console.WriteLine(DateTime.Now.ToString() + errormsg.ToString());
            }
            #endregion

            #region Execute
            try
            {
                Console.WriteLine(DateTime.Now.ToString() + " : Store Procedure Executing..");

                if (DataProcess[3].ToString().Trim() == "1") //jika reject
                {
                    SqlConnection CONNECT_RJ = new SqlConnection(Properties.Settings.Default.DB_PLANT_PPE_NEW_KPT);
                    CONNECT_RJ.Open();

                    var updateQuery = "UPDATE TBL_T_PPE SET STATUS = 'REJECT', UPDATED_DATE = GETDATE(), UPDATED_BY = @UpdatedBy, REMARKS = @Remarks WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";

                    SqlCommand rejectCommand = new SqlCommand(updateQuery, CONNECT_RJ);
                    rejectCommand.Parameters.AddWithValue("@UpdatedBy", DataProcess[8]);
                    rejectCommand.Parameters.AddWithValue("@Remarks", DataProcess[6]);
                    rejectCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                    rejectCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                    Int32 rowsrejectAffected = rejectCommand.ExecuteNonQuery();
                    Console.WriteLine(DateTime.Now.ToString() + " :RowsAffected: {0}", rowsrejectAffected);

                    CONNECT_RJ.Close();
                }
                else
                {
                    SqlConnection CONNECT = new SqlConnection(Properties.Settings.Default.DB_PLANT_PPE_NEW_KPT);
                    CONNECT.Open();
                    if (DataProcess[1].ToString().Trim() == "1") //jika approval_order 1 | posisi = sect.head
                    {

                        var ppeNo = DataProcess[5];
                        var url = "http://10.14.101.181/ReportServer_RPTPROD?/PPE/Rpt_PPE_PlantManager&PPE_NO=" + ppeNo;

                        string selectDistrictQuery = "SELECT DISTRICT_FROM FROM TBL_T_PPE WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";
                        SqlCommand selectDistrictCommand = new SqlCommand(selectDistrictQuery, CONNECT);
                        selectDistrictCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                        selectDistrictCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                        string district = selectDistrictCommand.ExecuteScalar()?.ToString();
                        if (district != "KPHO")
                        {
                            var updateQuery = "UPDATE TBL_T_PPE SET APPROVAL_ORDER = 2, POSISI_PPE = 'Plant Manager', STATUS = 'SEC.HEAD APPROVED', UPDATED_DATE = GETDATE(), UPDATED_BY = @UpdatedBy, REMARKS = @Remarks, URL_FORM_PLNTMNGR = @URL WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";

                            SqlCommand updateCommand = new SqlCommand(updateQuery, CONNECT);
                            updateCommand.Parameters.AddWithValue("@UpdatedBy", DataProcess[8]);
                            updateCommand.Parameters.AddWithValue("@Remarks", DataProcess[6]);
                            updateCommand.Parameters.AddWithValue("@URL", url);
                            updateCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                            updateCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                            Int32 rowsAffected = updateCommand.ExecuteNonQuery();
                            Console.WriteLine(DateTime.Now.ToString() + " :RowsAffected: {0}", rowsAffected);

                            // SELECT data from TBL_T_PPE
                            var selectQuery = "SELECT * FROM TBL_T_PPE WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";
                            SqlCommand selectCommand = new SqlCommand(selectQuery, CONNECT);
                            selectCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                            selectCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                            using (SqlDataReader reader = selectCommand.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    var equipNo = reader["EQUIP_NO"].ToString();
                                    reader.Close();
                                    // Insert data into TBL_H_APPROVAL_PPE
                                    var insertQuery = "INSERT INTO TBL_H_APPROVAL_PPE (Ppe_NO, Equip_No, Posisi_Ppe, Approval_Order, Approved_Date, Approved_By) VALUES (@Ppe_NO, @Equip_No, 'Sect. Head', 2, GETDATE(), @UpdatedBy)";
                                    SqlCommand insertCommand = new SqlCommand(insertQuery, CONNECT);
                                    insertCommand.Parameters.AddWithValue("@Ppe_NO", DataProcess[5]);
                                    insertCommand.Parameters.AddWithValue("@Equip_No", equipNo);
                                    insertCommand.Parameters.AddWithValue("@UpdatedBy", DataProcess[8]);

                                    Int32 insertRowsAffected = insertCommand.ExecuteNonQuery();
                                    Console.WriteLine(DateTime.Now.ToString() + " :Insert RowsAffected: {0}", insertRowsAffected);
                                }
                                else
                                {
                                    Console.WriteLine("No data found for PPE_NO: " + DataProcess[5]);
                                }
                            }
                            // send mail
                            try
                            {
                                SqlConnection DBPPE = new SqlConnection(Properties.Settings.Default.DB_PLANT_PPE_NEW_KPT);
                                DBPPE.Open();
                                SqlCommand cmdSP = new SqlCommand("cusp_insertNotifEmail_PlantManager", DBPPE);
                                cmdSP.CommandType = System.Data.CommandType.StoredProcedure;
                                cmdSP.Parameters.Add("@PPE_NO", System.Data.SqlDbType.VarChar, 20).Value = DataProcess[5].ToString();

                                //cmdSP.ExecuteNonQuery();
                                Int32 sprowsAffected = cmdSP.ExecuteNonQuery();
                                Console.WriteLine(DateTime.Now.ToString() + " :RowsAffected: {0}", sprowsAffected);
                                DBPPE.Close();
                            }
                            catch (Exception ex)
                            {
                                errormsg = "Error Send Mail : " + ex.ToString();
                            }
                        }
                        else
                        {
                            var updateQuery = "UPDATE TBL_T_PPE SET APPROVAL_ORDER = 2, POSISI_PPE = 'Plant Adm & Dev Manager', STATUS = 'SEC.HEAD APPROVED', UPDATED_DATE = GETDATE(), UPDATED_BY = @UpdatedBy, REMARKS = @Remarks WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";

                            SqlCommand updateCommand = new SqlCommand(updateQuery, CONNECT);
                            updateCommand.Parameters.AddWithValue("@UpdatedBy", DataProcess[8]);
                            updateCommand.Parameters.AddWithValue("@Remarks", DataProcess[6]);
                            updateCommand.Parameters.AddWithValue("@URL", url);
                            updateCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                            updateCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                            Int32 rowsAffected = updateCommand.ExecuteNonQuery();
                            Console.WriteLine(DateTime.Now.ToString() + " :RowsAffected: {0}", rowsAffected);

                            // SELECT data from TBL_T_PPE
                            var selectQuery = "SELECT * FROM TBL_T_PPE WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";
                            SqlCommand selectCommand = new SqlCommand(selectQuery, CONNECT);
                            selectCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                            selectCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                            using (SqlDataReader reader = selectCommand.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    var equipNo = reader["EQUIP_NO"].ToString();
                                    reader.Close();
                                    // Insert data into TBL_H_APPROVAL_PPE
                                    var insertQuery = "INSERT INTO TBL_H_APPROVAL_PPE (Ppe_NO, Equip_No, Posisi_Ppe, Approval_Order, Approved_Date, Approved_By) VALUES (@Ppe_NO, @Equip_No, 'Sect. Head', 2, GETDATE(), @UpdatedBy)";
                                    SqlCommand insertCommand = new SqlCommand(insertQuery, CONNECT);
                                    insertCommand.Parameters.AddWithValue("@Ppe_NO", DataProcess[5]);
                                    insertCommand.Parameters.AddWithValue("@Equip_No", equipNo);
                                    insertCommand.Parameters.AddWithValue("@UpdatedBy", DataProcess[8]);

                                    Int32 insertRowsAffected = insertCommand.ExecuteNonQuery();
                                    Console.WriteLine(DateTime.Now.ToString() + " :Insert RowsAffected: {0}", insertRowsAffected);
                                }
                                else
                                {
                                    Console.WriteLine("No data found for PPE_NO: " + DataProcess[5]);
                                }
                            }
                            // send mail
                            try
                            {
                                SqlConnection DBPPE = new SqlConnection(Properties.Settings.Default.DB_PLANT_PPE_NEW_KPT);
                                DBPPE.Open();
                                SqlCommand cmdSP = new SqlCommand("cusp_insertNotifEmail_PlantAdmDevManager", DBPPE);
                                cmdSP.CommandType = System.Data.CommandType.StoredProcedure;
                                cmdSP.Parameters.Add("@PPE_NO", System.Data.SqlDbType.VarChar, 20).Value = DataProcess[5].ToString();

                                //cmdSP.ExecuteNonQuery();
                                Int32 sprowsAffected = cmdSP.ExecuteNonQuery();
                                Console.WriteLine(DateTime.Now.ToString() + " :RowsAffected: {0}", sprowsAffected);
                                DBPPE.Close();
                            }
                            catch (Exception ex)
                            {
                                errormsg = "Error Send Mail : " + ex.ToString();
                            }
                        }
                    }
                    else if (DataProcess[1].ToString().Trim() == "2") //jika approval_order 2 | posisi = plant manager/adm
                    {
                        var ppeNo = DataProcess[5];
                        var url = "http://10.14.101.181/ReportServer_RPTPROD?/PPE/Rpt_PPE_PlantDeptHead&PPE_NO=" + ppeNo;

                        string selectDistrictQuery = "SELECT DISTRICT_FROM FROM TBL_T_PPE WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";
                        SqlCommand selectDistrictCommand = new SqlCommand(selectDistrictQuery, CONNECT);
                        selectDistrictCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                        selectDistrictCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                        string district = selectDistrictCommand.ExecuteScalar()?.ToString();
                        if (district != "KPHO")
                        {
                            var updateQuery = "UPDATE TBL_T_PPE SET APPROVAL_ORDER = 3, POSISI_PPE = 'Plant Dept. Head', STATUS = 'PLANT MANAGER APPROVED', UPDATED_DATE = GETDATE(), UPDATED_BY = @UpdatedBy, REMARKS = @Remarks, URL_FORM_PLNTDH = @URL WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";

                            SqlCommand updateCommand = new SqlCommand(updateQuery, CONNECT);
                            updateCommand.Parameters.AddWithValue("@UpdatedBy", DataProcess[8]);
                            updateCommand.Parameters.AddWithValue("@Remarks", DataProcess[6]);
                            updateCommand.Parameters.AddWithValue("@URL", url);
                            updateCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                            updateCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                            Int32 rowsAffected = updateCommand.ExecuteNonQuery();
                            Console.WriteLine(DateTime.Now.ToString() + " :RowsAffected: {0}", rowsAffected);

                            // SELECT data from TBL_T_PPE
                            var selectQuery = "SELECT * FROM TBL_T_PPE WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";
                            SqlCommand selectCommand = new SqlCommand(selectQuery, CONNECT);
                            selectCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                            selectCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                            using (SqlDataReader reader = selectCommand.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    var equipNo = reader["EQUIP_NO"].ToString();
                                    reader.Close();
                                    // Insert data into TBL_H_APPROVAL_PPE
                                    var insertQuery = "INSERT INTO TBL_H_APPROVAL_PPE (Ppe_NO, Equip_No, Posisi_Ppe, Approval_Order, Approved_Date, Approved_By) VALUES (@Ppe_NO, @Equip_No, 'Plant Manager', 3, GETDATE(), @UpdatedBy)";
                                    SqlCommand insertCommand = new SqlCommand(insertQuery, CONNECT);
                                    insertCommand.Parameters.AddWithValue("@Ppe_NO", DataProcess[5]);
                                    insertCommand.Parameters.AddWithValue("@Equip_No", equipNo);
                                    insertCommand.Parameters.AddWithValue("@UpdatedBy", DataProcess[8]);

                                    Int32 insertRowsAffected = insertCommand.ExecuteNonQuery();
                                    Console.WriteLine(DateTime.Now.ToString() + " :Insert RowsAffected: {0}", insertRowsAffected);
                                }
                                else
                                {
                                    Console.WriteLine("No data found for PPE_NO: " + DataProcess[5]);
                                }
                            }
                            // send mail
                            try
                            {
                                SqlConnection DBPPE = new SqlConnection(Properties.Settings.Default.DB_PLANT_PPE_NEW_KPT);
                                DBPPE.Open();
                                SqlCommand cmdSP = new SqlCommand("cusp_insertNotifEmail_PlantDeptHead", DBPPE);
                                cmdSP.CommandType = System.Data.CommandType.StoredProcedure;
                                cmdSP.Parameters.Add("@PPE_NO", System.Data.SqlDbType.VarChar, 20).Value = DataProcess[5].ToString();

                                //cmdSP.ExecuteNonQuery();
                                Int32 sprowsAffected = cmdSP.ExecuteNonQuery();
                                Console.WriteLine(DateTime.Now.ToString() + " :RowsAffected: {0}", sprowsAffected);
                                DBPPE.Close();
                            }
                            catch (Exception ex)
                            {
                                errormsg = "Error Send Mail : " + ex.ToString();
                            }
                        }
                        else
                        {
                            var urls = "http://10.14.101.181/ReportServer_RPTPROD?/PPE/Rpt_PPE_PlantManager&PPE_NO=" + ppeNo;
                            var updateQuery = "UPDATE TBL_T_PPE SET APPROVAL_ORDER = 3, POSISI_PPE = 'Plant Manager', STATUS = 'PLANT ADM & DEV MANAGER APPROVED', UPDATED_DATE = GETDATE(), UPDATED_BY = @UpdatedBy, REMARKS = @Remarks, URL_FORM_PLNTMNGR = @URL WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";

                            SqlCommand updateCommand = new SqlCommand(updateQuery, CONNECT);
                            updateCommand.Parameters.AddWithValue("@UpdatedBy", DataProcess[8]);
                            updateCommand.Parameters.AddWithValue("@Remarks", DataProcess[6]);
                            updateCommand.Parameters.AddWithValue("@URL", urls);
                            updateCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                            updateCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                            Int32 rowsAffected = updateCommand.ExecuteNonQuery();
                            Console.WriteLine(DateTime.Now.ToString() + " :RowsAffected: {0}", rowsAffected);

                            // SELECT data from TBL_T_PPE
                            var selectQuery = "SELECT * FROM TBL_T_PPE WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";
                            SqlCommand selectCommand = new SqlCommand(selectQuery, CONNECT);
                            selectCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                            selectCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                            using (SqlDataReader reader = selectCommand.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    var equipNo = reader["EQUIP_NO"].ToString();
                                    reader.Close();
                                    // Insert data into TBL_H_APPROVAL_PPE
                                    var insertQuery = "INSERT INTO TBL_H_APPROVAL_PPE (Ppe_NO, Equip_No, Posisi_Ppe, Approval_Order, Approved_Date, Approved_By) VALUES (@Ppe_NO, @Equip_No, 'Plant Adm & Dev Manager', 3, GETDATE(), @UpdatedBy)";
                                    SqlCommand insertCommand = new SqlCommand(insertQuery, CONNECT);
                                    insertCommand.Parameters.AddWithValue("@Ppe_NO", DataProcess[5]);
                                    insertCommand.Parameters.AddWithValue("@Equip_No", equipNo);
                                    insertCommand.Parameters.AddWithValue("@UpdatedBy", DataProcess[8]);

                                    Int32 insertRowsAffected = insertCommand.ExecuteNonQuery();
                                    Console.WriteLine(DateTime.Now.ToString() + " :Insert RowsAffected: {0}", insertRowsAffected);
                                }
                                else
                                {
                                    Console.WriteLine("No data found for PPE_NO: " + DataProcess[5]);
                                }
                            }
                            // send mail
                            try
                            {
                                SqlConnection DBPPE = new SqlConnection(Properties.Settings.Default.DB_PLANT_PPE_NEW_KPT);
                                DBPPE.Open();
                                SqlCommand cmdSP = new SqlCommand("cusp_insertNotifEmail_PlantManager", DBPPE);
                                cmdSP.CommandType = System.Data.CommandType.StoredProcedure;
                                cmdSP.Parameters.Add("@PPE_NO", System.Data.SqlDbType.VarChar, 20).Value = DataProcess[5].ToString();

                                //cmdSP.ExecuteNonQuery();
                                Int32 sprowsAffected = cmdSP.ExecuteNonQuery();
                                Console.WriteLine(DateTime.Now.ToString() + " :RowsAffected: {0}", sprowsAffected);
                                DBPPE.Close();
                            }
                            catch (Exception ex)
                            {
                                errormsg = "Error Send Mail : " + ex.ToString();
                            }
                        }
                        
                    }
                    else if (DataProcess[1].ToString().Trim() == "3") //jika approval_order 3 | posisi = plant dept head
                    {
                        var ppeNo = DataProcess[5];
                        var url = "http://10.14.101.181/ReportServer_RPTPROD?/PPE/Rpt_PPE_PMPengirim&PPE_NO=" + ppeNo;

                        string selectDistrictQuery = "SELECT DISTRICT_FROM FROM TBL_T_PPE WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";
                        SqlCommand selectDistrictCommand = new SqlCommand(selectDistrictQuery, CONNECT);
                        selectDistrictCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                        selectDistrictCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                        string district = selectDistrictCommand.ExecuteScalar()?.ToString();
                        if (district != "KPHO")
                        {
                            var updateQuery = "UPDATE TBL_T_PPE SET APPROVAL_ORDER = 4, POSISI_PPE = 'Project Manager Pengirim', STATUS = 'PLANT DEPT. HEAD APPROVED', UPDATED_DATE = GETDATE(), UPDATED_BY = @UpdatedBy, REMARKS = @Remarks, URL_FORM_PM_PENGIRIM = @URL WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";

                            SqlCommand updateCommand = new SqlCommand(updateQuery, CONNECT);
                            updateCommand.Parameters.AddWithValue("@UpdatedBy", DataProcess[8]);
                            updateCommand.Parameters.AddWithValue("@Remarks", DataProcess[6]);
                            updateCommand.Parameters.AddWithValue("@URL", url);
                            updateCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                            updateCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                            Int32 rowsAffected = updateCommand.ExecuteNonQuery();
                            Console.WriteLine(DateTime.Now.ToString() + " :RowsAffected: {0}", rowsAffected);

                            // SELECT data from TBL_T_PPE
                            var selectQuery = "SELECT * FROM TBL_T_PPE WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";
                            SqlCommand selectCommand = new SqlCommand(selectQuery, CONNECT);
                            selectCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                            selectCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                            using (SqlDataReader reader = selectCommand.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    var equipNo = reader["EQUIP_NO"].ToString();
                                    reader.Close();
                                    // Insert data into TBL_H_APPROVAL_PPE
                                    var insertQuery = "INSERT INTO TBL_H_APPROVAL_PPE (Ppe_NO, Equip_No, Posisi_Ppe, Approval_Order, Approved_Date, Approved_By) VALUES (@Ppe_NO, @Equip_No, 'Plant Dept. Head', 4, GETDATE(), @UpdatedBy)";
                                    SqlCommand insertCommand = new SqlCommand(insertQuery, CONNECT);
                                    insertCommand.Parameters.AddWithValue("@Ppe_NO", DataProcess[5]);
                                    insertCommand.Parameters.AddWithValue("@Equip_No", equipNo);
                                    insertCommand.Parameters.AddWithValue("@UpdatedBy", DataProcess[8]);

                                    Int32 insertRowsAffected = insertCommand.ExecuteNonQuery();
                                    Console.WriteLine(DateTime.Now.ToString() + " :Insert RowsAffected: {0}", insertRowsAffected);
                                }
                                else
                                {
                                    Console.WriteLine("No data found for PPE_NO: " + DataProcess[5]);
                                }
                            }
                            // send mail
                            try
                            {
                                SqlConnection DBPPE = new SqlConnection(Properties.Settings.Default.DB_PLANT_PPE_NEW_KPT);
                                DBPPE.Open();
                                SqlCommand cmdSP = new SqlCommand("cusp_insertNotifEmail_PMPengirim", DBPPE);
                                cmdSP.CommandType = System.Data.CommandType.StoredProcedure;
                                cmdSP.Parameters.Add("@PPE_NO", System.Data.SqlDbType.VarChar, 20).Value = DataProcess[5].ToString();

                                //cmdSP.ExecuteNonQuery();
                                Int32 sprowsAffected = cmdSP.ExecuteNonQuery();
                                Console.WriteLine(DateTime.Now.ToString() + " :RowsAffected: {0}", sprowsAffected);
                                DBPPE.Close();
                            }
                            catch (Exception ex)
                            {
                                errormsg = "Error Send Mail : " + ex.ToString();
                            }
                        }
                        else
                        {
                            var urls = "http://10.14.101.181/ReportServer_RPTPROD?/PPE/Rpt_PPE_PMPenerima&PPE_NO=" + ppeNo;
                            var updateQuery = "UPDATE TBL_T_PPE SET APPROVAL_ORDER = 4, POSISI_PPE = 'Project Manager Penerima', STATUS = 'PLANT MANAGER APPROVED', UPDATED_DATE = GETDATE(), UPDATED_BY = @UpdatedBy, REMARKS = @Remarks, URL_FORM_PM_PENERIMA = @URL WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";

                            SqlCommand updateCommand = new SqlCommand(updateQuery, CONNECT);
                            updateCommand.Parameters.AddWithValue("@UpdatedBy", DataProcess[8]);
                            updateCommand.Parameters.AddWithValue("@Remarks", DataProcess[6]);
                            updateCommand.Parameters.AddWithValue("@URL", urls);
                            updateCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                            updateCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                            Int32 rowsAffected = updateCommand.ExecuteNonQuery();
                            Console.WriteLine(DateTime.Now.ToString() + " :RowsAffected: {0}", rowsAffected);

                            // SELECT data from TBL_T_PPE
                            var selectQuery = "SELECT * FROM TBL_T_PPE WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";
                            SqlCommand selectCommand = new SqlCommand(selectQuery, CONNECT);
                            selectCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                            selectCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                            using (SqlDataReader reader = selectCommand.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    var equipNo = reader["EQUIP_NO"].ToString();
                                    reader.Close();
                                    // Insert data into TBL_H_APPROVAL_PPE
                                    var insertQuery = "INSERT INTO TBL_H_APPROVAL_PPE (Ppe_NO, Equip_No, Posisi_Ppe, Approval_Order, Approved_Date, Approved_By) VALUES (@Ppe_NO, @Equip_No, 'Plant Manager', 4, GETDATE(), @UpdatedBy)";
                                    SqlCommand insertCommand = new SqlCommand(insertQuery, CONNECT);
                                    insertCommand.Parameters.AddWithValue("@Ppe_NO", DataProcess[5]);
                                    insertCommand.Parameters.AddWithValue("@Equip_No", equipNo);
                                    insertCommand.Parameters.AddWithValue("@UpdatedBy", DataProcess[8]);

                                    Int32 insertRowsAffected = insertCommand.ExecuteNonQuery();
                                    Console.WriteLine(DateTime.Now.ToString() + " :Insert RowsAffected: {0}", insertRowsAffected);
                                }
                                else
                                {
                                    Console.WriteLine("No data found for PPE_NO: " + DataProcess[5]);
                                }
                            }
                            // send mail
                            try
                            {
                                SqlConnection DBPPE = new SqlConnection(Properties.Settings.Default.DB_PLANT_PPE_NEW_KPT);
                                DBPPE.Open();
                                SqlCommand cmdSP = new SqlCommand("cusp_insertNotifEmail_PMPenerima", DBPPE);
                                cmdSP.CommandType = System.Data.CommandType.StoredProcedure;
                                cmdSP.Parameters.Add("@PPE_NO", System.Data.SqlDbType.VarChar, 20).Value = DataProcess[5].ToString();

                                //cmdSP.ExecuteNonQuery();
                                Int32 sprowsAffected = cmdSP.ExecuteNonQuery();
                                Console.WriteLine(DateTime.Now.ToString() + " :RowsAffected: {0}", sprowsAffected);
                                DBPPE.Close();
                            }
                            catch (Exception ex)
                            {
                                errormsg = "Error Send Mail : " + ex.ToString();
                            }
                        }
                        
                    }
                    else if (DataProcess[1].ToString().Trim() == "4") //jika approval_order 4 | posisi = pm pengirim
                    {
                        var ppeNo = DataProcess[5];
                        var url = "http://10.14.101.181/ReportServer_RPTPROD?/PPE/Rpt_PPE_PMPenerima&PPE_NO=" + ppeNo;

                        string selectDistrictQuery = "SELECT DISTRICT_FROM, DISTRICT_TO FROM TBL_T_PPE WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";
                        SqlCommand selectDistrictCommand = new SqlCommand(selectDistrictQuery, CONNECT);
                        selectDistrictCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                        selectDistrictCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                        SqlDataReader districtReader = selectDistrictCommand.ExecuteReader();
                        string districtFrom = "";
                        string districtTo = "";

                        if (districtReader.Read())
                        {
                            districtFrom = districtReader["DISTRICT_FROM"].ToString();
                            districtTo = districtReader["DISTRICT_TO"].ToString();
                        }
                        districtReader.Close();

                        if (districtFrom != "KPHO")
                        {
                            //distrik to
                            if (districtTo != "KPHO") //jika distrik tujuan bukan kpho
                            {
                                var updateQuery = "UPDATE TBL_T_PPE SET APPROVAL_ORDER = 5, POSISI_PPE = 'Project Manager Penerima', STATUS = 'PM PENGIRIM APPROVED', UPDATED_DATE = GETDATE(), UPDATED_BY = @UpdatedBy, REMARKS = @Remarks, URL_FORM_PM_PENERIMA = @URL WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";

                                SqlCommand updateCommand = new SqlCommand(updateQuery, CONNECT);
                                updateCommand.Parameters.AddWithValue("@UpdatedBy", DataProcess[8]);
                                updateCommand.Parameters.AddWithValue("@Remarks", DataProcess[6]);
                                updateCommand.Parameters.AddWithValue("@URL", url);
                                updateCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                                updateCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                                Int32 rowsAffected = updateCommand.ExecuteNonQuery();
                                Console.WriteLine(DateTime.Now.ToString() + " :RowsAffected: {0}", rowsAffected);

                                // SELECT data from TBL_T_PPE
                                var selectQuery = "SELECT * FROM TBL_T_PPE WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";
                                SqlCommand selectCommand = new SqlCommand(selectQuery, CONNECT);
                                selectCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                                selectCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                                using (SqlDataReader reader = selectCommand.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        var equipNo = reader["EQUIP_NO"].ToString();
                                        reader.Close();
                                        // Insert data into TBL_H_APPROVAL_PPE
                                        var insertQuery = "INSERT INTO TBL_H_APPROVAL_PPE (Ppe_NO, Equip_No, Posisi_Ppe, Approval_Order, Approved_Date, Approved_By) VALUES (@Ppe_NO, @Equip_No, 'Project Manager Pengirim', 5, GETDATE(), @UpdatedBy)";
                                        SqlCommand insertCommand = new SqlCommand(insertQuery, CONNECT);
                                        insertCommand.Parameters.AddWithValue("@Ppe_NO", DataProcess[5]);
                                        insertCommand.Parameters.AddWithValue("@Equip_No", equipNo);
                                        insertCommand.Parameters.AddWithValue("@UpdatedBy", DataProcess[8]);

                                        Int32 insertRowsAffected = insertCommand.ExecuteNonQuery();
                                        Console.WriteLine(DateTime.Now.ToString() + " :Insert RowsAffected: {0}", insertRowsAffected);
                                    }
                                    else
                                    {
                                        Console.WriteLine("No data found for PPE_NO: " + DataProcess[5]);
                                    }
                                }
                                // send mail
                                try
                                {
                                    SqlConnection DBPPE = new SqlConnection(Properties.Settings.Default.DB_PLANT_PPE_NEW_KPT);
                                    DBPPE.Open();
                                    SqlCommand cmdSP = new SqlCommand("cusp_insertNotifEmail_PMPenerima", DBPPE);
                                    cmdSP.CommandType = System.Data.CommandType.StoredProcedure;
                                    cmdSP.Parameters.Add("@PPE_NO", System.Data.SqlDbType.VarChar, 20).Value = DataProcess[5].ToString();

                                    //cmdSP.ExecuteNonQuery();
                                    Int32 sprowsAffected = cmdSP.ExecuteNonQuery();
                                    Console.WriteLine(DateTime.Now.ToString() + " :RowsAffected: {0}", sprowsAffected);
                                    DBPPE.Close();
                                }
                                catch (Exception ex)
                                {
                                    errormsg = "Error Send Mail : " + ex.ToString();
                                }
                            }
                            else //jika distrik tujuan = kpho
                            {
                                var urls = "http://10.14.101.181/ReportServer_RPTPROD?/PPE/Rpt_PPE_DivHead_Eng&PPE_NO=" + ppeNo;
                                var updateQuery = "UPDATE TBL_T_PPE SET APPROVAL_ORDER = 5, POSISI_PPE = 'Division Head ENG', STATUS = 'PM PENGIRIM APPROVED', UPDATED_DATE = GETDATE(), UPDATED_BY = @UpdatedBy, REMARKS = @Remarks, URL_FORM_DIVHEAD_ENG = @URL WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";

                                SqlCommand updateCommand = new SqlCommand(updateQuery, CONNECT);
                                updateCommand.Parameters.AddWithValue("@UpdatedBy", DataProcess[8]);
                                updateCommand.Parameters.AddWithValue("@Remarks", DataProcess[6]);
                                updateCommand.Parameters.AddWithValue("@URL", urls);
                                updateCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                                updateCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                                Int32 rowsAffected = updateCommand.ExecuteNonQuery();
                                Console.WriteLine(DateTime.Now.ToString() + " :RowsAffected: {0}", rowsAffected);

                                // SELECT data from TBL_T_PPE
                                var selectQuery = "SELECT * FROM TBL_T_PPE WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";
                                SqlCommand selectCommand = new SqlCommand(selectQuery, CONNECT);
                                selectCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                                selectCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                                using (SqlDataReader reader = selectCommand.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        var equipNo = reader["EQUIP_NO"].ToString();
                                        reader.Close();
                                        // Insert data into TBL_H_APPROVAL_PPE
                                        var insertQuery = "INSERT INTO TBL_H_APPROVAL_PPE (Ppe_NO, Equip_No, Posisi_Ppe, Approval_Order, Approved_Date, Approved_By) VALUES (@Ppe_NO, @Equip_No, 'Project Manager Pengirim', 5, GETDATE(), @UpdatedBy)";
                                        SqlCommand insertCommand = new SqlCommand(insertQuery, CONNECT);
                                        insertCommand.Parameters.AddWithValue("@Ppe_NO", DataProcess[5]);
                                        insertCommand.Parameters.AddWithValue("@Equip_No", equipNo);
                                        insertCommand.Parameters.AddWithValue("@UpdatedBy", DataProcess[8]);

                                        Int32 insertRowsAffected = insertCommand.ExecuteNonQuery();
                                        Console.WriteLine(DateTime.Now.ToString() + " :Insert RowsAffected: {0}", insertRowsAffected);
                                    }
                                    else
                                    {
                                        Console.WriteLine("No data found for PPE_NO: " + DataProcess[5]);
                                    }
                                }
                                // send mail
                                try
                                {
                                    SqlConnection DBPPE = new SqlConnection(Properties.Settings.Default.DB_PLANT_PPE_NEW_KPT);
                                    DBPPE.Open();
                                    SqlCommand cmdSP = new SqlCommand("cusp_insertNotifEmail_Divhead_Eng", DBPPE);
                                    cmdSP.CommandType = System.Data.CommandType.StoredProcedure;
                                    cmdSP.Parameters.Add("@PPE_NO", System.Data.SqlDbType.VarChar, 20).Value = DataProcess[5].ToString();

                                    //cmdSP.ExecuteNonQuery();
                                    Int32 sprowsAffected = cmdSP.ExecuteNonQuery();
                                    Console.WriteLine(DateTime.Now.ToString() + " :RowsAffected: {0}", sprowsAffected);
                                    DBPPE.Close();
                                }
                                catch (Exception ex)
                                {
                                    errormsg = "Error Send Mail : " + ex.ToString();
                                }
                            }
                        }
                        else
                        {
                            var urls = "http://10.14.101.181/ReportServer_RPTPROD?/PPE/Rpt_PPE_DivHead_Eng&PPE_NO=" + ppeNo;
                            var updateQuery = "UPDATE TBL_T_PPE SET APPROVAL_ORDER = 5, POSISI_PPE = 'Division Head ENG', STATUS = 'PM PENERIMA APPROVED', UPDATED_DATE = GETDATE(), UPDATED_BY = @UpdatedBy, REMARKS = @Remarks, URL_FORM_DIVHEAD_ENG = @URL WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";

                            SqlCommand updateCommand = new SqlCommand(updateQuery, CONNECT);
                            updateCommand.Parameters.AddWithValue("@UpdatedBy", DataProcess[8]);
                            updateCommand.Parameters.AddWithValue("@Remarks", DataProcess[6]);
                            updateCommand.Parameters.AddWithValue("@URL", urls);
                            updateCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                            updateCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                            Int32 rowsAffected = updateCommand.ExecuteNonQuery();
                            Console.WriteLine(DateTime.Now.ToString() + " :RowsAffected: {0}", rowsAffected);

                            // SELECT data from TBL_T_PPE
                            var selectQuery = "SELECT * FROM TBL_T_PPE WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";
                            SqlCommand selectCommand = new SqlCommand(selectQuery, CONNECT);
                            selectCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                            selectCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                            using (SqlDataReader reader = selectCommand.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    var equipNo = reader["EQUIP_NO"].ToString();
                                    reader.Close();
                                    // Insert data into TBL_H_APPROVAL_PPE
                                    var insertQuery = "INSERT INTO TBL_H_APPROVAL_PPE (Ppe_NO, Equip_No, Posisi_Ppe, Approval_Order, Approved_Date, Approved_By) VALUES (@Ppe_NO, @Equip_No, 'Project Manager Penerima', 5, GETDATE(), @UpdatedBy)";
                                    SqlCommand insertCommand = new SqlCommand(insertQuery, CONNECT);
                                    insertCommand.Parameters.AddWithValue("@Ppe_NO", DataProcess[5]);
                                    insertCommand.Parameters.AddWithValue("@Equip_No", equipNo);
                                    insertCommand.Parameters.AddWithValue("@UpdatedBy", DataProcess[8]);

                                    Int32 insertRowsAffected = insertCommand.ExecuteNonQuery();
                                    Console.WriteLine(DateTime.Now.ToString() + " :Insert RowsAffected: {0}", insertRowsAffected);
                                }
                                else
                                {
                                    Console.WriteLine("No data found for PPE_NO: " + DataProcess[5]);
                                }
                            }
                            // send mail
                            try
                            {
                                SqlConnection DBPPE = new SqlConnection(Properties.Settings.Default.DB_PLANT_PPE_NEW_KPT);
                                DBPPE.Open();
                                SqlCommand cmdSP = new SqlCommand("cusp_insertNotifEmail_Divhead_Eng", DBPPE);
                                cmdSP.CommandType = System.Data.CommandType.StoredProcedure;
                                cmdSP.Parameters.Add("@PPE_NO", System.Data.SqlDbType.VarChar, 20).Value = DataProcess[5].ToString();

                                //cmdSP.ExecuteNonQuery();
                                Int32 sprowsAffected = cmdSP.ExecuteNonQuery();
                                Console.WriteLine(DateTime.Now.ToString() + " :RowsAffected: {0}", sprowsAffected);
                                DBPPE.Close();
                            }
                            catch (Exception ex)
                            {
                                errormsg = "Error Send Mail : " + ex.ToString();
                            }
                        }

                    }
                    else if (DataProcess[1].ToString().Trim() == "5") //jika approval_order 5 | posisi = pm penerima
                    {
                        var ppeNo = DataProcess[5];
                        var url = "http://10.14.101.181/ReportServer_RPTPROD?/PPE/Rpt_PPE_DivHead_Eng&PPE_NO=" + ppeNo;

                        string selectDistrictQuery = "SELECT DISTRICT_FROM, DISTRICT_TO FROM TBL_T_PPE WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";
                        SqlCommand selectDistrictCommand = new SqlCommand(selectDistrictQuery, CONNECT);
                        selectDistrictCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                        selectDistrictCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                        SqlDataReader districtReader = selectDistrictCommand.ExecuteReader();
                        string districtFrom = "";
                        string districtTo = "";

                        if (districtReader.Read())
                        {
                            districtFrom = districtReader["DISTRICT_FROM"].ToString();
                            districtTo = districtReader["DISTRICT_TO"].ToString();
                        }
                        districtReader.Close();

                        if (districtFrom != "KPHO")
                        {
                            //distrik to
                            if (districtTo != "KPHO") //jika disrtik to bukan kpho
                            {
                                var updateQuery = "UPDATE TBL_T_PPE SET APPROVAL_ORDER = 6, POSISI_PPE = 'Division Head ENG', STATUS = 'PM PENERIMA APPROVED', UPDATED_DATE = GETDATE(), UPDATED_BY = @UpdatedBy, REMARKS = @Remarks, URL_FORM_DIVHEAD_ENG = @URL WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";

                                SqlCommand updateCommand = new SqlCommand(updateQuery, CONNECT);
                                updateCommand.Parameters.AddWithValue("@UpdatedBy", DataProcess[8]);
                                updateCommand.Parameters.AddWithValue("@Remarks", DataProcess[6]);
                                updateCommand.Parameters.AddWithValue("@URL", url);
                                updateCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                                updateCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                                Int32 rowsAffected = updateCommand.ExecuteNonQuery();
                                Console.WriteLine(DateTime.Now.ToString() + " :RowsAffected: {0}", rowsAffected);

                                // SELECT data from TBL_T_PPE
                                var selectQuery = "SELECT * FROM TBL_T_PPE WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";
                                SqlCommand selectCommand = new SqlCommand(selectQuery, CONNECT);
                                selectCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                                selectCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                                using (SqlDataReader reader = selectCommand.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        var equipNo = reader["EQUIP_NO"].ToString();
                                        reader.Close();
                                        // Insert data into TBL_H_APPROVAL_PPE
                                        var insertQuery = "INSERT INTO TBL_H_APPROVAL_PPE (Ppe_NO, Equip_No, Posisi_Ppe, Approval_Order, Approved_Date, Approved_By) VALUES (@Ppe_NO, @Equip_No, 'Project Manager Penerima', 6, GETDATE(), @UpdatedBy)";
                                        SqlCommand insertCommand = new SqlCommand(insertQuery, CONNECT);
                                        insertCommand.Parameters.AddWithValue("@Ppe_NO", DataProcess[5]);
                                        insertCommand.Parameters.AddWithValue("@Equip_No", equipNo);
                                        insertCommand.Parameters.AddWithValue("@UpdatedBy", DataProcess[8]);

                                        Int32 insertRowsAffected = insertCommand.ExecuteNonQuery();
                                        Console.WriteLine(DateTime.Now.ToString() + " :Insert RowsAffected: {0}", insertRowsAffected);
                                    }
                                    else
                                    {
                                        Console.WriteLine("No data found for PPE_NO: " + DataProcess[5]);
                                    }
                                }
                                // send mail
                                try
                                {
                                    SqlConnection DBPPE = new SqlConnection(Properties.Settings.Default.DB_PLANT_PPE_NEW_KPT);
                                    DBPPE.Open();
                                    SqlCommand cmdSP = new SqlCommand("cusp_insertNotifEmail_Divhead_Eng", DBPPE);
                                    cmdSP.CommandType = System.Data.CommandType.StoredProcedure;
                                    cmdSP.Parameters.Add("@PPE_NO", System.Data.SqlDbType.VarChar, 20).Value = DataProcess[5].ToString();

                                    //cmdSP.ExecuteNonQuery();
                                    Int32 sprowsAffected = cmdSP.ExecuteNonQuery();
                                    Console.WriteLine(DateTime.Now.ToString() + " :RowsAffected: {0}", sprowsAffected);
                                    DBPPE.Close();
                                }
                                catch (Exception ex)
                                {
                                    errormsg = "Error Send Mail : " + ex.ToString();
                                }
                            }
                            else //jika distrik to = kpho
                            {
                                var urls = "http://10.14.101.181/ReportServer_RPTPROD?/PPE/Rpt_PPE_DivHead_Opr&PPE_NO=" + ppeNo;
                                var updateQuery = "UPDATE TBL_T_PPE SET APPROVAL_ORDER = 7, POSISI_PPE = 'Division Head OPR', STATUS = 'DIVISION HEAD ENG APPROVED', UPDATED_DATE = GETDATE(), UPDATED_BY = @UpdatedBy, REMARKS = @Remarks, URL_FORM_DIVHEAD_OPR = @URL WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";

                                SqlCommand updateCommand = new SqlCommand(updateQuery, CONNECT);
                                updateCommand.Parameters.AddWithValue("@UpdatedBy", DataProcess[8]);
                                updateCommand.Parameters.AddWithValue("@Remarks", DataProcess[6]);
                                updateCommand.Parameters.AddWithValue("@URL", urls);
                                updateCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                                updateCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                                Int32 rowsAffected = updateCommand.ExecuteNonQuery();
                                Console.WriteLine(DateTime.Now.ToString() + " :RowsAffected: {0}", rowsAffected);

                                // SELECT data from TBL_T_PPE
                                var selectQuery = "SELECT * FROM TBL_T_PPE WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";
                                SqlCommand selectCommand = new SqlCommand(selectQuery, CONNECT);
                                selectCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                                selectCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                                using (SqlDataReader reader = selectCommand.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        var equipNo = reader["EQUIP_NO"].ToString();
                                        reader.Close();
                                        // Insert data into TBL_H_APPROVAL_PPE
                                        var insertQuery = "INSERT INTO TBL_H_APPROVAL_PPE (Ppe_NO, Equip_No, Posisi_Ppe, Approval_Order, Approved_Date, Approved_By) VALUES (@Ppe_NO, @Equip_No, 'Division Head ENG', 7, GETDATE(), @UpdatedBy)";
                                        SqlCommand insertCommand = new SqlCommand(insertQuery, CONNECT);
                                        insertCommand.Parameters.AddWithValue("@Ppe_NO", DataProcess[5]);
                                        insertCommand.Parameters.AddWithValue("@Equip_No", equipNo);
                                        insertCommand.Parameters.AddWithValue("@UpdatedBy", DataProcess[8]);

                                        Int32 insertRowsAffected = insertCommand.ExecuteNonQuery();
                                        Console.WriteLine(DateTime.Now.ToString() + " :Insert RowsAffected: {0}", insertRowsAffected);
                                    }
                                    else
                                    {
                                        Console.WriteLine("No data found for PPE_NO: " + DataProcess[5]);
                                    }
                                }
                                // send mail
                                try
                                {
                                    SqlConnection DBPPE = new SqlConnection(Properties.Settings.Default.DB_PLANT_PPE_NEW_KPT);
                                    DBPPE.Open();
                                    SqlCommand cmdSP = new SqlCommand("cusp_insertNotifEmail_Divhead_Opr", DBPPE);
                                    cmdSP.CommandType = System.Data.CommandType.StoredProcedure;
                                    cmdSP.Parameters.Add("@PPE_NO", System.Data.SqlDbType.VarChar, 20).Value = DataProcess[5].ToString();

                                    //cmdSP.ExecuteNonQuery();
                                    Int32 sprowsAffected = cmdSP.ExecuteNonQuery();
                                    Console.WriteLine(DateTime.Now.ToString() + " :RowsAffected: {0}", sprowsAffected);
                                    DBPPE.Close();
                                }
                                catch (Exception ex)
                                {
                                    errormsg = "Error Send Mail : " + ex.ToString();
                                }
                            }
                        }
                        else
                        {
                            var urls = "http://10.14.101.181/ReportServer_RPTPROD?/PPE/Rpt_PPE_DivHead_Opr&PPE_NO=" + ppeNo;
                            var updateQuery = "UPDATE TBL_T_PPE SET APPROVAL_ORDER = 6, POSISI_PPE = 'Division Head OPR', STATUS = 'DIVISION HEAD ENG APPROVED', UPDATED_DATE = GETDATE(), UPDATED_BY = @UpdatedBy, REMARKS = @Remarks, URL_FORM_DIVHEAD_OPR = @URL WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";

                            SqlCommand updateCommand = new SqlCommand(updateQuery, CONNECT);
                            updateCommand.Parameters.AddWithValue("@UpdatedBy", DataProcess[8]);
                            updateCommand.Parameters.AddWithValue("@Remarks", DataProcess[6]);
                            updateCommand.Parameters.AddWithValue("@URL", urls);
                            updateCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                            updateCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                            Int32 rowsAffected = updateCommand.ExecuteNonQuery();
                            Console.WriteLine(DateTime.Now.ToString() + " :RowsAffected: {0}", rowsAffected);

                            // SELECT data from TBL_T_PPE
                            var selectQuery = "SELECT * FROM TBL_T_PPE WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";
                            SqlCommand selectCommand = new SqlCommand(selectQuery, CONNECT);
                            selectCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                            selectCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                            using (SqlDataReader reader = selectCommand.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    var equipNo = reader["EQUIP_NO"].ToString();
                                    reader.Close();
                                    // Insert data into TBL_H_APPROVAL_PPE
                                    var insertQuery = "INSERT INTO TBL_H_APPROVAL_PPE (Ppe_NO, Equip_No, Posisi_Ppe, Approval_Order, Approved_Date, Approved_By) VALUES (@Ppe_NO, @Equip_No, 'Division Head ENG', 6, GETDATE(), @UpdatedBy)";
                                    SqlCommand insertCommand = new SqlCommand(insertQuery, CONNECT);
                                    insertCommand.Parameters.AddWithValue("@Ppe_NO", DataProcess[5]);
                                    insertCommand.Parameters.AddWithValue("@Equip_No", equipNo);
                                    insertCommand.Parameters.AddWithValue("@UpdatedBy", DataProcess[8]);

                                    Int32 insertRowsAffected = insertCommand.ExecuteNonQuery();
                                    Console.WriteLine(DateTime.Now.ToString() + " :Insert RowsAffected: {0}", insertRowsAffected);
                                }
                                else
                                {
                                    Console.WriteLine("No data found for PPE_NO: " + DataProcess[5]);
                                }
                            }
                            // send mail
                            try
                            {
                                SqlConnection DBPPE = new SqlConnection(Properties.Settings.Default.DB_PLANT_PPE_NEW_KPT);
                                DBPPE.Open();
                                SqlCommand cmdSP = new SqlCommand("cusp_insertNotifEmail_Divhead_Opr", DBPPE);
                                cmdSP.CommandType = System.Data.CommandType.StoredProcedure;
                                cmdSP.Parameters.Add("@PPE_NO", System.Data.SqlDbType.VarChar, 20).Value = DataProcess[5].ToString();

                                //cmdSP.ExecuteNonQuery();
                                Int32 sprowsAffected = cmdSP.ExecuteNonQuery();
                                Console.WriteLine(DateTime.Now.ToString() + " :RowsAffected: {0}", sprowsAffected);
                                DBPPE.Close();
                            }
                            catch (Exception ex)
                            {
                                errormsg = "Error Send Mail : " + ex.ToString();
                            }
                        }
                        
                    }
                    else if (DataProcess[1].ToString().Trim() == "6") //jika approval_order 6 | posisi divhead eng
                    {
                        var ppeNo = DataProcess[5];
                        var url = "http://10.14.101.181/ReportServer_RPTPROD?/PPE/Rpt_PPE_DivHead_Opr&PPE_NO=" + ppeNo;

                        string selectDistrictQuery = "SELECT DISTRICT_FROM FROM TBL_T_PPE WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";
                        SqlCommand selectDistrictCommand = new SqlCommand(selectDistrictQuery, CONNECT);
                        selectDistrictCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                        selectDistrictCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                        string district = selectDistrictCommand.ExecuteScalar()?.ToString();
                        if (district != "KPHO")
                        {
                            var updateQuery = "UPDATE TBL_T_PPE SET APPROVAL_ORDER = 7, POSISI_PPE = 'Division Head OPR', STATUS = 'DIVISION HEAD ENG APPROVED', UPDATED_DATE = GETDATE(), UPDATED_BY = @UpdatedBy, REMARKS = @Remarks, URL_FORM_DIVHEAD_OPR = @URL WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";

                            SqlCommand updateCommand = new SqlCommand(updateQuery, CONNECT);
                            updateCommand.Parameters.AddWithValue("@UpdatedBy", DataProcess[8]);
                            updateCommand.Parameters.AddWithValue("@Remarks", DataProcess[6]);
                            updateCommand.Parameters.AddWithValue("@URL", url);
                            updateCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                            updateCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                            Int32 rowsAffected = updateCommand.ExecuteNonQuery();
                            Console.WriteLine(DateTime.Now.ToString() + " :RowsAffected: {0}", rowsAffected);

                            // SELECT data from TBL_T_PPE
                            var selectQuery = "SELECT * FROM TBL_T_PPE WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";
                            SqlCommand selectCommand = new SqlCommand(selectQuery, CONNECT);
                            selectCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                            selectCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                            using (SqlDataReader reader = selectCommand.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    var equipNo = reader["EQUIP_NO"].ToString();
                                    reader.Close();
                                    // Insert data into TBL_H_APPROVAL_PPE
                                    var insertQuery = "INSERT INTO TBL_H_APPROVAL_PPE (Ppe_NO, Equip_No, Posisi_Ppe, Approval_Order, Approved_Date, Approved_By) VALUES (@Ppe_NO, @Equip_No, 'Division Head ENG', 7, GETDATE(), @UpdatedBy)";
                                    SqlCommand insertCommand = new SqlCommand(insertQuery, CONNECT);
                                    insertCommand.Parameters.AddWithValue("@Ppe_NO", DataProcess[5]);
                                    insertCommand.Parameters.AddWithValue("@Equip_No", equipNo);
                                    insertCommand.Parameters.AddWithValue("@UpdatedBy", DataProcess[8]);

                                    Int32 insertRowsAffected = insertCommand.ExecuteNonQuery();
                                    Console.WriteLine(DateTime.Now.ToString() + " :Insert RowsAffected: {0}", insertRowsAffected);
                                }
                                else
                                {
                                    Console.WriteLine("No data found for PPE_NO: " + DataProcess[5]);
                                }
                            }
                            // send mail
                            try
                            {
                                SqlConnection DBPPE = new SqlConnection(Properties.Settings.Default.DB_PLANT_PPE_NEW_KPT);
                                DBPPE.Open();
                                SqlCommand cmdSP = new SqlCommand("cusp_insertNotifEmail_Divhead_Opr", DBPPE);
                                cmdSP.CommandType = System.Data.CommandType.StoredProcedure;
                                cmdSP.Parameters.Add("@PPE_NO", System.Data.SqlDbType.VarChar, 20).Value = DataProcess[5].ToString();

                                //cmdSP.ExecuteNonQuery();
                                Int32 sprowsAffected = cmdSP.ExecuteNonQuery();
                                Console.WriteLine(DateTime.Now.ToString() + " :RowsAffected: {0}", sprowsAffected);
                                DBPPE.Close();
                            }
                            catch (Exception ex)
                            {
                                errormsg = "Error Send Mail : " + ex.ToString();
                            }
                        }
                        else
                        {
                            var urls = "http://10.14.101.181/ReportServer_RPTPROD?/PPE/Rpt_PPE_DONE_BARU&PPE_NO=" + ppeNo;
                            var updateQuery = "UPDATE TBL_T_PPE SET APPROVAL_ORDER = 8, POSISI_PPE = 'Waiting SM Dept', STATUS = 'DIVISION HEAD OPR APPROVED', UPDATED_DATE = GETDATE(), UPDATED_BY = @UpdatedBy, REMARKS = @Remarks, URL_FORM_DONE = @URL WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";

                            SqlCommand updateCommand = new SqlCommand(updateQuery, CONNECT);
                            updateCommand.Parameters.AddWithValue("@UpdatedBy", DataProcess[8]);
                            updateCommand.Parameters.AddWithValue("@Remarks", DataProcess[6]);
                            updateCommand.Parameters.AddWithValue("@URL", urls);
                            updateCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                            updateCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                            Int32 rowsAffected = updateCommand.ExecuteNonQuery();
                            Console.WriteLine(DateTime.Now.ToString() + " :RowsAffected: {0}", rowsAffected);

                            // SELECT data from TBL_T_PPE
                            var selectQuery = "SELECT * FROM TBL_T_PPE WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";
                            SqlCommand selectCommand = new SqlCommand(selectQuery, CONNECT);
                            selectCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                            selectCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                            using (SqlDataReader reader = selectCommand.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    var equipNo = reader["EQUIP_NO"].ToString();
                                    reader.Close();
                                    // Insert data into TBL_H_APPROVAL_PPE
                                    var insertQuery = "INSERT INTO TBL_H_APPROVAL_PPE (Ppe_NO, Equip_No, Posisi_Ppe, Approval_Order, Approved_Date, Approved_By) VALUES (@Ppe_NO, @Equip_No, 'Division Head OPR', 8, GETDATE(), @UpdatedBy)";
                                    SqlCommand insertCommand = new SqlCommand(insertQuery, CONNECT);
                                    insertCommand.Parameters.AddWithValue("@Ppe_NO", DataProcess[5]);
                                    insertCommand.Parameters.AddWithValue("@Equip_No", equipNo);
                                    insertCommand.Parameters.AddWithValue("@UpdatedBy", DataProcess[8]);

                                    Int32 insertRowsAffected = insertCommand.ExecuteNonQuery();
                                    Console.WriteLine(DateTime.Now.ToString() + " :Insert RowsAffected: {0}", insertRowsAffected);
                                }
                                else
                                {
                                    Console.WriteLine("No data found for PPE_NO: " + DataProcess[5]);
                                }
                            }
                            // send mail
                            try
                            {
                                SqlConnection DBPPE = new SqlConnection(Properties.Settings.Default.DB_PLANT_PPE_NEW_KPT);
                                DBPPE.Open();
                                SqlCommand cmdSP = new SqlCommand("cusp_insertNotifEmail_PPE_Done", DBPPE);
                                cmdSP.CommandType = System.Data.CommandType.StoredProcedure;
                                cmdSP.Parameters.Add("@PPE_NO", System.Data.SqlDbType.VarChar, 20).Value = DataProcess[5].ToString();

                                //cmdSP.ExecuteNonQuery();
                                Int32 sprowsAffected = cmdSP.ExecuteNonQuery();
                                Console.WriteLine(DateTime.Now.ToString() + " :RowsAffected: {0}", sprowsAffected);
                                DBPPE.Close();
                            }
                            catch (Exception ex)
                            {
                                errormsg = "Error Send Mail : " + ex.ToString();
                            }

                            //executeEllipse();
                            string ppeNos = DataProcess[5];
                            string idPpeSubstrings = DataProcess[2];
                            executeEllipse(ppeNos, idPpeSubstrings);
                        }
                        
                    }
                    else if (DataProcess[1].ToString().Trim() == "7") //jika approval_order 7 | posisi = divhead opr
                    {
                        var ppeNo = DataProcess[5];
                        var url = "http://10.14.101.181/ReportServer_RPTPROD?/PPE/Rpt_PPE_DONE_BARU&PPE_NO=" + ppeNo;

                        var updateQuery = "UPDATE TBL_T_PPE SET APPROVAL_ORDER = 8, POSISI_PPE = 'Waiting SM Dept', STATUS = 'DIVISION HEAD OPR APPROVED', UPDATED_DATE = GETDATE(), UPDATED_BY = @UpdatedBy, REMARKS = @Remarks, URL_FORM_DONE = @URL WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";

                        SqlCommand updateCommand = new SqlCommand(updateQuery, CONNECT);
                        updateCommand.Parameters.AddWithValue("@UpdatedBy", DataProcess[8]);
                        updateCommand.Parameters.AddWithValue("@Remarks", DataProcess[6]);
                        updateCommand.Parameters.AddWithValue("@URL", url);
                        updateCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                        updateCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                        Int32 rowsAffected = updateCommand.ExecuteNonQuery();
                        Console.WriteLine(DateTime.Now.ToString() + " :RowsAffected: {0}", rowsAffected);

                        // SELECT data from TBL_T_PPE
                        var selectQuery = "SELECT * FROM TBL_T_PPE WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";
                        SqlCommand selectCommand = new SqlCommand(selectQuery, CONNECT);
                        selectCommand.Parameters.AddWithValue("@PPE_NO", DataProcess[5]);
                        selectCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", DataProcess[2]);

                        using (SqlDataReader reader = selectCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var equipNo = reader["EQUIP_NO"].ToString();
                                reader.Close();
                                // Insert data into TBL_H_APPROVAL_PPE
                                var insertQuery = "INSERT INTO TBL_H_APPROVAL_PPE (Ppe_NO, Equip_No, Posisi_Ppe, Approval_Order, Approved_Date, Approved_By) VALUES (@Ppe_NO, @Equip_No, 'Division Head OPR', 8, GETDATE(), @UpdatedBy)";
                                SqlCommand insertCommand = new SqlCommand(insertQuery, CONNECT);
                                insertCommand.Parameters.AddWithValue("@Ppe_NO", DataProcess[5]);
                                insertCommand.Parameters.AddWithValue("@Equip_No", equipNo);
                                insertCommand.Parameters.AddWithValue("@UpdatedBy", DataProcess[8]);

                                Int32 insertRowsAffected = insertCommand.ExecuteNonQuery();
                                Console.WriteLine(DateTime.Now.ToString() + " :Insert RowsAffected: {0}", insertRowsAffected);
                            }
                            else
                            {
                                Console.WriteLine("No data found for PPE_NO: " + DataProcess[5]);
                            }
                        }
                        // send mail
                        try
                        {
                            SqlConnection DBPPE = new SqlConnection(Properties.Settings.Default.DB_PLANT_PPE_NEW_KPT);
                            DBPPE.Open();
                            SqlCommand cmdSP = new SqlCommand("cusp_insertNotifEmail_PPE_Done", DBPPE);
                            cmdSP.CommandType = System.Data.CommandType.StoredProcedure;
                            cmdSP.Parameters.Add("@PPE_NO", System.Data.SqlDbType.VarChar, 20).Value = DataProcess[5].ToString();

                            //cmdSP.ExecuteNonQuery();
                            Int32 sprowsAffected = cmdSP.ExecuteNonQuery();
                            Console.WriteLine(DateTime.Now.ToString() + " :RowsAffected: {0}", sprowsAffected);
                            DBPPE.Close();
                        }
                        catch (Exception ex)
                        {
                            errormsg = "Error Send Mail : " + ex.ToString();
                        }
                        string ppeNos = DataProcess[5];
                        string idPpeSubstrings = DataProcess[2];
                        executeEllipse(ppeNos, idPpeSubstrings);
                    }
                    CONNECT.Close();
                }
                
            }
            catch (Exception e)
            {
                Qry = "Update DCTM_Task_Integrator Set status = 8004, finish_date = getdate(), result = 'Action Falied in Execute SP' where id = '" + DataProcess[0].ToString() + "'";
                try
                {
                    DBJOB.Open();
                    SqlCommand cmd = new SqlCommand(Qry, DBJOB);
                    cmd.ExecuteNonQuery();
                    DBJOB.Close();
                }
                catch (Exception ex)
                {
                    errormsg = "Error Update Status 8004 : " + ex.ToString();
                    Console.WriteLine(DateTime.Now.ToString() + errormsg.ToString());
                }

                errormsg = " : Error Execute Store Procedure : " + e.ToString();
                Console.WriteLine(DateTime.Now.ToString() + errormsg.ToString());
            }
            #endregion

            #region Update Status 8003
            try
            {
                DBJOB.Open();
                Qry = "Update DCTM_Task_Integrator Set status = 8003, finish_date = getdate(), result = 'Action Complete (" + errormsg.ToString() + ")' where id = '" + DataProcess[0].ToString() + "'";
                SqlCommand cmd = new SqlCommand(Qry.ToString(), DBJOB);
                cmd.ExecuteNonQuery();
                DBJOB.Close();
                Console.WriteLine(DateTime.Now.ToString() + " : Job Completed");

            }
            catch (Exception e)
            {
                errormsg = "Error Update Status 8003 : " + e.ToString();
                Console.WriteLine(DateTime.Now.ToString() + errormsg.ToString());
            }
            #endregion

            sw.Close();
        }
        #region Ellipse mse600
        //static void executeEllipse()
        static void executeEllipse(string ppeNo, string idPpeSubstring)
        {
            try
            {
                SqlConnection CONNECT = new SqlConnection(Properties.Settings.Default.DB_PLANT_PPE_NEW_KPT);
                CONNECT.Open();
                string selectQuery = "SELECT * FROM TBL_T_PPE WHERE PPE_NO = @PPE_NO AND SUBSTRING(CONVERT(VARCHAR(36), ID_PPE), 1, 16) = @ID_PPE_SUBSTRING";
                SqlCommand selectCommand = new SqlCommand(selectQuery, CONNECT);
                selectCommand.Parameters.AddWithValue("@PPE_NO", ppeNo);
                selectCommand.Parameters.AddWithValue("@ID_PPE_SUBSTRING", idPpeSubstring);

                using (SqlDataReader reader = selectCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Membaca kolom-kolom yang diperlukan
                        string equipNo = reader["EQUIP_NO"].ToString();
                        string districtTo = reader["DISTRICT_TO"].ToString();
                        string districtfrom = reader["DISTRICT_FROM"].ToString();
                        // ... membaca kolom lainnya

                        EquipmentService.EquipmentService i_obj_services = new EquipmentService.EquipmentService();

                        EquipmentService.OperationContext i_obj_context = new EquipmentService.OperationContext();
                        EquipmentServiceModifyRequestDTO i_obj_equipment_dto1 = new EquipmentServiceModifyRequestDTO();

                        EquipmentServiceReadRequestDTO i_obj_equipment_request = new EquipmentServiceReadRequestDTO();
                        EquipmentServiceReadReplyDTO i_obj_equipment_result = new EquipmentServiceReadReplyDTO();
                        i_obj_equipment_request.equipmentNo = equipNo;
                        i_obj_equipment_request.equipmentRef = equipNo;

                        string str_username = ConfigurationManager.AppSettings["username"].ToString();
                        string str_password = ConfigurationManager.AppSettings["password"].ToString();
                        string str_posisi = ConfigurationManager.AppSettings["pos_id"].ToString();
                        ClientConversation.authenticate(str_username, str_password);

                        i_obj_context.district = districtfrom;
                        i_obj_context.position = str_posisi;

                        i_obj_equipment_result = i_obj_services.read(i_obj_context, i_obj_equipment_request);

                        i_obj_equipment_dto1.equipmentNo = equipNo;
                        i_obj_equipment_dto1.equipmentStatus = "MT";
                        if (i_obj_equipment_result.poNo != null)
                        {
                            i_obj_equipment_dto1.poNo = i_obj_equipment_result.poNo;
                        }
                        else
                        {
                            i_obj_equipment_dto1.poNo = "-";
                        }
                        i_obj_equipment_dto1.equipmentClassif3 = i_obj_equipment_result.equipmentClassif3;
                        i_obj_services.modify(i_obj_context, i_obj_equipment_dto1);
                    }
                    else
                    {
                        Console.WriteLine("No data found for PPE_NO: " + ppeNo);
                    }
                }

                CONNECT.Close();
            }
            catch (Exception ex)
            {
                var exMessage = ex.Message;
                Console.WriteLine(DateTime.Now.ToString() + exMessage);
            }
        }
        #endregion
    }
}
