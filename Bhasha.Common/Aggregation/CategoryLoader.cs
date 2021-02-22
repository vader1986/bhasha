using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.Extensions;
using Bhasha.Common.Queries;

namespace Bhasha.Common.Aggregation
{
    public interface ILoadCategory
    {
        ValueTask<Category?> NextCategory(UserProgress progress);
    }

    public class CategoryLoader : ILoadCategory
    {
        private readonly IListable<Category> _categories;

        public CategoryLoader(IListable<Category> categories)
        {
            _categories = categories;
        }

        public async ValueTask<Category?> NextCategory(UserProgress progress)
        {
            var categories = await _categories.List();
            var unfinishedCategories = categories.Where(c => !progress.Finished.Contains(c));

            return unfinishedCategories.RandomOrDefault();
        }
    }
}
