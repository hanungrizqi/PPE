using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EllipseWebServicesClient;
using System.Configuration;
using API_PLANT_PPE.EquipmentServices;
using API_PLANT_PPE.RefCodesService;
using API_PLANT_PPE.EquipmentReferenceService;
using API_PLANT_PPE.ScreenService;
using API_PLANT_PPE.Models;
using System.Web.Services.Description;

namespace API_PLANT_PPE.ViewModel
{
    public class ClsUpdate_MSO687
    {
        DB_Plant_PPEDataContext db = new DB_Plant_PPEDataContext();
        public ClsUpdate_MSO687_Result updatemse687_SM(TBL_T_PPE dataEquipment)
        {
            ClsUpdate_MSO687_Result cls = new ClsUpdate_MSO687_Result();
            //var data = db.VW_MSF685s.Where(a => a.ASSET_NO == dataEquipment.EQUIP_NO).FirstOrDefault(); //pertama
            //var data = db.VW_MSF685s.Where(a => a.ASSET_NO == dataEquipment.EQUIP_NO && a.DSTRCT_CODE == dataEquipment.DISTRICT_FROM).FirstOrDefault(); //kedua
            //var dataList = db.VW_MSF685s.Where(a => a.ASSET_NO == dataEquipment.EQUIP_NO).ToList(); //looping dengan duplikat
            var dataList = db.VW_MSF685s.Where(a => a.ASSET_NO == dataEquipment.EQUIP_NO).GroupBy(a => a.SUB_ASSET_NO).Select(g => g.First()).ToList();
            //var dataList = db.VW_MSF685s.Where(a => a.ASSET_NO == dataEquipment.EQUIP_NO).GroupBy(a => new { a.SUB_ASSET_NO, a.ACCT_PROFILE }).Where(g => g.Any(a => a.DSTRCT_CODE == dataEquipment.DISTRICT_FROM)).Select(g => g.First()).ToList();
            //var dataAcctSub = db.VW_R_ACCT_PROFILEs.Where(a => a.DSTRCT_CODE == dataEquipment.DISTRICT_TO && a.EQUIP_LOCATION == dataEquipment.LOC_TO).FirstOrDefault();
            try
            {
                ScreenService.ScreenService service = new ScreenService.ScreenService();
                ScreenService.OperationContext context = new ScreenService.OperationContext();
                ScreenService.ScreenDTO screen_DTO = new ScreenService.ScreenDTO();
                ScreenService.ScreenSubmitRequestDTO screen_request = new ScreenService.ScreenSubmitRequestDTO();

                string str_username = ConfigurationManager.AppSettings["username"].ToString();
                string str_password = ConfigurationManager.AppSettings["password"].ToString();
                string str_posisi = ConfigurationManager.AppSettings["pos_id"].ToString();

                //Random cap = new Random();
                //var cekL = str_username.Length;
                //int num = cap.Next(1, str_username.Length);

                //string uName = str_username;
                //string updatedName = uName.Substring(0, num).ToUpper() + uName.Substring(num).ToLower();
                string acak = AcakHurufBesarKecil(str_username);

                //login ellipse
                context.district = dataEquipment.DISTRICT_FROM;
                context.position = str_posisi;
                ClientConversation.authenticate(acak, str_password);

                screen_DTO = service.executeScreen(context, "MSO687");

                //cek jika bukan di screen MSM687A
                while (screen_DTO.mapName != "MSM687A")
                {
                    screen_request.screenFields = null;
                    screen_request.screenKey = "4";
                    screen_DTO = service.submit(context, screen_request);
                }
                
                foreach (var data in dataList)
                {
                    var dataAcctSub = db.VW_R_ACCT_PROFILEs.Where(a => a.DSTRCT_CODE == dataEquipment.DISTRICT_TO && a.EQUIP_LOCATION == dataEquipment.LOC_TO).FirstOrDefault();
                    
                    //deklarasi variabel input
                    List<ScreenNameValueDTO> listInsert = new List<ScreenNameValueDTO>();
                    ScreenNameValueDTO fieldDTO0 = new ScreenNameValueDTO();
                    ScreenNameValueDTO fieldDTO1 = new ScreenNameValueDTO();
                    ScreenNameValueDTO fieldDTO2 = new ScreenNameValueDTO();
                    ScreenNameValueDTO fieldDTO3 = new ScreenNameValueDTO();
                    ScreenNameValueDTO fieldDTO4 = new ScreenNameValueDTO();
                    ScreenNameValueDTO fieldDTO5 = new ScreenNameValueDTO();
                    ScreenNameValueDTO fieldDTO6 = new ScreenNameValueDTO();
                    ScreenNameValueDTO fieldDTO7 = new ScreenNameValueDTO();
                
                    //input dengan parameter fieldname dan value
                    fieldDTO0.fieldName = "DSTRCT_CODE_FR1I";
                    fieldDTO0.value = dataEquipment.DISTRICT_FROM; //fieldDTO0.value = "RANT";
                    fieldDTO1.fieldName = "DSTRCT_CODE_TO1I";
                    fieldDTO1.value = dataEquipment.DISTRICT_TO; //fieldDTO1.value = "MASS";
                    fieldDTO2.fieldName = "EQP_ASSET_REFA1I";
                    fieldDTO2.value = "E";
                    fieldDTO3.fieldName = "EQP_ASSET_REFB1I";
                    fieldDTO3.value = "E";
                    fieldDTO4.fieldName = "EQP_AS_REF_FR1I";
                    fieldDTO4.value = dataEquipment.EQUIP_NO; //fieldDTO4.value = "DT4011";
                    fieldDTO5.fieldName = "EQP_AS_REF_TO1I";
                    fieldDTO5.value = dataEquipment.EQUIP_NO; //fieldDTO5.value = "DT4011";
                    fieldDTO6.fieldName = "SUB_ASSET_FROM1I";
                    fieldDTO6.value = data.SUB_ASSET_NO; //fieldDTO6.value = "000001";
                    fieldDTO7.fieldName = "SUB_ASSET_TO1I";
                    fieldDTO7.value = data.SUB_ASSET_NO; //fieldDTO7.value = "000001";

                    //disatukan dalam list
                    listInsert.Add(fieldDTO0);
                    listInsert.Add(fieldDTO1);
                    listInsert.Add(fieldDTO2);
                    listInsert.Add(fieldDTO3);
                    listInsert.Add(fieldDTO4);
                    listInsert.Add(fieldDTO5);
                    listInsert.Add(fieldDTO6);
                    listInsert.Add(fieldDTO7);

                    screen_request.screenFields = listInsert.ToArray();
                    screen_request.screenKey = "1";
                    screen_DTO = service.submit(context, screen_request);

                    //Remove list
                    listInsert.Remove(fieldDTO0);
                    listInsert.Remove(fieldDTO1);
                    listInsert.Remove(fieldDTO2);
                    listInsert.Remove(fieldDTO3);
                    listInsert.Remove(fieldDTO4);
                    listInsert.Remove(fieldDTO5);
                    listInsert.Remove(fieldDTO6);
                    listInsert.Remove(fieldDTO7);

                    ScreenNameValueDTO fieldDTO8 = new ScreenNameValueDTO();
                    ScreenNameValueDTO fieldDTO9 = new ScreenNameValueDTO();

                    //input dengan parameter fieldname dan value
                    fieldDTO8.fieldName = "ACCT_PROFILE2I"; //fieldDTO8.value = "1";
                    fieldDTO8.value = dataAcctSub.ACCT_PROFILE;
                    fieldDTO9.fieldName = "DEPR_EXP_CODE2I"; //fieldDTO9.value = "1";
                    fieldDTO9.value = dataAcctSub.ACCT_PROFILE;

                    //disatukan dalam list
                    listInsert.Add(fieldDTO8);
                    listInsert.Add(fieldDTO9);

                    //submit data
                    screen_request.screenFields = listInsert.ToArray();
                    screen_request.screenKey = "1";
                    screen_DTO = service.submit(context, screen_request);

                    //screen_request.screenFields = null;
                    //screen_request.screenKey = "1";
                    //screen_DTO = service.submit(context, screen_request);

                    //Remove list
                    listInsert.Remove(fieldDTO8);
                    listInsert.Remove(fieldDTO9);

                    //ScreenNameValueDTO fieldDTO10 = new ScreenNameValueDTO();
                    //ScreenNameValueDTO fieldDTO11 = new ScreenNameValueDTO();
                    ScreenNameValueDTO fieldDTO12 = new ScreenNameValueDTO();
               
                    //input dengan parameter fieldname dan value
                    //fieldDTO10.fieldName = "DEPR_METHOD3I";
                    //fieldDTO10.value = "L";
                    //fieldDTO11.fieldName = "DEPR_RATE3I";
                    //fieldDTO11.value = "";
                    fieldDTO12.fieldName = "EST_MM_LIFE3I";
                    fieldDTO12.value = "12";

                    //disatukan dalam list
                    //listInsert.Add(fieldDTO10);
                    //listInsert.Add(fieldDTO11);
                    listInsert.Add(fieldDTO12);
                
                    //submit data
                    screen_request.screenFields = listInsert.ToArray();
                    //screen_request.screenFields = null;
                    screen_request.screenKey = "1";
                    screen_DTO = service.submit(context, screen_request);

                    //submit data
                    screen_request.screenFields = null;
                    screen_request.screenKey = "1";
                    screen_DTO = service.submit(context, screen_request);

                    //Remove list
                    listInsert.Remove(fieldDTO12);
                    
                    ScreenNameValueDTO fieldDTO13 = new ScreenNameValueDTO();
                    ScreenNameValueDTO fieldDTO14 = new ScreenNameValueDTO();

                    //input dengan parameter fieldname dan value
                    //DateTime datereceivedsm = (DateTime)dataEquipment.DATE_RECEIVED_SM;
                    fieldDTO13.fieldName = "XFER_DATE1I";
                    //fieldDTO13.value = datereceivedsm.ToString("yyyyMMdd");
                    fieldDTO13.value = "20220730";
                    fieldDTO14.fieldName = "XFER_PERCENT1I";
                    fieldDTO14.value = "100.00";

                    //disatukan dalam list
                    listInsert.Add(fieldDTO13);
                    listInsert.Add(fieldDTO14);

                    //submit data
                    screen_request.screenFields = listInsert.ToArray();
                    screen_request.screenKey = "1";
                    screen_DTO = service.submit(context, screen_request);

                    screen_request.screenFields = null;
                    screen_request.screenKey = "1";
                    screen_DTO = service.submit(context, screen_request);
                    
                    // Kosongkan listInsert untuk penggunaan selanjutnya
                    listInsert.Clear();

                    //ubah flag menjadi 1
                    var flag = db.TBL_T_PPEs.FirstOrDefault(p => p.EQUIP_NO == dataEquipment.EQUIP_NO);
                    if (flag != null)
                    {
                        flag.FLAG = 1;
                        db.SubmitChanges();
                    }
                }
                
                cls.Remarks = true;
                cls.Message = "Update Equipment MSF687 Berhasil";
                return cls;
            }
            catch (Exception ex)
            {
                cls.Remarks = false;
                cls.Message = ex.Message;
                return cls;
            }
        }
        private string AcakHurufBesarKecil(string input)
        {
            // Konversi string ke array karakter agar dapat diubah
            char[] characters = input.ToCharArray();

            // Menggunakan waktu saat ini sebagai seed untuk Random
            Random random = new Random(DateTime.Now.Millisecond);

            // Loop untuk mengacak setiap karakter
            for (int i = 0; i < characters.Length; i++)
            {
                // Jika karakter merupakan huruf (bukan angka atau simbol), maka acak besar atau kecilnya
                if (char.IsLetter(characters[i]))
                {
                    // Jika angka acak lebih besar dari 0.5, maka huruf menjadi besar
                    // Jika angka acak lebih kecil atau sama dengan 0.5, maka huruf menjadi kecil
                    characters[i] = random.NextDouble() > 0.5 ? char.ToUpper(characters[i]) : char.ToLower(characters[i]);
                }
            }

            // Kembalikan string hasil pengacakan
            return new string(characters);
        }
    }
}