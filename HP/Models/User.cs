namespace HP.Models
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Security.Claims;
    using System.Threading.Tasks;

    [Table("nlpool.User")]
    public partial class User : IdentityUser
    {
        public User()
        {
            Teams = new HashSet<UserTeam>();
        }

        public string Name { get; set; }

        public virtual ICollection<UserTeam> Teams { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom user claims here            
            userIdentity.AddClaim(new Claim(ClaimTypes.Name, Name ?? " "));
            //userIdentity.AddClaim(new Claim(ClaimTypes.UserData, Teams));
            foreach (var role in Roles)
                userIdentity.AddClaim(new Claim(ClaimTypes.Role, role.RoleId));
            return userIdentity;
        }

        public ICollection<Pool> GetPools()
        {
            ICollection<Pool> pools = new List<Pool>();

            foreach (UserTeam team in Teams)
                pools.Add(team.Team.Pool);

            return pools;
        }
    }
}
