using Bhasha.Domain;
using Bhasha.Infrastructure.EntityFramework.Dtos;
using Bhasha.Infrastructure.EntityFramework.Extensions;
using Chapter = Bhasha.Domain.Chapter;
using ChapterSelection = Bhasha.Domain.ChapterSelection;
using Expression = Bhasha.Domain.Expression;
using ExpressionType = Bhasha.Infrastructure.EntityFramework.Dtos.ExpressionType;
using PartOfSpeech = Bhasha.Infrastructure.EntityFramework.Dtos.PartOfSpeech;
using Profile = Bhasha.Domain.Profile;

namespace Bhasha.Infrastructure.EntityFramework;

public static class Converter
{
    public static ChapterDto Convert(Chapter chapter)
    {
        return new ChapterDto
        {
            Id = chapter.Id,
            RequiredLevel = chapter.RequiredLevel,
            ResourceId = chapter.ResourceId,
            AuthorId = chapter.AuthorId,
            Name = Convert(chapter.Name),
            Description = Convert(chapter.Description),
            Expressions = chapter.Pages.Select(Convert).ToList()
        };
    }

    public static Chapter Convert(ChapterDto dto)
    {
        return new Chapter(
            dto.Id,
            dto.RequiredLevel,
            Convert(dto.Name),
            Convert(dto.Description),
            dto.Expressions
                .Select(Convert)
                .OrderBy(x => x.Id)
                .ToArray(),
            dto.ResourceId,
            dto.AuthorId);
    }
    
    public static ExpressionDto Convert(Expression expression)
    {
        return new ExpressionDto
        {
            Id = expression.Id,
            ExpressionType = expression.ExpressionType switch
            {
                Domain.ExpressionType.Word => ExpressionType.Word,
                Domain.ExpressionType.Expression => ExpressionType.Expression,
                Domain.ExpressionType.Phrase => ExpressionType.Phrase,
                Domain.ExpressionType.Text => ExpressionType.Text,
                Domain.ExpressionType.Punctuation => ExpressionType.Punctuation,
                _ => null
            },
            PartOfSpeech = expression.PartOfSpeech switch
            {
                Domain.PartOfSpeech.Adjective => PartOfSpeech.Adjective,
                Domain.PartOfSpeech.Adverb => PartOfSpeech.Adverb,
                Domain.PartOfSpeech.AuxiliaryVerb => PartOfSpeech.AuxiliaryVerb,
                Domain.PartOfSpeech.Determiner => PartOfSpeech.Determiner,
                Domain.PartOfSpeech.Exclamation => PartOfSpeech.Exclamation,
                Domain.PartOfSpeech.ModalVerb => PartOfSpeech.ModalVerb,
                Domain.PartOfSpeech.Noun => PartOfSpeech.Noun,
                Domain.PartOfSpeech.PhrasalVerb => PartOfSpeech.PhrasalVerb,
                Domain.PartOfSpeech.Phrase => PartOfSpeech.Phrase,
                Domain.PartOfSpeech.Preposition => PartOfSpeech.Preposition,
                Domain.PartOfSpeech.Pronoun => PartOfSpeech.Pronoun,
                Domain.PartOfSpeech.Verb => PartOfSpeech.Verb,
                _ => null
            },
            Cefr = expression.Cefr switch
            {
                CEFR.A1 => Cefr.A1,
                CEFR.A2 => Cefr.A2,
                CEFR.B1 => Cefr.B1,
                CEFR.B2 => Cefr.B2,
                CEFR.C1 => Cefr.C1,
                CEFR.C2 => Cefr.C2,
                _ => null
            },
            ResourceId = expression.ResourceId,
            Labels = expression.Labels,
            Synonyms = expression.Synonyms,
            Level = expression.Level
        };
    }

    public static Expression Convert(ExpressionDto dto)
    {
        return new Expression(
            dto.Id,
            dto.ExpressionType switch
            {
                ExpressionType.Word => Domain.ExpressionType.Word,
                ExpressionType.Expression => Domain.ExpressionType.Expression,
                ExpressionType.Phrase => Domain.ExpressionType.Phrase,
                ExpressionType.Text => Domain.ExpressionType.Text,
                ExpressionType.Punctuation => Domain.ExpressionType.Punctuation,
                _ => null
            },
            dto.PartOfSpeech switch
            {
                PartOfSpeech.Adjective => Domain.PartOfSpeech.Adjective,
                PartOfSpeech.Adverb => Domain.PartOfSpeech.Adverb,
                PartOfSpeech.AuxiliaryVerb => Domain.PartOfSpeech.AuxiliaryVerb,
                PartOfSpeech.Determiner => Domain.PartOfSpeech.Determiner,
                PartOfSpeech.Exclamation => Domain.PartOfSpeech.Exclamation,
                PartOfSpeech.ModalVerb => Domain.PartOfSpeech.ModalVerb,
                PartOfSpeech.Noun => Domain.PartOfSpeech.Noun,
                PartOfSpeech.PhrasalVerb => Domain.PartOfSpeech.PhrasalVerb,
                PartOfSpeech.Phrase => Domain.PartOfSpeech.Phrase,
                PartOfSpeech.Preposition => Domain.PartOfSpeech.Preposition,
                PartOfSpeech.Pronoun => Domain.PartOfSpeech.Pronoun,
                PartOfSpeech.Verb => Domain.PartOfSpeech.Verb,
                _ => null
            },
            dto.Cefr switch
            {
                Cefr.A1 => CEFR.A1,
                Cefr.A2 => CEFR.A2,
                Cefr.B1 => CEFR.B1,
                Cefr.B2 => CEFR.B2,
                Cefr.C1 => CEFR.C1,
                Cefr.C2 => CEFR.C2,
                _ => null
            },
            dto.ResourceId,
            dto.Labels,
            dto.Synonyms,
            dto.Level);
    }
    
    public static ProfileDto Convert(Profile profile)
    {
        return new ProfileDto
        {
            Id = profile.Id,
            UserId = profile.Key.UserId,
            Native = profile.Key.Native,
            Target = profile.Key.Target,
            Level = profile.Level,
            CurrentChapterId = profile.CurrentChapter?.ChapterId,
            CurrentPageIndex = profile.CurrentChapter?.PageIndex,
            ValidationResults = profile.CurrentChapter?.Pages.Compactify() ?? string.Empty,
            CompletedChapters = profile.CompletedChapters
        };
    }

    public static Profile Convert(ProfileDto dto)
    {
        return new Profile(
            dto.Id,
            new ProfileKey(
                dto.UserId,
                dto.Native,
                dto.Target),
            dto.Level,
            dto.CompletedChapters,
            dto.CurrentChapterId is not null
                ? new ChapterSelection(
                    dto.CurrentChapterId.Value,
                    dto.CurrentPageIndex ?? default,
                    dto.ValidationResults.Decompactify(b => (ValidationResult)b))
                : null);
    }
    
    public static TranslationDto Convert(Translation translation)
    {
        return new TranslationDto
        {
            Id = translation.Id,
            Expression = Convert(translation.Expression),
            Language = translation.Language,
            Text = translation.Text,
            Spoken = translation.Spoken,
            AudioId = translation.AudioId
        };
    }

    public static Translation Convert(TranslationDto dto)
    {
        return new Translation(
            dto.Id,
            dto.Language,
            dto.Text,
            dto.Spoken,
            dto.AudioId,
            Convert(dto.Expression));
    }
}