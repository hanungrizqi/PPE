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

                    //update
                    cek.STATUS = ppe.STATUS;
                    cek.REMARKS = ppe.REMARKS;
                    cek.UPDATED_BY = ppe.UPDATED_BY;
                    cek.POSISI_PPE = ppe.POSISI_PPE;
                    cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();
                    cek.APPROVAL_ORDER = 2;
                    cek.URL_FORM_PLNTMNGR = ppe.URL_FORM_PLNTMNGR;

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

                //var nomorPPE = HttpContext.Current.Request.Form["nomorPPE"];
                //var nomorEQP = HttpContext.Current.Request.Form["nomorEQP"];
                //if (files.Count > 0)
                //{
                //    foreach (string file in files)
                //    {
                //        var postedFile = files[file];
                //        //var fileName = postedFile.FileName + nomorEQP;
                //        //var fileName = nomorPPE + nomorEQP;
                //        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(postedFile.FileName);
                //        var folderPath = HttpContext.Current.Server.MapPath("~/Content/UploadCAAB");

                //        if (!Directory.Exists(folderPath))
                //        {
                //            Directory.CreateDirectory(folderPath);
                //        }

                //        var filePath = Path.Combine(folderPath, fileName);
                //        if (File.Exists(filePath))
                //        {
                //            return Ok(new { Remarks = false });
                //        }

                //        // Simpan file dan atur atribut menjadi FileAttributes.Normal
                //        using (var fileStream = File.Create(filePath))
                //        {
                //            postedFile.InputStream.CopyTo(fileStream);
                //            fileStream.Flush();
                //        }
                //        File.SetAttributes(filePath, FileAttributes.Normal);

                //        var attachmentUrl = Url.Content("~/Content/UploadCAAB/" + fileName);
                //        attachmentUrls.Add(attachmentUrl);
                //    }

                //    using (var dbContext = new DB_Plant_PPEDataContext())
                //    {
                //        foreach (var attachmentUrl in attachmentUrls)
                //        {
                //            // Lakukan pengecekan disini jika nomorPPE == PPE_NO
                //            var nomorrPPE = HttpContext.Current.Request.Form["nomorPPE"];
                //            var existingPPE = dbContext.TBL_T_PPEs.FirstOrDefault(p => p.PPE_NO == nomorrPPE);
                //            if (existingPPE != null)
                //            {
                //                existingPPE.UPLOAD_FORM_CAAB = attachmentUrl;
                //            }
                //        }

                //        dbContext.SubmitChanges();
                //    }

                //    return Ok(new { Remarks = true, AttachmentUrls = attachmentUrls });
                //}
                //else
                //{
                //    return Ok(new { Remarks = true });
                //}

                // Mendapatkan nilai dari form dengan indeks yang sesuai
                var nomorPPE = httpRequest.Form.GetValues("nomorPPE[]"); // Menggunakan "nomorPPE[]" untuk mendapatkan semua nilai
                var nomorEQP = httpRequest.Form.GetValues("nomorEQP[]"); // Menggunakan "nomorEQP[]" untuk mendapatkan semua nilai

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

                            // Simpan file dan atur atribut menjadi FileAttributes.Normal
                            using (var fileStream = File.Create(filePath))
                            {
                                postedFile.InputStream.CopyTo(fileStream);
                                fileStream.Flush();
                            }
                            File.SetAttributes(filePath, FileAttributes.Normal);

                            var attachmentUrl = Url.Content("~/Content/UploadCAAB/" + fileName);
                            attachmentUrls.Add(attachmentUrl);

                            //using (var dbContext = new DB_Plant_PPEDataContext())
                            //{
                            // Lakukan pengecekan disini jika nomorPPE == PPE_NO
                            //var existingPPE = dbContext.TBL_T_PPEs.FirstOrDefault(p => p.PPE_NO == nomorEQP[i]);
                            //if (existingPPE != null)
                            //{
                            //    existingPPE.UPLOAD_FORM_CAAB = attachmentUrl;
                            //}

                            //dbContext.SubmitChanges();
                            for (int j = 0; j < nomorEQP.Length; j++)
                            {
                                // Lakukan pengecekan disini jika nomorPPE[j] == PPE_NO
                                //var existingPPE = dbContext.TBL_T_PPEs.FirstOrDefault(p => p.PPE_NO == nomorPPE[j] && p.EQUIP_NO == nomorEQP[j]);
                                var existingPPE = dbContext.TBL_T_PPEs.FirstOrDefault(p => p.PPE_NO == nomorPPE[j] && p.EQUIP_NO == nomorEQP[i]);
                                if (existingPPE != null)
                                {
                                    existingPPE.UPLOAD_FORM_CAAB = attachmentUrl;
                                    //existingPPE.UPLOAD_FORM_CAAB = attachmentUrls[j];
                                }
                            }

                            //dbContext.SubmitChanges();
                            //}
                        }
                        dbContext.SubmitChanges();
                    }

                    return Ok(new { Remarks = true, AttachmentUrls = attachmentUrls });
                }
                else
                {
                    return Ok(new { Remarks = true });
                }
                
                //var nomorPPE = httpRequest.Form["nomorPPE"].Split(',');
                //var nomorEQP = httpRequest.Form["nomorEQP"].Split(',');
                //if (files.Count > 0)
                //{
                //    for (int i = 0; i < files.Count; i++)
                //    {
                //        var postedFile = files[i];
                //        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(postedFile.FileName);
                //        var folderPath = HttpContext.Current.Server.MapPath("~/Content/UploadCAAB");

                //        if (!Directory.Exists(folderPath))
                //        {
                //            Directory.CreateDirectory(folderPath);
                //        }

                //        var filePath = Path.Combine(folderPath, fileName);
                //        if (File.Exists(filePath))
                //        {
                //            return Ok(new { Remarks = false });
                //        }

                //        // Simpan file dan atur atribut menjadi FileAttributes.Normal
                //        using (var fileStream = File.Create(filePath))
                //        {
                //            postedFile.InputStream.CopyTo(fileStream);
                //            fileStream.Flush();
                //        }
                //        File.SetAttributes(filePath, FileAttributes.Normal);

                //        var attachmentUrl = Url.Content("~/Content/UploadCAAB/" + fileName);
                //        attachmentUrls.Add(attachmentUrl);
                //    }

                //    using (var dbContext = new DB_Plant_PPEDataContext())
                //    {
                //        for (int i = 0; i < attachmentUrls.Count; i++)
                //        {
                //            var attachmentUrl = attachmentUrls[i];
                //            var existingPPE = dbContext.TBL_T_PPEs.FirstOrDefault(p => p.PPE_NO == nomorPPE[i]);
                //            if (existingPPE != null)
                //            {
                //                existingPPE.UPLOAD_FORM_CAAB = attachmentUrl;
                //            }
                //        }

                //        dbContext.SubmitChanges();
                //    }

                //    return Ok(new { Remarks = true, AttachmentUrls = attachmentUrls });
                //}
                //else
                //{
                //    return Ok(new { Remarks = true });
                //}
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        
    }
}
