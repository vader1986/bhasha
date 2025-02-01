using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bhasha.Infrastructure.EntityFramework.Dtos;

public class TranslationDto
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	public required string Language { get; set; }
	public required string Text { get; set; }
	public string? Spoken { get; set; }
	public string? AudioId { get; set; }
	public ExpressionDto Expression { get; set; }
}