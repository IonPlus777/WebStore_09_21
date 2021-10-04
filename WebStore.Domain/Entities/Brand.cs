using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities
{
    //sostavnoj index ,nameof(Order)
    [Index(nameof(Name),IsUnique = true)]
    //[Table("Brandsss")]
    public class Brand : NamedEntity,IOrderedEntity
    {
        [Column("BrandOrder")]
        public int Order { get; set; }
    }
}
