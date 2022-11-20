
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
            CreatedBy = Defaults.SysUserEmail.ToString(),
            CreatedOn = Defaults.CreatedOn,
            ModifiedBy = Defaults.SysUserEmail.ToString(),
            ModifiedOn = Defaults.ModifiedOn,
            Id = Guid.Parse("C611F116-75DA-4D1C-9975-C1F862D12C20"),
            Email = Defaults.SysUserEmail.ToString(),
            FirstName= Defaults.SysUserEmail.ToString(),
            LastName= Defaults.SysUserEmail.ToString(),
            EmailConfirmed = true,
            NormalizedEmail = Defaults.SysUserEmail.ToString().ToUpper(),
            PhoneNumber = "08009300832",
            UserName = Defaults.SysUserEmail.ToString(),
            NormalizedUserName = Defaults.SysUserEmail.ToString().ToUpper(),
            TwoFactorEnabled = false,
            PhoneNumberConfirmed = true,
            PasswordHash = Hasher.HashPassword(null, "micr0s0ft_"),
            SecurityStamp = "536f8ac3-0df8-45d2-8f34-630d0a2ed6e6",
            UserType = UserTypes.inBuilt,
        };


        builder.HasData(WalureUser);
    }

   
}