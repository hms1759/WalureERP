using DataAccess.Maps;
using DataAccess.Model;
using DataAccess.Model.Identity;
using DataAccess.Model.OpenIddict;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Map;

namespace DataAccess.Config
{
    public class ApplicationDbContext : IdentityDbContext<WalureUser, WalureRole, Guid, WalureUserClaim, WalureUserRole, WalureUserLogin, WalureRoleClaim, WalureUserToken>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new WalureRoleClaimMap());
            modelBuilder.ApplyConfiguration(new WalureRoleMap());
            modelBuilder.ApplyConfiguration(new WalureUserMap());
            modelBuilder.ApplyConfiguration(new WalureUserRoleMap());
            modelBuilder.ApplyConfiguration(new WalureUserLoginMap());
            modelBuilder.ApplyConfiguration(new WalureUserTokenMap());
            modelBuilder.ApplyConfiguration(new WalureUserClaimMap());

            modelBuilder.UseOpenIddict<WalureOpenIddictApplication, WalureOpenIddictAuthorization, WalureOpenIddictScope, WalureOpenIddictToken, Guid>();

            var typesToRegister = typeof(BaseEntity).Assembly.GetTypes().Where(type => !String.IsNullOrEmpty(type.Namespace))
          .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(BaseEntityTypeConfiguration<>));

            foreach (var configurationInstance in typesToRegister.Select(Activator.CreateInstance))
            {
                modelBuilder.ApplyConfiguration((dynamic)configurationInstance);
            }

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }


    }
}
