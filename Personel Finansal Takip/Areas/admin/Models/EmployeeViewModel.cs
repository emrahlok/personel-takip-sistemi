using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Personel_Finansal_Takip.Models;

namespace Personel_Finansal_Takip.Areas.admin.Models
{
    public class EmployeeViewModel
    {
        personeltakipsistemiEntities db = new personeltakipsistemiEntities();
        public List<personel> personeller { get; set; }
        public EmployeeViewModel()
        {
            personeller = db.personels.ToList();
        }
    }
}