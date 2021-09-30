using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class Employee
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public int Age { get; set; }

        //public static bool operator == (Employee e1,Employee e2)
        //{
        //    return e1.Id == e2.Id;
        //}
        //public static bool operator !=(Employee e1, Employee e2)
        //{
        //    return e1.Id != e2.Id;
        //}
    }
    //public record Employee2(int Id,string FirstName,string LastName,string Patronymic,int Age);
}
