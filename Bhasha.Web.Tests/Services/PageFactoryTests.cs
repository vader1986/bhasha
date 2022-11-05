using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bhasha.Web.Domain;
using Bhasha.Web.Domain.Pages;
using Bhasha.Web.Interfaces;
using Bhasha.Web.Services;
using NSubstitute;
using Xunit;

namespace Bhasha.Web.Tests.Services;

public class PageFactoryTests
{
	private readonly PageFactory _pageFactory;
	private readonly IAsyncFactory<Page, LangKey, DisplayedPage<MultipleChoice>> _multipleChoicePageFactory;

	public PageFactoryTests()
	{
		_multipleChoicePageFactory = Substitute.For<IAsyncFactory<Page, LangKey, DisplayedPage<MultipleChoice>>>();
		_pageFactory = new PageFactory(_multipleChoicePageFactory);
	}

	[Theory, AutoData]
	public async Task GivenPageAndProfile_WhenCreateMultipleChoicePage_ThenReturnFactoryProduct(Page page, LangKey languages, DisplayedPage<MultipleChoice> displayedPage)
	{
		// prepare
		page = page with { PageType = PageType.MultipleChoice };
		_multipleChoicePageFactory.CreateAsync(page, languages).Returns(displayedPage);

		// act
		var result = await _pageFactory.CreateAsync(page, languages);

		// verify
		Assert.Equal(displayedPage, result);
	}
}

