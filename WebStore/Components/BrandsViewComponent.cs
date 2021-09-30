using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Components
{
    //[ViewComponent(Name = "BrandsView")]
    public class BrandsViewComponent : ViewComponent
    {
        //public async Task<IViewComponentResult> InvokeAsync() => View();
        private readonly IProductData _ProductData;

        public BrandsViewComponent(IProductData ProductData) => _ProductData = ProductData;
        public IViewComponentResult Invoke() => View(GetBrands());

        private IEnumerable<BrandViewModel> GetBrands() =>
            _ProductData.GetBrands()
            .OrderBy(b => b.Order)
            .Select(b => new BrandViewModel
            {
                Id = b.Id,
                Name = b.Name,
            });

        
    }
}
