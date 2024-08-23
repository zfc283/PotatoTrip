using Microsoft.EntityFrameworkCore;
using System;
using WebApplication1.Models;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Database
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>  // 使数据库支持身份认证功能        // : DbContext   // 代码和数据库数据的连接器      
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<TravelRoute> TravelRoutes { get; set; }                 // 映射模型到数据库中
        public DbSet<TravelRoutePicture> TravelRoutePictures { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<LineItem> LineItems { get; set; }
        public DbSet<Order> Orders { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)     // modelBuilder 用来在创建模型和数据库表之间映射关系时进行补充说明, 可以自定义表名, 自定义表字段, 初始化数据库数据等
        {
            //modelBuilder.Entity<TravelRoute>().HasData(new TravelRoute()
            //{
            //    Id = Guid.NewGuid(),
            //    Title = "test",
            //    Description = "test description",
            //    OriginalPrice = 0,
            //    CreateTime = DateTime.Now
            //});

            var travelRouteJsonData = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/Database/travelRoutesMockData.json");       // The @ sign tells the compiler that the string should be interpreted exactly as it is typed
            IList<TravelRoute> travelRoutes = JsonConvert.DeserializeObject<IList<TravelRoute>>(travelRouteJsonData);
            modelBuilder.Entity<TravelRoute>().HasData(travelRoutes);

            var travelRoutePictureJsonData = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/Database/travelRoutePicturesMockData.json");       
            IList<TravelRoutePicture> travelRoutePictures = JsonConvert.DeserializeObject<IList<TravelRoutePicture>>(travelRoutePictureJsonData);
            modelBuilder.Entity<TravelRoutePicture>().HasData(travelRoutePictures);

            // 初始化用户与角色的种子数据
            // 1. Configure the foreign-key relationship between ApplicationUser and UserRoles

            modelBuilder.Entity<ApplicationUser>(user => {
                user.HasMany(x => x.UserRoles)          // configures the one-to-many relationship from ApplicationUser to IdentityUserRole<string> (each instance of ApplicationUser can have many instances of IdentityUserRole<string>)
                .WithOne()       // specifies that the other side of the relationship (from IdentityUserRole<string> to ApplicationUser) is a one-to-zero-or-one relationship (Typically, you might see .WithOne(y => y.User) if there is a navigation property on the IdentityUserRole pointing back to the ApplicationUser. However, if there's no such navigation property, you simply use .WithOne() without parameters)
                .HasForeignKey(ur => ur.UserId)    // specifies which property on the IdentityUserRole<string> serves as the foreign key
                .IsRequired();         // enforces that the foreign key UserId cannot be null (each role must be associated with a user)
            });

            // 2. 添加管理员角色
            var adminRoleId = "308660dc-ae51-480f-824d-7dca6714c3e2"; // guid 
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper()
                }
            );

            // 3. 添加用户
            var adminUserId = "90184155-dee0-40c9-bb1e-b5ed07afc04e";
            ApplicationUser adminUser = new ApplicationUser
            {
                Id = adminUserId,
                UserName = "admin@potatotrip.site",
                NormalizedUserName = "admin@potatotrip.site".ToUpper(),
                Email = "admin@potatotrip.site",
                NormalizedEmail = "admin@potatotrip.site".ToUpper(),
                TwoFactorEnabled = false,
                EmailConfirmed = true,
                PhoneNumber = "123456789",
                PhoneNumberConfirmed = false
            };
            PasswordHasher<ApplicationUser> ph = new PasswordHasher<ApplicationUser>();
            adminUser.PasswordHash = ph.HashPassword(adminUser, "Abc123$");
            modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

            // 4. 给用户加入管理员角色
            // 通过使用 linking table：IdentityUserRole
            modelBuilder.Entity<IdentityUserRole<string>>()
                .HasData(new IdentityUserRole<string>()
                {
                    RoleId = adminRoleId,
                    UserId = adminUserId
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
