using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
            if(_db.Sections.Any())
            {
                _Logger.LogInformation("Инициализация БД информатией о товарах не требуется");
                return;
            }

            _Logger.LogInformation("Zapis seczij...");
            await using(await _db.Database.BeginTransactionAsync())
            {
                _db.Sections.AddRange(TestData.Sections);

                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Sections] ON");
                await _db.SaveChangesAsync();
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Sections] OFF");
                await _db.Database.CommitTransactionAsync();
            }
            _Logger.LogInformation("Zapis seczij vipolnena uspeshno");

            _Logger.LogInformation("Zapis Brandov...");
            await using (await _db.Database.BeginTransactionAsync())
            {
                _db.Brands.AddRange(TestData.Brands);

                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Brands] ON");
                await _db.SaveChangesAsync();
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Brands] OFF");
                await _db.Database.CommitTransactionAsync();
            }
            _Logger.LogInformation("Zapis brendov vipolnena uspeshno");

            _Logger.LogInformation("Zapis tovarov...");
            await using (await _db.Database.BeginTransactionAsync())
            {
                _db.Products.AddRange(TestData.Products);

                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] ON");
                await _db.SaveChangesAsync();
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] OFF");
                await _db.Database.CommitTransactionAsync();
            }
            _Logger.LogInformation("Zapis tovarov vipolnena uspeshno");
        }
       
    }
}
