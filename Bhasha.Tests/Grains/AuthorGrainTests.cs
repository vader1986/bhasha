using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Bhasha.Grains;
using FluentAssertions;
using NSubstitute;
using Orleans.TestKit;
using Xunit;

namespace Bhasha.Tests.Grains;

public class AuthorGrainTests : TestKitBase
{
	private readonly IChapterRepository _chapterRepository = Substitute.For<IChapterRepository>();
    private readonly IExpressionRepository _expressionRepository = Substitute.For<IExpressionRepository>();
    private readonly ITranslationRepository _translationRepository = Substitute.For<ITranslationRepository>();

    public AuthorGrainTests()
	{
        Silo.AddService(_chapterRepository);
        Silo.AddService(_expressionRepository);
        Silo.AddService(_translationRepository);
    }

    #region AddOrUpdateChapter

    [Theory(Skip = "OrleansTestKit is broken"), AutoData]
    public async Task GivenChapter_WhenAddOrReplaceChapter_ThenUpdateRepository(Chapter chapter)
    {
        // setup
        var grain = await Silo.CreateGrainAsync<AuthorGrain>("test");

        // act
        await grain.AddOrUpdateChapter(chapter);

        // verify
        await _chapterRepository
            .Received(1)
            .AddOrReplace(chapter);
    }

    #endregion

    #region AddOrUpdateTranslation

    [Theory(Skip = "OrleansTestKit is broken"), AutoData]
    public async Task GivenTranslation_WhenAddOrReplaceTranslation_ThenUpdateRepository(Translation translation)
    {
        // setup
        var grain = await Silo.CreateGrainAsync<AuthorGrain>("test");

        // act
        await grain.AddOrUpdateTranslation(translation);

        // verify
        await _translationRepository
            .Received(1)
            .AddOrReplace(translation);
    }

    #endregion

    #region GetOrAddExpressionId

    [Theory(Skip = "OrleansTestKit is broken"), AutoData]
    public async Task GivenTranslationInRepository_WhenGetOrAddExpressionId_ThenReturnExpressionIdOfTranslation(Translation translation)
    {
        // setup
        _translationRepository
            .Find("test", Language.Reference)
            .Returns(translation);

        var grain = await Silo.CreateGrainAsync<AuthorGrain>("test");

        // act
        var result = await grain.GetOrAddExpressionId("test");

        // verify
        result.Should().Be(translation.ExpressionId);
    }

    [Theory(Skip = "OrleansTestKit is broken"), AutoData]
    public async Task GivenNoTranslation_WhenGetOrAddExpressionId_ThenAddNewExpressionToRepository(Expression expression)
    {
        // setup
        _translationRepository
            .Find("test", Language.Reference)
            .Returns(default(Translation?));

        _expressionRepository
            .Add(Arg.Any<Expression>())
            .Returns(expression);

        var grain = await Silo.CreateGrainAsync<AuthorGrain>("test");

        // act
        var result = await grain.GetOrAddExpressionId("test");

        // verify
        result.Should().Be(expression.Id);

        await _expressionRepository
            .Received(1)
            .Add(Arg.Is<Expression>(x => x.Level == 1));
    }

    [Theory(Skip = "OrleansTestKit is broken"), AutoData]
    public async Task GivenNoTranslation_WhenGetOrAddExpressionId_ThenAddNewTranslationToRepository(Expression expression)
    {
        // setup
        _translationRepository
            .Find("test", Language.Reference)
            .Returns(default(Translation?));

        _expressionRepository
            .Add(Arg.Any<Expression>())
            .Returns(expression);

        var grain = await Silo.CreateGrainAsync<AuthorGrain>("test");

        // act
        var result = await grain.GetOrAddExpressionId("test");

        // verify
        await _translationRepository
            .Received(1)
            .AddOrReplace(Arg.Is<Translation>(
                x => x.Language == Language.Reference &&
                     x.ExpressionId == expression.Id &&
                     x.Text == "test"));
    }

    #endregion
}

