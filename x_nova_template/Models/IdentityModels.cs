using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace x_nova_template.Models
{
    // Чтобы добавить данные профиля для пользователя, можно добавить дополнительные свойства в класс ApplicationUser. Дополнительные сведения см. по адресу: http://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }

        [Display(Name = "Телефон")]
        public string Phone { get; set; }
        
        [Display(Name = "Имя")]
        public string Name { get; set; }
        [Display(Name = "Фамилия")]
        public string Sirname { get; set; }
        [Display(Name = "Фамилия")]
        public string Firstname { get; set; }
        [Display(Name = "Адрес")]
        public string Address { get; set; }

        [Display(Name = "Тип доставки")]
        public string Delivery { get; set; }
        [Display(Name = "Способ оплаты")]
        public string Payment { get; set; }
       
        
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }


        public DbSet<ImportDataProduct> ImportProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Config> Configs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Menu> Menues { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<ProdImage> ProdImages { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<LiveUser> LiveUsers { get; set; }
        public DbSet<StaticSection> StaticSections { get; set; }
        //public DbSet<LiveRoom> LiveRooms{ get; set; }
        //public DbSet<LiveConnection> LiveConnections { get; set; }
        public DbSet<Message> LiveMessages { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}