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
                var cek = db.TBL_M_AGREEMENTs.Where(a => a.CONTENT == param.CONTENT).FirstOrDefault();
                if (cek != null)
                {
                    cek.CONTENT = param.CONTENT;
                    db.TBL_M_AGREEMENTs.InsertOnSubmit(cek);
                }
                else
                {
                    TBL_M_AGREEMENT tbl = new TBL_M_AGREEMENT();
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
        
    }
}
