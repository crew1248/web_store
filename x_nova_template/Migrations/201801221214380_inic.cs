namespace x_nova_template.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class inic : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    CategoryName = c.String(maxLength: 300),
                    Sequance = c.Int(),
                    CatDescription = c.String(),
                    CatType = c.String(maxLength: 300),
                    Sortindex = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.Products",
                c => new
                {
                    //                   [StringLength(500)]
                    //public string Packaging { get; set; } // ����

                    //public float PackagingSize {get;set;} //������ ����

                    //public float Weight { get; set; } // ���
                    // [StringLength(500)]
                    //public string Manufacturer { get; set; } //�������������
                    // [StringLength(500)]
                    //public string Cloth { get; set;  }//�����
                    // [StringLength(500)]
                    //public string Color { get; set; } //�����
                    // [StringLength(500)]
                    //public string Lacquering { get; set; }//���������
                    // [StringLength(500)]
                    //public string Decor { get; set; } //�����

                    //public int Discount { get; set; } //������
                    // [StringLength(500)]
                    //public string Material { get; set;  }//��������
                    ID = c.Int(nullable: false, identity: true),
                    ProductName = c.String(maxLength: 500),
                    Description = c.String(),
                    PackagingSize = c.Int(),
                    Weight = c.Int(),
                    Size = c.String(maxLength: 500),
                    Fill = c.String(maxLength: 500),
                    Manufacturer = c.String(maxLength: 500),
                    Cloth = c.String(maxLength: 500),
                    Color = c.String(maxLength: 500),
                    VisitesCount = c.Int(),
                    Lacquering = c.String(maxLength: 500),
                    Decor = c.String(maxLength: 500),
                    Discount = c.Int(),
                    Sortindex = c.Int(nullable: false),
                    CategoryID = c.Int(nullable: false),
                    Price = c.Single(nullable: false),
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.CategoryID);

            CreateTable(
                "dbo.ProdImages",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    ImageDataType = c.Binary(),
                    ImageMimeType = c.String(maxLength: 300),
                    IsPreview = c.Int(nullable: false),
                    ProductID = c.Int(nullable: false),
                    Sortindex = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID);

            CreateTable(
                "dbo.Configs",
                c => new
                {
                    ConfigID = c.Int(nullable: false, identity: true),
                    SiteName = c.String(maxLength: 150),
                    Robots = c.String(maxLength: 500),
                    SiteAddress = c.String(maxLength: 100),
                    Description = c.String(maxLength: 250),
                    Keywords = c.String(maxLength: 150),
                    Email = c.String(maxLength: 150),
                    SelectedIsOnlineID = c.Boolean(nullable: false),
                    OfflineMessage = c.String(),
                })
                .PrimaryKey(t => t.ConfigID);

            CreateTable(
                "dbo.Galleries",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    GalleryTitle = c.String(maxLength: 500),
                    GalleryData = c.Binary(),
                    Sortindex = c.Int(nullable: false),
                    GalleryMimeType = c.String(maxLength: 100),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.Images",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    ImageTitle = c.String(maxLength: 500),
                    ImageData = c.Binary(),
                    GalleryID = c.Int(nullable: false),
                    ImageMimeType = c.String(maxLength: 100),
                    Sortindex = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Galleries", t => t.GalleryID, cascadeDelete: true)
                .Index(t => t.GalleryID);

            CreateTable(
                "dbo.ImportDataProducts",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Title = c.String(),
                    Description = c.String(),
                    Category = c.String(),
                    Price = c.String(),
                    IsDeleted = c.String(),
                    ImgUrl = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Messages",
                c => new
                {
                    MessID = c.Int(nullable: false, identity: true),
                    TextMess = c.String(),
                    DateAdded = c.DateTime(nullable: false),
                    Visited = c.Boolean(nullable: false),
                    UserID = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.MessID)
                .ForeignKey("dbo.LiveUsers", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);

            CreateTable(
                "dbo.LiveUsers",
                c => new
                {
                    UserID = c.Int(nullable: false, identity: true),
                    UserName = c.String(nullable: false),
                    IsAdmin = c.Boolean(nullable: false),
                    IsOnline = c.Boolean(nullable: false),
                    FeedMessage = c.String(nullable: false),
                    Email = c.String(nullable: false),
                    ConnId = c.String(),
                    GroupId = c.String(),
                })
                .PrimaryKey(t => t.UserID);

            CreateTable(
                "dbo.Menus",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ParentId = c.Int(nullable: false),
                    Text = c.String(nullable: false, maxLength: 200),
                    Url = c.String(nullable: false, maxLength: 200),
                    Body = c.String(),
                    BodyEng = c.String(),
                    SeoDescription = c.String(),
                    LastModifiedDate = c.DateTime(nullable: false),
                    SeoKeywords = c.String(),
                    SortOrder = c.Int(nullable: false),
                    MenuSection = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.OrderItems",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    ProductName = c.String(maxLength: 300),
                    Quantity = c.Int(nullable: false),
                    OrderID = c.Int(nullable: false),
                    Price = c.Single(nullable: false),
                    Category = c.String(maxLength: 300),
                    Description = c.String(maxLength: 500),
                    Cloth = c.String(maxLength: 500),
                    Color = c.String(maxLength: 500),
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .Index(t => t.OrderID);

            CreateTable(
                "dbo.Orders",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 300),
                    Address = c.String(maxLength: 500),
                    Phone = c.String(nullable: false),
                    OrderStatus = c.String(maxLength: 300),
                    Country = c.String(maxLength: 300),
                    Delivery = c.String(maxLength: 300),
                    Payment = c.String(maxLength: 300),
                    Comment = c.String(),
                    EmailAddress = c.String(),
                    CreatedAt = c.DateTime(nullable: false),
                    OrderSum = c.Single(nullable: false),
                    Sequance = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.OrderStatus",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    Sequance = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.Portfolios",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    ImageData = c.Binary(),
                    ImageMimeType = c.String(),
                    Title = c.String(nullable: false, maxLength: 200),
                    Description = c.String(maxLength: 250),
                    Price = c.Int(),
                    Sortindex = c.Int(),
                    ProdLink = c.String(),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.Posts",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Title = c.String(nullable: false, maxLength: 500),
                    Body = c.String(nullable: false),
                    PreviewPhoto = c.Binary(),
                    CreatedAt = c.DateTime(nullable: false),
                    Preview = c.String(maxLength: 500),
                    Sortindex = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.AspNetRoles",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    Name = c.String(nullable: false, maxLength: 256),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");

            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                {
                    UserId = c.String(nullable: false, maxLength: 128),
                    RoleId = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);

            CreateTable(
                "dbo.StaticSections",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Title = c.String(maxLength: 300),
                    Sequance = c.Int(nullable: false),
                    Content = c.String(),
                    CreatedAt = c.DateTime(),
                    Preview = c.String(maxLength: 500),
                    Type = c.Int(nullable: false),
                    SectionType = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.AspNetUsers",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    Phone = c.String(),
                    Name = c.String(),
                    Sirname = c.String(),
                    Firstname = c.String(),
                    Address = c.String(),
                    Delivery = c.String(),
                    Payment = c.String(),
                    Email = c.String(maxLength: 256),
                    EmailConfirmed = c.Boolean(nullable: false),
                    PasswordHash = c.String(),
                    SecurityStamp = c.String(),
                    PhoneNumber = c.String(),
                    PhoneNumberConfirmed = c.Boolean(nullable: false),
                    TwoFactorEnabled = c.Boolean(nullable: false),
                    LockoutEndDateUtc = c.DateTime(),
                    LockoutEnabled = c.Boolean(nullable: false),
                    AccessFailedCount = c.Int(nullable: false),
                    UserName = c.String(nullable: false, maxLength: 256),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");

            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UserId = c.String(nullable: false, maxLength: 128),
                    ClaimType = c.String(),
                    ClaimValue = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                {
                    LoginProvider = c.String(nullable: false, maxLength: 128),
                    ProviderKey = c.String(nullable: false, maxLength: 128),
                    UserId = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.OrderItems", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.Messages", "UserID", "dbo.LiveUsers");
            DropForeignKey("dbo.Images", "GalleryID", "dbo.Galleries");
            DropForeignKey("dbo.ProdImages", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Products", "CategoryID", "dbo.Categories");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.OrderItems", new[] { "OrderID" });
            DropIndex("dbo.Messages", new[] { "UserID" });
            DropIndex("dbo.Images", new[] { "GalleryID" });
            DropIndex("dbo.ProdImages", new[] { "ProductID" });
            DropIndex("dbo.Products", new[] { "CategoryID" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.StaticSections");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Posts");
            DropTable("dbo.Portfolios");
            DropTable("dbo.OrderStatus");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderItems");
            DropTable("dbo.Menus");
            DropTable("dbo.LiveUsers");
            DropTable("dbo.Messages");
            DropTable("dbo.ImportDataProducts");
            DropTable("dbo.Images");
            DropTable("dbo.Galleries");
            DropTable("dbo.Configs");
            DropTable("dbo.ProdImages");
            DropTable("dbo.Products");
            DropTable("dbo.Categories");
        }
    }
}
