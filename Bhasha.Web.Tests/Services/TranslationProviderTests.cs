using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;
using Bhasha.Web.Services;
using NSubstitute;
using Xunit;

namespace Bhasha.Web.Tests.Services;

public class TranslationProviderTests
{
	private readonly IRepository<Translation> _repository;
	private readonly TranslationProvider _translationProvider;

	public TranslationProviderTests()
	{
		_repository = Substitute.For<IRepository<Translation>>();
		_translationProvider = new TranslationProvider(_repository);
	}

	[Fact]
	public async Task GivenNoMatchingTranslation_WhenFind_ThenReturnNull()
	{
		// setup
		_repository.Find(default!).ReturnsForAnyArgs(Array.Empty<Translation>());

		// act
		var result = await _translationProvider.Find(Guid.NewGuid(), Language.Bengali);

		// verify
		Assert.Null(result);
	}

	[Theory, AutoData]
	public async Task GivenTranslation_WhenFind_ThenReturnTranslation(Translation translation)
	{
		// setup
		_repository.Find(default!).ReturnsForAnyArgs(new[] { translation });

		// act
		var result = await _translationProvider.Find(Guid.NewGuid(), Language.Bengali);

		// verify
		Assert.Equal(translation, result);
	}

	[Theory, AutoData]
	public void GivenSomeExpressions_WhenFindAll_ThenThrowException(Translation[] translations)
	{
		// setup
		_repository.Find(default!).ReturnsForAnyArgs(translations);

		var guids = Enumerable.Range(0, translations.Length - 1).Select(_ => Guid.NewGuid()).ToArray();

		// act & verify
		Assert.ThrowsAsync<InvalidOperationException>(async () =>
			await _translationProvider.FindAll(Language.Bengali, guids));
	}

	[Theory, AutoData]
	public async Task GivenAllExpressionsWithTranslations_WhenFindAll_ThenReturnTranslations(Translation[] translations)
	{
		// setup
		_repository.Find(default!).ReturnsForAnyArgs(translations);
		var guids = Enumerable.Range(0, translations.Length).Select(_ => Guid.NewGuid()).ToArray();

		// act
		var result = await _translationProvider.FindAll(Language.Bengali, guids);

		// verify
		Assert.Equal(result.Count, guids.Length);
	}
}

