using API_PLANT_PPE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_PLANT_PPE.ViewModel
{
    public class ClsPPE
    {
        DB_Plant_PPEDataContext db = new DB_Plant_PPEDataContext();
        public void insertnotifEmail_SH(string ppenosh)
        {
            db.cusp_insertNotifEmail_SectionHead(ppenosh);
        }
    }
}