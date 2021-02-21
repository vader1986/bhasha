using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Queries
{
    public class Storage
    {
        private readonly IListable<Category> _categories;
        private readonly IListable<ProcedureId> _procedures;
        private readonly IQueryable<Procedure, ProcedureQuery> _procedure;
        private readonly IQueryable<Translation, TranslationsQuery> _translations;

        public Storage(
            IListable<Category> categories,
            IListable<ProcedureId> procedures,
            IQueryable<Procedure, ProcedureQuery> procedure,
            IQueryable<Translation, TranslationsQuery> translations)
        {
            _categories = categories;
            _procedures = procedures;
            _procedure = procedure;
            _translations = translations;
        }

        private async Task<Category?> ChooseCategory(IEnumerable<Category> finished)
        {
            var categories = await _categories.List();
            var availableCategories = categories.Where(x => !finished.Contains(x));

            return categories.Where(x => !finished.Contains(x)).RandomOrDefault();
        }

        private async Task<Procedure?> ChooseProcedure()
        {
            var procedures = (await _procedures.List()).ToList();

            while (procedures.Any())
            {
                var procedureId = procedures.Random();
                var procedure = (await _procedure.Query(new ProcedureIdQuery(procedureId))).Single();

                if (!procedure.Support.Any())
                {
                    return procedure;
                }

                procedures.Remove(procedureId);
            }

            return default;
        }

        public async Task<LearningProcedure?> FetchFor(LanguageLevel level, IEnumerable<Category> finished)
        {
            
            var category = ChooseCategory(finished);
            var procedure = ChooseProcedure();

            await Task.WhenAll(category, procedure);

            var chosenCategory = await category;
            var chosenProcedure = await procedure;

            if (chosenCategory == default || chosenProcedure == default)
            {
                return default;
            }

            var query = new TranslationsCategoryQuery(level, chosenCategory);
            var translations = await _translations.Query(query);

            return new LearningProcedure(translations, chosenProcedure);
        }
    }
}
