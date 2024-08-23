using Microsoft.AspNetCore.Identity;
using System.Collections;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Address { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public ICollection<Order> Orders { get; set; }

        // The virtual keyword is used to enable lazy loading in Entity Framework
        // When we mark a navigation property as 'virtual', EF ensures that the navigation property is only loaded from the database when they are needed

        public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; }       // Each ApplicationUser is associated with multiple UserRoles   (corresponding to the table AspNetUserRoles)
        //public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }        // Corresponding to the table AspNetUserClaims
        //public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }        // Corresponding to the table AspNetUserLogins
        //public virtual ICollection<IdentityUserToken<string>> Tokens { get; set; }        // Corresponding to the table AspNetUserTokens

    }
}
