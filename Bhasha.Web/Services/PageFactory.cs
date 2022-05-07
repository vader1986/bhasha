using Bhasha.Web.Domain;
using Bhasha.Web.Domain.Pages;
using Bhasha.Web.Interfaces;

namespace Bhasha.Web.Services
{
    public class PageFactory : IAsyncFactory<Page, Profile, DisplayedPage>
    {
        private readonly IAsyncFactory<Page, Profile, DisplayedPage<MultipleChoice>> _multipleChoiceFactory;

        public PageFactory(
            IAsyncFactory<Page, Profile, DisplayedPage<MultipleChoice>> multipleChoiceFactory)
        {
            _multipleChoiceFactory = multipleChoiceFactory;
        }

        public async Task<DisplayedPage> CreateAsync(Page page, Profile profile)
        {
            if (page.PageType == PageType.MultipleChoice)
            {
                return await _multipleChoiceFactory.CreateAsync(page, profile);
            }

            throw new ArgumentException($"{page.PageType} is not supported", nameof(page));
        }
    }
}

