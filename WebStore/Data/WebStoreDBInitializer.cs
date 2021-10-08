using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Data
{
    public class WebStoreDBInitializer
    {
        private readonly WebStoreDB _db;
        private readonly UserManager<User> _UserManager;
        private readonly RoleManager<Role> _RoleManager;
        private readonly ILogger<WebStoreDBInitializer> _Logger;
        
        public WebStoreDBInitializer(
            WebStoreDB db,
            UserManager<User> UserManager,
            RoleManager<Role> RoleManager,
            ILogger<WebStoreDBInitializer> Logger)
        {
            _db = db;
            _UserManager = UserManager;
            _RoleManager = RoleManager;
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

            try 
            { 
                await InitializeProductsAsync();
            }
            catch(Exception e)
            {
                //Console.WriteLine(e);
                _Logger.LogError(e, "Error of initialization of catalog of products");
                throw;
            }

            try
            {
                await InitializeIdentityAsync();
            }
            catch(Exception e)
            {
                _Logger.LogError(e, "Error initialization of system Identity");
                throw;
            }
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

        private async Task InitializeIdentityAsync()
        {
            _Logger.LogInformation("Initialization of system Identity");
            var timer = Stopwatch.StartNew();

            //if (!await _RoleManager.RoleExistsAsync(Role.Administrators))
            //    await _RoleManager.CreateAsync(new Role { Name = Role.Administrators });

            async Task CheckRole(string RoleName)
            { 
                if(await _RoleManager.RoleExistsAsync(RoleName))
                _Logger.LogInformation("Role exist{0}",RoleName);
                else
                {
                    _Logger.LogInformation("Role {0} don't exist",RoleName);
                    await _RoleManager.CreateAsync(new Role { Name = RoleName });
                    _Logger.LogInformation("Role {0} is created",RoleName);
                }
            }
            await CheckRole(Role.Administrators);
            await CheckRole(Role.Users);

            if(await _RoleManager.FindByNameAsync(User.Administrator)is null)
            {
                _Logger.LogInformation("User {0} don't exist",User.Administrator);
                var admin = new User
                {
                    UserName = User.Administrator,
                };
                var creation_result = await _UserManager.CreateAsync(admin, User.DefaultAdminPassword);
                if(creation_result.Succeeded)
                {
                    _Logger.LogInformation("User {0} is created", User.Administrator);
                    await _UserManager.AddToRoleAsync(admin, Role.Administrators);

                    _Logger.LogInformation("Role of User {0} is created {1}", User.Administrator, Role.Administrators);
                }
                else 
                {
                    var errors = creation_result.Errors.Select(err => err.Description).ToArray();
                    _Logger.LogError("Row of administrator don't created!Errors:{0}",string.Join(",",errors));

                    throw new InvalidOperationException($"impossible create administrator{string.Join(",",errors)}");
                }

                _Logger.LogInformation("The donnes ofsystem Identity are created {0} mc", timer.Elapsed.TotalMilliseconds);

            }

        }
       
    }
}
