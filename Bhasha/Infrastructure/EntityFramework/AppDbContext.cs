using Bhasha.Identity;
using Bhasha.Infrastructure.EntityFramework.Dtos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bhasha.Infrastructure.EntityFramework;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser, AppRole, string>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ChapterDto>()
            .HasMany(e => e.Expressions)
            .WithMany(e => e.ChapterDtos);
        
        base.OnModelCreating(builder);
    }

    public required DbSet<ChapterDto> Chapters { get; set; }
    public required DbSet<ExpressionDto> Expressions { get; set; }
    public required DbSet<ProfileDto> Profiles { get; set; }
    public required DbSet<TranslationDto> Translations { get; set; }
}