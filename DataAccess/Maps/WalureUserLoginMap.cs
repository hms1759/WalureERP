using DataAccess.Model.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models.Map;

public class WalureUserLoginMap : IEntityTypeConfiguration<WalureUserLogin>
{
    public void Configure(EntityTypeBuilder<WalureUserLogin> builder)
    {
        builder.ToTable(nameof(WalureUserLogin));
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd();
        builder.HasKey(u => new { u.LoginProvider, u.ProviderKey });
    }
}
