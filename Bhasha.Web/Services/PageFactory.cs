using Bhasha.Web.Domain;
using Bhasha.Web.Domain.Pages;
using Bhasha.Web.Interfaces;

namespace Bhasha.Web.Services
{
    public class PageFactory : IFactory<(Translation Word, PageType Page), DisplayedPage>
    {
        private readonly IRepository<Expression> _expressionRepository;

        public PageFactory(IRepository<Expression> expressionRepository)
        {
            _expressionRepository = expressionRepository;
        }

        public DisplayedPage Create((Translation Word, PageType Page) config)
        {
            if (config.Page == PageType.MultipleChoice)
            {
            }

            throw new NotImplementedException();
        }
    }
}

