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
                var data = db.TBL_T_PPEs.OrderByDescending(a => a.PPE_NO).FirstOrDefault();

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
                //var cek = db.TBL_T_PPEs.Where(a => a.PPE_NO == param.PPE_NO).FirstOrDefault();

                //if (cek != null)
                //{
                //    cek.PPE_NO = param.PPE_NO;
                //    cek.DATE = param.DATE;
                //    cek.DISTRICT_FROM = param.DISTRICT_FROM;
                //    cek.DISTRICT_TO = param.DISTRICT_TO;
                //    cek.LOC_FROM = param.LOC_FROM;
                //    cek.LOC_TO = param.LOC_TO;
                //    cek.EQUIP_NO = param.EQUIP_NO;
                //    cek.PPE_DESC = param.PPE_DESC;
                //    cek.EGI = param.EGI;
                //    cek.EQUIP_CLASS = param.EQUIP_CLASS;
                //    cek.SERIAL_NO = param.SERIAL_NO;
                //    cek.REMARKS = param.REMARKS;
                //    cek.STATUS = param.STATUS;
                //    cek.CREATED_DATE = param.CREATED_DATE;
                //    cek.CREATED_BY = param.CREATED_BY;
                //    cek.UPDATED_DATE = param.UPDATED_DATE;
                //    cek.UPDATED_BY = param.UPDATED_BY;
                //    cek.APPROVAL_NO = param.APPROVAL_NO;
                //    cek.PATH_ATTACHMENT = param.PATH_ATTACHMENT;

                //    db.TBL_T_PPEs.InsertOnSubmit(cek);

                //}
                //else
                //{
                //    TBL_T_PPE tbl = new TBL_T_PPE();
                //    tbl.PPE_NO = param.PPE_NO;
                //    tbl.DATE = param.DATE;
                //    tbl.DISTRICT_FROM = param.DISTRICT_FROM;
                //    tbl.DISTRICT_TO = param.DISTRICT_TO;
                //    tbl.LOC_FROM = param.LOC_FROM;
                //    tbl.LOC_TO = param.LOC_TO;
                //    tbl.EQUIP_NO = param.EQUIP_NO;
                //    tbl.PPE_DESC = param.PPE_DESC;
                //    tbl.EGI = param.EGI;
                //    tbl.EQUIP_CLASS = param.EQUIP_CLASS;
                //    tbl.SERIAL_NO = param.SERIAL_NO;
                //    tbl.REMARKS = param.REMARKS;
                //    tbl.STATUS = param.STATUS;
                //    tbl.CREATED_DATE = DateTime.Now;
                //    tbl.CREATED_BY = param.CREATED_BY;
                //    tbl.UPDATED_DATE = param.UPDATED_DATE;
                //    tbl.UPDATED_BY = param.UPDATED_BY;
                //    tbl.APPROVAL_NO = param.APPROVAL_NO;
                //    tbl.PATH_ATTACHMENT = param.PATH_ATTACHMENT;

                //    db.TBL_T_PPEs.InsertOnSubmit(tbl);
                //}
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
                    tbl.STATUS = param.STATUS;
                    tbl.CREATED_DATE = DateTime.Now;
                    tbl.CREATED_BY = param.CREATED_BY;
                    tbl.UPDATED_DATE = param.UPDATED_DATE;
                    tbl.UPDATED_BY = param.UPDATED_BY;
                    tbl.APPROVAL_NO = param.APPROVAL_NO;
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
                    var filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/AttachmentFile"), fileName);
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
    }
}
