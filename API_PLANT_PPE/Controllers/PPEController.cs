using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
                var data = db.TBL_T_PPEs.FirstOrDefault();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("Save_Temporary_PPE")]
        public IHttpActionResult Save_Temporary_PPE(TBL_H_PPE_TEMPORARY param)
        {
            try
            {
                var cek = db.TBL_H_PPE_TEMPORARies.Where(a => a.PPE_NO == param.PPE_NO).FirstOrDefault();
                
                if (cek != null)
                {
                    cek.PPE_NO = param.PPE_NO;
                    cek.DATE = param.DATE;
                    cek.DISTRICT_FROM = param.DISTRICT_FROM;
                    cek.DISTRICT_TO = param.DISTRICT_TO;
                    cek.LOC_FROM = param.LOC_FROM;
                    cek.LOC_TO = param.LOC_TO;
                    cek.EQUIP_NO = param.EQUIP_NO;
                    cek.PPE_DESC = param.PPE_DESC;
                    cek.EGI = param.EGI;
                    cek.EQUIP_CLASS = param.EQUIP_CLASS;
                    cek.SERIAL_NO = param.SERIAL_NO;
                    cek.REMARKS = param.REMARKS;
                    cek.PATH_ATTACHMENT = param.PATH_ATTACHMENT;

                    db.TBL_H_PPE_TEMPORARies.InsertOnSubmit(cek);
                    
                }
                else
                {
                    TBL_H_PPE_TEMPORARY tbl = new TBL_H_PPE_TEMPORARY();
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
                    tbl.PATH_ATTACHMENT = param.PATH_ATTACHMENT;
                    
                    db.TBL_H_PPE_TEMPORARies.InsertOnSubmit(tbl);
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
        [Route("Get_TemporaryPPE")]
        public IHttpActionResult Get_TemporaryPPE()
        {
            try
            {
                db.CommandTimeout = 120;
                var data = db.TBL_H_PPE_TEMPORARies.FirstOrDefault();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
