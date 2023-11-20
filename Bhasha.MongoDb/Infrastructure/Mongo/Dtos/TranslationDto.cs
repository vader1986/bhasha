namespace Bhasha.MongoDb.Infrastructure.Mongo.Dtos;

public record TranslationDto(
	Guid Id,
	Guid ExpressionId,
	string Language,
	string Text,
	string? Spoken,
	string? AudioId);