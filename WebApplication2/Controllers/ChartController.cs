using Entity.EF6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class ChartController : Controller
    {
        DataContext db = new DataContext();
        // GET: Chart
        public ActionResult Index()
        {
            var model = from a in (from c in db.companies
                                   join p in db.prices
                                   on c.id equals p.id_company
                                   orderby p.id_company
                                   select new Price()
                                   {
                                       nameCompany = c.name,
                                       price = p.price1,
                                       firstChange = p.firstChange,
                                       lastChange = p.lastChange,
                                       lastUpdate = p.lastUpdate
                                   })
                        group a by a.nameCompany into companyGroup
                        select new
                        {
                            Team = companyGroup.Key,
                            Count = companyGroup.Count(),
                        }; ;

            var abc = model.ToList();
            List<Price> lst = new List<Price>();
            for (int i = 0; i < abc.Count; i++)
            {
                lst.Add(new Price(abc[i].Team, abc[i].Count));
            }
            return View(lst);
        }
    }
}