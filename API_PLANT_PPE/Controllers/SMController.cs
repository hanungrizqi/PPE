using API_PLANT_PPE.Models;
using API_PLANT_PPE.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        public string _cloudUploadUrl = ConfigurationManager.AppSettings["fileUploadPath"].ToString();

        [HttpGet]
        [Route("Get_PPE_EquipmentPart")]
        public IHttpActionResult Get_PPE_EquipmentPart(string ppe)
        {
            try
            {

                var data = db.TBL_T_PPEs.Where(a => a.PPE_NO == ppe && a.STATUS == "DIVISION HEAD OPR APPROVED").ToList();

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
                    cek.UPDATED_DATE = DateTime.Now;
                    cek.APPROVAL_ORDER = ppe.APPROVAL_ORDER;
                    cek.DATE_RECEIVED_SM = ppe.DATE_RECEIVED_SM;

                    //history ppe
                    TBL_H_APPROVAL_PPE his = new TBL_H_APPROVAL_PPE();

                    if (cek.PPE_NO != null && cek.POSISI_PPE != "")
                    {
                        his.Ppe_NO = ppe.PPE_NO;
                        his.Equip_No = ppe.EQUIP_NO;
                        his.Posisi_Ppe = "SM";
                        his.Approval_Order = ppe.APPROVAL_ORDER;
                        his.Approved_Date = DateTime.Now;
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
            string documentFolder = null;
            documentFolder = DocumentFolderConstant.BA;

            var baseUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);

            var ctx = HttpContext.Current;
            string root;
            //root = ctx.Server.MapPath($"~/Content{documentFolder}"); //local
            root = _cloudUploadUrl + documentFolder; //azure

            try
            {
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }

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
                            var folderPath = System.IO.Path.Combine(root, fileName);

                            using (var fileStream = File.Create(folderPath))
                            {
                                postedFile.InputStream.CopyTo(fileStream);
                                fileStream.Flush();
                            }
                            File.SetAttributes(folderPath, FileAttributes.Normal);

                            //var filesnm = folderPath.Substring(folderPath.LastIndexOf(@"\Content\")); //local
                            var filesnm = folderPath.Substring(folderPath.LastIndexOf(documentFolder)); //azure

                            var modifiedFilePath = filesnm.Replace('\\', '/');

                            //var attachmentUrl = baseUrl + modifiedFilePath; //local
                            var attachmentUrl = baseUrl + "/FileUpload" + modifiedFilePath; //azure

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
