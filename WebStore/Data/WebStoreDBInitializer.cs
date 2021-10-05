using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;

namespace WebStore.Data
{
    public class WebStoreDBInitializer
    {
        private readonly WebStoreDB _db;
        private readonly ILogger<WebStoreDBInitializer> _Logger;
        public WebStoreDBInitializer(WebStoreDB db, ILogger<WebStoreDBInitializer> Logger)
        {
            _db = db;
            _Logger = Logger;

        }

        public async Task InitializeAsync()
        {
            //dlya otladki DB
            //var db_deleted= await _db.Database.EnsureDeletedAsync();
            //esli sozdaem sami nebolshie tablizi
            //var db_created = await _db.Database.EnsureCreatedAsync();

            //var ss = TestData.Sections.GroupBy(s => s.Name)
            //    .Where(s => s.Count() > 1)
            //    .Select(s => s.Key)
            //    .ToArray();

            _Logger.LogInformation("Zapusk inizializazii BD");

            var pending_migration = await _db.Database.GetPendingMigrationsAsync();
            var applied_migrations = await _db.Database.GetAppliedMigrationsAsync();

            if (pending_migration.Any())
            {
                _Logger.LogInformation("Primenenie migrazij:{0}", string.Join(",",pending_migration));
                await _db.Database.MigrateAsync();
            }

            await InitializeProductsAsync();
        }

        private async Task InitializeProductsAsync()
        {

            var timer = Stopwatch.StartNew();
            if(_db.Sections.Any())
            {
                _Logger.LogInformation("Инициализация БД информатией о товарах не требуется");
                return;
            }


            var sections_pool = TestData.Sections.ToDictionary(section => section.Id);
            var brands_pool = TestData.Brands.ToDictionary(brand => brand.Id);

            foreach (var child_section in TestData.Sections.Where(s => s.ParentId is not null))
                child_section.Parent = sections_pool[(int)child_section.ParentId!];

            foreach(var product in TestData.Products)
            {
                product.Section = sections_pool[product.SectionId];
                if (product.BrandId is { } brand_id)
                    product.Brand = brands_pool[brand_id];

                product.Id = 0;
                product.SectionId = 0;
                product.BrandId = null;
            }

            foreach(var section in TestData.Sections)
            {
                section.Id = 0;
                section.ParentId = null;
            }

            foreach (var brand in TestData.Brands)
                brand.Id = 0;

            _Logger.LogInformation("Zapis tovarov...");
            await using (await _db.Database.BeginTransactionAsync())
            {
                _db.Sections.AddRange(TestData.Sections);
                _db.Brands.AddRange(TestData.Brands);
                _db.Products.AddRange(TestData.Products);


                await _db.SaveChangesAsync();
                await _db.Database.CommitTransactionAsync();
            }
            _Logger.LogInformation("Zapis tovarov vipolnena uspeshno za {0} ms",timer.Elapsed.TotalMilliseconds);
        }
       
    }
}
