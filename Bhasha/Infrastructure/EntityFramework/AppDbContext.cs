using Bhasha.Identity;
using Bhasha.Infrastructure.EntityFramework.Dtos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bhasha.Infrastructure.EntityFramework;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser, AppRole, string>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ChapterDto>()
            .HasMany(e => e.Expressions)
            .WithMany(e => e.ChapterDtos);

        builder.Entity<ChapterDto>()
            .HasOne(e => e.Description);

        builder.Entity<ChapterDto>()
            .HasOne(e => e.Name);

        base.OnModelCreating(builder);
    }

    public required DbSet<ChapterDto> Chapters { get; set; }
    public required DbSet<ExpressionDto> Expressions { get; set; }
    public required DbSet<ProfileDto> Profiles { get; set; }
    public required DbSet<TranslationDto> Translations { get; set; }
    public required DbSet<StudyCardDto> StudyCards { get; set; }
}