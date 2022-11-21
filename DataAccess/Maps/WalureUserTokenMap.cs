
using DataAccess.Model.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Maps
{
    public class WalureUserTokenMap : IEntityTypeConfiguration<WalureUserToken>
    {
        public void Configure(EntityTypeBuilder<WalureUserToken> builder)
        {
            builder.ToTable(nameof(WalureUserToken));
            builder.HasKey(p => p.UserId);
        }
    }
}
