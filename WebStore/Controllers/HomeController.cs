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
        private static readonly List<Employee> _Employees = new()
       {
            new Employee{Id = 1, FirstName = "Петр", LastName = "Петров", Patronymic = "Петрович", Age = 31},
            new Employee{Id = 1, FirstName = "Иван", LastName = "Иванов", Patronymic = "Иванович", Age = 32},
            new Employee { Id = 1, FirstName = "Сидор", LastName = "Сидоров", Patronymic = "Сидорович", Age = 36}
        };
        public IActionResult Index()
        {
            //return Content("Hello from controller!");
            return View();
        }

        public IActionResult SecondAction(string id)
        {
            return Content($"Second action with parametre {id}");
        }
        public IActionResult Employees()
        {
            //return Content("Hello from controller!");
            return View(_Employees);
        }
    }
}
