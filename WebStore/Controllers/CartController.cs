﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Services.Interfaces;

namespace WebStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _CartService;


        public CartController(ICartService CartService) => _CartService = CartService;

        public IActionResult Index() => View(_CartService.GetViewModel());

        public IActionResult Add(int id) 
        {
            _CartService.Add(id);
            return RedirectToAction("Index", "Cart");
        }

        public IActionResult Decrement(int id)
        {
            _CartService.Decrement(id);
            return RedirectToAction("Index", "Cart");
        }

        public IActionResult Remove(int id)
        {
            _CartService.Remove(id);
            return RedirectToAction("Index", "Cart");
        }

    }
}
