#nullable enable
namespace Bhasha.Common.Storage
{
    public interface IStorage
    {
        LearningProcedure Query(QueryParams query);
    }
}
