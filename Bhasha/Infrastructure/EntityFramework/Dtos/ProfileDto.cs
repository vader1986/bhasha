using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bhasha.Infrastructure.EntityFramework.Dtos;

public class ProfileDto
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [MaxLength(100)]
    public required string UserId { get; set; }
    [MaxLength(20)]
    public required string Native { get; set; }
    [MaxLength(20)]
    public required string Target { get; set; }
    public int Level { get; set; }
    public int? CurrentChapterId { get; set; }
    public int? CurrentPageIndex { get; set; }
    [MaxLength(50)]
    public required string ValidationResults { get; set; }
    public int[] CompletedChapters { get; set; } = [];
}