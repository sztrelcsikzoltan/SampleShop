﻿using SampleShop.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SampleShop.WebUI.Controllers
{
    public class CartController : Controller
    {
        ICartService cartService;

        public CartController(ICartService cartService)
        {
            this .cartService = cartService;
        }
        // GET: Cart
        public ActionResult Index()
        {
            var model = cartService.GetCartItems(this.HttpContext);
            return View(model);
        }

        public ActionResult AddToCart(string id)
        {
            cartService.AddToCart(this.HttpContext, id);

            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromCart(string id)
        {
            cartService.RemoveFromCart(this.HttpContext, id);

            return RedirectToAction("Index");
        }

        public PartialViewResult CartSummary()
        {
            var cartSummary = cartService.GetCartSummary(this.HttpContext);

            return PartialView(cartSummary);
        }
    }
}