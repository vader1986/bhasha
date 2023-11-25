using System;
using Bhasha.Shared.Domain;
using NSubstitute;

namespace Bhasha.Tests.Services.Scenarios;

public static class AuthoringServiceScenarioExtensions
{
    public static AuthoringServiceScenario WithTranslation(this AuthoringServiceScenario scenario, string text, Language? language = default)
    {
        scenario
            .TranslationRepository
            .Find(text, Language.Reference)
            .Returns(new Translation(
                Id: Random.Shared.Next(),
                Expression: scenario.Expression,
                Language: language ?? Language.Reference,
                Text: text,
                Spoken: default,
                AudioId: default));
        
        return scenario;
    }
}