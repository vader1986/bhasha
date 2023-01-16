using Bhasha.Web.Domain;
using Bhasha.Web.Domain.Interfaces;

namespace Bhasha.Web.Services;

public class ExpressionFactory : IFactory<Expression>
{
    public Expression Create()
    {
        return new Expression(
            Id: default,
            ExpressionType: default,
            PartOfSpeech: default,
            Cefr: default,
            ResourceId: default,
            Labels: Array.Empty<string>(),
            Synonyms: Array.Empty<string>(),
            Level: default);
    }
}

