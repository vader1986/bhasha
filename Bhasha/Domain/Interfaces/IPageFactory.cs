using Bhasha.Shared.Domain;
using Bhasha.Shared.Domain.Pages;

namespace Bhasha.Domain.Interfaces;

public interface IPageFactory : IAsyncFactory<Expression, ProfileKey, DisplayedPage>;

public interface IMultipleChoicePageFactory : IAsyncFactory<Expression, ProfileKey, DisplayedPage<MultipleChoice>>;