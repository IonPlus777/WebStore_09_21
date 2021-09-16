using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //return Content("Hello from controller!");
            return View();
        }

        public IActionResult SecondAction(string id)
        {
            return Content($"Second action with parametre {id}");
        }
    }
}
