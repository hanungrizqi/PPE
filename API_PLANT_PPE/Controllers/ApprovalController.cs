using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using API_PLANT_PPE.Models;
using API_PLANT_PPE.ViewModel;
using Newtonsoft.Json;

namespace API_PLANT_PPE.Controllers
{
    [RoutePrefix("api/Approval")]
    public class ApprovalController : ApiController
    {
        DB_Plant_PPEDataContext db = new DB_Plant_PPEDataContext();
        
        [HttpPost]
        [Route("Approve_PPE")]
        public IHttpActionResult Approve_PPE(TBL_T_PPE[] param)
        {
            try
            {
                foreach (var ppe in param)
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
                    cek.URL_FORM_PLNTDH = ppe.URL_FORM_PLNTDH;
                    cek.URL_FORM_PM_PENGIRIM = ppe.URL_FORM_PM_PENGIRIM;
                    cek.URL_FORM_PM_PENERIMA = ppe.URL_FORM_PM_PENERIMA;
                    cek.URL_FORM_DIVHEAD_ENG = ppe.URL_FORM_DIVHEAD_ENG;
                    cek.URL_FORM_DIVHEAD_OPR = ppe.URL_FORM_DIVHEAD_OPR;
                    cek.URL_FORM_DONE = ppe.URL_FORM_DONE;

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
        [Route("Approve_PPE_DivHead_Opr")]
        public IHttpActionResult Approve_PPE_DivHead_Opr(TBL_T_PPE[] param)
        {
            try
            {
                foreach (var ppe in param)
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
                        cek.APPROVAL_ORDER = 8;
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
        [Route("Approve_PPE_DivHead_Eng")]
        public IHttpActionResult Approve_PPE_DivHead_Eng(TBL_T_PPE[] param)
        {
            try
            {
                foreach (var ppe in param)
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

                        TBL_H_APPROVAL_PPE his = new TBL_H_APPROVAL_PPE();

                        if (cek.PPE_NO != null && cek.POSISI_PPE != "")
                        {
                            his.Ppe_NO = ppe.PPE_NO;
                            his.Equip_No = ppe.EQUIP_NO;
                            his.Posisi_Ppe = old_posisi;
                            his.Approval_Order = 6;
                            his.Approved_Date = DateTime.UtcNow.ToLocalTime();
                            his.Approved_By = ppe.UPDATED_BY;

                            if (ppe.STATUS != "REJECT")
                            {
                                db.TBL_H_APPROVAL_PPEs.InsertOnSubmit(his);
                            }
                        }
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
                    //TBL_H_APPROVAL_PPE his = new TBL_H_APPROVAL_PPE();

                    //if (cek.PPE_NO != null && cek.POSISI_PPE != "")
                    //{
                    //    his.Ppe_NO = ppe.PPE_NO;
                    //    his.Equip_No = ppe.EQUIP_NO;
                    //    his.Posisi_Ppe = old_posisi;
                    //    his.Approval_Order = ppe.APPROVAL_ORDER;
                    //    his.Approved_Date = DateTime.UtcNow.ToLocalTime();
                    //    his.Approved_By = ppe.UPDATED_BY;

                    //    if (ppe.STATUS != "REJECT")
                    //    {
                    //        db.TBL_H_APPROVAL_PPEs.InsertOnSubmit(his);
                    //    }
                    //}
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
        [Route("Approve_PPE_PMPenerima")]
        public IHttpActionResult Approve_PPE_PMPenerima(TBL_T_PPE[] param)
        {
            try
            {
                foreach (var ppe in param)
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

                        TBL_H_APPROVAL_PPE his = new TBL_H_APPROVAL_PPE();

                        if (cek.PPE_NO != null && cek.POSISI_PPE != "")
                        {
                            his.Ppe_NO = ppe.PPE_NO;
                            his.Equip_No = ppe.EQUIP_NO;
                            his.Posisi_Ppe = old_posisi;
                            his.Approval_Order = 5;
                            his.Approved_Date = DateTime.UtcNow.ToLocalTime();
                            his.Approved_By = ppe.UPDATED_BY;

                            if (ppe.STATUS != "REJECT")
                            {
                                db.TBL_H_APPROVAL_PPEs.InsertOnSubmit(his);
                            }
                        }
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
                    //TBL_H_APPROVAL_PPE his = new TBL_H_APPROVAL_PPE();

                    //if (cek.PPE_NO != null && cek.POSISI_PPE != "")
                    //{
                    //    his.Ppe_NO = ppe.PPE_NO;
                    //    his.Equip_No = ppe.EQUIP_NO;
                    //    his.Posisi_Ppe = old_posisi;
                    //    his.Approval_Order = ppe.APPROVAL_ORDER;
                    //    his.Approved_Date = DateTime.UtcNow.ToLocalTime();
                    //    his.Approved_By = ppe.UPDATED_BY;

                    //    if (ppe.STATUS != "REJECT")
                    //    {
                    //        db.TBL_H_APPROVAL_PPEs.InsertOnSubmit(his);
                    //    }
                    //}
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
        [Route("Approve_PPE_PlantManager")]
        public IHttpActionResult Approve_PPE_PlantManager(TBL_T_PPE[] param)
        {
            try
            {
                foreach (var ppe in param)
                {
                    string old_posisi = "";
                    var cek = db.TBL_T_PPEs.FirstOrDefault(a => a.PPE_NO == ppe.PPE_NO && a.EQUIP_NO == ppe.EQUIP_NO);

                    old_posisi = cek.POSISI_PPE;
                    if (cek.DISTRICT_FROM == "KPHO" && cek.STATUS != "PLANT ADM & DEV MANAGER APPROVED") //posisi plant adm dev manager
                    {
                        cek.STATUS = "PLANT ADM & DEV MANAGER APPROVED";
                        cek.REMARKS = ppe.REMARKS;
                        cek.UPDATED_BY = ppe.UPDATED_BY;
                        cek.POSISI_PPE = "Plant Manager";
                        cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();
                        cek.APPROVAL_ORDER = ppe.APPROVAL_ORDER; //3;
                        //cek.URL_FORM_PLNTDH = ppe.URL_FORM_PLNTDH;

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

                        if (cek.PPE_NO != null && cek.POSISI_PPE != "")
                        {
                            TBL_H_APPROVAL_PPE his = new TBL_H_APPROVAL_PPE();

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
                    }
                    else if (cek.DISTRICT_FROM == "KPHO" && cek.STATUS == "PLANT ADM & DEV MANAGER APPROVED")
                    {
                        cek.STATUS = ppe.STATUS;
                        cek.REMARKS = ppe.REMARKS;
                        cek.UPDATED_BY = ppe.UPDATED_BY;
                        cek.POSISI_PPE = "Project Manager Penerima";
                        cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();
                        cek.APPROVAL_ORDER = 4;
                        cek.URL_FORM_PM_PENERIMA = "http://10.14.101.181/ReportServer_RPTPROD?/PPE/Rpt_PPE_PMPenerima&PPE_NO=" + cek.PPE_NO;

                        TBL_H_APPROVAL_PPE his = new TBL_H_APPROVAL_PPE();

                        if (cek.PPE_NO != null && cek.POSISI_PPE != "")
                        {
                            his.Ppe_NO = ppe.PPE_NO;
                            his.Equip_No = ppe.EQUIP_NO;
                            his.Posisi_Ppe = old_posisi;
                            his.Approval_Order = 4;
                            his.Approved_Date = DateTime.UtcNow.ToLocalTime();
                            his.Approved_By = ppe.UPDATED_BY;

                            if (ppe.STATUS != "REJECT")
                            {
                                db.TBL_H_APPROVAL_PPEs.InsertOnSubmit(his);
                            }
                        }
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
                    //TBL_H_APPROVAL_PPE his = new TBL_H_APPROVAL_PPE();

                    //if (cek.PPE_NO != null && cek.POSISI_PPE != "")
                    //{
                    //    his.Ppe_NO = ppe.PPE_NO;
                    //    his.Equip_No = ppe.EQUIP_NO;
                    //    his.Posisi_Ppe = old_posisi;
                    //    his.Approval_Order = ppe.APPROVAL_ORDER;
                    //    his.Approved_Date = DateTime.UtcNow.ToLocalTime();
                    //    his.Approved_By = ppe.UPDATED_BY;

                    //    if (ppe.STATUS != "REJECT")
                    //    {
                    //        db.TBL_H_APPROVAL_PPEs.InsertOnSubmit(his);
                    //    }
                    //}
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
        [Route("Approve_PPE_DeptHead")]
        public IHttpActionResult Approve_PPE_DeptHead(TBL_T_PPE[] param)
        {
            try
            {
                foreach (var ppe in param)
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
                    cek.URL_FORM_PM_PENGIRIM = ppe.URL_FORM_PM_PENGIRIM;

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
        [Route("Approve_PPE_PMPengirim")]
        public IHttpActionResult Approve_PPE_PMPengirim(TBL_T_PPE[] param)
        {
            try
            {
                foreach (var ppe in param)
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
        [Route("Approve_Section_Head")]
        public IHttpActionResult Approve_Section_Head(TBL_T_PPE[] param)
        {
            try
            {
                foreach (var ppe in param)
                {
                    string old_posisi = "";
                    var cek = db.TBL_T_PPEs.FirstOrDefault(a => a.PPE_NO == ppe.PPE_NO && a.EQUIP_NO == ppe.EQUIP_NO);

                    old_posisi = cek.POSISI_PPE;
                    if (cek.DISTRICT_FROM == "KPHO")
                    {
                        cek.STATUS = ppe.STATUS;
                        cek.REMARKS = ppe.REMARKS;
                        cek.UPDATED_BY = ppe.UPDATED_BY;
                        cek.POSISI_PPE = "Plant Adm & Dev Manager";
                        cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();
                        cek.APPROVAL_ORDER = 2;
                        //cek.URL_FORM_PLNTMNGR = ppe.URL_FORM_PLNTMNGR;
                    }
                    else if (cek.DISTRICT_FROM != "KPHO")
                    {
                        //update
                        cek.STATUS = ppe.STATUS;
                        cek.REMARKS = ppe.REMARKS;
                        cek.UPDATED_BY = ppe.UPDATED_BY;
                        cek.POSISI_PPE = ppe.POSISI_PPE;
                        cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();
                        cek.APPROVAL_ORDER = 2;
                        cek.URL_FORM_PLNTMNGR = ppe.URL_FORM_PLNTMNGR;
                    }
                    //update
                    //cek.STATUS = ppe.STATUS;
                    //cek.REMARKS = ppe.REMARKS;
                    //cek.UPDATED_BY = ppe.UPDATED_BY;
                    //cek.POSISI_PPE = ppe.POSISI_PPE;
                    //cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();
                    //cek.APPROVAL_ORDER = 2;
                    //cek.URL_FORM_PLNTMNGR = ppe.URL_FORM_PLNTMNGR;

                    //history ppe
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
        [Route("Upload_CAAB")]
        public IHttpActionResult Upload_CAAB()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                var files = httpRequest.Files;
                var attachmentUrls = new List<string>();
                
                var nomorPPE = httpRequest.Form.GetValues("nomorPPE[]"); 
                var nomorEQP = httpRequest.Form.GetValues("nomorEQP[]"); 

                //if (files.Count > 0)
                if (files.Count > 0 && nomorPPE.Length == files.Count && nomorEQP.Length == files.Count)
                {
                    using (var dbContext = new DB_Plant_PPEDataContext())
                    {
                        for (int i = 0; i < files.Count; i++)
                        {
                            var postedFile = files[i];
                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(postedFile.FileName);
                            var folderPath = HttpContext.Current.Server.MapPath("~/Content/UploadCAAB");

                            if (!Directory.Exists(folderPath))
                            {
                                Directory.CreateDirectory(folderPath);
                            }

                            var filePath = Path.Combine(folderPath, fileName);
                            if (File.Exists(filePath))
                            {
                                return Ok(new { Remarks = false });
                            }
                            
                            using (var fileStream = File.Create(filePath))
                            {
                                postedFile.InputStream.CopyTo(fileStream);
                                fileStream.Flush();
                            }
                            File.SetAttributes(filePath, FileAttributes.Normal);

                            var attachmentUrl = Url.Content("~/Content/UploadCAAB/" + fileName);
                            attachmentUrls.Add(attachmentUrl);
                            for (int j = 0; j < nomorEQP.Length; j++)
                            {
                                var existingPPE = dbContext.TBL_T_PPEs.FirstOrDefault(p => p.PPE_NO == nomorPPE[j] && p.EQUIP_NO == nomorEQP[i]);
                                if (existingPPE != null)
                                {
                                    existingPPE.UPLOAD_FORM_CAAB = attachmentUrl;
                                }
                            }
                        }
                        dbContext.SubmitChanges();
                    }

                    return Ok(new { Remarks = true, AttachmentUrls = attachmentUrls });
                }
                else
                {
                    return Ok(new { Remarks = true });
                }
                
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("Reject_Section_Head")]
        public IHttpActionResult Reject_Section_Head(TBL_T_PPE[] param)
        {
            try
            {
                foreach (var ppe in param)
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
        [Route("Reject_PPE_PlantManager")]
        public IHttpActionResult Reject_PPE_PlantManager(TBL_T_PPE[] param)
        {
            try
            {
                foreach (var ppe in param)
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
        [Route("Reject_Approval")]
        public IHttpActionResult Reject_Approval(TBL_T_PPE[] param)
        {
            try
            {
                foreach (var ppe in param)
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
                }

                db.SubmitChanges();

                return Ok(new { Remarks = true });
            }
            catch (Exception e)
            {
                return Ok(new { Remarks = false, Message = e });
            }
        }
        
        [HttpGet]
        [Route("Get_PPE_EquipmentPart")]
        public IHttpActionResult Get_PPE_EquipmentPart(string ppe)
        {
            try
            {

                var data = db.TBL_T_PPEs.Where(a => a.PPE_NO == ppe && a.STATUS == "PLANT MANAGER APPROVED").ToList();

                return Ok(new { Data = data, Total = data.Count() });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("Approve_Detail_DeptHead")]
        public IHttpActionResult Approve_Detail_DeptHead(TBL_T_PPE[] param)
        {
            try
            {
                foreach (var ppe in param)
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
                    cek.URL_FORM_PM_PENGIRIM = ppe.URL_FORM_PM_PENGIRIM;

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
        [Route("DetailDeptHead_Upload_CAAB")]
        public IHttpActionResult DetailDeptHead_Upload_CAAB()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                var files = httpRequest.Files;
                var attachmentUrls = new List<string>();

                var nomorPPE = httpRequest.Form.GetValues("nomorPPE[]");
                var nomorEQP = httpRequest.Form.GetValues("nomorEQP[]");

                //if (files.Count > 0)
                if (files.Count > 0 && nomorPPE.Length == files.Count && nomorEQP.Length == files.Count)
                {
                    using (var dbContext = new DB_Plant_PPEDataContext())
                    {
                        for (int i = 0; i < files.Count; i++)
                        {
                            var postedFile = files[i];
                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(postedFile.FileName);
                            var folderPath = HttpContext.Current.Server.MapPath("~/Content/UploadCAAB");

                            if (!Directory.Exists(folderPath))
                            {
                                Directory.CreateDirectory(folderPath);
                            }

                            var filePath = Path.Combine(folderPath, fileName);
                            if (File.Exists(filePath))
                            {
                                return Ok(new { Remarks = false });
                            }

                            using (var fileStream = File.Create(filePath))
                            {
                                postedFile.InputStream.CopyTo(fileStream);
                                fileStream.Flush();
                            }
                            File.SetAttributes(filePath, FileAttributes.Normal);

                            var attachmentUrl = Url.Content("~/Content/UploadCAAB/" + fileName);
                            attachmentUrls.Add(attachmentUrl);
                            for (int j = 0; j < nomorEQP.Length; j++)
                            {
                                //upload path ke tbl tansaksi
                                var existingPPE = dbContext.TBL_T_PPEs.FirstOrDefault(p => p.PPE_NO == nomorPPE[j] && p.EQUIP_NO == nomorEQP[i]);
                                if (existingPPE != null)
                                {
                                    existingPPE.UPLOAD_FORM_CAAB = attachmentUrl;
                                }
                            }
                        }
                        dbContext.SubmitChanges();
                    }

                    return Ok(new { Remarks = true, AttachmentUrls = attachmentUrls });
                }
                else
                {
                    return Ok(new { Remarks = true });
                }

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
