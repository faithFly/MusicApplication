using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Music.DbMigrator.Domain;

namespace Music.DbMigrator.ModelConfig;

public class FaithUserConfig:IEntityTypeConfiguration<FaithUser>
{
    public void Configure(EntityTypeBuilder<FaithUser> builder)
    {
        builder.ToTable("m_user");
        builder.Property(b => b.id).ValueGeneratedNever().HasMaxLength(50);
        builder.Property(b => b.userName).HasMaxLength(255);
        builder.Property(b => b.phoneNumber).HasMaxLength(50);
        builder.Property(b => b.passWord).HasMaxLength(255);
        builder.Property(b => b.userAddress).HasMaxLength(255);
        builder.Property(b => b.userEmail).HasMaxLength(255);
        builder.Property(b => b.isdel).HasMaxLength(2);

    }
}