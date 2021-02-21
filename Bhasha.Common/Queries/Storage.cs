using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.Exceptions;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Queries
{
    public interface IStorage
    {
        Task<LearningProcedure?> FetchFor(UserProgress progress);
    }

    public class Storage : IStorage
    {
        private readonly IListable<Category> _categories;
        private readonly IListable<ProcedureId> _procedureIds;
        private readonly IQueryable<Procedure, ProcedureQuery> _procedures;
        private readonly IQueryable<Translation, TranslationsQuery> _translations;

        public Storage(IListable<Category> categories, IListable<ProcedureId> procedureIds, IQueryable<Procedure, ProcedureQuery> procedures, IQueryable<Translation, TranslationsQuery> translations)
        {
            _categories = categories;
            _procedureIds = procedureIds;
            _procedures = procedures;
            _translations = translations;
        }

        private async Task<Category?> ChooseCategory(IEnumerable<Category> finished)
        {
            var categories = await _categories.List();
            var availableCategories = categories.Where(x => !finished.Contains(x));

            return categories.Where(x => !finished.Contains(x)).RandomOrDefault();
        }

        private async Task<Procedure> ChooseProcedure()
        {
            var procedureIds = (await _procedureIds.List()).ToList();

            while (procedureIds.Any())
            {
                var procedureId = procedureIds.Random();
                var procedure = (await _procedures.Query(new ProcedureIdQuery(procedureId))).FirstOrDefault();

                if (procedure != default &&
                    procedure.Support.Length == 0)
                {
                    return procedure;
                }

                procedureIds.Remove(procedureId);
            }

            throw new NoProcedureFoundException("could not find any suitable procedure");
        }

        public async Task<LearningProcedure?> FetchFor(UserProgress progress)
        {
            var category = ChooseCategory(progress.Finished);
            var procedure = ChooseProcedure();

            await Task.WhenAll(category, procedure);

            var chosenCategory = await category;
            var chosenProcedure = await procedure;

            if (chosenCategory == default)
            {
                return default;
            }

            var query = new TranslationsCategoryQuery(progress.From, progress.To, progress.Level, chosenCategory);
            var translations = await _translations.Query(query);
            var pool = translations.ToArray();

            if (pool.IsEmpty())
            {
                throw new NoTranslationFoundException($"could not find any translation for query {query.Stringify()}");
            }

            return new LearningProcedure(pool, chosenProcedure);
        }
    }
}
