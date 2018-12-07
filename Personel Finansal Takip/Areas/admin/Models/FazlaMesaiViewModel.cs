using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Personel_Finansal_Takip.Models;

namespace Personel_Finansal_Takip.Areas.admin.Models
{
    public class FazlaMesaiViewModel
    {
        public SelectList personeller { get; set; }
        public fazla_mesai fm_model { get; set; }
        public FazlaMesaiViewModel()
        {
            personeltakipsistemiEntities db = new personeltakipsistemiEntities();
            //personeller = new SelectList(db.personel.ToList().Select(
            //    x => new SelectListItem
            //    {
            //        Text = x.ad + " " + x.soyad,
            //        Value = x.id.ToString()
            //    }), "Value", "Text");
            personeller = new SelectList(new List<DateTime>().Select(
                x => new SelectListItem
                {
                    Text = x.Date.ToString(),
                    Value = x.Date.ToString()
                }), "Value", "Text");
        }
    }
}