using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Music.DbMigrator.Domain;

namespace Music.DbMigrator.ModelConfig;

public class MusicInfoConfig:IEntityTypeConfiguration<MusicInfo>
{
    public void Configure(EntityTypeBuilder<MusicInfo> builder)
    {
        builder.ToTable("m_music_info");
        builder.Property(b => b.id).ValueGeneratedNever().HasMaxLength(50);
        builder.Property(b => b.musicName).HasMaxLength(50);
        builder.Property(b => b.musicAlbum).HasMaxLength(50);
        builder.Property(b => b.musicStar).HasMaxLength(50);
        builder.Property(b => b.musicDuration).HasMaxLength(50);
        builder.Property(b => b.musicSingerName).HasMaxLength(50);
        builder.Property(b => b.musicComposer).HasMaxLength(50);
        builder.Property(b => b.createTime).HasMaxLength(50);
        builder.Property(b => b.createBy).HasMaxLength(50);
        builder.Property(b => b.isDel).HasMaxLength(2);
        builder.Property(b => b.updateTime).HasMaxLength(50);
        builder.Property(b => b.updateBy).HasMaxLength(50);
        builder.Property(b => b.musicUrl).HasMaxLength(50);
        builder.Property(b => b.arr02).HasMaxLength(50);
        builder.Property(b => b.arr03).HasMaxLength(50);
    }
}