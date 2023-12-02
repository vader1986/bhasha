using Bhasha.Identity;
using Bhasha.Infrastructure.EntityFramework.Dtos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bhasha.Infrastructure.EntityFramework;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public required DbSet<ChapterDto> Chapters { get; set; }
    public required DbSet<ExpressionDto> Expressions { get; set; }
    public required DbSet<ProfileDto> Profiles { get; set; }
    public required DbSet<TranslationDto> Translations { get; set; }
}