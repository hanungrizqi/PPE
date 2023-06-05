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

        [HttpPost]
        [Route("Upload_CAAB")]
        public IHttpActionResult Upload_CAAB()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                var files = httpRequest.Files;
                var attachmentUrls = new List<string>();
                var nomorPPE = HttpContext.Current.Request.Form["nomorPPE"];
                if (files.Count > 0)
                {
                    foreach (string file in files)
                    {
                        var postedFile = files[file];
                        var fileName = postedFile.FileName;
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

                        // Simpan file dan atur atribut menjadi FileAttributes.Normal
                        using (var fileStream = File.Create(filePath))
                        {
                            postedFile.InputStream.CopyTo(fileStream);
                            fileStream.Flush();
                        }
                        File.SetAttributes(filePath, FileAttributes.Normal);

                        var attachmentUrl = Url.Content("~/Content/UploadCAAB/" + fileName);
                        attachmentUrls.Add(attachmentUrl);
                    }

                    using (var dbContext = new DB_Plant_PPEDataContext())
                    {
                        foreach (var attachmentUrl in attachmentUrls)
                        {
                            // Lakukan pengecekan disini jika nomorPPE == PPE_NO
                            var nomorrPPE = HttpContext.Current.Request.Form["nomorPPE"];
                            var existingPPE = dbContext.TBL_T_PPEs.FirstOrDefault(p => p.PPE_NO == nomorrPPE);
                            if (existingPPE != null)
                            {
                                existingPPE.UPLOAD_FORM_CAAB = attachmentUrl;
                                //dbContext.TBL_T_PPEs.InsertOnSubmit(existingPPE);
                                //return Ok(new { Remarks = false, Message = "PPE with the given number already exists." });
                            }

                            //var caab = new TBL_T_PPE
                            //{
                            //    UPLOAD_FORM_CAAB = attachmentUrl,
                            //    PPE_NO = nomorPPE
                            //};

                            //dbContext.TBL_T_PPEs.InsertOnSubmit(caab);
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

        //[HttpPost]
        //[Route("Approve_PPE_nCAAB")]
        //public IHttpActionResult Approve_PPE_nCAAB([FromBody] TBL_T_PPE[] param)
        //{
        //    try
        //    {
        //        foreach (var ppe in param)
        //        {
        //            string old_posisi = "";
        //            var cek = db.TBL_T_PPEs.FirstOrDefault(a => a.PPE_NO == ppe.PPE_NO && a.EQUIP_NO == ppe.EQUIP_NO);

        //            old_posisi = cek.POSISI_PPE;

        //            //untuk upload caab
        //            var httpRequest = HttpContext.Current.Request;
        //            var files = httpRequest.Files;
        //            var attachmentUrls = new List<string>();
        //            if (files.Count > 0)
        //            {
        //                foreach (string file in files)
        //                {
        //                    var postedFile = files[file];
        //                    var fileName = postedFile.FileName;
        //                    var folderPath = HttpContext.Current.Server.MapPath("~/Content/UploadCAAB");

        //                    if (!Directory.Exists(folderPath))
        //                    {
        //                        Directory.CreateDirectory(folderPath);
        //                    }

        //                    var filePath = Path.Combine(folderPath, fileName);
        //                    if (File.Exists(filePath))
        //                    {
        //                        return Ok(new { Remarks = false });
        //                    }

        //                    // Simpan file dan atur atribut menjadi FileAttributes.Normal
        //                    using (var fileStream = File.Create(filePath))
        //                    {
        //                        postedFile.InputStream.CopyTo(fileStream);
        //                        fileStream.Flush();
        //                    }
        //                    File.SetAttributes(filePath, FileAttributes.Normal);

        //                    var attachmentUrl = Url.Content("~/Content/UploadCAAB/" + fileName);
        //                    attachmentUrls.Add(attachmentUrl);
        //                }

        //                //update
        //                cek.STATUS = ppe.STATUS;
        //                cek.REMARKS = ppe.REMARKS;
        //                cek.UPDATED_BY = ppe.UPDATED_BY;
        //                cek.POSISI_PPE = ppe.POSISI_PPE;
        //                cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();
        //                cek.UPLOAD_FORM_CAAB = attachmentUrls[0];

        //                //history ppe
        //                TBL_H_APPROVAL_PPE his = new TBL_H_APPROVAL_PPE();

        //                his.Ppe_NO = ppe.PPE_NO;
        //                his.Posisi_Ppe = old_posisi;
        //                his.Approved_Date = DateTime.UtcNow.ToLocalTime();

        //                //db.TBL_H_APPROVAL_PPEs.InsertOnSubmit(his);
        //                if (ppe.STATUS != "REJECT")
        //                {
        //                    db.TBL_H_APPROVAL_PPEs.InsertOnSubmit(his);
        //                }

        //            }
        //            else
        //            {
        //                return BadRequest("No file found in the request.");
        //            }
        //        }

        //        db.SubmitChanges();
        //        return Ok(new { Remarks = true });
        //    }
        //    catch (Exception e)
        //    {
        //        return Ok(new { Remarks = false, Message = e });
        //    }
        //}
    }
}
