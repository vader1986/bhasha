using Bhasha.Domain;

namespace Bhasha.Web.Shared.Components.Student;

public sealed record ChapterPageBarViewModel(
    ProfileKey ProfileKey,
    int ExpressionId,
    string? UserInput);