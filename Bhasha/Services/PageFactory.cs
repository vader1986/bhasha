using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Chapter = Bhasha.Domain.Chapter;
using Expression = Bhasha.Domain.Expression;

namespace Bhasha.Services;

public class PageFactory(IMultipleChoicePageFactory multipleChoiceFactory) : IPageFactory
{
    public async Task<DisplayedPage> Create(Chapter chapter, Expression page, ProfileKey languages)
    {
        return await multipleChoiceFactory.Create(chapter, page, languages);
    }
}

