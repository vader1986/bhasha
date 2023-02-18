using Bhasha.Domain.Pages;
using Bhasha.Services.Pages;

namespace Bhasha.Domain.Interfaces;

public interface IPageFactory : IAsyncFactory<Page, LangKey, DisplayedPage>{}

public interface IMultipleChoicePageFactory : IAsyncFactory<Page, LangKey, DisplayedPage<MultipleChoice>> { }