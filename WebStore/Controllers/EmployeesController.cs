using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Models;
using WebStore.Services.Interfaces;

namespace WebStore.Controllers
{
    //[Route("Employees/[action]/{id?}")]
    //[Route("Staff/[action]/{id?}")]
    public class EmployeesController : Controller
    {
        //private readonly IEnumerable<Employee> _Employees;
        private readonly IEmployeesData _EmployeesData;
        private readonly ILogger<EmployeesController> _Logger;

        public EmployeesController(IEmployeesData EmployeesData,ILogger<EmployeesController> Logger)
        {
            _EmployeesData = EmployeesData;
            _Logger = Logger;
        }
        //[Route("~/employees/all")]
        public IActionResult Index()
        {
            return View(_EmployeesData.GetAll());
        }
        //[Route("~/employees/info[[{id}]]")]
        //[Route("~/employees/info={id}")]
        public IActionResult Details(int id)
        {
            //var employee = _Employees.FirstOrDefault(e => e.Id == id);
            var employee = _EmployeesData.GetById(id);

            if (employee is null)
                return NotFound();

            return View(employee);
        }
        //public IActionResult TestAction(string Parameter1,int Param2)
        //{
        //    return Content($"P1:{Parameter1} -P2{Param2}");
        //}
    }

}
