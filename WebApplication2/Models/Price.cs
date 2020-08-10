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
        public string lastUpdate { get; set; }
    }
}