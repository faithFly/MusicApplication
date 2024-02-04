using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Music.DbMigrator.Domain;

namespace Music.DbMigrator;

public class MusicDbContext :DbContext
{
    public readonly IConfiguration _config;
    public DbSet<FaithUser> FaithUsers { get; set; }

    public MusicDbContext(){}

    public MusicDbContext(DbContextOptions<MusicDbContext> options) : base(options)
    {
        _config = _config;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMySql("Database=faith_db;Data Source=localhost;Port=3306;User Id=root;Password=Xyf12138.;Charset=utf8;TreatTinyAsBoolean=false;", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.27-mysql"));
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //应用程序集中的配置
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}