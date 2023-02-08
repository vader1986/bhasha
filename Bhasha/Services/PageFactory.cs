using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Bhasha.Domain.Pages;

namespace Bhasha.Services;

public class PageFactory : IPageFactory
{
    private readonly IMultipleChoicePageFactory _multipleChoiceFactory;

    public PageFactory(IMultipleChoicePageFactory multipleChoiceFactory)
    {
        _multipleChoiceFactory = multipleChoiceFactory;
    }

    public async Task<DisplayedPage> CreateAsync(Page page, LangKey languages)
    {
        if (page.PageType == PageType.MultipleChoice)
        {
            return await _multipleChoiceFactory.CreateAsync(page, languages);
        }

        throw new ArgumentException($"{page.PageType} is not supported", nameof(page));
    }
}

