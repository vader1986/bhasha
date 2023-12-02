using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bhasha.Infrastructure.EntityFramework.Dtos;

public class ChapterDto
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	public int RequiredLevel { get; set; }
	public string? ResourceId { get; set; }
	public string AuthorId { get; set; }
	public ExpressionDto Name { get; set; }
	public ExpressionDto Description{ get; set; }
	public ICollection<ExpressionDto> Expressions { get; set; }
}