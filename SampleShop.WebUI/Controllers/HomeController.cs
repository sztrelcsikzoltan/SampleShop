using SampleShop.Core.Contracts;
using SampleShop.Core.Models;
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

        public ActionResult Index()
        {
            List<Product> products = context.GetAll().ToList();
            return View(products);
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