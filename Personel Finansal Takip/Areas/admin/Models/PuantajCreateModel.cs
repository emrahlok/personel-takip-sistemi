using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Personel_Finansal_Takip.Areas.admin.Models
{
    public class PuantajCreateModel
    {
        public int personel_id { get; set; }
        public int ay { get; set; }
        public int yil { get; set; }
    }
}