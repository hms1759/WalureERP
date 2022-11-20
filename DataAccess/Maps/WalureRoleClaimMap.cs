using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Model.Identity;
using DataAccess.Enums;
using DataAccess.Helper;

namespace DataAccess.Maps
{
    public class WalureRoleClaimMap : IEntityTypeConfiguration<WalureRoleClaim>
    {
        private static int counter = 0;

        public void Configure(EntityTypeBuilder<WalureRoleClaim> builder)
        {
            builder.ToTable(nameof(WalureRoleClaim));
            SetupData(builder);
        }

        private void SetupData(EntityTypeBuilder<WalureRoleClaim> builder)
        {
            var roleDictionary = new Dictionary<string, Guid>()
            {
                { RoleHelpers.INBUILT_ADMIN, RoleHelpers.INBUILT_ADMIN_ID()},
                { RoleHelpers.WALURE_ADMIN, RoleHelpers.WALURE_ADMIN_ID() },
                { RoleHelpers.WALURE_BASIC_USER, RoleHelpers.WALURE_BASIC_USER_ID()},
              
            };
            var permissions = (Permission[])Enum.GetValues(typeof(Permission));

            Array.ForEach(permissions, (p) =>
            {
                if (!string.IsNullOrWhiteSpace(p.GetPermissionCategory()) || roleDictionary.ContainsKey(p.GetPermissionCategory()))
                {
                    builder.HasData(new WalureRoleClaim()
                    {
                        Id = ++counter,
                        RoleId = roleDictionary[p.GetPermissionCategory()],
                        ClaimType = nameof(WalureRoleClaim),
                        ClaimValue = p.ToString(),
                    });
                }
            });
        }

    }
}
