using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;
using Bhasha.Web.Services;
using Bhasha.Web.Tests.Support;
using NSubstitute;
using Xunit;

namespace Bhasha.Web.Tests.Services;

public class ValidatorTests
{
	private readonly Validator _validator;
	private readonly IRepository<Translation> _translations;

	public ValidatorTests()
	{
		_translations = Substitute.For<IRepository<Translation>>();
		_validator = new Validator(_translations);
	}

	[Theory, AutoData]
	public async Task GivenCorrectInput_WhenValidated_ThenReturnCorrectResult(ValidationInput input)
	{
		// setup
		var consolidatedInputs = input with { Languages = SupportedLanguageKey.Create() with { Target = input.Translation.Language } };
		_translations.Get(input.Translation.Id).Returns(input.Translation);

		// act
		var result = await _validator.Validate(consolidatedInputs);

		// verify
		Assert.Equal(ValidationResultType.Correct, result.Result);
	}

	[Theory, AutoData]
	public async Task GivenWrongInput_WhenValidated_ThenReturnWrongResult(ValidationInput input, Translation solution)
	{
		// setup
		var consolidatedInputs = input with { Languages = SupportedLanguageKey.Create() with { Target = input.Translation.Language } };
		_translations.Get(input.Translation.Id).Returns(solution);

		// act
		var result = await _validator.Validate(consolidatedInputs);

		// verify
		Assert.Equal(ValidationResultType.Wrong, result.Result);
		Assert.Null(result.Reason);
	}

	[Theory, AutoData]
	public async Task GivenWrongLanguage_WhenValidated_ThenReturnWrongResult(ValidationInput input, Translation solution)
	{
		// setup
		var wrongInputs = input with
		{
			Languages = SupportedLanguageKey.Create() with
			{
				Target = Language.Bengali
			},
			Translation = input.Translation with
			{
				Language = Language.English
			}
		};
		_translations.Get(input.Translation.Id).Returns(solution);

		// act
		var result = await _validator.Validate(wrongInputs);

		// verify
		Assert.Equal(ValidationResultType.Wrong, result.Result);
		Assert.NotNull(result.Reason);
	}
}