using Bhasha.Services.Pages;
using Bhasha.Shared.Domain;
using Bhasha.Shared.Domain.Pages;

namespace Bhasha.Domain.Interfaces;

public interface IPageFactory : IAsyncFactory<Page, ProfileKey, DisplayedPage>{}

public interface IMultipleChoicePageFactory : IAsyncFactory<Page, ProfileKey, DisplayedPage<MultipleChoice>> { }