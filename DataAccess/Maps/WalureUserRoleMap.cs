using System;
using System.Collections.Generic;
using DataAccess.Helper;
using DataAccess.Model.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DataAccess.Maps
{
    public class WalureUserRoleMap : IEntityTypeConfiguration<WalureUserRole>
    {
        public void Configure(EntityTypeBuilder<WalureUserRole> builder)
        {
            builder.ToTable("WalureUserRole");
            builder.HasKey(p => new { p.UserId, p.RoleId });
            SetupData(builder);
        }

        private void SetupData(EntityTypeBuilder<WalureUserRole> builder)
        {
            List<WalureUserRole> dataList = new List<WalureUserRole>()
            {
                new WalureUserRole()
                {
                    UserId = Defaults.SysUserId,
                    RoleId = RoleHelpers.INBUILT_ADMIN_ID(),
                    
                }
            };

            builder.HasData(dataList);
        }
    }
}