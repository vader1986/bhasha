using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;

namespace Bhasha.Web.Services
{
    public class ExpressionFactory : IFactory<Expression>
    {
        public Expression Create()
        {
            return new Expression(
                Id: Guid.Empty,
                ExpressionType: default,
                PartOfSpeech: default,
                Cefr: default,
                ResourceId: default,
                Labels: Array.Empty<string>(),
                Level: default);
        }
    }
}

