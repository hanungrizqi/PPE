using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
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

        [HttpGet]
        [Route("Get_Employee")]
        public IHttpActionResult Get_Employee()
        {
            try
            {
                var data = db.VW_KARYAWAN_ALLs.ToList();

                return Ok(new { Data = data, Total = data.Count() });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Get_Employee/{id}")]
        public IHttpActionResult Get_Employee(string id)
        {
            try
            {
                var data = db.VW_KARYAWAN_ALLs.Where(a => a.EMPLOYEE_ID == id).FirstOrDefault();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        //Keperluan Transaksi
        [HttpGet]
        [Route("getDistrict")]
        public IHttpActionResult getDistrict()
        {
            try
            {
                var data = db.VW_DISTRICTs.ToList();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("getLoc/{dstrct}")]
        public IHttpActionResult getLoc(string dstrct = "")
        {
            try
            {
                var data = db.VW_R_DISTRICT_LOCATIONs.Where(a => a.DSTRCT_CODE == dstrct).ToList();

                return Ok(new { Data = data, Total = data.Count() });

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Get_EqNumber/{site}/{nrp}")]
        public IHttpActionResult Get_EqNumber(string site = "", string nrp = "")
        {
            try
            {
                if (site == "INDE")
                {
                    //valisasi wheel
                    var karyawan_wheel = db.VW_KARYAWAN_PLANT_WHEELs.Where(x => x.EMPLOYEE_ID == nrp).FirstOrDefault();
                    var karyawan_hauling = db.VW_KARYAWAN_PLANT_HAULINGs.Where(x => x.EMPLOYEE_ID == nrp).FirstOrDefault();
                    var karyawan_track = db.VW_KARYAWAN_PLANT_TRACKs.Where(x => x.EMPLOYEE_ID == nrp).FirstOrDefault();

                    if (karyawan_hauling != null)
                    {
                        var data = db.VW_R_EQ_NUMBER_HAULINGs.Where(a => a.DSTRCT_CODE == site).ToList();

                        return Ok(new { Data = data, Total = data.Count() });
                    }
                    else if (karyawan_track != null)
                    {
                        var data = db.VW_R_EQ_NUMBER_TRACKs.Where(a => a.DSTRCT_CODE == site).ToList();

                        return Ok(new { Data = data, Total = data.Count() });
                    }
                    else if (karyawan_wheel != null)
                    {
                        var data = db.VW_R_EQ_NUMBER_WHEELs.Where(a => a.DSTRCT_CODE == site).ToList();

                        return Ok(new { Data = data, Total = data.Count() });
                    }
                    else
                    {
                        var data = db.VW_R_EQ_NUMBERs.Where(a => a.DSTRCT_CODE == site).ToList();

                        return Ok(new { Data = data, Total = data.Count() });
                    }
                }
                else
                {
                    var data = db.VW_R_EQ_NUMBERs.Where(a => a.DSTRCT_CODE == site).ToList();

                    return Ok(new { Data = data, Total = data.Count() });
                }

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
