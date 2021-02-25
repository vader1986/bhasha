using System.Collections.Generic;
using System.Threading.Tasks;
using Bhasha.Common.MongoDB.Collections;
using Bhasha.Common.Queries;

namespace Bhasha.Common.MongoDB
{
    using Langs = Collections.LanguageCollection;

    public class MongoDbLayer : IDatabase
    {
        private readonly ProcedureIdCollection _procedureIds;
        private readonly ProcedureCollection _procedures;
        private readonly TranslationCollection _translations;
        private readonly UserProgressCollection _users;
        private readonly Langs _languages;

        public MongoDbLayer(string connectionString)
        {
            var db = MongoDb
                .Create(connectionString)
                .GetAwaiter()
                .GetResult();

            _procedureIds = new ProcedureIdCollection(db);
            _procedures = new ProcedureCollection(db);
            _translations = new TranslationCollection(db);
            _users = new UserProgressCollection(db);
            _languages = new Langs(db);
        }

        public ValueTask<IEnumerable<Translation>> Query(TranslationQuery query)
        {
            return _translations.Query(query);
        }

        public ValueTask<IEnumerable<Procedure>> Query(ProcedureQuery query)
        {
            return _procedures.Query(query);
        }

        public ValueTask<IEnumerable<UserProgress>> Query(UserProgressQuery query)
        {
            return _users.Query(query);
        }

        ValueTask<IEnumerable<ProcedureId>> IListable<ProcedureId>.List()
        {
            return _procedureIds.List();
        }

        ValueTask<IEnumerable<Language>> IListable<Language>.List()
        {
            return _languages.List();
        }
    }
}
