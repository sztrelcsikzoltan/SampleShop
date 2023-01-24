﻿using SampleShop.Core.Models;
using SampleShop.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SampleShop.WebUI.Controllers
{
    public class ProductCategoryManagerController : Controller
    {
        InMemoryRepository<ProductCategory> context;

        public ProductCategoryManagerController()
        {
            context = new InMemoryRepository<ProductCategory>();
        }
        // GET: ProductCategoryManager
        public ActionResult Index()
        {
            List<ProductCategory> productCategories = context.GetAll().ToList();
            return View(productCategories);
        }

        public ActionResult Create()
        {
            ProductCategory productCategory = new ProductCategory();
            return View(productCategory);
        }

        [HttpPost]
        public ActionResult Create(ProductCategory productCategory)
        {
            if (!ModelState.IsValid)
            {
                return View(productCategory);
            }

            context.Create(productCategory);
            context.Commit();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(string id)
        {
            ProductCategory productCategory = context.Get(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }

            return View(productCategory);
        }

        [HttpPost]
        public ActionResult Edit(ProductCategory productCategory, string id)
        {
            ProductCategory productCategoryToEdit = context.Get(id);
            if (productCategoryToEdit == null)
            {
                return HttpNotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(productCategory);
            }

            productCategoryToEdit.Category = productCategory.Category;
            
            context.Commit();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            ProductCategory productCategory = context.Get(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }

            return View(productCategory);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(string id)
        {
            ProductCategory productCategory = context.Get(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }

            context.Delete(productCategory.Id);
            context.Commit();
            return RedirectToAction("Index");
        }

    }
}