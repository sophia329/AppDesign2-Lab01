using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Products_HW1.Models;
using PagedList;

namespace Products_HW1.Controllers
{
    public class HomeController : Controller
    {
        ProductsEntities db = new ProductsEntities();

        public ActionResult ProductData(string option, string search, int? pageNumber)
        {
            List<Products2> prd = db.Products2.ToList();
            //return View(prd);

            if (option == "Description")
            {

                return View(db.Products2.Where
                    (x => x.Description == search || search == null).ToList().ToPagedList(pageNumber ?? 1, 3));
            }

            else if (option == "Price")
            {
                return View(db.Products2.Where
                    (x => x.Price == search || search == null).ToList().ToPagedList(pageNumber ?? 1, 3));
            }

            else
            {
                return View(db.Products2.Where
                    (x => x.Name.StartsWith(search) || search == null).ToList().ToPagedList(pageNumber ?? 1, 3));
            }
           
        }

    

        [HttpGet]
        public ActionResult Create()
        { 
            return PartialView("_Create");
        }

        [HttpPost]
        public ActionResult Create(Products2 add)
        {
            try
            {
                if (add !=null)
                {
                    Products2 addData = new Products2();
                    addData.ID = add.ID;
                    addData.Name = add.Name;
                    addData.Description = add.Description;
                    addData.Price = add.Price;

                    db.Products2.Add(addData);
                    db.SaveChanges();
                }
                return RedirectToAction("ProductData");
            }
            catch (Exception)
            {
                ViewBag.msg = "Some error ocurred.";
                return RedirectToAction("ProductData");
            }
           
        }

        [HttpGet]
        public ActionResult EditUpdate(int id)
        {
            try
            {
                if (id != 0)
                {
                    Products2 prod_data = db.Products2.SingleOrDefault(x => x.ID == id);
                    return PartialView("_EditUpdate", prod_data);
                }
                else
                {
                    return RedirectToAction("ProductData");
                }
            }
            catch (Exception)
            {
                ViewBag.msg = "Some error ocurred";
                return RedirectToAction("ProductData");
            }
        }

        [HttpPost]
        public ActionResult EditUpdate(Products2 prod_modified)
        {
            try
            {
                if (prod_modified != null)
                {
                    Products2 prod_update = db.Products2.SingleOrDefault(x => x.ID == prod_modified.ID);

                    prod_update.Name = prod_modified.Name;
                    prod_update.Description = prod_modified.Description;
                    prod_update.Price = prod_modified.Price;
                    db.SaveChanges();
                }
                return RedirectToAction("ProductData");
            }
            catch (Exception)
            {
                ViewBag.msg = "Some error ocurred";
                return RedirectToAction("ProductData");
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                Products2 prod_data = db.Products2.SingleOrDefault(x => x.ID == id);
                if (prod_data != null)
                {
                    db.Products2.Remove(prod_data);
                    db.SaveChanges();
                }

                return RedirectToAction("ProductData");

            }
            catch (Exception)
            {
                ViewBag.msg = "Some error ocurred.";
                return RedirectToAction("ProductData");
            }
        }

        [HttpGet]
        public ActionResult Read(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products2 prd = db.Products2.Find(id);
            if (prd == null)
            {
                return HttpNotFound();
            }
            return View(prd);
        }


    }
}