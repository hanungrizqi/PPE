using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EllipseWebServicesClient;
using System.Configuration;
using API_PLANT_PPE.EquipmentServices;
using API_PLANT_PPE.RefCodesService;
using API_PLANT_PPE.EquipmentReferenceService;
using API_PLANT_PPE.Models;
using System.Web.Services.Description;

namespace API_PLANT_PPE.ViewModel
{
    public class ClsUpdate_MSE600
    {
        public ClsUpdate_MSF600_Result updatemse600(TBL_T_PPE dataEquipment)
        {
            ClsUpdate_MSF600_Result cls = new ClsUpdate_MSF600_Result();
            try
            {
                EquipmentService i_obj_services = new EquipmentService();

                EquipmentServices.OperationContext i_obj_context = new EquipmentServices.OperationContext();
                EquipmentServiceModifyRequestDTO i_obj_equipment_dto1 = new EquipmentServiceModifyRequestDTO();

                EquipmentServiceReadRequestDTO i_obj_equipment_request = new EquipmentServiceReadRequestDTO();
                EquipmentServiceReadReplyDTO i_obj_equipment_result = new EquipmentServiceReadReplyDTO();
                i_obj_equipment_request.equipmentNo = dataEquipment.EQUIP_NO;
                i_obj_equipment_request.equipmentRef = dataEquipment.EQUIP_NO;

                string str_username = ConfigurationManager.AppSettings["username"].ToString();
                string str_password = ConfigurationManager.AppSettings["password"].ToString();
                string str_posisi = ConfigurationManager.AppSettings["pos_id"].ToString();
                ClientConversation.authenticate(str_username, str_password);

                i_obj_context.district = dataEquipment.DISTRICT_FROM;
                i_obj_context.position = str_posisi;

                i_obj_equipment_result = i_obj_services.read(i_obj_context, i_obj_equipment_request);

                i_obj_equipment_dto1.equipmentNo = dataEquipment.EQUIP_NO;
                i_obj_equipment_dto1.costingFlag = "W";
                i_obj_equipment_dto1.districtCode = dataEquipment.DISTRICT_TO;
                i_obj_equipment_dto1.equipmentLocation = dataEquipment.LOC_TO;
                i_obj_equipment_dto1.poNo = "-";
                i_obj_equipment_dto1.equipmentClassif3 = "SC";
                //i_obj_equipment_dto1.equipmentClassif3 = i_obj_equipment_result.equipmentClassif3;
                
                i_obj_services.modify(i_obj_context, i_obj_equipment_dto1);
                ClsUpdate_MSF600_Result result = updatemse600_ReferenceCOdes(dataEquipment);
                return result;
                
                //cls.Remarks = true;
                //cls.Message = "Update Equipment MSF600 Berhasil";
                //return (cls);
            }
            catch (Exception ex)
            {
                cls.Remarks = false;
                cls.Message = ex.Message;
                return (cls);
            }
        }

        public ClsUpdate_MSF600_Result updatemse600_ReferenceCOdes(TBL_T_PPE dataEquipment)
        {
            ClsUpdate_MSF600_Result cls = new ClsUpdate_MSF600_Result();
            try
            {
                RefCodesService.RefCodesService i_obj_services = new RefCodesService.RefCodesService();
                RefCodesService.OperationContext i_obj_context = new RefCodesService.OperationContext();
                RefCodesServiceModifyRequestDTO i_obj_equipment_dto1 = new RefCodesServiceModifyRequestDTO();
                
                RefCodesServiceRetrieveRequestDTO i_obj_request = new RefCodesServiceRetrieveRequestDTO();
                RefCodesServiceRetrieveReplyCollectionDTO i_obj_result = new RefCodesServiceRetrieveReplyCollectionDTO();
                RefCodesServiceRetrieveRequiredAttributesDTO rerquired = new RefCodesServiceRetrieveRequiredAttributesDTO();

                string str_username = ConfigurationManager.AppSettings["username"].ToString();
                string str_password = ConfigurationManager.AppSettings["password"].ToString();
                string str_posisi = ConfigurationManager.AppSettings["pos_id"].ToString();
                
                ClientConversation.authenticate(str_username, str_password);
                i_obj_context.district = dataEquipment.DISTRICT_FROM;
                i_obj_context.position = str_posisi;

                //cek
                string restartinfo = "";
                i_obj_request.entityType = "EQP";
                i_obj_request.entityValue = dataEquipment.EQUIP_NO;
                i_obj_result = i_obj_services.retrieve(i_obj_context, i_obj_request, rerquired, restartinfo);
                var cek_district = i_obj_result.replyElements[0].refCode;

                //insert
                var distrik1 = dataEquipment.DISTRICT_FROM;
                var lokasi1 = dataEquipment.LOC_FROM;
                var tglkluar1 = DateTime.Now.ToString("yyyyMMdd");
                var distrik2 = dataEquipment.DISTRICT_TO;
                var lokasi2 = dataEquipment.LOC_TO;

                RefCodesServiceModifyRequestDTO mod_req = new RefCodesServiceModifyRequestDTO();
                RefCodesServiceModifyReplyDTO mod_result = new RefCodesServiceModifyReplyDTO();

                if (distrik1 != "")
                {
                    mod_req.refCode = distrik1;
                    mod_req.refNo = i_obj_result.replyElements[0].refNo;
                    mod_req.seqNum = i_obj_result.replyElements[0].seqNum;
                    mod_req.entityType = "EQP";
                    mod_req.entityValue = dataEquipment.EQUIP_NO;

                    mod_result = i_obj_services.modify(i_obj_context, mod_req);
                }
                if (lokasi1 != "")
                {
                    mod_req.refCode = lokasi1;
                    mod_req.refNo = i_obj_result.replyElements[1].refNo;
                    mod_req.seqNum = i_obj_result.replyElements[1].seqNum;
                    mod_req.entityType = "EQP";
                    mod_req.entityValue = dataEquipment.EQUIP_NO;

                    mod_result = i_obj_services.modify(i_obj_context, mod_req);
                }
                if (tglkluar1 != "")
                {
                    mod_req.refCode = tglkluar1;
                    mod_req.refNo = i_obj_result.replyElements[4].refNo;
                    mod_req.seqNum = i_obj_result.replyElements[4].seqNum;
                    mod_req.entityType = "EQP";
                    mod_req.entityValue = dataEquipment.EQUIP_NO;

                    mod_result = i_obj_services.modify(i_obj_context, mod_req);
                }
                if (distrik2 != "")
                {
                    mod_req.refCode = distrik2;
                    mod_req.refNo = i_obj_result.replyElements[5].refNo;
                    mod_req.seqNum = i_obj_result.replyElements[5].seqNum;
                    mod_req.entityType = "EQP";
                    mod_req.entityValue = dataEquipment.EQUIP_NO;

                    mod_result = i_obj_services.modify(i_obj_context, mod_req);
                }
                if (lokasi2 != "")
                {
                    mod_req.refCode = lokasi2;
                    mod_req.refNo = i_obj_result.replyElements[6].refNo;
                    mod_req.seqNum = i_obj_result.replyElements[6].seqNum;
                    mod_req.entityType = "EQP";
                    mod_req.entityValue = dataEquipment.EQUIP_NO;

                    mod_result = i_obj_services.modify(i_obj_context, mod_req);
                }

                cls.Remarks = true;
                cls.Message = "Update Equipment MSF600 Berhasil";
                return cls;
            }
            catch (Exception ex)
            {
                cls.Remarks = false;
                cls.Message = ex.Message;
                return cls;
            }
        }

        public ClsUpdate_MSF600_Result updatemse600_SM(TBL_T_PPE dataEquipment)
        {
            ClsUpdate_MSF600_Result cls = new ClsUpdate_MSF600_Result();
            try
            {
                EquipmentService i_obj_services = new EquipmentService();

                EquipmentServices.OperationContext i_obj_context = new EquipmentServices.OperationContext();
                EquipmentServiceModifyRequestDTO i_obj_equipment_dto1 = new EquipmentServiceModifyRequestDTO();

                EquipmentServiceReadRequestDTO i_obj_equipment_request = new EquipmentServiceReadRequestDTO();
                EquipmentServiceReadReplyDTO i_obj_equipment_result = new EquipmentServiceReadReplyDTO();
                i_obj_equipment_request.equipmentNo = dataEquipment.EQUIP_NO;
                i_obj_equipment_request.equipmentRef = dataEquipment.EQUIP_NO;

                string str_username = ConfigurationManager.AppSettings["username"].ToString();
                string str_password = ConfigurationManager.AppSettings["password"].ToString();
                string str_posisi = ConfigurationManager.AppSettings["pos_id"].ToString();
                ClientConversation.authenticate(str_username, str_password);

                i_obj_context.district = dataEquipment.DISTRICT_FROM;
                i_obj_context.position = str_posisi;

                i_obj_equipment_result = i_obj_services.read(i_obj_context, i_obj_equipment_request);

                i_obj_equipment_dto1.equipmentNo = dataEquipment.EQUIP_NO;
                i_obj_equipment_dto1.costingFlag = "A";
                i_obj_equipment_dto1.equipmentClassif3 = "SC";
                //i_obj_equipment_dto1.equipmentClassif3 = i_obj_equipment_result.equipmentClassif3;

                i_obj_services.modify(i_obj_context, i_obj_equipment_dto1);
                ClsUpdate_MSF600_Result result = SM_ReferenceCodes(dataEquipment);
                return result;

                //cls.Remarks = true;
                //cls.Message = "Update Equipment MSF600 Berhasil";
                //return (cls);
            }
            catch (Exception ex)
            {
                cls.Remarks = false;
                cls.Message = ex.Message;
                return cls;
            }
        }

        public ClsUpdate_MSF600_Result SM_ReferenceCodes(TBL_T_PPE dataEquipment)
        {
            ClsUpdate_MSF600_Result cls = new ClsUpdate_MSF600_Result();
            try
            {
                RefCodesService.RefCodesService i_obj_services = new RefCodesService.RefCodesService();
                RefCodesService.OperationContext i_obj_context = new RefCodesService.OperationContext();
                RefCodesServiceModifyRequestDTO i_obj_equipment_dto1 = new RefCodesServiceModifyRequestDTO();

                RefCodesServiceRetrieveRequestDTO i_obj_request = new RefCodesServiceRetrieveRequestDTO();
                RefCodesServiceRetrieveReplyCollectionDTO i_obj_result = new RefCodesServiceRetrieveReplyCollectionDTO();
                RefCodesServiceRetrieveRequiredAttributesDTO rerquired = new RefCodesServiceRetrieveRequiredAttributesDTO();

                string str_username = ConfigurationManager.AppSettings["username"].ToString();
                string str_password = ConfigurationManager.AppSettings["password"].ToString();
                string str_posisi = ConfigurationManager.AppSettings["pos_id"].ToString();

                ClientConversation.authenticate(str_username, str_password);
                i_obj_context.district = dataEquipment.DISTRICT_FROM;
                i_obj_context.position = str_posisi;

                //cek
                string restartinfo = "";
                i_obj_request.entityType = "EQP";
                i_obj_request.entityValue = dataEquipment.EQUIP_NO;
                i_obj_result = i_obj_services.retrieve(i_obj_context, i_obj_request, rerquired, restartinfo);
                var cek_district = i_obj_result.replyElements[0].refCode;

                //insert
                var tglkluar = DateTime.Now.ToString("yyyyMMdd");

                RefCodesServiceModifyRequestDTO mod_req = new RefCodesServiceModifyRequestDTO();
                RefCodesServiceModifyReplyDTO mod_result = new RefCodesServiceModifyReplyDTO();

                if (tglkluar != "")
                {
                    mod_req.refCode = tglkluar;
                    mod_req.refNo = i_obj_result.replyElements[8].refNo;
                    mod_req.seqNum = i_obj_result.replyElements[8].seqNum;
                    mod_req.entityType = "EQP";
                    mod_req.entityValue = dataEquipment.EQUIP_NO;

                    mod_result = i_obj_services.modify(i_obj_context, mod_req);
                }

                cls.Remarks = true;
                cls.Message = "Update Equipment MSF600 Berhasil";
                return cls;
            }
            catch (Exception ex)
            {
                cls.Remarks = false;
                cls.Message = ex.Message;
                return cls;
            }
        }
    }
}