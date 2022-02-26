using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Bhasha.Web.Domain;
using Bhasha.Web.Domain.Pages;
using Bhasha.Web.Interfaces;
using Bhasha.Web.Services;
using FakeItEasy;
using NUnit.Framework;

namespace Bhasha.Web.Tests.Services
{
	public class PageFactoryTests
	{
		private PageFactory _pageFactory = default!;
		private IAsyncFactory<Page, Profile, DisplayedPage<MultipleChoice>> _multipleChoicePageFactory = default!;

		[SetUp]
		public void Before()
		{
			_multipleChoicePageFactory = A.Fake<IAsyncFactory<Page, Profile, DisplayedPage<MultipleChoice>>>();
			_pageFactory = new PageFactory(_multipleChoicePageFactory);
		}

		[Test, AutoData]
		public async Task GivenPageAndProfile_WhenCreateMultipleChoicePage_ThenReturnFactoryProduct(Page page, Profile profile, DisplayedPage<MultipleChoice> displayedPage)
		{
			// prepare
			page = page with { PageType = PageType.MultipleChoice };
			A.CallTo(() => _multipleChoicePageFactory.CreateAsync(page, profile)).Returns(displayedPage);

			// act
			var result = await _pageFactory.CreateAsync(page, profile);

			// verify
			Assert.AreEqual(displayedPage, result);
		}
	}
}

