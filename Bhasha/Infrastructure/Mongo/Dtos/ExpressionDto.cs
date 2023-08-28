using Generator.Equals;

namespace Bhasha.Infrastructure.Mongo.Dtos;

[Equatable]
public partial record ExpressionDto
(
	Guid Id,
	ExpressionType? ExpressionType,
	PartOfSpeech? PartOfSpeech,
	Cefr? Cefr,
	string? ResourceId,
	
	[property: OrderedEquality] 
	string[] Labels,
	
	[property: OrderedEquality] 
	string[] Synonyms,
	
	int Level
);

public enum ExpressionType
{
	Word,
	Expression,
	Phrase,
	Text,
	Punctuation
}

public enum Cefr
{
	A1, 
	A2,
	B1, 
	B2, 
	C1, 
	C2
}

public enum PartOfSpeech
{
	Noun,
	Pronoun,
	Adjective,
	Verb,
	Adverb,
	Preposition,
	Conjunction,
	Article
}

