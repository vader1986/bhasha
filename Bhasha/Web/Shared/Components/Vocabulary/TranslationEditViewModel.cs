using Bhasha.Domain;

namespace Bhasha.Web.Shared.Components.Vocabulary;

public class TranslationEditViewModel
{
    private TranslationEditViewModel()
    {
        
    }
    
    public Translation? Origin { get; init; }
    
    public string Text { get; set; } = string.Empty;
    public string? Spoken { get; set; } = string.Empty;
    public required string Language { get; set; }

    public TranslationViewModelStatus Status { get; set; } = TranslationViewModelStatus.Initial;
    
    public static TranslationEditViewModel Create(Translation translation)
    {
        return new TranslationEditViewModel
        {
            Text = translation.Text,
            Spoken = translation.Spoken,
            Language = translation.Language,
            Origin = translation,
            Status = TranslationViewModelStatus.Initial
        };
    }
    
    public static TranslationEditViewModel Create(Language language)
    {
        return new TranslationEditViewModel
        {
            Language = language,
            Status = TranslationViewModelStatus.Created
        };
    }

    public Translation ToTranslation(Expression expression) => Origin switch
    {
        null => Translation.Create(expression, Language, Text) with
        {
            Spoken = Spoken
        },
        _ => Origin with
        {
            Text = Text,
            Spoken = Spoken
        }
    };
}

public enum TranslationViewModelStatus
{
    Initial,
    Changed,
    Deleted, 
    Created
}