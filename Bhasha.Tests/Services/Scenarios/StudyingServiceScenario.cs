using Bhasha.Domain.Interfaces;
using Bhasha.Services;
using NSubstitute;

namespace Bhasha.Tests.Services.Scenarios;

public class StudyingServiceScenario
{
    public IProfileRepository Repository { get; } = Substitute.For<IProfileRepository>();
    public IValidator Validator { get; } = Substitute.For<IValidator>();
    public IChapterSummariesProvider SummariesProvider { get; } = Substitute.For<IChapterSummariesProvider>();
    public IChapterProvider ChapterProvider { get; } = Substitute.For<IChapterProvider>();

    public StudyingService Sut { get; }

    public StudyingServiceScenario()
    {
        Sut = new StudyingService(Repository, Validator, SummariesProvider, ChapterProvider);
    }
}