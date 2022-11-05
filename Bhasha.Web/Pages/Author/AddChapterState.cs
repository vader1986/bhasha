using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;

namespace Bhasha.Web.Pages.Author;

public class AddChapterState
{
    public IRepository<Chapter> ChapterRepository { get; set; } = default!;
    public ITranslationManager TranslationManager { get; set; } = default!;

    private readonly IDictionary<Guid, string> _expressionNames = new Dictionary<Guid, string>();

    public string? UserId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? NativeLanguage { get; set; }
    public string? TargetLanguage { get; set; }
    public string? ReferenceName { get; set; }
    public string? ReferenceDescription { get; set; }
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

        if (string.IsNullOrWhiteSpace(ReferenceName) && NativeLanguage != Language.Reference)
        {
            return $"REFERENCE NAME must be set to the {Language.Reference} translation of name!";
        }

        if (string.IsNullOrWhiteSpace(ReferenceDescription) && NativeLanguage != Language.Reference)
        {
            return $"REFERENCE DESCRIPTION must be set to the {Language.Reference} translation of description!";
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
        ReferenceName = null;
        Description = null;
        ReferenceDescription = null;
        NativeLanguage = null;
        TargetLanguage = null;
        RequiredLevel = null;
        Pages.Clear();

        _expressionNames.Clear();
    }

    private Translation? GetReference(AddPageState pageState, Translation native, Translation target)
    {
        if (NativeLanguage != null && NativeLanguage == Language.Reference)
            return native;

        if (TargetLanguage != null && TargetLanguage == Language.Reference)
            return target;

        if (string.IsNullOrEmpty(pageState.NativeReference))
            return null;

        return new Translation(Guid.Empty, Guid.Empty, Language.Reference, pageState.NativeReference, default, default);
    }

    public async Task SubmitPageState(AddPageState pageState)
    {
        var native = new Translation(
            Guid.Empty, Guid.Empty, NativeLanguage!, pageState.Native!, pageState.NativeSpoken, default);

        var target = new Translation(
            Guid.Empty, Guid.Empty, TargetLanguage!, pageState.Target!, pageState.TargetSpoken, default);

        var reference = GetReference(pageState, native, target);

        if (reference == null)
        {
            Error = $"Page translation for reference language {Language.Reference} not set";
            return;
        }

        native = await TranslationManager.AddOrUpdate(native, reference);
        await TranslationManager.AddOrUpdate(target, reference);

        _expressionNames[native.ExpressionId] = native.Native;

        Pages.Add(new Domain.Page(pageState.PageType, native.ExpressionId, pageState.Leads.ToArray()));
    }

    public async Task Submit()
    {
        Error = Validate();

        if (Error != null)
            return;

        try
        {
            var refName = ReferenceName ?? Name;
            var refNameTranslation = new Translation(Guid.Empty, Guid.Empty, Language.Reference, refName!, default, default);
            var nameTranslation = new Translation(Guid.Empty, Guid.Empty, NativeLanguage!, Name!, default, default);
            var name = await TranslationManager.AddOrUpdate(nameTranslation, refNameTranslation);

            var refDescription = ReferenceDescription ?? Description;
            var refDescriptionTranslation = new Translation(Guid.Empty, Guid.Empty, Language.Reference, refDescription!, default, default);
            var descriptionTranslation = new Translation(Guid.Empty, Guid.Empty, NativeLanguage!, Description!, default, default);
            var description = await TranslationManager.AddOrUpdate(descriptionTranslation, refDescriptionTranslation);

            var chapter = new Chapter(Guid.Empty, RequiredLevel!.Value, name.ExpressionId, description.ExpressionId, Pages.ToArray(), null, UserId!);
            await ChapterRepository.Add(chapter);

            Clear();
        }
        catch (Exception e)
        {
            Error = e.Message;
        }
    }
}

