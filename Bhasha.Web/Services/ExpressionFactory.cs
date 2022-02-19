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
                ExpressionType: ExpressionType.Word,
                Cefr: CEFR.Unknown,
                ResourceId: default,
                Labels: new string[0],
                Level: default,
                Translations: new Translation[0]);
        }
    }
}

