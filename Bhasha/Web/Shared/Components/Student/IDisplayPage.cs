using Microsoft.AspNetCore.Components;

namespace Bhasha.Web.Shared.Components.Student;

public interface IDisplayPage
{
    public ChapterPageViewModel ViewModel { get; set; }
    public string? Value { get; set; }
    public EventCallback<string?> ValueChanged { get; set; }
    public EventCallback<Exception> OnError { get; set; }
}