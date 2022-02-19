using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;

namespace Bhasha.Web.Pages
{
	public class AddChapterState
	{
        public IRepository<Chapter> ChapterRepository { get; set; } = default!;
        public IExpressionManager ExpressionManager { get; set; } = default!;
        public IFactory<Expression> ExpressionFactory { get; set; } = default!;

        private readonly IDictionary<Guid, string> _expressionNames = new Dictionary<Guid, string>();

        public string? UserId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? NativeLanguage { get; set; }
        public string? TargetLanguage { get; set; }
        public int? RequiredLevel { get; set; }
        public List<Page> Pages { get; } = new List<Page>();
        public string? Error { get; private set; }
        public bool DisableCreatePage =>
            string.IsNullOrWhiteSpace(NativeLanguage) ||
            string.IsNullOrWhiteSpace(TargetLanguage) ||
            NativeLanguage == TargetLanguage;

        public string GetPageTitle(Page page)
        {
            return _expressionNames[page.ExpressionId];
        }

        private string? Validate()
        {
            if (string.IsNullOrWhiteSpace(UserId))
            {
                return "Unknown user! Make sure you're logged in!";
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                return "NAME must not be empty!";
            }

            if (string.IsNullOrWhiteSpace(Description))
            {
                return "DESCRIPTION must not be empty!";
            }

            if (RequiredLevel == null)
            {
                return "Please select a REQUIRED LEVEL!";
            }

            if (NativeLanguage == null)
            {
                return "Please select a NATIVE LANGUAGE!";
            }

            if (TargetLanguage == null)
            {
                return "Please select a TARGET LANGUAGE!";
            }

            if (TargetLanguage == NativeLanguage)
            {
                return "TARGET language must be different from NATIVE language!";
            }

            if (Pages.Count < 3)
            {
                return "A chapter requires at least 3 chapters!";
            }

            return null;
        }

        private void Clear()
        {
            Error = null;
            Name = null;
            Description = null;
            NativeLanguage = null;
            TargetLanguage = null;
            RequiredLevel = null;
            Pages.Clear();

            _expressionNames.Clear();
        }

        public async Task SubmitPageState(AddPageState pageState)
        {

            var expression =
                ExpressionFactory.Create() with
                {
                    ExpressionType = pageState.Expr,
                    Cefr = pageState.Cefr,
                    Translations = new[]
                    {
                        new Translation(NativeLanguage!, pageState.Native!, pageState.NativeSpoken, default),
                        new Translation(TargetLanguage!, pageState.Target!, pageState.TargetSpoken, default)
                    }
                };

            var expressionId = (await ExpressionManager.AddOrUpdate(expression)).Id;

            _expressionNames[expressionId] = pageState.Native!;

            Pages.Add(new Domain.Page(pageState.PageType, expressionId, pageState.Leads.ToArray()));
        }

        public async Task Submit()
        {
            Error = Validate();

            if (Error != null)
                return;

            var name = ExpressionFactory.Create() with { Translations = new[] { new Translation(Language.Reference, Name!, default, default) } };
            var nameId = (await ExpressionManager.AddOrUpdate(name)).Id;

            var description = ExpressionFactory.Create() with { Translations = new[] { new Translation(Language.Reference, Description!, default, default) } };
            var descriptionId = (await ExpressionManager.AddOrUpdate(description)).Id;

            var chapter = new Chapter(Guid.Empty, RequiredLevel!.Value, nameId, descriptionId, Pages.ToArray(), null, UserId!);
            await ChapterRepository.Add(chapter);

            Clear();
        }
    }
}

