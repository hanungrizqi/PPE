using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EllipseWebServicesClient;
using System.Configuration;
using API_PLANT_PPE.EquipmentServices;
using API_PLANT_PPE.Models;

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
                OperationContext i_obj_context = new OperationContext();
                EquipmentServiceModifyRequestDTO i_obj_equipment_dto1 = new EquipmentServiceModifyRequestDTO();

                EquipmentServiceReadRequestDTO i_obj_equipment_reply = new EquipmentServiceReadRequestDTO();
                EquipmentServiceReadReplyDTO i_obj_equipment_request = new EquipmentServiceReadReplyDTO();
                i_obj_equipment_reply.equipmentNo = dataEquipment.EQUIP_NO;
                i_obj_equipment_reply.equipmentRef = dataEquipment.EQUIP_NO;

                string str_username = ConfigurationManager.AppSettings["username"].ToString();
                string str_password = ConfigurationManager.AppSettings["password"].ToString();
                string str_posisi = ConfigurationManager.AppSettings["pos_id"].ToString();
                ClientConversation.authenticate(str_username, str_password);

                i_obj_context.district = dataEquipment.DISTRICT_FROM;
                i_obj_context.position = str_posisi;

                i_obj_equipment_request = i_obj_services.read(i_obj_context, i_obj_equipment_reply);

                i_obj_equipment_dto1.equipmentNo = dataEquipment.EQUIP_NO;
                i_obj_equipment_dto1.costingFlag = "Not Allowed (Warning)";
                //i_obj_equipment_dto1.equipmentClassif3 = "KP";
                i_obj_equipment_dto1.equipmentClassif3 = i_obj_equipment_request.equipmentClassif3;

                i_obj_services.modify(i_obj_context, i_obj_equipment_dto1);

                cls.Remarks = true;
                cls.Message = "Update Equipment MSF600 Berhasil";
                return (cls);
            }
            catch (Exception ex)
            {
                cls.Remarks = false;
                cls.Message = ex.Message;
                return (cls);
            }
        }
    }
}