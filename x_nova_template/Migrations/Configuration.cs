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

            //var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sql1.sql");
            //context.Database.ExecuteSqlCommand(File.ReadAllText(path));

            // context.Database.ExecuteSqlCommand("alter table dbo.Menus drop column Author,IsPublic");
            //context.Menues.AddOrUpdate(x => x.Author, new Menu {Url="strstr", Body="strstr", Author="strstr", IsPublic=true, LastModifiedDate=DateTime.Now, ParentId=0, MenuSection=1, SortOrder=6, Text="strstr"});
            //for (int t = 0; t <= 5; t++)
            //{
            //    context.Categories.AddOrUpdate(x => x.CategoryName, new Category { CategoryName = "Категория номер" + t });
            //}

            //for (int t = 0; t <= 15; t++)
            //{
            //    context.Products.AddOrUpdate(x => x.ProductName, new Product { ProductName = "Категория номер" + t, Price = 1756, CategoryID = 1, Description = "Описчание товара для наглядности отображения" });
            //}

            //context.SaveChanges();
            //context.Database.ExecuteSqlCommand("DELETE FROM dbo.AspNetUsers WHERE Email!={0}","admin@admin.ru");


            if (!context.Configs.Any())
            {
                context.Configs.AddOrUpdate(x => x.ConfigID, new Config { SelectedIsOnlineID = false });
                context.SaveChanges();
            }
           //dfdf sadf
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
