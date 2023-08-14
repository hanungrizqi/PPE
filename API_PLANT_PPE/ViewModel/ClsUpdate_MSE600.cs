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
                i_obj_equipment_dto1.costingFlag = "W";
                //i_obj_equipment_dto1.districtCode = dataEquipment.DISTRICT_TO;
                //i_obj_equipment_dto1.equipmentLocation = dataEquipment.LOC_TO;
                i_obj_equipment_dto1.poNo = "-";
                //i_obj_equipment_dto1.equipmentClassif3 = "SC";
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
                var distrik_asal = dataEquipment.DISTRICT_FROM;
                var lokasi_asal = dataEquipment.LOC_FROM;
                var tglkluar_dstrctasal = DateTime.Now.ToString("yyyyMMdd");
                var distrik_tujuan = dataEquipment.DISTRICT_TO;
                var lokasi_tujuan = dataEquipment.LOC_TO;

                RefCodesServiceModifyRequestDTO mod_req = new RefCodesServiceModifyRequestDTO();
                RefCodesServiceModifyReplyDTO mod_result = new RefCodesServiceModifyReplyDTO();

                if (distrik_asal != "")
                {
                    mod_req.refCode = distrik_asal;
                    mod_req.refNo = i_obj_result.replyElements[0].refNo;
                    mod_req.seqNum = i_obj_result.replyElements[0].seqNum;
                    mod_req.entityType = "EQP";
                    mod_req.entityValue = dataEquipment.EQUIP_NO;

                    mod_result = i_obj_services.modify(i_obj_context, mod_req);
                }
                if (lokasi_asal != "")
                {
                    mod_req.refCode = lokasi_asal; //"RT";
                    mod_req.refNo = i_obj_result.replyElements[1].refNo;
                    mod_req.seqNum = i_obj_result.replyElements[1].seqNum;
                    mod_req.entityType = "EQP";
                    mod_req.entityValue = dataEquipment.EQUIP_NO;

                    mod_result = i_obj_services.modify(i_obj_context, mod_req);
                }
                if (tglkluar_dstrctasal != "")
                {
                    mod_req.refCode = tglkluar_dstrctasal;
                    mod_req.refNo = i_obj_result.replyElements[4].refNo;
                    mod_req.seqNum = i_obj_result.replyElements[4].seqNum;
                    mod_req.entityType = "EQP";
                    mod_req.entityValue = dataEquipment.EQUIP_NO;

                    mod_result = i_obj_services.modify(i_obj_context, mod_req);
                }
                if (distrik_tujuan != "")
                {
                    mod_req.refCode = distrik_tujuan;
                    mod_req.refNo = i_obj_result.replyElements[5].refNo;
                    mod_req.seqNum = i_obj_result.replyElements[5].seqNum;
                    mod_req.entityType = "EQP";
                    mod_req.entityValue = dataEquipment.EQUIP_NO;

                    mod_result = i_obj_services.modify(i_obj_context, mod_req);
                }
                if (lokasi_tujuan != "")
                {
                    mod_req.refCode = lokasi_tujuan;
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
                i_obj_equipment_dto1.districtCode = dataEquipment.DISTRICT_TO;
                i_obj_equipment_dto1.equipmentLocation = dataEquipment.LOC_TO;
                i_obj_equipment_dto1.costingFlag = "A";
                i_obj_equipment_dto1.poNo = "-";
                //i_obj_equipment_dto1.equipmentClassif3 = "KP";
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
                var distrik_asal = dataEquipment.DISTRICT_FROM; //gk perlu insert ini
                var lokasi_asal = dataEquipment.LOC_FROM; //gk perlu insert ini
                var tglkluar_dstrctasal = DateTime.Now.ToString("yyyyMMdd");
                var distrik_tujuan = dataEquipment.DISTRICT_TO;
                var lokasi_tujuan = dataEquipment.LOC_TO;

                var tglmasuk_dstrcttujuan = DateTime.Now.ToString("yyyyMMdd");

                RefCodesServiceModifyRequestDTO mod_req = new RefCodesServiceModifyRequestDTO();
                RefCodesServiceModifyReplyDTO mod_result = new RefCodesServiceModifyReplyDTO();

                //tgl keluar distrik asal
                if (tglkluar_dstrctasal != "")
                {
                    //jika distrik 1 = distrik asal, insert tgl keluar 1
                    if (i_obj_result.replyElements[0].refCode == dataEquipment.DISTRICT_FROM || i_obj_result.replyElements[0].refCode == "")
                    {
                        mod_req.refCode = tglkluar_dstrctasal;
                        mod_req.refNo = i_obj_result.replyElements[4].refNo;
                        mod_req.seqNum = i_obj_result.replyElements[4].seqNum;
                        mod_req.entityType = "EQP";
                        mod_req.entityValue = dataEquipment.EQUIP_NO;

                        mod_result = i_obj_services.modify(i_obj_context, mod_req);
                    }
                    else
                    {
                        //jika distrik 2 = distrik asal, insert tgl keluar 2
                        if (i_obj_result.replyElements[5].refCode == dataEquipment.DISTRICT_FROM)
                        {
                            mod_req.refCode = tglkluar_dstrctasal;
                            mod_req.refNo = i_obj_result.replyElements[9].refNo;
                            mod_req.seqNum = i_obj_result.replyElements[9].seqNum;
                            mod_req.entityType = "EQP";
                            mod_req.entityValue = dataEquipment.EQUIP_NO;

                            mod_result = i_obj_services.modify(i_obj_context, mod_req);
                        }
                        else
                        {
                            //jika distrik 3 = distrik asal, insert tgl keluar 3
                            if (i_obj_result.replyElements[10].refCode == dataEquipment.DISTRICT_FROM)
                            {
                                mod_req.refCode = tglkluar_dstrctasal;
                                mod_req.refNo = i_obj_result.replyElements[14].refNo;
                                mod_req.seqNum = i_obj_result.replyElements[14].seqNum;
                                mod_req.entityType = "EQP";
                                mod_req.entityValue = dataEquipment.EQUIP_NO;

                                mod_result = i_obj_services.modify(i_obj_context, mod_req);
                            }
                            else
                            {
                                //jika distrik 4 = disrtik asal, insert tgl keluar 4
                                if (i_obj_result.replyElements[15].refCode == dataEquipment.DISTRICT_FROM)
                                {
                                    mod_req.refCode = tglkluar_dstrctasal;
                                    mod_req.refNo = i_obj_result.replyElements[19].refNo;
                                    mod_req.seqNum = i_obj_result.replyElements[19].seqNum;
                                    mod_req.entityType = "EQP";
                                    mod_req.entityValue = dataEquipment.EQUIP_NO;

                                    mod_result = i_obj_services.modify(i_obj_context, mod_req);
                                }
                                else
                                {
                                    //jika distrik 5 = distrik asal, insert tgl keluar 5
                                    if (i_obj_result.replyElements[20].refCode == dataEquipment.DISTRICT_FROM)
                                    {
                                        mod_req.refCode = tglkluar_dstrctasal;
                                        mod_req.refNo = i_obj_result.replyElements[24].refNo;
                                        mod_req.seqNum = i_obj_result.replyElements[24].seqNum;
                                        mod_req.entityType = "EQP";
                                        mod_req.entityValue = dataEquipment.EQUIP_NO;

                                        mod_result = i_obj_services.modify(i_obj_context, mod_req);
                                    }
                                    else
                                    {
                                        //jika distrik 6 = distrik asal, insert tgl keluar 6
                                        if (i_obj_result.replyElements[25].refCode == dataEquipment.DISTRICT_FROM)
                                        {
                                            mod_req.refCode = tglkluar_dstrctasal;
                                            mod_req.refNo = i_obj_result.replyElements[29].refNo;
                                            mod_req.seqNum = i_obj_result.replyElements[29].seqNum;
                                            mod_req.entityType = "EQP";
                                            mod_req.entityValue = dataEquipment.EQUIP_NO;

                                            mod_result = i_obj_services.modify(i_obj_context, mod_req);
                                        }
                                        else
                                        {
                                            //jika distrik 7 = distrik asal, insert tgl keluar 7
                                            if (i_obj_result.replyElements[30].refCode == dataEquipment.DISTRICT_FROM)
                                            {
                                                mod_req.refCode = tglkluar_dstrctasal;
                                                mod_req.refNo = i_obj_result.replyElements[34].refNo;
                                                mod_req.seqNum = i_obj_result.replyElements[34].seqNum;
                                                mod_req.entityType = "EQP";
                                                mod_req.entityValue = dataEquipment.EQUIP_NO;

                                                mod_result = i_obj_services.modify(i_obj_context, mod_req);
                                            }
                                            else
                                            {
                                                //jika distrik 8 = distrik asal, insert tgl keluar 8
                                                if (i_obj_result.replyElements[35].refCode == dataEquipment.DISTRICT_FROM)
                                                {
                                                    mod_req.refCode = tglkluar_dstrctasal;
                                                    mod_req.refNo = i_obj_result.replyElements[39].refNo;
                                                    mod_req.seqNum = i_obj_result.replyElements[39].seqNum;
                                                    mod_req.entityType = "EQP";
                                                    mod_req.entityValue = dataEquipment.EQUIP_NO;

                                                    mod_result = i_obj_services.modify(i_obj_context, mod_req);
                                                }
                                                else
                                                {
                                                    //jika distrik 9 = distrik asal, insert tgl keluar 9
                                                    if (i_obj_result.replyElements[40].refCode == dataEquipment.DISTRICT_FROM)
                                                    {
                                                        mod_req.refCode = tglkluar_dstrctasal;
                                                        mod_req.refNo = i_obj_result.replyElements[44].refNo;
                                                        mod_req.seqNum = i_obj_result.replyElements[44].seqNum;
                                                        mod_req.entityType = "EQP";
                                                        mod_req.entityValue = dataEquipment.EQUIP_NO;

                                                        mod_result = i_obj_services.modify(i_obj_context, mod_req);
                                                    }
                                                    else
                                                    {
                                                        //jika distrik 10 = distrik asal, insert tgl keluar 10
                                                        if (i_obj_result.replyElements[45].refCode == dataEquipment.DISTRICT_FROM)
                                                        {
                                                            mod_req.refCode = tglkluar_dstrctasal;
                                                            mod_req.refNo = i_obj_result.replyElements[49].refNo;
                                                            mod_req.seqNum = i_obj_result.replyElements[49].seqNum;
                                                            mod_req.entityType = "EQP";
                                                            mod_req.entityValue = dataEquipment.EQUIP_NO;

                                                            mod_result = i_obj_services.modify(i_obj_context, mod_req);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                
                //distrik tujuan
                if (distrik_tujuan != "")
                {
                    //jika district 2 kosong, insert
                    if (i_obj_result.replyElements[5].refCode == null || i_obj_result.replyElements[5].refCode == "")
                    {
                        mod_req.refCode = distrik_tujuan;
                        mod_req.refNo = i_obj_result.replyElements[5].refNo;
                        mod_req.seqNum = i_obj_result.replyElements[5].seqNum;
                        mod_req.entityType = "EQP";
                        mod_req.entityValue = dataEquipment.EQUIP_NO;

                        mod_result = i_obj_services.modify(i_obj_context, mod_req);
                    }
                    //jika district 2 gk kosong, cek ke distrik 3
                    else //if (i_obj_result.replyElements[5].refCode != null || i_obj_result.replyElements[5].refCode != "")
                    {
                        //jika distrik 3 kosong, insert
                        if (i_obj_result.replyElements[10].refCode == null || i_obj_result.replyElements[10].refCode == "")
                        {
                            mod_req.refCode = distrik_tujuan;
                            mod_req.refNo = i_obj_result.replyElements[10].refNo;
                            mod_req.seqNum = i_obj_result.replyElements[10].seqNum;
                            mod_req.entityType = "EQP";
                            mod_req.entityValue = dataEquipment.EQUIP_NO;

                            mod_result = i_obj_services.modify(i_obj_context, mod_req);
                        }
                        //jika distrik 3 gk kosong, cek distrik 4
                        else
                        {
                            //jika distrik 4 kosong, insert
                            if (i_obj_result.replyElements[15].refCode == null || i_obj_result.replyElements[15].refCode == "")
                            {
                                mod_req.refCode = distrik_tujuan;
                                mod_req.refNo = i_obj_result.replyElements[15].refNo;
                                mod_req.seqNum = i_obj_result.replyElements[15].seqNum;
                                mod_req.entityType = "EQP";
                                mod_req.entityValue = dataEquipment.EQUIP_NO;

                                mod_result = i_obj_services.modify(i_obj_context, mod_req);
                            }
                            //jika distrik 4 gk kosoong, cek distrik 5
                            else
                            {
                                //jika distrik 5 kosong, insert
                                if (i_obj_result.replyElements[20].refCode == null || i_obj_result.replyElements[20].refCode == "")
                                {
                                    mod_req.refCode = distrik_tujuan;
                                    mod_req.refNo = i_obj_result.replyElements[20].refNo;
                                    mod_req.seqNum = i_obj_result.replyElements[20].seqNum;
                                    mod_req.entityType = "EQP";
                                    mod_req.entityValue = dataEquipment.EQUIP_NO;

                                    mod_result = i_obj_services.modify(i_obj_context, mod_req);
                                }
                                //jika distrik 5 gk kosong, cek distrik 6
                                else
                                {
                                    //jika distrik 6 kosong, insert
                                    if (i_obj_result.replyElements[25].refCode == null || i_obj_result.replyElements[25].refCode ==     "")
                                    {
                                        mod_req.refCode = distrik_tujuan;
                                        mod_req.refNo = i_obj_result.replyElements[25].refNo;
                                        mod_req.seqNum = i_obj_result.replyElements[25].seqNum;
                                        mod_req.entityType = "EQP";
                                        mod_req.entityValue = dataEquipment.EQUIP_NO;

                                        mod_result = i_obj_services.modify(i_obj_context, mod_req);
                                    }
                                    //jika distrik 6 gk kosong, cek distrik 7
                                    else
                                    {
                                        //jika distrik 7 kosong, insert
                                        if (i_obj_result.replyElements[30].refCode == null || i_obj_result.replyElements[30].refCode        == "")
                                        {
                                            mod_req.refCode = distrik_tujuan;
                                            mod_req.refNo = i_obj_result.replyElements[30].refNo;
                                            mod_req.seqNum = i_obj_result.replyElements[30].seqNum;
                                            mod_req.entityType = "EQP";
                                            mod_req.entityValue = dataEquipment.EQUIP_NO;

                                            mod_result = i_obj_services.modify(i_obj_context, mod_req);
                                        }
                                        //jika distrik 7 gk kosong, cek distrik 8
                                        else
                                        {
                                            //jika distrik 8 kosong, insert
                                            if (i_obj_result.replyElements[35].refCode == null || i_obj_result.replyElements                    [35].refCode == "")
                                            {
                                                mod_req.refCode = distrik_tujuan;
                                                mod_req.refNo = i_obj_result.replyElements[35].refNo;
                                                mod_req.seqNum = i_obj_result.replyElements[35].seqNum;
                                                mod_req.entityType = "EQP";
                                                mod_req.entityValue = dataEquipment.EQUIP_NO;

                                                mod_result = i_obj_services.modify(i_obj_context, mod_req);
                                            }
                                            //jika distrik 8 gk kosong, cek distrik 9
                                            else
                                            {
                                                //jika distrik 9 kosong, insert
                                                if (i_obj_result.replyElements[40].refCode == null || i_obj_result.replyElements                    [40].refCode == "")
                                                {
                                                    mod_req.refCode = distrik_tujuan;
                                                    mod_req.refNo = i_obj_result.replyElements[40].refNo;
                                                    mod_req.seqNum = i_obj_result.replyElements[40].seqNum;
                                                    mod_req.entityType = "EQP";
                                                    mod_req.entityValue = dataEquipment.EQUIP_NO;

                                                    mod_result = i_obj_services.modify(i_obj_context, mod_req);
                                                }
                                                //jika distrik 9 gk kosong, cek distrik 10
                                                else
                                                {
                                                    //jika distrik 10 kosong, insert
                                                    if (i_obj_result.replyElements[45].refCode == null || i_obj_result.replyElements                    [45].refCode == "")
                                                    {
                                                        mod_req.refCode = distrik_tujuan;
                                                        mod_req.refNo = i_obj_result.replyElements[45].refNo;
                                                        mod_req.seqNum = i_obj_result.replyElements[45].seqNum;
                                                        mod_req.entityType = "EQP";
                                                        mod_req.entityValue = dataEquipment.EQUIP_NO;

                                                        mod_result = i_obj_services.modify(i_obj_context, mod_req);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                //lokasi tujuan
                if (lokasi_tujuan != "")
                {
                    //jika distrik 2 kosong, insert lokasi 2
                    //if (i_obj_result.replyElements[6].refCode == null || i_obj_result.replyElements[6].refCode == "")
                    if (i_obj_result.replyElements[5].refCode == null || i_obj_result.replyElements[5].refCode == "")
                    {
                        mod_req.refCode = lokasi_tujuan;
                        mod_req.refNo = i_obj_result.replyElements[6].refNo;
                        mod_req.seqNum = i_obj_result.replyElements[6].seqNum;
                        mod_req.entityType = "EQP";
                        mod_req.entityValue = dataEquipment.EQUIP_NO;

                        mod_result = i_obj_services.modify(i_obj_context, mod_req);
                    }
                    else
                    {
                        //jika distrik 3 kosong, insert lokasi 3
                        if (i_obj_result.replyElements[10].refCode == null || i_obj_result.replyElements[10].refCode == "")
                        {
                            mod_req.refCode = lokasi_tujuan;
                            mod_req.refNo = i_obj_result.replyElements[11].refNo;
                            mod_req.seqNum = i_obj_result.replyElements[11].seqNum;
                            mod_req.entityType = "EQP";
                            mod_req.entityValue = dataEquipment.EQUIP_NO;

                            mod_result = i_obj_services.modify(i_obj_context, mod_req);
                        }
                        else
                        {
                            //jika distrik 4 kosong, insert lokasi 4
                            if (i_obj_result.replyElements[15].refCode == null || i_obj_result.replyElements[15].refCode == "")
                            {
                                mod_req.refCode = lokasi_tujuan;
                                mod_req.refNo = i_obj_result.replyElements[16].refNo;
                                mod_req.seqNum = i_obj_result.replyElements[16].seqNum;
                                mod_req.entityType = "EQP";
                                mod_req.entityValue = dataEquipment.EQUIP_NO;

                                mod_result = i_obj_services.modify(i_obj_context, mod_req);
                            }
                            else
                            {
                                //jika distrik 5 kosong, insert lokasi 5
                                if (i_obj_result.replyElements[20].refCode == null || i_obj_result.replyElements[20].refCode == "")
                                {
                                    mod_req.refCode = lokasi_tujuan;
                                    mod_req.refNo = i_obj_result.replyElements[21].refNo;
                                    mod_req.seqNum = i_obj_result.replyElements[21].seqNum;
                                    mod_req.entityType = "EQP";
                                    mod_req.entityValue = dataEquipment.EQUIP_NO;

                                    mod_result = i_obj_services.modify(i_obj_context, mod_req);
                                }
                                else
                                {
                                    //jika distrik 6 kosong, insert lokasi 6
                                    if (i_obj_result.replyElements[25].refCode == null || i_obj_result.replyElements[25].refCode ==     "")
                                    {
                                        mod_req.refCode = lokasi_tujuan;
                                        mod_req.refNo = i_obj_result.replyElements[26].refNo;
                                        mod_req.seqNum = i_obj_result.replyElements[26].seqNum;
                                        mod_req.entityType = "EQP";
                                        mod_req.entityValue = dataEquipment.EQUIP_NO;

                                        mod_result = i_obj_services.modify(i_obj_context, mod_req);
                                    }
                                    else
                                    {
                                        //jika distrik 7 kosong, insert lokasi 7
                                        if (i_obj_result.replyElements[30].refCode == null || i_obj_result.replyElements[30].refCode        == "")
                                        {
                                            mod_req.refCode = lokasi_tujuan;
                                            mod_req.refNo = i_obj_result.replyElements[31].refNo;
                                            mod_req.seqNum = i_obj_result.replyElements[31].seqNum;
                                            mod_req.entityType = "EQP";
                                            mod_req.entityValue = dataEquipment.EQUIP_NO;

                                            mod_result = i_obj_services.modify(i_obj_context, mod_req);
                                        }
                                        else
                                        {
                                            //jika distrik 8 kosong, insert lokasi 8
                                            if (i_obj_result.replyElements[35].refCode == null || i_obj_result.replyElements                    [35].refCode == "")
                                            {
                                                mod_req.refCode = lokasi_tujuan;
                                                mod_req.refNo = i_obj_result.replyElements[36].refNo;
                                                mod_req.seqNum = i_obj_result.replyElements[36].seqNum;
                                                mod_req.entityType = "EQP";
                                                mod_req.entityValue = dataEquipment.EQUIP_NO;

                                                mod_result = i_obj_services.modify(i_obj_context, mod_req);
                                            }
                                            else
                                            {
                                                //jika distrik 9 kosong, insert lokasi 9
                                                if (i_obj_result.replyElements[40].refCode == null || i_obj_result.replyElements                    [40].refCode == "")
                                                {
                                                    mod_req.refCode = lokasi_tujuan;
                                                    mod_req.refNo = i_obj_result.replyElements[41].refNo;
                                                    mod_req.seqNum = i_obj_result.replyElements[41].seqNum;
                                                    mod_req.entityType = "EQP";
                                                    mod_req.entityValue = dataEquipment.EQUIP_NO;

                                                    mod_result = i_obj_services.modify(i_obj_context, mod_req);
                                                }
                                                else
                                                {
                                                    //jika distrik 10 kosong, insert lokasi 10
                                                    if (i_obj_result.replyElements[45].refCode == null || i_obj_result.replyElements                    [45].refCode == "")
                                                    {
                                                        mod_req.refCode = lokasi_tujuan;
                                                        mod_req.refNo = i_obj_result.replyElements[46].refNo;
                                                        mod_req.seqNum = i_obj_result.replyElements[46].seqNum;
                                                        mod_req.entityType = "EQP";
                                                        mod_req.entityValue = dataEquipment.EQUIP_NO;

                                                        mod_result = i_obj_services.modify(i_obj_context, mod_req);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                //tgl masuk distrik tujuan
                if (tglmasuk_dstrcttujuan != "")
                {
                    //jika distrik 2 kosong, insert tgl masuk 2
                    if (i_obj_result.replyElements[5].refCode == null || i_obj_result.replyElements[5].refCode == "")
                    {
                        mod_req.refCode = tglmasuk_dstrcttujuan;
                        mod_req.refNo = i_obj_result.replyElements[8].refNo;
                        mod_req.seqNum = i_obj_result.replyElements[8].seqNum;
                        mod_req.entityType = "EQP";
                        mod_req.entityValue = dataEquipment.EQUIP_NO;

                        mod_result = i_obj_services.modify(i_obj_context, mod_req);
                    }
                    else
                    {
                        //jika distrik 3 kosong, insert tgl masuk 3
                        if (i_obj_result.replyElements[10].refCode == null || i_obj_result.replyElements[10].refCode == "")
                        {
                            mod_req.refCode = tglmasuk_dstrcttujuan;
                            mod_req.refNo = i_obj_result.replyElements[13].refNo;
                            mod_req.seqNum = i_obj_result.replyElements[13].seqNum;
                            mod_req.entityType = "EQP";
                            mod_req.entityValue = dataEquipment.EQUIP_NO;

                            mod_result = i_obj_services.modify(i_obj_context, mod_req);
                        }
                        else
                        {
                            //jika distrik 4 kosong, insert tgl masuk 4
                            if (i_obj_result.replyElements[15].refCode == null || i_obj_result.replyElements[15].refCode == "")
                            {
                                mod_req.refCode = tglmasuk_dstrcttujuan;
                                mod_req.refNo = i_obj_result.replyElements[18].refNo;
                                mod_req.seqNum = i_obj_result.replyElements[18].seqNum;
                                mod_req.entityType = "EQP";
                                mod_req.entityValue = dataEquipment.EQUIP_NO;

                                mod_result = i_obj_services.modify(i_obj_context, mod_req);
                            }
                            else
                            {
                                //jika distrik 5 kosong, insert tgl masuk 5
                                if (i_obj_result.replyElements[20].refCode == null || i_obj_result.replyElements[20].refCode == "")
                                {
                                    mod_req.refCode = tglmasuk_dstrcttujuan;
                                    mod_req.refNo = i_obj_result.replyElements[23].refNo;
                                    mod_req.seqNum = i_obj_result.replyElements[23].seqNum;
                                    mod_req.entityType = "EQP";
                                    mod_req.entityValue = dataEquipment.EQUIP_NO;

                                    mod_result = i_obj_services.modify(i_obj_context, mod_req);
                                }
                                else
                                {
                                    //jika distrik 6 kosong, insert tgl masuk 6
                                    if (i_obj_result.replyElements[25].refCode == null || i_obj_result.replyElements[25].refCode ==     "")
                                    {
                                        mod_req.refCode = tglmasuk_dstrcttujuan;
                                        mod_req.refNo = i_obj_result.replyElements[28].refNo;
                                        mod_req.seqNum = i_obj_result.replyElements[28].seqNum;
                                        mod_req.entityType = "EQP";
                                        mod_req.entityValue = dataEquipment.EQUIP_NO;

                                        mod_result = i_obj_services.modify(i_obj_context, mod_req);
                                    }
                                    else
                                    {
                                        //jika distrik 7 kosong, insert tgl masuk 7
                                        if (i_obj_result.replyElements[30].refCode == null || i_obj_result.replyElements[30].refCode        == "")
                                        {
                                            mod_req.refCode = tglmasuk_dstrcttujuan;
                                            mod_req.refNo = i_obj_result.replyElements[33].refNo;
                                            mod_req.seqNum = i_obj_result.replyElements[33].seqNum;
                                            mod_req.entityType = "EQP";
                                            mod_req.entityValue = dataEquipment.EQUIP_NO;

                                            mod_result = i_obj_services.modify(i_obj_context, mod_req);
                                        }
                                        else
                                        {
                                            //jika distrik 8 kosong, insert tgl masuk 8
                                            if (i_obj_result.replyElements[35].refCode == null || i_obj_result.replyElements                    [35].refCode == "")
                                            {
                                                mod_req.refCode = tglmasuk_dstrcttujuan;
                                                mod_req.refNo = i_obj_result.replyElements[38].refNo;
                                                mod_req.seqNum = i_obj_result.replyElements[38].seqNum;
                                                mod_req.entityType = "EQP";
                                                mod_req.entityValue = dataEquipment.EQUIP_NO;

                                                mod_result = i_obj_services.modify(i_obj_context, mod_req);
                                            }
                                            else
                                            {
                                                //jika distrik 9 kosong, insert tgl masuk 9
                                                if (i_obj_result.replyElements[40].refCode == null || i_obj_result.replyElements                    [40].refCode == "")
                                                {
                                                    mod_req.refCode = tglmasuk_dstrcttujuan;
                                                    mod_req.refNo = i_obj_result.replyElements[43].refNo;
                                                    mod_req.seqNum = i_obj_result.replyElements[43].seqNum;
                                                    mod_req.entityType = "EQP";
                                                    mod_req.entityValue = dataEquipment.EQUIP_NO;

                                                    mod_result = i_obj_services.modify(i_obj_context, mod_req);
                                                }
                                                else
                                                {
                                                    //jika distrik 10 kosong, insert tgl masuk 10
                                                    if (i_obj_result.replyElements[45].refCode == null || i_obj_result.replyElements                    [45].refCode == "")
                                                    {
                                                        mod_req.refCode = tglmasuk_dstrcttujuan;
                                                        mod_req.refNo = i_obj_result.replyElements[48].refNo;
                                                        mod_req.seqNum = i_obj_result.replyElements[48].seqNum;
                                                        mod_req.entityType = "EQP";
                                                        mod_req.entityValue = dataEquipment.EQUIP_NO;

                                                        mod_result = i_obj_services.modify(i_obj_context, mod_req);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
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