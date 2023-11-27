using Bhasha.Services;
using Bhasha.Shared.Domain;
using Bhasha.Shared.Domain.Interfaces;
using NSubstitute;

namespace Bhasha.Tests.Services.Scenarios;

public class MultipleChoicePageFactoryScenario
{
    public Chapter Chapter { get; set; }
    public Expression CorrectExpression { get; set; }
    
    public Translation CorrectTranslation { get; set; }
    
    public ITranslationRepository Repository { get; } = Substitute.For<ITranslationRepository>();
    
    public MultipleChoicePageFactory Sut { get; }

    public MultipleChoicePageFactoryScenario()
    {
        Sut = new MultipleChoicePageFactory(Repository);

        CorrectExpression = Expression
            .Create() with { Id = 7 };
        
        CorrectTranslation = new Translation(
            Id: default, 
            Language:Language.Bengali,
            Text: "text",
            Spoken: default, 
            AudioId: default,
            Expression: CorrectExpression);
        
        Chapter = new Chapter(
            Id: 1,
            RequiredLevel: 1,
            Name: Expression.Create(),
            Description: Expression.Create(),
            new []
            {
                CorrectExpression
            },
            ResourceId: default,
            AuthorId: "author");
    }
}