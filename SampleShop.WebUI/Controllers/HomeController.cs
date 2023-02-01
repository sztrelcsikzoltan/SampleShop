using SampleShop.Core.Contracts;
using SampleShop.Core.Models;
using SampleShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SampleShop.WebUI.Controllers
{
    public class HomeController : Controller
    {
        IRepository<Product> context;
        IRepository<ProductCategory> productCategories;

        public HomeController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext)
        {
            context = productContext;
            productCategories = productCategoryContext;
        }

        public ActionResult Index(string category=null)
        {
            // List<Product> products = context.GetAll().ToList();
            List<Product> products;
            List<ProductCategory> categories = productCategories.GetAll().ToList();
            if (category == null)
            {
                products = context.GetAll().ToList();
            }
            else
            {
                products = context.GetAll().Where(p=> p.Category == category).ToList();
            }

            ProductListViewModel model = new ProductListViewModel();
            model.Products = products;
            model.ProductCategories = categories;

            return View(model);
        }

        public ActionResult Details(string id)
        {
            Product product= context.Get(id);
            if (product == null)
            {
                return  HttpNotFound();
            }
            
            return View(product);
        }
        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}