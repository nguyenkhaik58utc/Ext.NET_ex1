using Entity.EF6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Price
    {
       

        public string nameCompany { get; set; }
        public double price { get; set; }
        public double firstChange { get; set; }
        public double lastChange { get; set; }
        public DateTime? lastUpdate { get; set; }

        public int countCmp { get; set; }

        public string nameCmp { get; set; }

        public Price(string nameCompany, double price, double firstChange, double lastChange, DateTime? lastUpdate)
        {
            this.nameCompany = nameCompany;
            this.price = price;
            this.firstChange = firstChange;
            this.lastChange = lastChange;
            this.lastUpdate = lastUpdate;
        }

        public Price(string nameCmp, int countCmp)
        {
            this.nameCmp = nameCmp;
            this.countCmp = countCmp;
        }

        public Price()
        {
        }
    }
}