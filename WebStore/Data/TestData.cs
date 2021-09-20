﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;

namespace WebStore.Data
{
    public static class TestData
    {
        public static List<Employee> Employees{ get; } = new()
        {
            new Employee { Id = 1, FirstName = "Петр", LastName = "Петров", Patronymic = "Петрович", Age = 31 },
            new Employee { Id = 2, FirstName = "Иван", LastName = "Иванов", Patronymic = "Иванович", Age = 32 },
            new Employee { Id = 3, FirstName = "Сидор", LastName = "Сидоров", Patronymic = "Сидорович", Age = 36 }
        };
    }
}
