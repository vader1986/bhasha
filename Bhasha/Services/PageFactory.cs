using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Chapter = Bhasha.Domain.Chapter;
using Expression = Bhasha.Domain.Expression;

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

