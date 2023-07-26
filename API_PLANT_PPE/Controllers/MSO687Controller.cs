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
    [RoutePrefix("api/MSO687")]
    public class MSO687Controller : ApiController
    {
        DB_Plant_PPEDataContext db = new DB_Plant_PPEDataContext();

        [HttpPost]
        [Route("SM_Update_MSO687")]
        public IHttpActionResult SM_Update_MSO687(TBL_T_PPE[] param)
        {
            try
            {
                ClsUpdate_MSO687 cls = new ClsUpdate_MSO687();
                ClsUpdate_MSO687_Result result = new ClsUpdate_MSO687_Result();

                foreach (var ppe in param)
                {
                    var dataEquipment = db.TBL_T_PPEs.FirstOrDefault(a => a.EQUIP_NO == ppe.EQUIP_NO);
                    if (dataEquipment != null)
                    {
                        //cls.updatemse600(dataEquipment);
                        result = cls.updatemse687_SM(dataEquipment);
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
    }
}
