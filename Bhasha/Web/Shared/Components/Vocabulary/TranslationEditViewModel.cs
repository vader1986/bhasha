using Bhasha.Domain;

namespace Bhasha.Web.Shared.Components.Vocabulary;

public class TranslationEditViewModel
{
    public required Translation Origin { get; init; }
    
    public string Text { get; set; } = string.Empty;

    public static TranslationEditViewModel Create(Translation translation)
    {
        return new TranslationEditViewModel
        {
            Text = translation.Text,
            Origin = translation
        };
    }
    
    public Translation ToTranslation()
    {
        return Origin with
        {
            Text = Text
        };
    }
}