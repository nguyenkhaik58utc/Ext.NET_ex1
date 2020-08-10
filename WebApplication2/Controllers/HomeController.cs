using Entity.EF6;
using Ext.Net;
using Ext.Net.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        DataContext db = new DataContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Button_Click()
        {
            X.Msg.Alert("Server Time", DateTime.Now.ToLongTimeString()).Show();
            return this.Direct();
        }
        public ActionResult Book()
        {
            return View();
        }

        public ActionResult CreateSprite(string button)
        {
            RectSprite sprite = new RectSprite
            {
                SpriteID = "Sprite1",
                Width = 100,
                Height = 100,
                X = 150,
                Y = 150,
                FillStyle = "green"
            };

            var container = this.GetCmp<DrawContainer>("Draw1");
            container.Add(sprite);
            container.RenderFrame();

            return this.Direct();
        }

        public ActionResult ChangeColor(string button)
        {
            DrawContainer dc = this.GetCmp<DrawContainer>("Draw1");
            dc.GetSprite("Sprite1").SetAttributes(new Sprite { FillStyle = "red" });
            dc.RenderFrame();

            return this.Direct();
        }

        public ActionResult RotateLeft(string button)
        {
            DrawContainer dc = this.GetCmp<DrawContainer>("Draw1");
            dc.GetSprite("Sprite1").SetAttributes(new Sprite { RotationDegrees = -45 });
            dc.RenderFrame();

            return this.Direct();
        }

        public ActionResult RotateRight(string button)
        {
            DrawContainer dc = this.GetCmp<DrawContainer>("Draw1");
            dc.GetSprite("Sprite1").SetAttributes(new Sprite
            {
                Duration = 1000,
                RotationDegrees = 0,
                Easing = Easing.ElasticIn
            });
            dc.RenderFrame();

            return this.Direct();
        }

        public ActionResult Scaling(string button)
        {
            DrawContainer dc = this.GetCmp<DrawContainer>("Draw1");
            dc.GetSprite("Sprite1").SetAttributes(new Sprite { ScalingX = 2, ScalingY = 2, Duration = 0 });
            dc.RenderFrame();

            return this.Direct();
        }

        public ActionResult Translation(string button)
        {
            DrawContainer dc = this.GetCmp<DrawContainer>("Draw1");
            dc.GetSprite("Sprite1").SetAttributes(new Sprite { TranslationX = -100, TranslationY = -100 });
            dc.RenderFrame();

            return this.Direct();
        }

        public ActionResult Table()
        {
            string xyz = DateTime.Now.ToString("MM/dd/yyyy hh:mm");
            var selectValue = from c in db.companies
                              join p in db.prices
                              on c.id equals p.id_company
                              select new Price()
                              {
                                  nameCompany = c.name,
                                  price = p.price1,
                                  firstChange = p.firstChange,
                                  lastChange = p.lastChange,
                                  lastUpdate = xyz
                              };
            var abc = selectValue.ToList();
            return View(abc);
        }
        public ActionResult Add_Click(string nameCompany, string price,string firstChange,string lassChange,string date)
        {
            DirectResult r = new DirectResult();
            var id = db.companies.Where(c => c.name == nameCompany).ToList();
            int idComp = id[id.Count - 1].id;
            string[] date1 = date.Split('T');
            String[] date2 = date1[0].Split('-');
            string dateNow = date2[1] + "/" + date2[2] + "/" + date2[0].Remove(0,1);
            var newPrice = new price()
            {
                id_company = idComp,
                price1 = Convert.ToDouble(price),
                firstChange = Convert.ToDouble(firstChange),
                lastChange = Convert.ToDouble(lassChange),
                lastUpdate = Convert.ToDateTime(dateNow)

            };
            db.prices.Add(newPrice);
            db.SaveChanges();
            r.Success = false;
            r.ErrorMessage = "Done";
            return Redirect("/Home/Table");
        }

    }

    internal class DataContext1
    {
        public DataContext1()
        {
        }
    }
}