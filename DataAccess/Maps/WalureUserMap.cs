
using DataAccess.Enums;
using DataAccess.Model.Identity;
using DataAccess.Password;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;

namespace DataAccess.Maps;

public class WalureUserMap : IEntityTypeConfiguration<WalureUser>
{
    public CustomPasswordHasher Hasher { get; set; } = new CustomPasswordHasher();

    public void Configure(EntityTypeBuilder<WalureUser> builder)
    {
        builder.ToTable(nameof(WalureUser));
        SetupUsers(builder);
    }

    private void SetupUsers(EntityTypeBuilder<WalureUser> builder)
    {
        var WalureUser = new WalureUser
        {
            CreatedBy = Defaults.SysUserEmail,
            CreatedOn = Defaults.CreatedOn,
            ModifiedBy = Defaults.SysUserEmail,
            ModifiedOn = Defaults.ModifiedOn,
            Id = Defaults.SysUserId,
            Email = Defaults.SysUserEmail,
            FirstName= Defaults.SysName.ToString(),
            LastName= Defaults.SysName.ToString(),
            NormalizedEmail = Defaults.SysUserEmail.ToUpper(),
            PhoneNumber = "08009300832",
            UserName = Defaults.SysUserEmail,
            NormalizedUserName = Defaults.SysUserEmail.ToUpper(),
            TwoFactorEnabled = false,
            PhoneNumberConfirmed = false,
            PasswordHash = Hasher.HashPassword(null, "micr0s0ft_"),
            SecurityStamp = Guid.NewGuid().ToString(),
            UserType = UserTypes.inBuilt,
            AccessFailedCount = 1
        };


        builder.HasData(WalureUser);
    }

   
}