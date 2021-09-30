using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {

        //public IActionResult Index()
        //{
        //    //return Content("Hello from controller!");
        //    return View();
        //}
        public IActionResult Index() => View();

        public IActionResult Blogs() => View();

        public IActionResult BlogSingle() => View();

        public IActionResult About() => View();
        public IActionResult Catalog() => View();

        public IActionResult Status(string Code) => Content($"Status code - {Code}");


    }
}
