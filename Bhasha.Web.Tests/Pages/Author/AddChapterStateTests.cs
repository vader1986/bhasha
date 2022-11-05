using System;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;
using Bhasha.Web.Pages.Author;
using NSubstitute;
using Xunit;

namespace Bhasha.Web.Tests.Pages.Author;

public class AddChapterStateTests
{
	private readonly AddChapterState _state;

	/*
		* ToDo:
		* - throw exception leads to error text
		* - name & description added to translations
		* - handling of references
		*/

	public AddChapterStateTests()
	{
        _state = new AddChapterState
        {
            ChapterRepository = Substitute.For<IRepository<Chapter>>(),
            TranslationManager = Substitute.For<ITranslationManager>(),
            UserId = "Test User 123",
            Name = "Introduction",
            Description = "First steps ...",
            NativeLanguage = Language.English,
            TargetLanguage = Language.Bengali,
            RequiredLevel = 1
        };
	    _state.Pages.Add(new Page(PageType.ClozeChoice, Guid.NewGuid(), Array.Empty<string>()));
		_state.Pages.Add(new Page(PageType.ClozeChoice, Guid.NewGuid(), Array.Empty<string>()));
		_state.Pages.Add(new Page(PageType.ClozeChoice, Guid.NewGuid(), Array.Empty<string>()));
	}

	[Theory, AutoData]
	public async Task GivenAddPageState_WhenSubmitPageState_ThenAddPage(AddPageState pageState, Translation translation)
	{
		// setup
		_state.TranslationManager.AddOrUpdate(default!, default).ReturnsForAnyArgs(translation);
		_state.Pages.Clear();

		// act
		await _state.SubmitPageState(pageState);

		// verify
		Assert.Single(_state.Pages);
	}

	[Theory, AutoData]
	public async Task GivenAllData_WhenSubmit_ThenNoErrorAndAddChapter(Translation translation)
	{
		// setup
		_state.TranslationManager.AddOrUpdate(default!, default).ReturnsForAnyArgs(translation);

		// act
		await _state.Submit();

		// verify
		Assert.Null(_state.Error);

		await _state.ChapterRepository
			.Received(1)
			.Add(Arg.Any<Chapter>());
	}

	[Fact]
	public async Task GivenMissingUserId_WhenSubmit_ThenSetError()
    {
		// setup
		_state.UserId = null;

		// act
		await _state.Submit();

		// verify
		Assert.Equal("Unknown user! Make sure you're logged in!", _state.Error);
    }

	[Fact]
	public async Task GivenMissingName_WhenSubmit_ThenSetError()
	{
		// setup
		_state.Name = null;

		// act
		await _state.Submit();

		// verify
		Assert.Equal("NAME must not be empty!", _state.Error);
	}

	[Fact]
	public async Task GivenMissingDescription_WhenSubmit_ThenSetError()
	{
		// setup
		_state.Description = null;

		// act
		await _state.Submit();

		// verify
		Assert.Equal("DESCRIPTION must not be empty!", _state.Error);
	}

	[Fact]
	public async Task GivenMissingRequiredLevel_WhenSubmit_ThenSetError()
	{
		// setup
		_state.RequiredLevel = null;

		// act
		await _state.Submit();

		// verify
		Assert.Equal("Please select a REQUIRED LEVEL!", _state.Error);
	}

	[Fact]
	public async Task GivenMissingNativeLanguage_WhenSubmit_ThenSetError()
	{
		// setup
		_state.NativeLanguage = null;

		// act
		await _state.Submit();

		// verify
		Assert.Equal("Please select a NATIVE LANGUAGE!", _state.Error);
	}

	[Fact]
	public async Task GivenMissingTargetLanguage_WhenSubmit_ThenSetError()
	{
		// setup
		_state.TargetLanguage = null;

		// act
		await _state.Submit();

		// verify
		Assert.Equal("Please select a TARGET LANGUAGE!", _state.Error);
	}

	[Fact]
	public async Task GivenTargetLanguageEqualToNativeLanguage_WhenSubmit_ThenSetError()
	{
		// setup
		_state.TargetLanguage = _state.NativeLanguage;

		// act
		await _state.Submit();

		// verify
		Assert.Equal("TARGET language must be different from NATIVE language!", _state.Error);
	}

	[Fact]
	public async Task GivenMissingPages_WhenSubmit_ThenSetError()
	{
		// setup
		_state.Pages.Clear();

		// act
		await _state.Submit();

		// verify
		Assert.Equal("A chapter requires at least 3 chapters!", _state.Error);
	}
}

