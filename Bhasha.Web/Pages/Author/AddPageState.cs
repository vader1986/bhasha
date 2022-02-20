using Bhasha.Web.Domain;

namespace Bhasha.Web.Pages.Author
{
	public class AddPageState
	{
        public bool CannotCreatePage => string.IsNullOrWhiteSpace(Native) || string.IsNullOrWhiteSpace(Target);

        public PageType PageType { get; set; } = PageType.MultipleChoice;
        public string? Native { get; set; }
        public string? NativeSpoken { get; set; }
        public string? Target { get; set; }
        public string? TargetSpoken { get; set; }
        public string? NativeReference { get; set; }
        public List<string> Leads { get; set; } = new List<string>();
    }
}

