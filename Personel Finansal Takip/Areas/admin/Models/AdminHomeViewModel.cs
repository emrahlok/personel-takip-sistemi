using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Personel_Finansal_Takip.Models;

namespace Personel_Finansal_Takip.Areas.admin.Models
{
    public class AdminHomeViewModel
    {
        personeltakipsistemiEntities db = new personeltakipsistemiEntities();
        public List<DepartmentCount> departments { get; set; }
        public List<string> colors { get; set; }
        public List<izinler> yaklasan_izinler { get; set; }
        public List<resmi_tatil> yaklasan_resmi_tatiller { get; set; }
        public List<personel> yaklasan_dogum_gunu { get; set; }
        public int employee_count { get; set; }
        public AdminHomeViewModel()
        {
            departments = new List<DepartmentCount>();
            colors = new List<string>();
            var random = new Random();
            var personels = db.personels.ToList();
            employee_count = personels.Count();
            foreach (var users in personels)
            {
                var founded = departments.Where(x => x.label == users.departman).FirstOrDefault();
                if (founded == null)
                {
                    founded = new DepartmentCount();
                    founded.value = 1;
                    founded.label = users.departman;
                    departments.Add(founded);
                    colors.Add(String.Format("#{0:X6}", random.Next(0x1000000)));
                }
                else
                {
                    founded.value++;
                }
            }
            var next_month = DateTime.Now.AddMonths(1);
            yaklasan_izinler = db.izinlers.Where(x => x.izin_tur.tur.Contains("izin") && x.izin_baslangic > DateTime.Now && x.izin_baslangic < next_month).ToList();
            yaklasan_resmi_tatiller = db.resmi_tatil.Where(x => x.tarih > DateTime.Now && x.tarih < next_month).ToList();
            yaklasan_dogum_gunu = db.personels.Where(x => x.dogum_tarihi > DateTime.Now && x.dogum_tarihi < next_month).ToList();
        }
    }

    public class DepartmentCount
    {
        public string label { get; set; }
        public int value { get; set; }
    }
}