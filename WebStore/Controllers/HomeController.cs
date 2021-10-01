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

        //public IActionResult Status(string id) => Content($"Status code --- {id}");

        public IActionResult Status(string id)
        {
            switch (id)
            {
                default: return  Content($"Status code --- {id}");
                case "404": return View("Error404");
            }
            
        }


    }
}
