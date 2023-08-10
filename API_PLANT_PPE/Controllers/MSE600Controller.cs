using API_PLANT_PPE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API_PLANT_PPE.ViewModel;
using System.Data;
using System.Web.Services.Description;

namespace API_PLANT_PPE.Controllers
{
    [RoutePrefix("api/MSE600")]
    public class MSE600Controller : ApiController
    {
        DB_Plant_PPEDataContext db = new DB_Plant_PPEDataContext();

        [HttpPost]
        [Route("Update_Equipment")]
        public IHttpActionResult Update_Equipment(TBL_T_PPE[] param)
        {
            try
            {
                ClsUpdate_MSE600 cls = new ClsUpdate_MSE600();
                ClsUpdate_MSF600_Result result = new ClsUpdate_MSF600_Result();

                foreach (var ppe in param)
                {
                    var dataEquipment = db.TBL_T_PPEs.FirstOrDefault(a => a.EQUIP_NO == ppe.EQUIP_NO);
                    if(dataEquipment != null)
                    {
                        //cls.updatemse600(dataEquipment);
                        result = cls.updatemse600(dataEquipment);
                    }
                }
                //db.SubmitChanges();
                return Ok(new { Remarks = result.Remarks, Message = result.Message });
            }
            catch (Exception ex)
            {
                return Ok(new { Remarks = false, Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("SM_Update")]
        public IHttpActionResult SM_Update(TBL_T_PPE[] param)
        {
            try
            {
                ClsUpdate_MSE600 cls = new ClsUpdate_MSE600();
                ClsUpdate_MSF600_Result result = new ClsUpdate_MSF600_Result();

                foreach (var ppe in param)
                {
                    var dataEquipment = db.TBL_T_PPEs.FirstOrDefault(a => a.EQUIP_NO == ppe.EQUIP_NO);
                    if (dataEquipment != null)
                    {
                        //cls.updatemse600(dataEquipment);
                        result = cls.updatemse600_SM(dataEquipment);
                        //result = cls.SM_ReferenceCodes(dataEquipment);
                    }
                }
                //db.SubmitChanges();
                return Ok(new { Remarks = result.Remarks, Message = result.Message });
            }
            catch (Exception ex)
            {
                return Ok(new { Remarks = false, Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("Update_Equipment_FromDetail")]
        public IHttpActionResult Update_Equipment_FromDetail(TBL_T_PPE ppe)
        {
            try
            {
                ClsUpdate_MSE600 cls = new ClsUpdate_MSE600();
                ClsUpdate_MSF600_Result result = new ClsUpdate_MSF600_Result();

                var dataEquipment = db.TBL_T_PPEs.FirstOrDefault(a => a.EQUIP_NO == ppe.EQUIP_NO);
                if (dataEquipment != null)
                {
                    //cls.updatemse600(dataEquipment);
                    result = cls.updatemse600(dataEquipment);
                }
                //db.SubmitChanges();
                return Ok(new { Remarks = result.Remarks, Message = result.Message });
            }
            catch (Exception ex)
            {
                return Ok(new { Remarks = false, Message = ex.Message });
            }
        }
    }
}
