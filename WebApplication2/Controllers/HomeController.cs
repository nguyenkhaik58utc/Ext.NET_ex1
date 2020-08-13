﻿using Entity.EF6;
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
            List<Price> lst = new List<Price>();
            var a1 = db.prices.GroupBy(g => g.id_company).OrderBy(o => o.Key).Select(s => s.Key).ToList();
            for (int i = 0;  i < a1.Count ; i++)
            {
                int id = a1[i];
                var a2 = db.prices.Where(p => p.id_company == id).ToList();
                if(a2.Count != 0)
                {
                    int check = 0;
                    DateTime max = (DateTime)a2[0].lastUpdate;
                    for(int j = 1; j < a2.Count; j++)
                    {
                        if (max < a2[j].lastUpdate) check = j;
                    }
                    int id2 = a2[check].id_company;
                    var nameComp = db.companies.Where(c => c.id == id2).ToList();
                    lst.Add(new Price(nameComp[0].name, a2[check].price1, a2[check].firstChange, a2[check].lastChange, a2[check].lastUpdate));

                }    
            }    
            
            string xyz = DateTime.Now.ToString("MM/dd/yyyy hh:mm");
            var selectValue = from c in db.companies
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
                              };
            var abc = selectValue.ToList();
            return View(lst);
        }
        public ActionResult Add_Click(string nameCompany, string price,string firstChange,string lassChange,string date)
        {
            var a1 = db.prices.GroupBy(p => p.id_company).ToList();
            string[] date1 = date.Split('T');
            String[] date2 = date1[0].Split('-');
            string dateNow = date2[1] + "/" + date2[2] + "/" + date2[0].Remove(0,1);
            var newComp = new company()
            {
                name = nameCompany
            };
            db.companies.Add(newComp);
            db.SaveChanges();
            int maxId = db.companies.Select(p => p.id).DefaultIfEmpty(0).Max();
            var newPrice = new price()
            {
                id_company = maxId,
                price1 = Convert.ToDouble(price),
                firstChange = 0.00,
                lastChange = 0.00,
                lastUpdate = Convert.ToDateTime(dateNow)

            };
            db.prices.Add(newPrice);
            db.SaveChanges();
            return Redirect("/Home/Table");
        }

        public ActionResult Edit_Click(string nameCompany, string price, string change,string date)
        {
            //var a1 = db.prices.GroupBy(p => p.id_company).ToList();
            var selectComp = db.companies.Where(c => c.name == nameCompany).FirstOrDefault<company>();
            int idComp = selectComp.id;
            string[] date1 = date.Split('T');
            string[] date2 = date1[0].Split('-');
            string dateNow = date2[1] + "/" + (Convert.ToInt32(date2[2]) + 1).ToString() + "/" + date2[0].Remove(0, 1);
            double priceChange;
            priceChange = Convert.ToDouble(price) + Convert.ToDouble(price) * Convert.ToDouble(change);
            var selectLastChange = db.prices.Where(p => p.id_company == idComp).OrderByDescending(p => p.lastUpdate).FirstOrDefault<price>();
            double valueLastChange = selectLastChange.lastChange;
            var newPrice = new price()
            {
                id_company = idComp,
                price1 = priceChange,
                firstChange = valueLastChange,
                lastChange = Convert.ToDouble(change),
                lastUpdate = Convert.ToDateTime(dateNow)

            };
            db.prices.Add(newPrice);
            db.SaveChanges();
            return Redirect("/Home/Table");
        }

        public ActionResult Delete_Click(string nameCompany)
        {
            var selectId = db.companies.Where(c => c.name == nameCompany).FirstOrDefault<company>();
            int idComp = selectId.id;
            var selectComp = db.companies.Where(c => c.id == idComp).FirstOrDefault<company>();
            db.companies.Remove(selectComp);
            var selectPrice = db.prices.Where(c => c.id_company == idComp).ToList();
            for(int i = 0; i < selectPrice.Count; i++)
            {
                db.prices.Remove(selectPrice[i]);
                db.SaveChanges();
            }    
            
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