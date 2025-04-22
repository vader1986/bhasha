using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bhasha.Infrastructure.EntityFramework.Dtos;

public sealed class StudyCardDto
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [MaxLength(20)]
    public string Language { get; set; } = string.Empty;

    [MaxLength(20)]
    public string StudyLanguage { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(2000)]
    public string Content { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string? AudioId { get; set; }
}