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

                //edit 10/08/2023 ubah costing flag menjadi not allowed
                i_obj_equipment_dto1.equipmentNo = dataEquipment.EQUIP_NO;
                //i_obj_equipment_dto1.costingFlag = "W";
                i_obj_equipment_dto1.equipmentStatus = "MT";
                if (i_obj_equipment_result.poNo != null)
                {
                    i_obj_equipment_dto1.poNo = i_obj_equipment_result.poNo;
                }
                else
                {
                    i_obj_equipment_dto1.poNo = "-";
                }
                i_obj_equipment_dto1.equipmentClassif3 = i_obj_equipment_result.equipmentClassif3;
                
                i_obj_services.modify(i_obj_context, i_obj_equipment_dto1);
                //ClsUpdate_MSF600_Result result = updatemse600_ReferenceCOdes(dataEquipment);
                //return result;

                cls.Remarks = true;
                cls.Message = "Update Equipment MSF600 Berhasil";
                return cls;
            }
            catch (Exception ex)
            {
                cls.Remarks = false;
                cls.Message = ex.Message;
                return (cls);
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
                i_obj_equipment_dto1.districtCode = dataEquipment.DISTRICT_TO;
                i_obj_equipment_dto1.equipmentLocation = dataEquipment.LOC_TO;
                //i_obj_equipment_dto1.costingFlag = "A";
                i_obj_equipment_dto1.equipmentStatus = "AV";
                if (i_obj_equipment_result.poNo != null)
                {
                    i_obj_equipment_dto1.poNo = i_obj_equipment_result.poNo;
                }
                else
                {
                    i_obj_equipment_dto1.poNo = "-";
                }
                i_obj_equipment_dto1.equipmentClassif3 = i_obj_equipment_result.equipmentClassif3;

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
                RefCodesService.RefCodesService service = new RefCodesService.RefCodesService();
                RefCodesService.OperationContext context = new RefCodesService.OperationContext();
                //RefCodesServiceModifyRequestDTO dto = new RefCodesServiceModifyRequestDTO();
                RefCodesServiceRetrieveRequestDTO dto = new RefCodesServiceRetrieveRequestDTO();

                RefCodesServiceRetrieveRequestDTO i_obj_request = new RefCodesServiceRetrieveRequestDTO();
                RefCodesServiceRetrieveReplyCollectionDTO read_result = new RefCodesServiceRetrieveReplyCollectionDTO();
                RefCodesServiceRetrieveRequiredAttributesDTO required = new RefCodesServiceRetrieveRequiredAttributesDTO();

                string str_username = ConfigurationManager.AppSettings["username"].ToString();
                string str_password = ConfigurationManager.AppSettings["password"].ToString();
                string str_posisi = ConfigurationManager.AppSettings["pos_id"].ToString();

                ClientConversation.authenticate(str_username, str_password);
                context.district = dataEquipment.DISTRICT_FROM;
                context.position = str_posisi;

                var distrik_asal = dataEquipment.DISTRICT_FROM; //gk perlu insert ini
                var lokasi_asal = dataEquipment.LOC_FROM; //gk perlu insert ini
                var tglkluar_dstrctasal = DateTime.Now.ToString("yyyyMMdd");
                var distrik_tujuan = dataEquipment.DISTRICT_TO;
                var lokasi_tujuan = dataEquipment.LOC_TO;
                var tglmasuk_dstrcttujuan = DateTime.Now.ToString("yyyyMMdd");

                #region distrik tujuan
                string district = "District Tujuan";
                if (district == "District Tujuan")
                {
                    //CEK DISTRICT 1 NULL OR NOT
                    string restartinfo = "";
                    dto.entityType = "EQP";
                    dto.entityValue = dataEquipment.EQUIP_NO;
                    dto.refNo = "001";

                    read_result = service.retrieve(context, dto, required, restartinfo);

                    RefCodesServiceModifyRequestDTO mod_req = new RefCodesServiceModifyRequestDTO();
                    RefCodesServiceModifyReplyDTO mod_result = new RefCodesServiceModifyReplyDTO();

                    var district2 = read_result.replyElements[5].refCode;
                    var district3 = read_result.replyElements[10].refCode;
                    var district4 = read_result.replyElements[15].refCode;

                    if (district2 == null || district2 == "")
                    {

                        mod_req.refCode = dataEquipment.DISTRICT_TO;
                        mod_req.refNo = read_result.replyElements[5].refNo;
                        mod_req.seqNum = read_result.replyElements[5].seqNum;
                        mod_req.entityType = read_result.replyElements[5].entityType;
                        mod_req.entityValue = read_result.replyElements[5].entityValue;

                        mod_result = service.modify(context, mod_req);
                    }
                    else if (district3 == null || district3 == "")
                    {
                        mod_req.refCode = dataEquipment.DISTRICT_TO;
                        mod_req.refNo = read_result.replyElements[10].refNo;
                        mod_req.seqNum = read_result.replyElements[10].seqNum;
                        mod_req.entityType = read_result.replyElements[10].entityType;
                        mod_req.entityValue = read_result.replyElements[10].entityValue;

                        mod_result = service.modify(context, mod_req);
                    }
                    else if (district4 == null || district4 == "")
                    {
                        mod_req.refCode = dataEquipment.DISTRICT_TO;
                        mod_req.refNo = read_result.replyElements[15].refNo;
                        mod_req.seqNum = read_result.replyElements[15].seqNum;
                        mod_req.entityType = read_result.replyElements[15].entityType;
                        mod_req.entityValue = read_result.replyElements[15].entityValue;

                        mod_result = service.modify(context, mod_req);
                    }
                    else
                    {
                        dto.entityType = "EQP";
                        dto.entityValue = dataEquipment.EQUIP_NO;
                        dto.refNo = "021";

                        read_result = service.retrieve(context, dto, required, restartinfo);

                        var district5 = read_result.replyElements[0].refCode;
                        var district6 = read_result.replyElements[5].refCode;
                        var district7 = read_result.replyElements[10].refCode;
                        var district8 = read_result.replyElements[15].refCode;

                        if (district5 == null || district5 == "")
                        {
                            mod_req.refCode = dataEquipment.DISTRICT_TO;
                            mod_req.refNo = read_result.replyElements[0].refNo;
                            mod_req.seqNum = read_result.replyElements[0].seqNum;
                            mod_req.entityType = read_result.replyElements[0].entityType;
                            mod_req.entityValue = read_result.replyElements[0].entityValue;

                            mod_result = service.modify(context, mod_req);
                        }
                        else if (district6 == null || district6 == "")
                        {
                            mod_req.refCode = dataEquipment.DISTRICT_TO;
                            mod_req.refNo = read_result.replyElements[5].refNo;
                            mod_req.seqNum = read_result.replyElements[5].seqNum;
                            mod_req.entityType = read_result.replyElements[5].entityType;
                            mod_req.entityValue = read_result.replyElements[5].entityValue;

                            mod_result = service.modify(context, mod_req);
                        }
                        else if (district7 == null || district7 == "")
                        {
                            mod_req.refCode = dataEquipment.DISTRICT_TO;
                            mod_req.refNo = read_result.replyElements[10].refNo;
                            mod_req.seqNum = read_result.replyElements[10].seqNum;
                            mod_req.entityType = read_result.replyElements[10].entityType;
                            mod_req.entityValue = read_result.replyElements[10].entityValue;

                            mod_result = service.modify(context, mod_req);
                        }
                        else if (district8 == null || district8 == "")
                        {
                            mod_req.refCode = dataEquipment.DISTRICT_TO;
                            mod_req.refNo = read_result.replyElements[15].refNo;
                            mod_req.seqNum = read_result.replyElements[15].seqNum;
                            mod_req.entityType = read_result.replyElements[15].entityType;
                            mod_req.entityValue = read_result.replyElements[15].entityValue;

                            mod_result = service.modify(context, mod_req);
                        }
                        else
                        {
                            dto.entityType = "EQP";
                            dto.entityValue = dataEquipment.EQUIP_NO;
                            dto.refNo = "041";

                            read_result = service.retrieve(context, dto, required, restartinfo);

                            var district9 = read_result.replyElements[0].refCode;
                            var district10 = read_result.replyElements[5].refCode;

                            if (district9 == null || district9 == "")
                            {
                                mod_req.refCode = dataEquipment.DISTRICT_TO;
                                mod_req.refNo = read_result.replyElements[0].refNo;
                                mod_req.seqNum = read_result.replyElements[0].seqNum;
                                mod_req.entityType = read_result.replyElements[0].entityType;
                                mod_req.entityValue = read_result.replyElements[0].entityValue;

                                mod_result = service.modify(context, mod_req);
                            }
                            else if (district10 == null || district10 == "")
                            {
                                mod_req.refCode = dataEquipment.DISTRICT_TO;
                                mod_req.refNo = read_result.replyElements[5].refNo;
                                mod_req.seqNum = read_result.replyElements[5].seqNum;
                                mod_req.entityType = read_result.replyElements[5].entityType;
                                mod_req.entityValue = read_result.replyElements[5].entityValue;

                                mod_result = service.modify(context, mod_req);
                            }
                        }
                    }
                }
                #endregion

                #region lokasi tujuan
                string lokasi = "Lokasi Tujuan";
                if (lokasi == "Lokasi Tujuan")
                {
                    //CEK LOKASI 1 NULL OR NOT
                    string restartinfo = "";
                    dto.entityType = "EQP";
                    dto.entityValue = dataEquipment.EQUIP_NO;
                    dto.refNo = "001";

                    read_result = service.retrieve(context, dto, required, restartinfo);

                    RefCodesServiceModifyRequestDTO mod_req = new RefCodesServiceModifyRequestDTO();
                    RefCodesServiceModifyReplyDTO mod_result = new RefCodesServiceModifyReplyDTO();

                    var lokasi2 = read_result.replyElements[6].refCode;
                    var lokasi3 = read_result.replyElements[11].refCode;
                    var lokasi4 = read_result.replyElements[16].refCode;

                    if (lokasi2 == null || lokasi2 == "")
                    {

                        mod_req.refCode = dataEquipment.LOC_TO;
                        mod_req.refNo = read_result.replyElements[6].refNo;
                        mod_req.seqNum = read_result.replyElements[6].seqNum;
                        mod_req.entityType = read_result.replyElements[6].entityType;
                        mod_req.entityValue = read_result.replyElements[6].entityValue;

                        mod_result = service.modify(context, mod_req);
                    }
                    else if (lokasi3 == null || lokasi3 == "")
                    {
                        mod_req.refCode = dataEquipment.LOC_TO;
                        mod_req.refNo = read_result.replyElements[11].refNo;
                        mod_req.seqNum = read_result.replyElements[11].seqNum;
                        mod_req.entityType = read_result.replyElements[11].entityType;
                        mod_req.entityValue = read_result.replyElements[11].entityValue;

                        mod_result = service.modify(context, mod_req);
                    }
                    else if (lokasi4 == null || lokasi4 == "")
                    {
                        mod_req.refCode = dataEquipment.LOC_TO;
                        mod_req.refNo = read_result.replyElements[16].refNo;
                        mod_req.seqNum = read_result.replyElements[16].seqNum;
                        mod_req.entityType = read_result.replyElements[16].entityType;
                        mod_req.entityValue = read_result.replyElements[16].entityValue;

                        mod_result = service.modify(context, mod_req);
                    }
                    else
                    {
                        dto.entityType = "EQP";
                        dto.entityValue = dataEquipment.EQUIP_NO;
                        dto.refNo = "022";

                        read_result = service.retrieve(context, dto, required, restartinfo);

                        var lokasi5 = read_result.replyElements[0].refCode;
                        var lokasi6 = read_result.replyElements[5].refCode;
                        var lokasi7 = read_result.replyElements[10].refCode;
                        var lokasi8 = read_result.replyElements[15].refCode;

                        if (lokasi5 == null || lokasi5 == "")
                        {
                            mod_req.refCode = dataEquipment.LOC_TO;
                            mod_req.refNo = read_result.replyElements[0].refNo;
                            mod_req.seqNum = read_result.replyElements[0].seqNum;
                            mod_req.entityType = read_result.replyElements[0].entityType;
                            mod_req.entityValue = read_result.replyElements[0].entityValue;

                            mod_result = service.modify(context, mod_req);
                        }
                        else if (lokasi6 == null || lokasi6 == "")
                        {
                            mod_req.refCode = dataEquipment.LOC_TO;
                            mod_req.refNo = read_result.replyElements[5].refNo;
                            mod_req.seqNum = read_result.replyElements[5].seqNum;
                            mod_req.entityType = read_result.replyElements[5].entityType;
                            mod_req.entityValue = read_result.replyElements[5].entityValue;

                            mod_result = service.modify(context, mod_req);
                        }
                        else if (lokasi7 == null || lokasi7 == "")
                        {
                            mod_req.refCode = dataEquipment.LOC_TO;
                            mod_req.refNo = read_result.replyElements[10].refNo;
                            mod_req.seqNum = read_result.replyElements[10].seqNum;
                            mod_req.entityType = read_result.replyElements[10].entityType;
                            mod_req.entityValue = read_result.replyElements[10].entityValue;

                            mod_result = service.modify(context, mod_req);
                        }
                        else if (lokasi8 == null || lokasi8 == "")
                        {
                            mod_req.refCode = dataEquipment.LOC_TO;
                            mod_req.refNo = read_result.replyElements[15].refNo;
                            mod_req.seqNum = read_result.replyElements[15].seqNum;
                            mod_req.entityType = read_result.replyElements[15].entityType;
                            mod_req.entityValue = read_result.replyElements[15].entityValue;

                            mod_result = service.modify(context, mod_req);
                        }
                        else
                        {
                            dto.entityType = "EQP";
                            dto.entityValue = dataEquipment.EQUIP_NO;
                            dto.refNo = "042";

                            read_result = service.retrieve(context, dto, required, restartinfo);

                            var lokasi9 = read_result.replyElements[0].refCode;
                            var lokasi10 = read_result.replyElements[5].refCode;

                            if (lokasi9 == null || lokasi9 == "")
                            {
                                mod_req.refCode = dataEquipment.LOC_TO;
                                mod_req.refNo = read_result.replyElements[0].refNo;
                                mod_req.seqNum = read_result.replyElements[0].seqNum;
                                mod_req.entityType = read_result.replyElements[0].entityType;
                                mod_req.entityValue = read_result.replyElements[0].entityValue;

                                mod_result = service.modify(context, mod_req);
                            }
                            else if (lokasi10 == null || lokasi10 == "")
                            {
                                mod_req.refCode = dataEquipment.LOC_TO;
                                mod_req.refNo = read_result.replyElements[5].refNo;
                                mod_req.seqNum = read_result.replyElements[5].seqNum;
                                mod_req.entityType = read_result.replyElements[5].entityType;
                                mod_req.entityValue = read_result.replyElements[5].entityValue;

                                mod_result = service.modify(context, mod_req);
                            }
                        }
                    }
                }
                #endregion

                #region tgl masuk distrik tujuan
                string tgl_masuk = "Tgl Masuk District Tujuan";
                if (tgl_masuk == "Tgl Masuk District Tujuan")
                {
                    //CEK TGL MASUK 1 NULL OR NOT
                    string restartinfo = "";
                    dto.entityType = "EQP";
                    dto.entityValue = dataEquipment.EQUIP_NO;
                    dto.refNo = "001";

                    read_result = service.retrieve(context, dto, required, restartinfo);

                    RefCodesServiceModifyRequestDTO mod_req = new RefCodesServiceModifyRequestDTO();
                    RefCodesServiceModifyReplyDTO mod_result = new RefCodesServiceModifyReplyDTO();

                    var tglmasuk2 = read_result.replyElements[8].refCode;
                    var tglmasuk3 = read_result.replyElements[13].refCode;
                    var tglmasuk4 = read_result.replyElements[18].refCode;

                    if (tglmasuk2 == null || tglmasuk2 == "")
                    {

                        mod_req.refCode = tglmasuk_dstrcttujuan;
                        mod_req.refNo = read_result.replyElements[8].refNo;
                        mod_req.seqNum = read_result.replyElements[8].seqNum;
                        mod_req.entityType = read_result.replyElements[8].entityType;
                        mod_req.entityValue = read_result.replyElements[8].entityValue;

                        mod_result = service.modify(context, mod_req);
                    }
                    else if (tglmasuk3 == null || tglmasuk3 == "")
                    {
                        mod_req.refCode = tglmasuk_dstrcttujuan;
                        mod_req.refNo = read_result.replyElements[13].refNo;
                        mod_req.seqNum = read_result.replyElements[13].seqNum;
                        mod_req.entityType = read_result.replyElements[13].entityType;
                        mod_req.entityValue = read_result.replyElements[13].entityValue;

                        mod_result = service.modify(context, mod_req);
                    }
                    else if (tglmasuk4 == null || tglmasuk4 == "")
                    {
                        mod_req.refCode = tglmasuk_dstrcttujuan;
                        mod_req.refNo = read_result.replyElements[18].refNo;
                        mod_req.seqNum = read_result.replyElements[18].seqNum;
                        mod_req.entityType = read_result.replyElements[18].entityType;
                        mod_req.entityValue = read_result.replyElements[18].entityValue;

                        mod_result = service.modify(context, mod_req);
                    }
                    else
                    {
                        dto.entityType = "EQP";
                        dto.entityValue = dataEquipment.EQUIP_NO;
                        dto.refNo = "024";

                        read_result = service.retrieve(context, dto, required, restartinfo);

                        var tglmasuk5 = read_result.replyElements[0].refCode;
                        var tglmasuk6 = read_result.replyElements[5].refCode;
                        var tglmasuk7 = read_result.replyElements[10].refCode;
                        var tglmasuk8 = read_result.replyElements[15].refCode;

                        if (tglmasuk5 == null || tglmasuk5 == "")
                        {
                            mod_req.refCode = tglmasuk_dstrcttujuan;
                            mod_req.refNo = read_result.replyElements[0].refNo;
                            mod_req.seqNum = read_result.replyElements[0].seqNum;
                            mod_req.entityType = read_result.replyElements[0].entityType;
                            mod_req.entityValue = read_result.replyElements[0].entityValue;

                            mod_result = service.modify(context, mod_req);
                        }
                        else if (tglmasuk6 == null || tglmasuk6 == "")
                        {
                            mod_req.refCode = tglmasuk_dstrcttujuan;
                            mod_req.refNo = read_result.replyElements[5].refNo;
                            mod_req.seqNum = read_result.replyElements[5].seqNum;
                            mod_req.entityType = read_result.replyElements[5].entityType;
                            mod_req.entityValue = read_result.replyElements[5].entityValue;

                            mod_result = service.modify(context, mod_req);
                        }
                        else if (tglmasuk7 == null || tglmasuk7 == "")
                        {
                            mod_req.refCode = tglmasuk_dstrcttujuan;
                            mod_req.refNo = read_result.replyElements[10].refNo;
                            mod_req.seqNum = read_result.replyElements[10].seqNum;
                            mod_req.entityType = read_result.replyElements[10].entityType;
                            mod_req.entityValue = read_result.replyElements[10].entityValue;

                            mod_result = service.modify(context, mod_req);
                        }
                        else if (tglmasuk8 == null || tglmasuk8 == "")
                        {
                            mod_req.refCode = tglmasuk_dstrcttujuan;
                            mod_req.refNo = read_result.replyElements[15].refNo;
                            mod_req.seqNum = read_result.replyElements[15].seqNum;
                            mod_req.entityType = read_result.replyElements[15].entityType;
                            mod_req.entityValue = read_result.replyElements[15].entityValue;

                            mod_result = service.modify(context, mod_req);
                        }
                        else
                        {
                            dto.entityType = "EQP";
                            dto.entityValue = dataEquipment.EQUIP_NO;
                            dto.refNo = "044";

                            read_result = service.retrieve(context, dto, required, restartinfo);

                            var tglmasuk9 = read_result.replyElements[0].refCode;
                            var tglmasuk10 = read_result.replyElements[5].refCode;

                            if (tglmasuk9 == null || tglmasuk9 == "")
                            {
                                mod_req.refCode = tglmasuk_dstrcttujuan;
                                mod_req.refNo = read_result.replyElements[0].refNo;
                                mod_req.seqNum = read_result.replyElements[0].seqNum;
                                mod_req.entityType = read_result.replyElements[0].entityType;
                                mod_req.entityValue = read_result.replyElements[0].entityValue;

                                mod_result = service.modify(context, mod_req);
                            }
                            else if (tglmasuk10 == null || tglmasuk10 == "")
                            {
                                mod_req.refCode = tglmasuk_dstrcttujuan;
                                mod_req.refNo = read_result.replyElements[5].refNo;
                                mod_req.seqNum = read_result.replyElements[5].seqNum;
                                mod_req.entityType = read_result.replyElements[5].entityType;
                                mod_req.entityValue = read_result.replyElements[5].entityValue;

                                mod_result = service.modify(context, mod_req);
                            }
                        }
                    }
                }
                #endregion

                #region tgl keluar distrik asal
                string tgl_keluar = "Tgl Keluar District Asal";
                if (tgl_keluar == "Tgl Keluar District Asal")
                {
                    //CEK TGL MASUK 1 NULL OR NOT
                    string restartinfo = "";
                    dto.entityType = "EQP";
                    dto.entityValue = dataEquipment.EQUIP_NO;
                    dto.refNo = "001";

                    read_result = service.retrieve(context, dto, required, restartinfo);

                    RefCodesServiceModifyRequestDTO mod_req = new RefCodesServiceModifyRequestDTO();
                    RefCodesServiceModifyReplyDTO mod_result = new RefCodesServiceModifyReplyDTO();

                    var tglkeluar1 = read_result.replyElements[4].refCode;
                    var tglkeluar2 = read_result.replyElements[9].refCode;
                    var tglkeluar3 = read_result.replyElements[14].refCode;
                    var tglkeluar4 = read_result.replyElements[19].refCode;

                    if (read_result.replyElements[0].refCode == dataEquipment.DISTRICT_FROM && tglkeluar1 == "")
                    {
                        mod_req.refCode = tglkluar_dstrctasal;
                        mod_req.refNo = read_result.replyElements[4].refNo;
                        mod_req.seqNum = read_result.replyElements[4].seqNum;
                        mod_req.entityType = read_result.replyElements[4].entityType;
                        mod_req.entityValue = read_result.replyElements[4].entityValue;

                        mod_result = service.modify(context, mod_req);
                    }
                    else if (read_result.replyElements[5].refCode == dataEquipment.DISTRICT_FROM && tglkeluar2 == "")
                    {
                        mod_req.refCode = tglkluar_dstrctasal;
                        mod_req.refNo = read_result.replyElements[9].refNo;
                        mod_req.seqNum = read_result.replyElements[9].seqNum;
                        mod_req.entityType = read_result.replyElements[9].entityType;
                        mod_req.entityValue = read_result.replyElements[9].entityValue;

                        mod_result = service.modify(context, mod_req);
                    }
                    else if (read_result.replyElements[10].refCode == dataEquipment.DISTRICT_FROM && tglkeluar3 == "")
                    {
                        mod_req.refCode = tglkluar_dstrctasal;
                        mod_req.refNo = read_result.replyElements[14].refNo;
                        mod_req.seqNum = read_result.replyElements[14].seqNum;
                        mod_req.entityType = read_result.replyElements[14].entityType;
                        mod_req.entityValue = read_result.replyElements[14].entityValue;

                        mod_result = service.modify(context, mod_req);
                    }
                    else if (read_result.replyElements[15].refCode == dataEquipment.DISTRICT_FROM && tglkeluar4 == "")
                    {
                        mod_req.refCode = tglkluar_dstrctasal;
                        mod_req.refNo = read_result.replyElements[19].refNo;
                        mod_req.seqNum = read_result.replyElements[19].seqNum;
                        mod_req.entityType = read_result.replyElements[19].entityType;
                        mod_req.entityValue = read_result.replyElements[19].entityValue;

                        mod_result = service.modify(context, mod_req);
                    }
                    else
                    {
                        dto.entityType = "EQP";
                        dto.entityValue = dataEquipment.EQUIP_NO;
                        dto.refNo = "021";

                        read_result = service.retrieve(context, dto, required, restartinfo);

                        var tglkeluar5 = read_result.replyElements[4].refCode;
                        var tglkeluar6 = read_result.replyElements[9].refCode;
                        var tglkeluar7 = read_result.replyElements[14].refCode;
                        var tglkeluar8 = read_result.replyElements[19].refCode;

                        if (read_result.replyElements[0].refCode == dataEquipment.DISTRICT_FROM && tglkeluar5 == "")
                        {
                            mod_req.refCode = tglkluar_dstrctasal;
                            mod_req.refNo = read_result.replyElements[4].refNo;
                            mod_req.seqNum = read_result.replyElements[4].seqNum;
                            mod_req.entityType = read_result.replyElements[4].entityType;
                            mod_req.entityValue = read_result.replyElements[4].entityValue;

                            mod_result = service.modify(context, mod_req);
                        }
                        else if (read_result.replyElements[5].refCode == dataEquipment.DISTRICT_FROM && tglkeluar6 == "")
                        {
                            mod_req.refCode = tglkluar_dstrctasal;
                            mod_req.refNo = read_result.replyElements[9].refNo;
                            mod_req.seqNum = read_result.replyElements[9].seqNum;
                            mod_req.entityType = read_result.replyElements[9].entityType;
                            mod_req.entityValue = read_result.replyElements[9].entityValue;

                            mod_result = service.modify(context, mod_req);
                        }
                        else if (read_result.replyElements[10].refCode == dataEquipment.DISTRICT_FROM && tglkeluar7 == "")
                        {
                            mod_req.refCode = tglkluar_dstrctasal;
                            mod_req.refNo = read_result.replyElements[14].refNo;
                            mod_req.seqNum = read_result.replyElements[14].seqNum;
                            mod_req.entityType = read_result.replyElements[14].entityType;
                            mod_req.entityValue = read_result.replyElements[14].entityValue;

                            mod_result = service.modify(context, mod_req);
                        }
                        else if (read_result.replyElements[15].refCode == dataEquipment.DISTRICT_FROM && tglkeluar8 == "")
                        {
                            mod_req.refCode = tglkluar_dstrctasal;
                            mod_req.refNo = read_result.replyElements[19].refNo;
                            mod_req.seqNum = read_result.replyElements[19].seqNum;
                            mod_req.entityType = read_result.replyElements[19].entityType;
                            mod_req.entityValue = read_result.replyElements[19].entityValue;

                            mod_result = service.modify(context, mod_req);
                        }
                        else
                        {
                            dto.entityType = "EQP";
                            dto.entityValue = dataEquipment.EQUIP_NO;
                            dto.refNo = "041";

                            read_result = service.retrieve(context, dto, required, restartinfo);

                            var tglkeluar9 = read_result.replyElements[4].refCode;
                            var tglkeluar10 = read_result.replyElements[9].refCode;

                            if (read_result.replyElements[0].refCode == dataEquipment.DISTRICT_FROM && tglkeluar9 == "")
                            {
                                mod_req.refCode = tglkluar_dstrctasal;
                                mod_req.refNo = read_result.replyElements[4].refNo;
                                mod_req.seqNum = read_result.replyElements[4].seqNum;
                                mod_req.entityType = read_result.replyElements[4].entityType;
                                mod_req.entityValue = read_result.replyElements[4].entityValue;

                                mod_result = service.modify(context, mod_req);
                            }
                            else if (read_result.replyElements[5].refCode == dataEquipment.DISTRICT_FROM && tglkeluar10 == "")
                            {
                                mod_req.refCode = tglkluar_dstrctasal;
                                mod_req.refNo = read_result.replyElements[9].refNo;
                                mod_req.seqNum = read_result.replyElements[9].seqNum;
                                mod_req.entityType = read_result.replyElements[9].entityType;
                                mod_req.entityValue = read_result.replyElements[9].entityValue;

                                mod_result = service.modify(context, mod_req);
                            }
                        }
                    }
                }
                #endregion

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