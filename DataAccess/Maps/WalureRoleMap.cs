using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Model.Identity;
using DataAccess.Helper;

namespace DataAccess.Maps
{
    public class WalureRoleMap : IEntityTypeConfiguration<WalureRole>
    {
        public void Configure(EntityTypeBuilder<WalureRole> builder)
        {
            builder.ToTable(nameof(WalureRole));
            SetupData(builder);
        }
        private void SetupData(EntityTypeBuilder<WalureRole> builder)
        {
            var roles = new WalureRole[]
            {
                new WalureRole
                {
                    Id = RoleHelpers.INBUILT_ADMIN_ID(),
                    Name = RoleHelpers.INBUILT_ADMIN,
                    NormalizedName = RoleHelpers.INBUILT_ADMIN.ToUpper(),
                    InBuilt = true,
                    ConcurrencyStamp = "e437a567e45c4b01a5d1cefe125023d7",
                    CreatedOn = Defaults.CreatedOn,
                    ModifiedOn= Defaults.ModifiedOn,
                },
                new WalureRole
                {
                    Id = RoleHelpers.WALURE_ADMIN_ID(),
                    Name = RoleHelpers.WALURE_ADMIN,
                    NormalizedName = RoleHelpers.WALURE_ADMIN.ToUpper(),
                    InBuilt = true,
                     ConcurrencyStamp = "42b12c0386ce4c9baaea63aec3ade765",
                       CreatedOn = Defaults.CreatedOn,
                    ModifiedOn= Defaults.ModifiedOn,
                },
                new WalureRole
                {
                    Id = RoleHelpers.WALURE_BASIC_USER_ID(),
                    Name = RoleHelpers.WALURE_BASIC_USER,
                    NormalizedName = RoleHelpers.WALURE_BASIC_USER.ToUpper(),
                    InBuilt = true,
                     ConcurrencyStamp = "42b13c0386ce4c9baaea63aec3ade775",
                       CreatedOn = Defaults.CreatedOn,
                    ModifiedOn= Defaults.ModifiedOn,
                }
            };

            builder.HasData(roles);
        }

    }
}