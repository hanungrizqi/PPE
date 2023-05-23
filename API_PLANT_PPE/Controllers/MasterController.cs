using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API_PLANT_PPE.Models;

namespace API_PLANT_PPE.Controllers
{
    [RoutePrefix("api/Master")]
    public class MasterController : ApiController
    {
        DB_Plant_PPEDataContext db = new DB_Plant_PPEDataContext();

        [HttpGet]
        [Route("Get_Jobsite")]
        public IHttpActionResult Get_Jobsite()
        {
            try
            {
                var data = db.VW_M_JOBSITEs.ToList();

                return Ok(new { Data = data, Total = data.Count() });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }



        [HttpGet]
        [Route("Get_JobsiteByUsername")]
        public IHttpActionResult Get_JobsiteByUsername(string username = "")
        {
            try
            {
                var data = db.VW_MSF020s.Where(x => x.ENTITY == username).ToList();

                return Ok(new { Data = data, Total = data.Count() });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
