#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
// ReSharper disable PropertyCanBeMadeInitOnly.Global

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bhasha.Infrastructure.EntityFramework.Dtos;

public sealed class ChapterDto
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
	
	public List<ExpressionDto> Expressions { get; set; }
	
	public List<StudyCardDto> StudyCards { get; set; }
}