using Personel_Finansal_Takip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Personel_Finansal_Takip.Areas.admin.Models
{
    public class BordroPartialModel
    {
        public maas personelmaas { get; set; }
        public personel_puantaj ppuantaj { get; set; }
    }
}