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
            Teams = new HashSet<Team>();
        }

        public string Name { get; set; }

        public virtual ICollection<Team> Teams { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom user claims here            
            userIdentity.AddClaim(new Claim("Name", Name != null ? Name : " "));
            return userIdentity;
        }

        public ICollection<Pool> GetPools()
        {
            ICollection<Pool> pools = new SortedSet<Pool>();

            foreach (Team team in Teams)
                pools.Add(team.Pool);

            return pools;
        }
    }
}
