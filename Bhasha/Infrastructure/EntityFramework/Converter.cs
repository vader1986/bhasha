using Bhasha.Infrastructure.EntityFramework.Dtos;
using Bhasha.Infrastructure.EntityFramework.Extensions;
using Bhasha.Shared.Domain;
using ExpressionType = Bhasha.Infrastructure.EntityFramework.Dtos.ExpressionType;
using PartOfSpeech = Bhasha.Infrastructure.EntityFramework.Dtos.PartOfSpeech;

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
            Expressions = chapter.Pages.Select(Convert).ToArray()
        };
    }

    public static Chapter Convert(ChapterDto dto)
    {
        return new Chapter(
            dto.Id,
            dto.RequiredLevel,
            Convert(dto.Name),
            Convert(dto.Description),
            dto.Expressions.Select(Convert).ToArray(),
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
                Shared.Domain.ExpressionType.Word => ExpressionType.Word,
                Shared.Domain.ExpressionType.Expression => ExpressionType.Expression,
                Shared.Domain.ExpressionType.Phrase => ExpressionType.Phrase,
                Shared.Domain.ExpressionType.Text => ExpressionType.Text,
                Shared.Domain.ExpressionType.Punctuation => ExpressionType.Punctuation,
                _ => null
            },
            PartOfSpeech = expression.PartOfSpeech switch
            {
                Shared.Domain.PartOfSpeech.Noun => PartOfSpeech.Noun,
                Shared.Domain.PartOfSpeech.Pronoun => PartOfSpeech.Pronoun,
                Shared.Domain.PartOfSpeech.Adjective => PartOfSpeech.Adjective,
                Shared.Domain.PartOfSpeech.Verb => PartOfSpeech.Verb,
                Shared.Domain.PartOfSpeech.Adverb => PartOfSpeech.Adverb,
                Shared.Domain.PartOfSpeech.Preposition => PartOfSpeech.Preposition,
                Shared.Domain.PartOfSpeech.Conjunction => PartOfSpeech.Conjunction,
                Shared.Domain.PartOfSpeech.Article => PartOfSpeech.Article,
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
                ExpressionType.Word => Shared.Domain.ExpressionType.Word,
                ExpressionType.Expression => Shared.Domain.ExpressionType.Expression,
                ExpressionType.Phrase => Shared.Domain.ExpressionType.Phrase,
                ExpressionType.Text => Shared.Domain.ExpressionType.Text,
                ExpressionType.Punctuation => Shared.Domain.ExpressionType.Punctuation,
                _ => null
            },
            dto.PartOfSpeech switch
            {
                PartOfSpeech.Noun => Shared.Domain.PartOfSpeech.Noun,
                PartOfSpeech.Pronoun => Shared.Domain.PartOfSpeech.Pronoun,
                PartOfSpeech.Adjective => Shared.Domain.PartOfSpeech.Adjective,
                PartOfSpeech.Verb => Shared.Domain.PartOfSpeech.Verb,
                PartOfSpeech.Adverb => Shared.Domain.PartOfSpeech.Adverb,
                PartOfSpeech.Preposition => Shared.Domain.PartOfSpeech.Preposition,
                PartOfSpeech.Conjunction => Shared.Domain.PartOfSpeech.Conjunction,
                PartOfSpeech.Article => Shared.Domain.PartOfSpeech.Article,
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