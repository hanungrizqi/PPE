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

        [HttpPost]
        [Route("Create_UserApprove")]
        public IHttpActionResult Create_UserApprove(TBL_M_USER_APPROVAL param)
        {
            try
            {
                TBL_M_USER_APPROVAL tbl = new TBL_M_USER_APPROVAL();
                tbl.Employee_id = param.Employee_id;
                tbl.Position_id = param.Position_id;
                tbl.Name = param.Name;
                tbl.sub_menu= param.sub_menu;
                tbl.dstrct_code = param.dstrct_code;

                db.TBL_M_USER_APPROVALs.InsertOnSubmit(tbl);
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

        [HttpGet]
        [Route("Get_UserApproveSetting")]
        public IHttpActionResult Get_UserApproveSetting()
        {
            var data = db.TBL_M_USER_APPROVALs.ToList();
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

        [HttpPost]
        [Route("Delete_UserApprove")]
        public IHttpActionResult Delete_UserApprove(int id)
        {
            try
            {
                var data = db.TBL_M_USER_APPROVALs.Where(a => a.id == id).FirstOrDefault();

                db.TBL_M_USER_APPROVALs.DeleteOnSubmit(data);
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
                    db.TBL_M_AGREEMENTs.DeleteOnSubmit(cek);
                }

                TBL_M_AGREEMENT tbl = new TBL_M_AGREEMENT();
                tbl.ID = param.ID;
                tbl.CONTENT = param.CONTENT;
                tbl.LAST_MODIFIED_DATE = DateTime.Now;

                db.TBL_M_AGREEMENTs.InsertOnSubmit(tbl);
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

                return Ok(new { Data = data.CONTENT, DATE = data.LAST_MODIFIED_DATE });
            }
            catch (Exception ex)
            {
                return Json(new { Remarks = false, Message = ex });
            }
        }
        //END Setting Agreement

        //Setting District
        //[HttpPost]
        //[Route("Create_District")]
        //public IHttpActionResult Create_District(TBL_M_DISTRICT param)
        //{
        //    try
        //    {
        //        TBL_M_DISTRICT tbl = new TBL_M_DISTRICT();
        //        tbl.DSTRCT_CODE = param.DSTRCT_CODE;
        //        tbl.LOCATION = param.LOCATION;

        //        db.TBL_M_DISTRICTs.InsertOnSubmit(tbl);
        //        db.SubmitChanges();
        //        return Json(new { Remarks = true });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Remarks = false, Message = ex });
        //    }
        //}

        [HttpGet]
        [Route("Get_AccountProfile")]
        public IHttpActionResult Get_AccountProfile()
        {
            try
            {
                var data = db.TBL_M_ACCOUNT_PROFILEs.OrderBy(a => a.DSTRCT_CODE).ToList();

                return Ok(new { Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new { Remarks = false, Message = ex });
            }
        }

        [HttpPost]
        [Route("Update_Accountprofile")]
        public IHttpActionResult Update_Accountprofile(TBL_M_ACCOUNT_PROFILE param)
        {
            try
            {
                var data = db.TBL_M_ACCOUNT_PROFILEs.Where(a => a.ID == param.ID).FirstOrDefault();
                data.ACCT_PROFILE = param.ACCT_PROFILE;
                data.DSTRCT_CODE = param.DSTRCT_CODE;
                data.DSTRCT_LOC = param.DSTRCT_LOC;
                db.SubmitChanges();
                return Ok(new { Remarks = true });
            }
            catch (Exception e)
            {
                return Ok(new { Remarks = false, Message = e });
            }
        }

        [HttpPost]
        [Route("Create_Accprofile")]
        public IHttpActionResult Create_Accprofile(TBL_M_ACCOUNT_PROFILE param)
        {
            try
            {
                TBL_M_ACCOUNT_PROFILE tbl = new TBL_M_ACCOUNT_PROFILE();
                tbl.ACCT_PROFILE = param.ACCT_PROFILE;
                tbl.DSTRCT_CODE = param.DSTRCT_CODE;
                tbl.DSTRCT_LOC = param.DSTRCT_LOC;

                db.TBL_M_ACCOUNT_PROFILEs.InsertOnSubmit(tbl);
                db.SubmitChanges();
                return Json(new { Remarks = true });
            }
            catch (Exception ex)
            {
                return Json(new { Remarks = false, Message = ex });
            }
        }

        [HttpPost]
        [Route("Delete_Acctprofile")]
        public IHttpActionResult Delete_Acctprofile(int id)
        {
            try
            {
                var data = db.TBL_M_ACCOUNT_PROFILEs.Where(a => a.ID == id).FirstOrDefault();

                db.TBL_M_ACCOUNT_PROFILEs.DeleteOnSubmit(data);
                db.SubmitChanges();
                return Json(new { Remarks = true });
            }
            catch (Exception ex)
            {
                return Json(new { Remarks = false, Message = ex });
            }

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

        [HttpGet]
        [Route("Get_District_Location")]
        public IHttpActionResult Get_District_Location()
        {
            try
            {
                var data = db.TBL_M_DISTRICTs.OrderBy(a => a.TABLE_DESC).ToList();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("Delete_Dist")]
        public IHttpActionResult Delete_Dist(int id)
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

        [HttpPost]
        [Route("Create_Dist")]
        public IHttpActionResult Create_Dist(TBL_M_DISTRICT param)
        {
            try
            {
                TBL_M_DISTRICT tbl = new TBL_M_DISTRICT();
                tbl.TABLE_CODE = param.TABLE_CODE;
                tbl.TABLE_DESC = param.TABLE_DESC;

                db.TBL_M_DISTRICTs.InsertOnSubmit(tbl);
                db.SubmitChanges();
                return Json(new { Remarks = true });
            }
            catch (Exception ex)
            {
                return Json(new { Remarks = false, Message = ex });
            }
        }
    }
}
