using Bhasha.Domain;

namespace Bhasha.Web.Shared.Components.Student;

public sealed record ChapterPageViewModel(
    ProfileKey ProfileKey,
    DisplayedChapter Chapter, 
    DisplayedPage Page);