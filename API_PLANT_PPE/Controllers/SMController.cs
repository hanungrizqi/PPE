using API_PLANT_PPE.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace API_PLANT_PPE.Controllers
{
    [RoutePrefix("api/SM")]
    public class SMController : ApiController
    {
        DB_Plant_PPEDataContext db = new DB_Plant_PPEDataContext();

        [HttpGet]
        [Route("Get_PPE_EquipmentPart")]
        public IHttpActionResult Get_PPE_EquipmentPart(string ppe)
        {
            try
            {

                var data = db.VW_T_PPEs.Where(a => a.PPE_NO == ppe && a.STATUS == "DIVISION HEAD OPR APPROVED").ToList();

                return Ok(new { Data = data, Total = data.Count() });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("Approve_SM")]
        public IHttpActionResult Approve_SM(TBL_T_PPE[] param)
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
                    cek.UPDATED_BY = ppe.UPDATED_BY;
                    cek.POSISI_PPE = ppe.POSISI_PPE;
                    cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();
                    cek.APPROVAL_ORDER = ppe.APPROVAL_ORDER;
                    cek.DATE_RECEIVED_SM = ppe.DATE_RECEIVED_SM;

                    //history ppe
                    TBL_H_APPROVAL_PPE his = new TBL_H_APPROVAL_PPE();

                    if (cek.PPE_NO != null && cek.POSISI_PPE != "")
                    {
                        his.Ppe_NO = ppe.PPE_NO;
                        his.Equip_No = ppe.EQUIP_NO;
                        //his.Posisi_Ppe = old_posisi;
                        his.Posisi_Ppe = "SM";
                        his.Approval_Order = ppe.APPROVAL_ORDER;
                        his.Approved_Date = DateTime.UtcNow.ToLocalTime();
                        his.Approved_By = ppe.UPDATED_BY;

                        if (ppe.STATUS != "REJECT")
                        {
                            db.TBL_H_APPROVAL_PPEs.InsertOnSubmit(his);
                        }
                    }
                    if (ppe.STATUS != "REJECT")
                    {
                        TBL_H_EQUIPNO_DONE equipDone = new TBL_H_EQUIPNO_DONE();

                        equipDone.Equip_No = ppe.EQUIP_NO;
                        equipDone.District_From = ppe.DISTRICT_FROM;
                        equipDone.District_To = ppe.DISTRICT_TO;
                        equipDone.Updated_By = ppe.UPDATED_BY;
                        equipDone.Date = ppe.DATE_RECEIVED_SM;

                        db.TBL_H_EQUIPNO_DONEs.InsertOnSubmit(equipDone);
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
        [Route("Upload_BA")]
        public IHttpActionResult Upload_BA()
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
                            var folderPath = HttpContext.Current.Server.MapPath("~/Content/UploadBA");

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

                            var attachmentUrl = Url.Content("~/Content/UploadBA/" + fileName);
                            attachmentUrls.Add(attachmentUrl);
                            for (int j = 0; j < nomorEQP.Length; j++)
                            {
                                //upload path ke tbl tansaksi
                                var existingPPE = dbContext.TBL_T_PPEs.FirstOrDefault(p => p.PPE_NO == nomorPPE[j] && p.EQUIP_NO == nomorEQP[i]);
                                if (existingPPE != null)
                                {
                                    existingPPE.BERITA_ACARA_SM = attachmentUrl;
                                }
                                //upload path ke tbl history
                                var existingEquip = dbContext.TBL_H_EQUIPNO_DONEs.FirstOrDefault(p => p.Equip_No == nomorEQP[i]);
                                if (existingEquip != null)
                                {
                                    existingEquip.BA = attachmentUrl;
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
