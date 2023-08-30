using API_PLANT_PPE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API_PLANT_PPE.Controllers
{
    [RoutePrefix("api/DetailApproval")]
    public class DetailApprovalController : ApiController
    {
        DB_Plant_PPEDataContext db = new DB_Plant_PPEDataContext();

        #region Section Head
        [HttpPost]
        [Route("Approve_Section_Head")]
        public IHttpActionResult Approve_Section_Head(TBL_T_PPE param)
        {
            try
            {
                string old_posisi = "";
                var cek = db.TBL_T_PPEs.FirstOrDefault(a => a.PPE_NO == param.PPE_NO && a.EQUIP_NO == param.EQUIP_NO);

                old_posisi = cek.POSISI_PPE;
                if (cek.DISTRICT_FROM == "KPHO")
                {
                    cek.STATUS = param.STATUS;
                    cek.REMARKS = param.REMARKS;
                    cek.UPDATED_BY = param.UPDATED_BY;
                    cek.POSISI_PPE = "Plant Adm & Dev Manager";
                    cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();
                    cek.APPROVAL_ORDER = 2;
                    cek.URL_FORM_PLNTMNGR = param.URL_FORM_PLNTMNGR;
                }
                else if (cek.DISTRICT_FROM != "KPHO")
                {
                    //update
                    cek.STATUS = param.STATUS;
                    cek.REMARKS = param.REMARKS;
                    cek.UPDATED_BY = param.UPDATED_BY;
                    cek.POSISI_PPE = param.POSISI_PPE;
                    cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();
                    cek.APPROVAL_ORDER = 2;
                    cek.URL_FORM_PLNTMNGR = param.URL_FORM_PLNTMNGR;
                }
                //update
                //cek.STATUS = param.STATUS;
                //cek.REMARKS = param.REMARKS;
                //cek.UPDATED_BY = param.UPDATED_BY;
                //cek.POSISI_PPE = param.POSISI_PPE;
                //cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();
                //cek.APPROVAL_ORDER = 2;
                //cek.URL_FORM_PLNTMNGR = param.URL_FORM_PLNTMNGR;

                //history ppe
                TBL_H_APPROVAL_PPE his = new TBL_H_APPROVAL_PPE();

                if (cek.PPE_NO != null && cek.POSISI_PPE != "")
                {
                    his.Ppe_NO = param.PPE_NO;
                    his.Equip_No = param.EQUIP_NO;
                    his.Posisi_Ppe = old_posisi;
                    his.Approval_Order = 2;
                    his.Approved_Date = DateTime.UtcNow.ToLocalTime();
                    his.Approved_By = param.UPDATED_BY;

                    if (param.STATUS != "REJECT")
                    {
                        db.TBL_H_APPROVAL_PPEs.InsertOnSubmit(his);
                    }
                }
                db.SubmitChanges();

                return Ok(new { Remarks = true });
            }
            catch (Exception e)
            {
                return Ok(new { Remarks = false, Message = e });
            }
        }

        [HttpPost]
        [Route("Sendmail_Plant_Manager")]
        public IHttpActionResult Sendmail_Plant_Manager(string ppe)
        {
            try
            {
                string decodedPpenosh = Uri.UnescapeDataString(ppe);

                db.CommandTimeout = 120;
                db.cusp_insertNotifEmail_PlantManager(decodedPpenosh);

                return Ok(new { Remarks = true });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("Reject_Section_Head")]
        public IHttpActionResult Reject_Section_Head(TBL_T_PPE ppe)
        {
            try
            {
                string old_posisi = "";
                var cek = db.TBL_T_PPEs.FirstOrDefault(a => a.PPE_NO == ppe.PPE_NO && a.EQUIP_NO == ppe.EQUIP_NO);

                old_posisi = cek.POSISI_PPE;

                // Update
                cek.STATUS = ppe.STATUS;
                cek.REMARKS = ppe.REMARKS;
                cek.UPDATED_BY = ppe.UPDATED_BY;
                cek.POSISI_PPE = ppe.POSISI_PPE;
                cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();
                cek.APPROVAL_ORDER = 2;

                // History ppe
                TBL_H_APPROVAL_PPE his = new TBL_H_APPROVAL_PPE();
                if (cek.PPE_NO != null && cek.POSISI_PPE != "")
                {
                    his.Ppe_NO = ppe.PPE_NO;
                    his.Equip_No = ppe.EQUIP_NO;
                    his.Posisi_Ppe = old_posisi;
                    his.Approval_Order = 2;
                    his.Approved_Date = DateTime.UtcNow.ToLocalTime();
                    his.Approved_By = ppe.UPDATED_BY;

                    if (ppe.STATUS != "REJECT")
                    {
                        db.TBL_H_APPROVAL_PPEs.InsertOnSubmit(his);
                    }
                }

                //if (ppe.STATUS == "REJECT")
                //{
                //    var rejectEquipments = db.TBL_T_PPEs.Where(a => a.PPE_NO == ppe.PPE_NO).ToList();
                //    foreach (var equipment in rejectEquipments)
                //    {
                //        equipment.STATUS = "REJECT";
                //    }
                //}
                
                db.SubmitChanges();
                return Ok(new { Remarks = true });
            }
            catch (Exception e)
            {
                return Ok(new { Remarks = false, Message = e });
            }
        }
        #endregion

        #region Plant Manager
        [HttpPost]
        [Route("Approve_PPE_PlantManager")]
        public IHttpActionResult Approve_PPE_PlantManager(TBL_T_PPE ppe)
        {
            try
            {
                string old_posisi = "";
                var cek = db.TBL_T_PPEs.FirstOrDefault(a => a.PPE_NO == ppe.PPE_NO && a.EQUIP_NO == ppe.EQUIP_NO);

                old_posisi = cek.POSISI_PPE;
                if (cek.DISTRICT_FROM == "KPHO" && cek.STATUS != "PLANT ADM & DEV MANAGER APPROVED")
                {
                    cek.STATUS = "PLANT ADM & DEV MANAGER APPROVED";
                    cek.REMARKS = ppe.REMARKS;
                    cek.UPDATED_BY = ppe.UPDATED_BY;
                    cek.POSISI_PPE = "Plant Manager";
                    cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();
                    cek.APPROVAL_ORDER = ppe.APPROVAL_ORDER; //3;
                    cek.URL_FORM_PLNTDH = ppe.URL_FORM_PLNTDH;
                }
                else if (cek.DISTRICT_FROM != "KPHO")
                {
                    //update
                    cek.STATUS = ppe.STATUS;
                    cek.REMARKS = ppe.REMARKS;
                    cek.UPDATED_BY = ppe.UPDATED_BY;
                    cek.POSISI_PPE = ppe.POSISI_PPE;
                    cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();
                    cek.APPROVAL_ORDER = ppe.APPROVAL_ORDER;
                    cek.URL_FORM_PLNTDH = ppe.URL_FORM_PLNTDH;
                }
                else if (cek.DISTRICT_FROM == "KPHO" && cek.STATUS == "PLANT ADM & DEV MANAGER APPROVED")
                {
                    cek.STATUS = ppe.STATUS;
                    cek.REMARKS = ppe.REMARKS;
                    cek.UPDATED_BY = ppe.UPDATED_BY;
                    cek.POSISI_PPE = "Project Manager Penerima";
                    cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();
                    cek.APPROVAL_ORDER = 4;
                    cek.URL_FORM_DIVHEAD_ENG = ppe.URL_FORM_DIVHEAD_ENG;
                }
                //update
                //cek.STATUS = ppe.STATUS;
                //cek.REMARKS = ppe.REMARKS;
                //cek.UPDATED_BY = ppe.UPDATED_BY;
                //cek.POSISI_PPE = ppe.POSISI_PPE;
                //cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();
                //cek.APPROVAL_ORDER = ppe.APPROVAL_ORDER;
                //cek.URL_FORM_PLNTDH = ppe.URL_FORM_PLNTDH;

                //history ppe
                TBL_H_APPROVAL_PPE his = new TBL_H_APPROVAL_PPE();

                if (cek.PPE_NO != null && cek.POSISI_PPE != "")
                {
                    his.Ppe_NO = ppe.PPE_NO;
                    his.Equip_No = ppe.EQUIP_NO;
                    his.Posisi_Ppe = old_posisi;
                    his.Approval_Order = ppe.APPROVAL_ORDER;
                    his.Approved_Date = DateTime.UtcNow.ToLocalTime();
                    his.Approved_By = ppe.UPDATED_BY;

                    if (ppe.STATUS != "REJECT")
                    {
                        db.TBL_H_APPROVAL_PPEs.InsertOnSubmit(his);
                    }
                }

                db.SubmitChanges();

                return Ok(new { Remarks = true });
            }
            catch (Exception e)
            {
                return Ok(new { Remarks = false, Message = e });
            }
        }

        [HttpPost]
        [Route("Sendmail_Plant_DeptHead")]
        public IHttpActionResult Sendmail_Plant_DeptHead(string ppe)
        {
            try
            {
                string decodedPpenosh = Uri.UnescapeDataString(ppe);

                db.CommandTimeout = 120;
                db.cusp_insertNotifEmail_PlantDeptHead(decodedPpenosh);

                return Ok(new { Remarks = true });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("Reject_PPE_PlantManager")]
        public IHttpActionResult Reject_PPE_PlantManager(TBL_T_PPE ppe)
        {
            try
            {
                string old_posisi = "";
                var cek = db.TBL_T_PPEs.FirstOrDefault(a => a.PPE_NO == ppe.PPE_NO && a.EQUIP_NO == ppe.EQUIP_NO);

                old_posisi = cek.POSISI_PPE;

                //update
                cek.STATUS = ppe.STATUS;
                cek.REMARKS = ppe.REMARKS;
                cek.UPDATED_BY = ppe.UPDATED_BY;
                cek.POSISI_PPE = ppe.POSISI_PPE;
                cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();
                cek.APPROVAL_ORDER = ppe.APPROVAL_ORDER;

                //history ppe
                TBL_H_APPROVAL_PPE his = new TBL_H_APPROVAL_PPE();

                if (cek.PPE_NO != null && cek.POSISI_PPE != "")
                {
                    his.Ppe_NO = ppe.PPE_NO;
                    his.Equip_No = ppe.EQUIP_NO;
                    his.Posisi_Ppe = old_posisi;
                    his.Approval_Order = ppe.APPROVAL_ORDER;
                    his.Approved_Date = DateTime.UtcNow.ToLocalTime();
                    his.Approved_By = ppe.UPDATED_BY;

                    if (ppe.STATUS != "REJECT")
                    {
                        db.TBL_H_APPROVAL_PPEs.InsertOnSubmit(his);
                    }
                }

                //if (ppe.STATUS == "REJECT")
                //{
                //    var rejectEquipments = db.TBL_T_PPEs.Where(a => a.PPE_NO == ppe.PPE_NO).ToList();
                //    foreach (var equipment in rejectEquipments)
                //    {
                //        equipment.STATUS = "REJECT";
                //    }
                //}

                db.SubmitChanges();

                return Ok(new { Remarks = true });
            }
            catch (Exception e)
            {
                return Ok(new { Remarks = false, Message = e });
            }
        }
        #endregion

        #region PM Pengirim
        [HttpPost]
        [Route("Approve_PPE_PMPengirim")]
        public IHttpActionResult Approve_PPE_PMPengirim(TBL_T_PPE ppe)
        {
            try
            {
                string old_posisi = "";
                var cek = db.TBL_T_PPEs.FirstOrDefault(a => a.PPE_NO == ppe.PPE_NO && a.EQUIP_NO == ppe.EQUIP_NO);

                old_posisi = cek.POSISI_PPE;
                if (cek.DISTRICT_TO == "KPHO")
                {
                    cek.STATUS = ppe.STATUS;
                    cek.REMARKS = ppe.REMARKS;
                    cek.UPDATED_BY = ppe.UPDATED_BY;
                    cek.POSISI_PPE = "Division Head ENG";
                    cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();
                    cek.APPROVAL_ORDER = ppe.APPROVAL_ORDER;
                    //cek.URL_FORM_DIVHEAD_ENG = ppe.URL_FORM_PM_PENERIMA;
                    cek.URL_FORM_DIVHEAD_ENG = "http://10.14.101.181/ReportServer_RPTPROD?/PPE/Rpt_PPE_DivHead_Eng&PPE_NO=" + cek.PPE_NO;
                }
                else if (cek.DISTRICT_TO != "KPHO")
                {
                    //update
                    cek.STATUS = ppe.STATUS;
                    cek.REMARKS = ppe.REMARKS;
                    cek.UPDATED_BY = ppe.UPDATED_BY;
                    cek.POSISI_PPE = ppe.POSISI_PPE;
                    cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();
                    cek.APPROVAL_ORDER = ppe.APPROVAL_ORDER;
                    cek.URL_FORM_PM_PENERIMA = ppe.URL_FORM_PM_PENERIMA;
                }
                //update
                //cek.STATUS = ppe.STATUS;
                //cek.REMARKS = ppe.REMARKS;
                //cek.UPDATED_BY = ppe.UPDATED_BY;
                //cek.POSISI_PPE = ppe.POSISI_PPE;
                //cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();
                //cek.APPROVAL_ORDER = ppe.APPROVAL_ORDER;
                //cek.URL_FORM_PM_PENERIMA = ppe.URL_FORM_PM_PENERIMA;

                //history ppe
                TBL_H_APPROVAL_PPE his = new TBL_H_APPROVAL_PPE();

                if (cek.PPE_NO != null && cek.POSISI_PPE != "")
                {
                    his.Ppe_NO = ppe.PPE_NO;
                    his.Equip_No = ppe.EQUIP_NO;
                    his.Posisi_Ppe = old_posisi;
                    his.Approval_Order = ppe.APPROVAL_ORDER;
                    his.Approved_Date = DateTime.UtcNow.ToLocalTime();
                    his.Approved_By = ppe.UPDATED_BY;

                    if (ppe.STATUS != "REJECT")
                    {
                        db.TBL_H_APPROVAL_PPEs.InsertOnSubmit(his);
                    }
                }

                db.SubmitChanges();

                return Ok(new { Remarks = true });
            }
            catch (Exception e)
            {
                return Ok(new { Remarks = false, Message = e });
            }
        }

        [HttpPost]
        [Route("Sendmail_PM_Penerima")]
        public IHttpActionResult Sendmail_PM_Penerima(string ppe)
        {
            try
            {
                string decodedPpenosh = Uri.UnescapeDataString(ppe);

                db.CommandTimeout = 120;
                db.cusp_insertNotifEmail_PMPenerima(decodedPpenosh);

                return Ok(new { Remarks = true });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("Reject_Approval")]
        public IHttpActionResult Reject_Approval(TBL_T_PPE ppe)
        {
            try
            {
                string old_posisi = "";
                var cek = db.TBL_T_PPEs.FirstOrDefault(a => a.PPE_NO == ppe.PPE_NO && a.EQUIP_NO == ppe.EQUIP_NO);

                old_posisi = cek.POSISI_PPE;

                //update
                cek.STATUS = ppe.STATUS;
                cek.REMARKS = ppe.REMARKS;
                cek.UPDATED_BY = ppe.UPDATED_BY;
                cek.POSISI_PPE = ppe.POSISI_PPE;
                cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();
                cek.APPROVAL_ORDER = ppe.APPROVAL_ORDER;

                //history ppe
                TBL_H_APPROVAL_PPE his = new TBL_H_APPROVAL_PPE();

                if (cek.PPE_NO != null && cek.POSISI_PPE != "")
                {
                    his.Ppe_NO = ppe.PPE_NO;
                    his.Equip_No = ppe.EQUIP_NO;
                    his.Posisi_Ppe = old_posisi;
                    his.Approval_Order = ppe.APPROVAL_ORDER;
                    his.Approved_Date = DateTime.UtcNow.ToLocalTime();
                    his.Approved_By = ppe.UPDATED_BY;

                    if (ppe.STATUS != "REJECT")
                    {
                        db.TBL_H_APPROVAL_PPEs.InsertOnSubmit(his);
                    }
                }

                //if (ppe.STATUS == "REJECT")
                //{
                //    var rejectEquipments = db.TBL_T_PPEs.Where(a => a.PPE_NO == ppe.PPE_NO).ToList();
                //    foreach (var equipment in rejectEquipments)
                //    {
                //        equipment.STATUS = "REJECT";
                //    }
                //}

                db.SubmitChanges();

                return Ok(new { Remarks = true });
            }
            catch (Exception e)
            {
                return Ok(new { Remarks = false, Message = e });
            }
        }
        #endregion

        #region PM Penerima
        [HttpPost]
        [Route("Approve_PPE_PMPenerima")]
        public IHttpActionResult Approve_PPE_PMPenerima(TBL_T_PPE ppe)
        {
            try
            {
                string old_posisi = "";
                var cek = db.TBL_T_PPEs.FirstOrDefault(a => a.PPE_NO == ppe.PPE_NO && a.EQUIP_NO == ppe.EQUIP_NO);

                old_posisi = cek.POSISI_PPE;
                if (cek.DISTRICT_FROM == "KPHO")
                {
                    cek.STATUS = ppe.STATUS;
                    cek.REMARKS = ppe.REMARKS;
                    cek.UPDATED_BY = ppe.UPDATED_BY;
                    cek.POSISI_PPE = ppe.POSISI_PPE;
                    cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();
                    cek.APPROVAL_ORDER = 5;
                    cek.URL_FORM_DIVHEAD_ENG = ppe.URL_FORM_DIVHEAD_ENG;
                }
                else if (cek.DISTRICT_FROM != "KPHO")
                {
                    //update
                    cek.STATUS = ppe.STATUS;
                    cek.REMARKS = ppe.REMARKS;
                    cek.UPDATED_BY = ppe.UPDATED_BY;
                    cek.POSISI_PPE = ppe.POSISI_PPE;
                    cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();
                    cek.APPROVAL_ORDER = ppe.APPROVAL_ORDER;
                    cek.URL_FORM_DIVHEAD_ENG = ppe.URL_FORM_DIVHEAD_ENG;
                }
                //update
                //cek.STATUS = ppe.STATUS;
                //cek.REMARKS = ppe.REMARKS;
                //cek.UPDATED_BY = ppe.UPDATED_BY;
                //cek.POSISI_PPE = ppe.POSISI_PPE;
                //cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();
                //cek.APPROVAL_ORDER = ppe.APPROVAL_ORDER;
                //cek.URL_FORM_DIVHEAD_ENG = ppe.URL_FORM_DIVHEAD_ENG;

                //history ppe
                TBL_H_APPROVAL_PPE his = new TBL_H_APPROVAL_PPE();

                if (cek.PPE_NO != null && cek.POSISI_PPE != "")
                {
                    his.Ppe_NO = ppe.PPE_NO;
                    his.Equip_No = ppe.EQUIP_NO;
                    his.Posisi_Ppe = old_posisi;
                    his.Approval_Order = ppe.APPROVAL_ORDER;
                    his.Approved_Date = DateTime.UtcNow.ToLocalTime();
                    his.Approved_By = ppe.UPDATED_BY;

                    if (ppe.STATUS != "REJECT")
                    {
                        db.TBL_H_APPROVAL_PPEs.InsertOnSubmit(his);
                    }
                }

                db.SubmitChanges();

                return Ok(new { Remarks = true });
            }
            catch (Exception e)
            {
                return Ok(new { Remarks = false, Message = e });
            }
        }

        [HttpPost]
        [Route("Sendmail_Divhead_Eng")]
        public IHttpActionResult Sendmail_Divhead_Eng(string ppe)
        {
            try
            {
                string decodedPpenosh = Uri.UnescapeDataString(ppe);

                db.CommandTimeout = 120;
                db.cusp_insertNotifEmail_Divhead_Eng(decodedPpenosh);

                return Ok(new { Remarks = true });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        #endregion

        #region Divhead Eng
        [HttpPost]
        [Route("Approve_PPE_DivHead_Eng")]
        public IHttpActionResult Approve_PPE_DivHead_Eng(TBL_T_PPE ppe)
        {
            try
            {
                string old_posisi = "";
                var cek = db.TBL_T_PPEs.FirstOrDefault(a => a.PPE_NO == ppe.PPE_NO && a.EQUIP_NO == ppe.EQUIP_NO);

                old_posisi = cek.POSISI_PPE;
                if (cek.DISTRICT_FROM == "KPHO")
                {
                    cek.STATUS = ppe.STATUS;
                    cek.REMARKS = ppe.REMARKS;
                    cek.UPDATED_BY = ppe.UPDATED_BY;
                    cek.POSISI_PPE = ppe.POSISI_PPE;
                    cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();
                    cek.APPROVAL_ORDER = 6;
                    cek.URL_FORM_DIVHEAD_OPR = ppe.URL_FORM_DIVHEAD_OPR;
                }
                else if (cek.DISTRICT_FROM != "KPHO")
                {
                    //update
                    cek.STATUS = ppe.STATUS;
                    cek.REMARKS = ppe.REMARKS;
                    cek.UPDATED_BY = ppe.UPDATED_BY;
                    cek.POSISI_PPE = ppe.POSISI_PPE;
                    cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();
                    cek.APPROVAL_ORDER = ppe.APPROVAL_ORDER;
                    cek.URL_FORM_DIVHEAD_OPR = ppe.URL_FORM_DIVHEAD_OPR;
                }
                //update
                //cek.STATUS = ppe.STATUS;
                //cek.REMARKS = ppe.REMARKS;
                //cek.UPDATED_BY = ppe.UPDATED_BY;
                //cek.POSISI_PPE = ppe.POSISI_PPE;
                //cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();
                //cek.APPROVAL_ORDER = ppe.APPROVAL_ORDER;
                //cek.URL_FORM_DIVHEAD_OPR = ppe.URL_FORM_DIVHEAD_OPR;

                //history ppe
                TBL_H_APPROVAL_PPE his = new TBL_H_APPROVAL_PPE();

                if (cek.PPE_NO != null && cek.POSISI_PPE != "")
                {
                    his.Ppe_NO = ppe.PPE_NO;
                    his.Equip_No = ppe.EQUIP_NO;
                    his.Posisi_Ppe = old_posisi;
                    his.Approval_Order = ppe.APPROVAL_ORDER;
                    his.Approved_Date = DateTime.UtcNow.ToLocalTime();
                    his.Approved_By = ppe.UPDATED_BY;

                    if (ppe.STATUS != "REJECT")
                    {
                        db.TBL_H_APPROVAL_PPEs.InsertOnSubmit(his);
                    }
                }

                db.SubmitChanges();

                return Ok(new { Remarks = true });
            }
            catch (Exception e)
            {
                return Ok(new { Remarks = false, Message = e });
            }
        }

        [HttpPost]
        [Route("Sendmail_Divhead_Opr")]
        public IHttpActionResult Sendmail_Divhead_Opr(string ppe)
        {
            try
            {
                string decodedPpenosh = Uri.UnescapeDataString(ppe);

                db.CommandTimeout = 120;
                db.cusp_insertNotifEmail_Divhead_Opr(decodedPpenosh);

                return Ok(new { Remarks = true });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        #endregion

        #region Divhead Opr
        [HttpPost]
        [Route("Approve_PPE_DivHead_Opr")]
        public IHttpActionResult Approve_PPE_DivHead_Opr(TBL_T_PPE ppe)
        {
            try
            {
                string old_posisi = "";
                var cek = db.TBL_T_PPEs.FirstOrDefault(a => a.PPE_NO == ppe.PPE_NO && a.EQUIP_NO == ppe.EQUIP_NO);

                old_posisi = cek.POSISI_PPE;
                if (cek.DISTRICT_FROM == "KPHO")
                {
                    cek.STATUS = ppe.STATUS;
                    cek.REMARKS = ppe.REMARKS;
                    cek.UPDATED_BY = ppe.UPDATED_BY;
                    cek.POSISI_PPE = ppe.POSISI_PPE;
                    cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();
                    cek.APPROVAL_ORDER = 7;
                    cek.URL_FORM_DONE = ppe.URL_FORM_DONE;
                }
                else if (cek.DISTRICT_FROM != "KPHO")
                {
                    //update
                    cek.STATUS = ppe.STATUS;
                    cek.REMARKS = ppe.REMARKS;
                    cek.UPDATED_BY = ppe.UPDATED_BY;
                    cek.POSISI_PPE = ppe.POSISI_PPE;
                    cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();
                    cek.APPROVAL_ORDER = ppe.APPROVAL_ORDER;
                    cek.URL_FORM_DONE = ppe.URL_FORM_DONE;
                }
                //update
                //cek.STATUS = ppe.STATUS;
                //cek.REMARKS = ppe.REMARKS;
                //cek.UPDATED_BY = ppe.UPDATED_BY;
                //cek.POSISI_PPE = ppe.POSISI_PPE;
                //cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();
                //cek.APPROVAL_ORDER = ppe.APPROVAL_ORDER;
                //cek.URL_FORM_DONE = ppe.URL_FORM_DONE;

                //history ppe
                TBL_H_APPROVAL_PPE his = new TBL_H_APPROVAL_PPE();

                if (cek.PPE_NO != null && cek.POSISI_PPE != "")
                {
                    his.Ppe_NO = ppe.PPE_NO;
                    his.Equip_No = ppe.EQUIP_NO;
                    his.Posisi_Ppe = old_posisi;
                    his.Approval_Order = ppe.APPROVAL_ORDER;
                    his.Approved_Date = DateTime.UtcNow.ToLocalTime();
                    his.Approved_By = ppe.UPDATED_BY;

                    if (ppe.STATUS != "REJECT")
                    {
                        db.TBL_H_APPROVAL_PPEs.InsertOnSubmit(his);
                    }
                }

                db.SubmitChanges();

                return Ok(new { Remarks = true });
            }
            catch (Exception e)
            {
                return Ok(new { Remarks = false, Message = e });
            }
        }

        [HttpPost]
        [Route("Sendmail_Done")]
        public IHttpActionResult Sendmail_Done(string ppe)
        {
            try
            {
                string decodedPpenosh = Uri.UnescapeDataString(ppe);

                db.CommandTimeout = 120;
                db.cusp_insertNotifEmail_PPE_Done(decodedPpenosh);

                return Ok(new { Remarks = true });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        #endregion
    }
}
