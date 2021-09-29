using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.ViewModels
{
    public class EmployeeViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [Display(Name ="")]
        [Required(ErrorMessage = "Name required")]
        [StringLength(200,MinimumLength = 2,ErrorMessage = "")]
        [RegularExpression(@"([A-z])",ErrorMessage = "")]
        public string Name { get; set; }
        [Display()]
        public string LastName { get; set; }
        [Display()]
        public string Patronymic { get; set; }
        [Display()]
        [Range(18,80,ErrorMessage = "")]
        public int Age { get; set; }
    }
}
