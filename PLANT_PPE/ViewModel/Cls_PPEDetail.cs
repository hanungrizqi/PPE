using Newtonsoft.Json;
using PLANT_PPE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLANT_PPE.ViewModel
{
    public class Cls_PPEDetail
    {
        [JsonProperty("Data")]
        public IList<VW_T_PPE> tbl { get; set; }
    }
}