using Bhasha.Common.Queries;

namespace Bhasha.Common
{
    public interface IDatabase :
        IListable<Category>,
        IListable<ProcedureId>,
        IListable<Language>,
        IQueryable<Translation, TranslationsQuery>,
        IQueryable<Procedure, ProcedureQuery>
    {
    }
}
