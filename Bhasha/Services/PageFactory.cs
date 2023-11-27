using Bhasha.Domain.Interfaces;
using Bhasha.Shared.Domain;

namespace Bhasha.Services;

public class PageFactory : IPageFactory
{
    private readonly IMultipleChoicePageFactory _multipleChoiceFactory;

    public PageFactory(IMultipleChoicePageFactory multipleChoiceFactory)
    {
        _multipleChoiceFactory = multipleChoiceFactory;
    }

    public async Task<DisplayedPage> Create(Chapter chapter, Expression page, ProfileKey languages)
    {
        return await _multipleChoiceFactory.Create(chapter, page, languages);
    }
}

