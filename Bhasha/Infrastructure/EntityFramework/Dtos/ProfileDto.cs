using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bhasha.Infrastructure.EntityFramework.Dtos;

public class ProfileDto
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Native { get; set; }
    public string Target { get; set; }
    public int Level { get; set; }
    public int? CurrentChapterId { get; set; }
    public int? CurrentPageIndex { get; set; }
    public string ValidationResults { get; set; }
    public int[] CompletedChapters { get; set; } = [];
}