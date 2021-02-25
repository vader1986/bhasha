using Bhasha.Common.Queries;

namespace Bhasha.Common
{
    public interface IDatabase :
        IListable<ProcedureId>,
        IListable<Language>,
        IQueryable<Translation, TranslationQuery>,
        IQueryable<Procedure, ProcedureQuery>,
        IQueryable<UserProgress, UserProgressQuery>
    {
    }
}
