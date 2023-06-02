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
                //string old_posisi = "";
                ////if (param.STATUS == "PLANNER CANCEL")
                ////{
                ////    db.cusp_NotifBacklogCancel(param.NO_BACKLOG);
                ////}

                //var cek = db.TBL_T_PPEs.Where(a => a.PPE_NO == param.PPE_NO).FirstOrDefault();

                //old_posisi = cek.POSISI_PPE;

                ////update
                //cek.STATUS = param.STATUS;
                //cek.REMARKS = param.REMARKS;
                //cek.UPDATED_BY = param.UPDATED_BY;
                //cek.POSISI_PPE = param.POSISI_PPE;
                //cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();

                ////history ppe
                //TBL_H_APPROVAL_PPE his = new TBL_H_APPROVAL_PPE();

                //his.Ppe_NO = param.PPE_NO;
                //his.Posisi_Ppe = old_posisi;
                //his.Approved_Date = DateTime.UtcNow.ToLocalTime();

                //db.TBL_H_APPROVAL_PPEs.InsertOnSubmit(his);

                //db.SubmitChanges();

                //return Ok(new { Remarks = true });
                foreach (var ppe in param)
                {
                    string old_posisi = "";
                    //if (ppe.STATUS == "PLANNER CANCEL")
                    //{
                    //    db.cusp_NotifBacklogCancel(ppe.NO_BACKLOG);
                    //}

                    var cek = db.TBL_T_PPEs.FirstOrDefault(a => a.PPE_NO == ppe.PPE_NO && a.EQUIP_NO == ppe.EQUIP_NO);

                    old_posisi = cek.POSISI_PPE;

                    //update
                    cek.STATUS = ppe.STATUS;
                    cek.REMARKS = ppe.REMARKS;
                    cek.UPDATED_BY = ppe.UPDATED_BY;
                    cek.POSISI_PPE = ppe.POSISI_PPE;
                    cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();

                    //history ppe
                    TBL_H_APPROVAL_PPE his = new TBL_H_APPROVAL_PPE();

                    his.Ppe_NO = ppe.PPE_NO;
                    his.Posisi_Ppe = old_posisi;
                    his.Approved_Date = DateTime.UtcNow.ToLocalTime();

                    //db.TBL_H_APPROVAL_PPEs.InsertOnSubmit(his);
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
    }
}
