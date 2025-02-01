using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bhasha.Infrastructure.EntityFramework.Dtos;

public class TranslationDto
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	[MaxLength(20)]
	public required string Language { get; set; }
	[MaxLength(500)]
	public required string Text { get; set; }
	[MaxLength(500)]
	public string? Spoken { get; set; }
	[MaxLength(50)]
	public string? AudioId { get; set; }
	public required  ExpressionDto Expression { get; set; }
}