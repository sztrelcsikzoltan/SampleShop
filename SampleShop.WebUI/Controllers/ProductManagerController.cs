using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SampleShop.Core.Contracts;
using SampleShop.Core.Models;
using SampleShop.Core.ViewModels;
using SampleShop.DataAccess.InMemory;

namespace SampleShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        IRepository<Product> context;
        IRepository<ProductCategory> productCategories;

        public ProductManagerController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext)
        {
            context = productContext;
            productCategories = productCategoryContext;
        }
        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> products = context.GetAll().ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            ProductManagerViewModel viewModel = new ProductManagerViewModel();
            viewModel.Product = new Product();
            viewModel.ProductCategories = productCategories.GetAll();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            if (file != null)
            {
                product.Image = product.Id + Path.GetExtension(file.FileName);
                file.SaveAs(Server.MapPath("//Content//ProductImages//") + product.Image);
            }
            
            context.Create(product);
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
            
            ProductManagerViewModel viewModel = new ProductManagerViewModel();
            viewModel.Product = product;
            viewModel.ProductCategories = productCategories.GetAll();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(Product product, string id, HttpPostedFileBase file)
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
            if (file != null)
            {
                productToEdit.Image = product.Id + Path.GetExtension(file.FileName);
                file.SaveAs(Server.MapPath("//Content//ProductImages//") + productToEdit.Image);
            }

            productToEdit.Name = product.Name;
            productToEdit.Category = product.Category;
            productToEdit.Description = product.Description;
            // productToEdit.Image = product.Image;
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
