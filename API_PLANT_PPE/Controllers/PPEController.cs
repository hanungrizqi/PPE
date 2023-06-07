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
    [RoutePrefix("api/PPE")]
    public class PPEController : ApiController
    {
        DB_Plant_PPEDataContext db = new DB_Plant_PPEDataContext();

        //Get Last No PPE
        [HttpGet]
        [Route("Get_LastNoPPE")]
        public IHttpActionResult Get_LastNoPPE()
        {
            try
            {
                var data = db.TBL_T_PPEs.OrderByDescending(a => a.ID).FirstOrDefault();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("Create_PPE")]
        public IHttpActionResult Create_PPE(List<TBL_T_PPE> ppeList)
        {
            try
            {
                foreach (var param in ppeList)
                {
                    TBL_T_PPE tbl = new TBL_T_PPE();
                    tbl.PPE_NO = param.PPE_NO;
                    tbl.DATE = param.DATE;
                    tbl.DISTRICT_FROM = param.DISTRICT_FROM;
                    tbl.DISTRICT_TO = param.DISTRICT_TO;
                    tbl.LOC_FROM = param.LOC_FROM;
                    tbl.LOC_TO = param.LOC_TO;
                    tbl.EQUIP_NO = param.EQUIP_NO;
                    tbl.PPE_DESC = param.PPE_DESC;
                    tbl.EGI = param.EGI;
                    tbl.EQUIP_CLASS = param.EQUIP_CLASS;
                    tbl.SERIAL_NO = param.SERIAL_NO;
                    tbl.REMARKS = param.REMARKS;
                    tbl.POSISI_PPE = param.POSISI_PPE;
                    tbl.STATUS = param.STATUS;
                    tbl.CREATED_DATE = DateTime.Now;
                    tbl.CREATED_BY = param.CREATED_BY;
                    tbl.UPDATED_DATE = param.UPDATED_DATE;
                    tbl.UPDATED_BY = param.UPDATED_BY;
                    tbl.PATH_ATTACHMENT = param.PATH_ATTACHMENT;

                    db.TBL_T_PPEs.InsertOnSubmit(tbl);
                    //db.GetTable<TBL_T_PPE>().InsertAllOnSubmit(ppeList);
                    db.SubmitChanges();
                }
                //db.SubmitChanges();
                return Ok(new { Remarks = true });
            }
            catch (Exception e)
            {
                return Ok(new { Remarks = false, Message = e });
            }
        }

        [HttpPost]
        [Route("UploadAttachment")]
        public IHttpActionResult UploadAttachment()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    var file = httpRequest.Files[0];
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    //tambah ini kalo pengin buat folder otomatis.....
                    var folderPath = HttpContext.Current.Server.MapPath("~/Content/AttachmentFile");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    var filePath = Path.Combine(folderPath, fileName); //pake filepath ini kalo buat foldernya otomatis
                    //var filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/AttachmentFile"), fileName);
                    file.SaveAs(filePath);

                    var attachmentUrl = Url.Content("~/Content/AttachmentFile/" + fileName);
                    return Ok(new { AttachmentUrl = attachmentUrl });
                }
                else
                {
                    return BadRequest("No file found in the request.");
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
        
        [HttpGet]
        [Route("Get_ListApprovalPPE")]
        public IHttpActionResult Get_ListApprovalPPE()
        {
            try
            {
                db.CommandTimeout = 120;
                var data = db.VW_T_PPEs.Where(a => a.POSISI_PPE == "Sect. Head" && a.STATUS != "REJECT").ToList();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Get_ListApprovalPM_PPE")]
        public IHttpActionResult Get_ListApprovalPM_PPE()
        {
            try
            {
                db.CommandTimeout = 120;
                var data = db.VW_T_PPEs.Where(a => a.POSISI_PPE == "Plant Manager" && a.STATUS != "REJECT").ToList();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Get_ListApprovalPDH_PPE")]
        public IHttpActionResult Get_ListApprovalPDH_PPE()
        {
            try
            {
                db.CommandTimeout = 120;
                var data = db.VW_T_PPEs.Where(a => a.POSISI_PPE == "Plant Dept. Head" && a.STATUS != "REJECT").ToList();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Get_ListApprovalProjMan_PPE")]
        public IHttpActionResult Get_ListApprovalProjMan_PPE()
        {
            try
            {
                db.CommandTimeout = 120;
                var data = db.VW_T_PPEs.Where(a => a.POSISI_PPE == "Project Manager" && a.STATUS != "REJECT").ToList();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        //Get Detail PPE
        [HttpGet]
        [Route("Get_PPEDetail/{idppe}")]
        public IHttpActionResult Get_PPEDetail(int idppe)
        {
            try
            {
                db.CommandTimeout = 120;
                var data = db.VW_T_PPEs.Where(a => a.ID == idppe).FirstOrDefault();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
