using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
      
            public IActionResult Register() => View();

            public IActionResult Login() => View();

            public IActionResult Logout() => RedirectToAction("Index","Home");

        public IActionResult AccessDenied() => View();
        
    }
}
