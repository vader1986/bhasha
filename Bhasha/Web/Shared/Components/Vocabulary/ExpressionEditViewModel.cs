using Bhasha.Domain;

namespace Bhasha.Web.Shared.Components.Vocabulary;

public sealed class ExpressionEditViewModel
{
    public int Id { get; set; }
    public int Level { get; set; } = 1;
    public ExpressionType? Type { get; set; }
    public PartOfSpeech? PartOfSpeech { get; set; }
    public CEFR? Cefr { get; set; }
    public List<string> Labels { get; set; } = [];
    public string? ResourceId { get; set; }
    public List<string> Synonyms { get; set; } = [];

    public Expression ToExpression() => new(
        Id: Id,
        ExpressionType: Type,
        PartOfSpeech: PartOfSpeech,
        Cefr: Cefr,
        ResourceId: ResourceId,
        Labels: Labels.ToArray(),
        Synonyms: Synonyms.ToArray(),
        Level: Level);
    

    public static ExpressionEditViewModel Create(Expression expression) => new()
    {
        Id = expression.Id,
        Level = expression.Level,
        Type = expression.ExpressionType,
        PartOfSpeech = expression.PartOfSpeech,
        Cefr = expression.Cefr,
        Labels = expression.Labels.ToList(),
        ResourceId = expression.ResourceId,
        Synonyms = expression.Synonyms.ToList()
    };
}