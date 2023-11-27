using Bhasha.Shared.Domain;
using Bhasha.Shared.Domain.Pages;

namespace Bhasha.Domain.Interfaces;

public interface IPageFactory
{
    Task<DisplayedPage> Create(Chapter chapter, Expression expression, ProfileKey key);
}

public interface IMultipleChoicePageFactory
{
    Task<DisplayedPage<MultipleChoice>> Create(Chapter chapter, Expression expression, ProfileKey key);
}