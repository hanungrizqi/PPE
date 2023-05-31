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
    [RoutePrefix("api/Setting")]
    public class SettingController : ApiController
    {
        DB_Plant_PPEDataContext db = new DB_Plant_PPEDataContext();

        //Setting User
        [HttpPost]
        [Route("Create_User")]
        public IHttpActionResult Create_User(TBL_M_USER param)
        {
            try
            {
                TBL_M_USER tbl = new TBL_M_USER();
                tbl.ID_Role = param.ID_Role;
                tbl.Username = param.Username;

                db.TBL_M_USERs.InsertOnSubmit(tbl);
                db.SubmitChanges();
                return Json(new { Remarks = true });
            }
            catch (Exception ex)
            {
                return Json(new { Remarks = false, Message = ex });
            }
        }

        [HttpGet]
        [Route("Get_UserSetting")]
        public IHttpActionResult Get_UserSetting()
        {
            var data = db.VW_Users.ToList();
            return Ok(new { Data = data });
        }

        [HttpPost]
        [Route("Delete_User")]
        public IHttpActionResult Delete_User(int role, string nrp)
        {
            try
            {
                var data = db.TBL_M_USERs.Where(a => a.ID_Role == role && a.Username == nrp).FirstOrDefault();

                db.TBL_M_USERs.DeleteOnSubmit(data);
                db.SubmitChanges();
                return Json(new { Remarks = true });
            }
            catch (Exception ex)
            {
                return Json(new { Remarks = false, Message = ex });
            }

        }
        //END Setting User
        //Setting Menu
        [HttpGet]
        [Route("Get_Menu/{group}")]
        public IHttpActionResult Get_Menu(int group)
        {
            try
            {
                var data = db.VW_MENUs.Where(a => a.ID_Role == group).ToList();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("Update_Menu")]
        public IHttpActionResult Update_Menu(TBL_M_AKSE param)
        {
            try
            {
                var data = db.TBL_M_AKSEs.Where(a => a.ID_Role == param.ID_Role && a.ID_Menu == param.ID_Menu).FirstOrDefault();
                data.IS_ALLOW = param.IS_ALLOW;

                db.SubmitChanges();
                return Json(new { Remarks = true });
            }
            catch (Exception ex)
            {
                return Json(new { Remarks = false, Message = ex });
            }

        }
        //END Setting Menu
        //Setting Agreement
        [HttpPost]
        [Route("Create_Agreement")]
        public IHttpActionResult Create_Agreement(TBL_M_AGREEMENT param)
        {
            try
            {
                var cek = db.TBL_M_AGREEMENTs.Where(a => a.ID == param.ID).FirstOrDefault();
                if (cek != null)
                {
                    cek.ID = param.ID;
                    cek.CONTENT = param.CONTENT;
                    db.TBL_M_AGREEMENTs.InsertOnSubmit(cek);
                }
                else
                {
                    TBL_M_AGREEMENT tbl = new TBL_M_AGREEMENT();
                    tbl.ID = param.ID;
                    tbl.CONTENT = param.CONTENT;
                    db.TBL_M_AGREEMENTs.InsertOnSubmit(tbl);
                }

                //TBL_M_AGREEMENT tbl = new TBL_M_AGREEMENT();
                //tbl.CONTENT = param.CONTENT;
                
                //db.TBL_M_AGREEMENTs.InsertOnSubmit(tbl);
                db.SubmitChanges();
                return Json(new { Remarks = true });
            }
            catch (Exception ex)
            {
                return Json(new { Remarks = false, Message = ex });
            }
        }

        [HttpGet]
        [Route("Get_Agreement")]
        public IHttpActionResult Get_Agreement()
        {
            try
            {
                var data = db.TBL_M_AGREEMENTs.FirstOrDefault();

                return Ok(new { Data = data.CONTENT });
            }
            catch (Exception ex)
            {
                return Json(new { Remarks = false, Message = ex });
            }
        }


        #region Setting District
        //Setting District
        [HttpPost]
        [Route("Create_District")]
        public IHttpActionResult Create_District(TBL_M_DISTRICT param)
        {
            try
            {
                TBL_M_DISTRICT tbl = new TBL_M_DISTRICT();
                tbl.DSTRCT_CODE = param.DSTRCT_CODE;
                tbl.LOCATION = param.LOCATION;

                db.TBL_M_DISTRICTs.InsertOnSubmit(tbl);
                db.SubmitChanges();
                return Json(new { Remarks = true });
            }
            catch (Exception ex)
            {
                return Json(new { Remarks = false, Message = ex });
            }
        }

        [HttpGet]
        [Route("Get_District")]
        public IHttpActionResult Get_District()
        {
            var data = db.TBL_M_DISTRICTs.ToList();
            return Ok(new { Data = data });
        }


        [HttpPost]
        [Route("Delete_District")]
        public IHttpActionResult Delete_District(int id)
        {
            try
            {
                var data = db.TBL_M_DISTRICTs.Where(a => a.ID == id).FirstOrDefault();

                db.TBL_M_DISTRICTs.DeleteOnSubmit(data);
                db.SubmitChanges();
                return Json(new { Remarks = true });
            }
            catch (Exception ex)
            {
                return Json(new { Remarks = false, Message = ex });
            }

        }

        #endregion


        #region Setting MappingApproval

        [HttpGet]
        [Route("Get_MappingApproval")]
        public IHttpActionResult Get_MappingApproval()
        {
            var data = db.TBL_M_MAPPING_APPROVALs.ToList();
            return Ok(new { Data = data });
        }

        [HttpGet]
        [Route("Get_DistrictLocation")]
        public IHttpActionResult Get_DistrictLocation(String dstrct)
        {
            var data = db.VW_R_DISTRICT_LOCATIONs.Where(a => a.DSTRCT_CODE == dstrct).ToList();
            return Ok(new { Data = data });
        }

        [HttpGet]
        [Route("Get_DistrictMap")]
        public IHttpActionResult Get_DistrictMap()
        {
            var data = db.VW_R_DISTRICT_LOCATIONs.ToList();
            return Ok(new { Data = data });
        }

        [HttpGet]
        [Route("Get_Position")]
        public IHttpActionResult Get_Position()
        {
            var data = db.TBL_M_POSITIONs.ToList();
            return Ok(new { Data = data });
        }


        [HttpGet]
        [Route("Get_PositionById")]
        public IHttpActionResult Get_PositionById(String id)
        {
            var data = db.TBL_M_POSITIONs.Where(a => a.POSITION_ID == id).ToList();
            return Ok(new { Data = data });
        }

        [HttpPost]
        [Route("Create_MappingApproval")]
        public IHttpActionResult Create_MappingApproval(TBL_M_MAPPING_APPROVAL param)
        {
            try
            {
               // var data = db.TBL_M_MAPPING_APPROVALs.Where(a => a.APPROVAL_NO == id).ToList();
               
                TBL_M_MAPPING_APPROVAL  tbl = new TBL_M_MAPPING_APPROVAL();
                tbl.APPROVAL_ACTION = param.APPROVAL_ACTION;
                tbl.APPROVAL_ORDER = param.APPROVAL_ORDER;
                tbl.APPROVAL_FROM = param.APPROVAL_FROM;
                tbl.APPROVAL_TO = param.APPROVAL_TO;
                tbl.LOCATION_FROM= param.LOCATION_FROM;
                tbl.LOCATION_TO = param.LOCATION_TO;
                tbl.CURR_POSITION_ID = param.CURR_POSITION_ID;
                tbl.NEXT_POSITION_ID = param.NEXT_POSITION_ID;
                tbl.CURRENT_STATUS = param.CURRENT_STATUS;
                tbl.APPROVAL_STATUS = param.APPROVAL_STATUS;
                
                db.TBL_M_MAPPING_APPROVALs.InsertOnSubmit(tbl);
                db.SubmitChanges();
                return Json(new { Remarks = true });
            }
            catch (Exception ex)
            {
                return Json(new { Remarks = false, Message = ex });
            }
        }

        [HttpPost]
        [Route("Update_MappingApproval")]
        public IHttpActionResult Update_MappingApproval(TBL_M_MAPPING_APPROVAL param)
        {
            try
            {
                 var tbl = db.TBL_M_MAPPING_APPROVALs.Where(a => a.APPROVAL_NO == param.APPROVAL_NO).FirstOrDefault();

                
                tbl.APPROVAL_NO = param.APPROVAL_NO;
                tbl.APPROVAL_ACTION = param.APPROVAL_ACTION;
                tbl.APPROVAL_ORDER = param.APPROVAL_ORDER;
                tbl.APPROVAL_FROM = param.APPROVAL_FROM;
                tbl.APPROVAL_TO = param.APPROVAL_TO;
                tbl.LOCATION_FROM = param.LOCATION_FROM;
                tbl.LOCATION_TO = param.LOCATION_TO;
                tbl.CURR_POSITION_ID = param.CURR_POSITION_ID;
                tbl.NEXT_POSITION_ID = param.NEXT_POSITION_ID;
                tbl.CURRENT_STATUS = param.CURRENT_STATUS;
                tbl.APPROVAL_STATUS = param.APPROVAL_STATUS;

                //db.TBL_M_MAPPING_APPROVALs.InsertOnSubmit;
                db.SubmitChanges();
                return Json(new { Remarks = true });
            }
            catch (Exception ex)
            {
                return Json(new { Remarks = false, Message = ex });
            }
        }



        [HttpPost]
        [Route("Delete_MappingApproval")]
        public IHttpActionResult Delete_MappingApproval(int id)
        {
            try
            {
                // var data = db.TBL_M_MAPPING_APPROVALs.Where(a => a.APPROVAL_NO == id).ToList();

                var data = db.TBL_M_MAPPING_APPROVALs.Where(a => a.APPROVAL_NO == id).FirstOrDefault();
                db.TBL_M_MAPPING_APPROVALs.DeleteOnSubmit(data);
                db.SubmitChanges();
                
                return Json(new { Remarks = true });
            }
            catch (Exception ex)
            {
                return Json(new { Remarks = false, Message = ex });
            }
        }
        /*[HttpPost]
        [Route("Delete_MappingApproval")]
        public IHttpActionResult Delete_MappingApproval(int id)
        {
            try
            {
                var data = db.TBL_M_MAPPING_APPROVALs.Where(a => a.APPROVAL_NO == id).FirstOrDefault();
                db.TBL_M_MAPPING_APPROVALs.DeleteOnSubmit(data);
                db.SubmitChanges();
                return Ok(new { Remarks = true });
            }
            catch (Exception e)
            {
                return Ok(new { Remarks = false, Message = e });
            }
        }*/

        [HttpGet]
        [Route("Get_MappingbyId")]
        public IHttpActionResult Get_MappingbyId(int ApproveNum)
        {
            var data = db.TBL_M_MAPPING_APPROVALs.Where(a => a.APPROVAL_NO == ApproveNum).ToList();
            return Ok(new { Data = data });
        }


        #endregion
    }
}
