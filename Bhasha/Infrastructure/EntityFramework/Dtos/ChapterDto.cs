using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bhasha.Infrastructure.EntityFramework.Dtos;

public class ChapterDto
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	public int RequiredLevel { get; set; }
	[MaxLength(50)]
	public string? ResourceId { get; set; }
	[MaxLength(50)]
	public required string AuthorId { get; set; }
	public required ExpressionDto Name { get; set; }
	public required ExpressionDto Description{ get; set; }
	public List<ExpressionDto> Expressions { get; set; } = [];
}