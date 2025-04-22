﻿// ReSharper disable CollectionNeverUpdated.Global

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bhasha.Infrastructure.EntityFramework.Dtos;

public sealed class ExpressionDto
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	
	public ExpressionType? ExpressionType { get; set; }
	
	public PartOfSpeech? PartOfSpeech { get; set; }
	
	public Cefr? Cefr { get; set; }
	
	public string? ResourceId { get; set; }
	
	public string[] Labels { get; set; } = [];
	
	public string[] Synonyms { get; set; } = [];
	
	public int Level { get; set; }

	public List<ChapterDto> ChapterDtos { get; set; } = [];
}

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
	Adjective,
	Adverb,
	AuxiliaryVerb,
	Determiner,
	Exclamation,
	ModalVerb,
	Noun,
	PhrasalVerb,
	Phrase,
	Preposition,
	Pronoun,
	Verb
}

