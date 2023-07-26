using MSO687_CONSOLEAPP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSO687_CONSOLEAPP.ScreenService;
using System.Configuration;
using EllipseWebServicesClient;

namespace MSO687_CONSOLEAPP
{
    class Program
    {
        static void Main(string[] args)
        {
            ScheduleExecute();
            Console.ReadLine(); // Keep the console application running
        }

        static void ScheduleExecute()
        {
            // Get the current time
            DateTime currentTime = DateTime.Now;

            // Set the desired execution time to 14:50 today
            DateTime scheduledTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 06, 00, 0);

            // If the scheduled time has already passed today, schedule it for tomorrow
            if (currentTime > scheduledTime)
            {
                scheduledTime = scheduledTime.AddDays(1);
            }

            // Calculate the time remaining until the scheduled time
            TimeSpan timeUntilScheduledTime = scheduledTime - currentTime;

            // Create a Timer to execute the Execute method at the scheduled time
            Timer timer = new Timer(Execute, null, timeUntilScheduledTime, Timeout.InfiniteTimeSpan);
        }

        private static void Execute(object state)
        {

            Console.WriteLine("EXEC-MSO687");

            DB_PLANT_PPE_CONSOLEDataContext db = new DB_PLANT_PPE_CONSOLEDataContext();

            var dataEquipment = db.TBL_T_PPEs.Where(item => item.DATE_RECEIVED_SM.ToString() == DateTime.Today.ToString("yyyy-MM-dd")).ToList();
            
            foreach (var item in dataEquipment)
            {
                var dataList = db.VW_MSF685s.Where(a => a.ASSET_NO == item.EQUIP_NO).GroupBy(a => a.SUB_ASSET_NO).Select(g => g.First()).ToList();

                ScreenService.ScreenService service = new ScreenService.ScreenService();
                ScreenService.OperationContext context = new ScreenService.OperationContext();
                ScreenService.ScreenDTO screen_DTO = new ScreenService.ScreenDTO();
                ScreenService.ScreenSubmitRequestDTO screen_request = new ScreenService.ScreenSubmitRequestDTO();

                string str_username = ConfigurationManager.AppSettings["username"].ToString();
                string str_password = ConfigurationManager.AppSettings["password"].ToString();
                string str_posisi = ConfigurationManager.AppSettings["pos_id"].ToString();

                string acak = AcakHurufBesarKecil(str_username);
                
                //login ellipse
                context.district = item.DISTRICT_FROM;
                context.position = str_posisi;
                ClientConversation.authenticate(acak, str_password);
                
                screen_DTO = service.executeScreen(context, "MSO687");

                while (screen_DTO.mapName != "MSM687A")
                {
                    screen_request.screenFields = null;
                    screen_request.screenKey = "4";
                    screen_DTO = service.submit(context, screen_request);
                }

                foreach (var data in dataList)
                {
                    var dataAcctSub = db.VW_R_ACCT_PROFILEs.Where(a => a.DSTRCT_CODE == item.DISTRICT_TO && a.EQUIP_LOCATION == item.LOC_TO).FirstOrDefault();
                    
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
                    fieldDTO0.value = item.DISTRICT_FROM; //fieldDTO0.value = "RANT";
                    fieldDTO1.fieldName = "DSTRCT_CODE_TO1I";
                    fieldDTO1.value = item.DISTRICT_TO; //fieldDTO1.value = "MASS";
                    fieldDTO2.fieldName = "EQP_ASSET_REFA1I";
                    fieldDTO2.value = "E";
                    fieldDTO3.fieldName = "EQP_ASSET_REFB1I";
                    fieldDTO3.value = "E";
                    fieldDTO4.fieldName = "EQP_AS_REF_FR1I";
                    fieldDTO4.value = item.EQUIP_NO; //fieldDTO4.value = "DT4011";
                    fieldDTO5.fieldName = "EQP_AS_REF_TO1I";
                    fieldDTO5.value = item.EQUIP_NO; //fieldDTO5.value = "DT4011";
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
                }
                
                Console.WriteLine(item.ID); // Print the "Name" property of each object
                Console.WriteLine(item.ID); // Print the "Name" property of each object
            }
        }
        private static string AcakHurufBesarKecil(string input)
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
