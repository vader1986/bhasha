using Bhasha.Shared.Domain;

namespace Bhasha.MongoDb.Infrastructure.Mongo.Dtos;

public static class ExpressionDtoConverter
{
    public static Expression Convert(this ExpressionDto dto)
    {
        return new Expression(
            dto.Id,
            dto.ExpressionType switch
            {
                ExpressionType.Expression => Shared.Domain.ExpressionType.Expression,
                ExpressionType.Text => Shared.Domain.ExpressionType.Text,
                ExpressionType.Phrase => Shared.Domain.ExpressionType.Phrase,
                ExpressionType.Punctuation => Shared.Domain.ExpressionType.Punctuation,
                ExpressionType.Word => Shared.Domain.ExpressionType.Word,
                _ => null
            },
            dto.PartOfSpeech switch
            {
                PartOfSpeech.Adjective => Shared.Domain.PartOfSpeech.Adjective,
                PartOfSpeech.Article => Shared.Domain.PartOfSpeech.Article,
                PartOfSpeech.Adverb => Shared.Domain.PartOfSpeech.Adverb,
                PartOfSpeech.Conjunction => Shared.Domain.PartOfSpeech.Conjunction,
                PartOfSpeech.Preposition => Shared.Domain.PartOfSpeech.Preposition,
                PartOfSpeech.Noun => Shared.Domain.PartOfSpeech.Noun,
                PartOfSpeech.Pronoun => Shared.Domain.PartOfSpeech.Pronoun,
                PartOfSpeech.Verb => Shared.Domain.PartOfSpeech.Verb,
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
    
    public static ExpressionDto Convert(this Expression dto)
    {
        return new ExpressionDto(
            dto.Id,
            dto.ExpressionType switch
            {
                Shared.Domain.ExpressionType.Expression => ExpressionType.Expression,
                Shared.Domain.ExpressionType.Text => ExpressionType.Text,
                Shared.Domain.ExpressionType.Phrase => ExpressionType.Phrase,
                Shared.Domain.ExpressionType.Punctuation => ExpressionType.Punctuation,
                Shared.Domain.ExpressionType.Word => ExpressionType.Word,
                _ => null
            },
            dto.PartOfSpeech switch
            {
                Shared.Domain.PartOfSpeech.Adjective => PartOfSpeech.Adjective,
                Shared.Domain.PartOfSpeech.Article => PartOfSpeech.Article,
                Shared.Domain.PartOfSpeech.Adverb => PartOfSpeech.Adverb,
                Shared.Domain.PartOfSpeech.Conjunction => PartOfSpeech.Conjunction,
                Shared.Domain.PartOfSpeech.Preposition => PartOfSpeech.Preposition,
                Shared.Domain.PartOfSpeech.Noun => PartOfSpeech.Noun,
                Shared.Domain.PartOfSpeech.Pronoun => PartOfSpeech.Pronoun,
                Shared.Domain.PartOfSpeech.Verb => PartOfSpeech.Verb,
                _ => null
            },
            dto.Cefr switch
            {
                CEFR.A1 => Cefr.A1,
                CEFR.A2 => Cefr.A2,
                CEFR.B1 => Cefr.B1,
                CEFR.B2 => Cefr.B2,
                CEFR.C1 => Cefr.C1,
                CEFR.C2 => Cefr.C2,
                _ => null
            },
            dto.ResourceId,
            dto.Labels,
            dto.Synonyms,
            dto.Level);
    }
}