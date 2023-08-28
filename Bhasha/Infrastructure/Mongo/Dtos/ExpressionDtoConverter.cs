using Bhasha.Domain;

namespace Bhasha.Infrastructure.Mongo.Dtos;

public static class ExpressionDtoConverter
{
    public static Expression Convert(this ExpressionDto dto)
    {
        return new Expression(
            dto.Id,
            dto.ExpressionType switch
            {
                ExpressionType.Expression => Domain.ExpressionType.Expression,
                ExpressionType.Text => Domain.ExpressionType.Text,
                ExpressionType.Phrase => Domain.ExpressionType.Phrase,
                ExpressionType.Punctuation => Domain.ExpressionType.Punctuation,
                ExpressionType.Word => Domain.ExpressionType.Word,
                _ => null
            },
            dto.PartOfSpeech switch
            {
                PartOfSpeech.Adjective => Domain.PartOfSpeech.Adjective,
                PartOfSpeech.Article => Domain.PartOfSpeech.Article,
                PartOfSpeech.Adverb => Domain.PartOfSpeech.Adverb,
                PartOfSpeech.Conjunction => Domain.PartOfSpeech.Conjunction,
                PartOfSpeech.Preposition => Domain.PartOfSpeech.Preposition,
                PartOfSpeech.Noun => Domain.PartOfSpeech.Noun,
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
    
    public static ExpressionDto Convert(this Expression dto)
    {
        return new ExpressionDto(
            dto.Id,
            dto.ExpressionType switch
            {
                Domain.ExpressionType.Expression => ExpressionType.Expression,
                Domain.ExpressionType.Text => ExpressionType.Text,
                Domain.ExpressionType.Phrase => ExpressionType.Phrase,
                Domain.ExpressionType.Punctuation => ExpressionType.Punctuation,
                Domain.ExpressionType.Word => ExpressionType.Word,
                _ => null
            },
            dto.PartOfSpeech switch
            {
                Domain.PartOfSpeech.Adjective => PartOfSpeech.Adjective,
                Domain.PartOfSpeech.Article => PartOfSpeech.Article,
                Domain.PartOfSpeech.Adverb => PartOfSpeech.Adverb,
                Domain.PartOfSpeech.Conjunction => PartOfSpeech.Conjunction,
                Domain.PartOfSpeech.Preposition => PartOfSpeech.Preposition,
                Domain.PartOfSpeech.Noun => PartOfSpeech.Noun,
                Domain.PartOfSpeech.Pronoun => PartOfSpeech.Pronoun,
                Domain.PartOfSpeech.Verb => PartOfSpeech.Verb,
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