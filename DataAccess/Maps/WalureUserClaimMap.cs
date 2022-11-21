using DataAccess.Model.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Maps
{
    public class WalureUserClaimMap : IEntityTypeConfiguration<WalureUserClaim>
    {
        public void Configure(EntityTypeBuilder<WalureUserClaim> builder)
        {
            builder.ToTable(nameof(WalureUserClaim));
            builder.HasKey(c => c.Id);
        }
    }
}
