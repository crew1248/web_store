namespace x_nova_template.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using x_nova_template.Models;
    using System;
    using System.Configuration;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;

    internal sealed class Configuration : DbMigrationsConfiguration<x_nova_template.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(x_nova_template.Models.ApplicationDbContext context)
        {

            //for (int t = 0; t <= 5; t++)
            //{
            //    context.Categories.AddOrUpdate(x => x.ProductName, new Product { CategoryID = context.Categories.Where(x => x.ID == 1).SingleOrDefault().ID, ProductName = "Продукт под номером" + t, Price = 1537, Description = "Описание для товара" });
            //}

            //context.SaveChanges();
            //context.Database.ExecuteSqlCommand("DELETE FROM dbo.AspNetUsers WHERE Email!={0}","admin@admin.ru");



            context.Configs.AddOrUpdate(x => x.ConfigID,
              new Config { ConfigID = 1, SelectedIsOnlineID = false }
            );
            context.SaveChanges();
            context.Menues.AddOrUpdate(x => x.Url,
              new Menu { Url = "Home", Text = "title", ParentId = 0, MenuSection = 0, SortOrder = 0, LastModifiedDate = DateTime.Now }
              );
            context.SaveChanges();

            context.StaticSections.AddOrUpdate(x => x.Title,
                new StaticSection { Content = "ss", SectionType = 1, Title = "static1" },
                new StaticSection { Content = "ss", SectionType = 2, Title = "static2" },
                new StaticSection { Content = "ss", SectionType = 3, Title = "static3" },
                new StaticSection { Content = "ss", SectionType = 4, Title = "static4" },
                new StaticSection { Content = "ss", SectionType = 5, Title = "static5" },
                new StaticSection { Content = "ss", SectionType = 6, Title = "static6" }
            );
            context.SaveChanges();



            /* context.Categories.ToList().ForEach(x => x.CatType = "�����-�����");*/
            // This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            InsertAdmin(context);
        }

        public void InsertAdmin(x_nova_template.Models.ApplicationDbContext context)
        {

            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (userManager.FindByName("admin") == null)
            {

                var adminName = "admin";
                var adminEmail = "admin@admin.ru";
                
                var role1 = new IdentityRole { Name = "admin" };
                var role2 = new IdentityRole { Name = "user" };

               
                roleManager.Create(role1);
                roleManager.Create(role2);

             
                var admin = new ApplicationUser { Email = adminEmail, UserName = adminName };
                string password = "123222";
                var result = userManager.Create(admin, password);
                var adminId = userManager.FindByEmail(adminEmail).Id;

                //userManager.SetTwoFactorEnabled(adminId, false);


                // ���� �������� ������������ ������ �������
                if (result.Succeeded)
                {



                    // ��������� ��� ������������ ����
                    userManager.AddToRole(admin.Id, role1.Name);
                    userManager.AddToRole(admin.Id, role2.Name);
                }
            }
        }
    }
}
