using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SampleShop.Core.Models;
using SampleShop.DataAccess.InMemory;

namespace SampleShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        ProductRepository context;

        public ProductManagerController()
        {
            context = new ProductRepository();
        }
        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> products = context.GetProducts().ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            Product product = new Product();
            return View(product);
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            context.Insert(product);
            context.Commit();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(string id)
        {
            Product product = context.Get(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product, string id)
        {
            Product productToEdit = context.Get(id);
            if (productToEdit == null)
            {
                return HttpNotFound();
            }
                        if (!ModelState.IsValid)
            {
                return View(product);
            }

            productToEdit.Name = product.Name;
            productToEdit.Category = product.Category;
            productToEdit.Description = product.Description;
            productToEdit.Image = product.Image;
            productToEdit.Price = product.Price;

            context.Commit();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            Product product = context.Get(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(string id)
        {
            Product product = context.Get(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            context.Delete(product.Id);
            context.Commit();
            return RedirectToAction("Index");
        }

    }
}
