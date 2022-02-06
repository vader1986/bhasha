using Bhasha.Web.Domain;

namespace Bhasha.Web.Pages
{
	public class AddPageState
	{
        public bool CannotCreatePage => string.IsNullOrWhiteSpace(Native) || string.IsNullOrWhiteSpace(Target);

        public PageType PageType { get; set; } = PageType.MultipleChoice;
        public CEFR Cefr { get; set; } = CEFR.Unknown;
        public ExpressionType Expr { get; set; } = ExpressionType.Word;
        public string? Native { get; set; }
        public string? NativeSpoken { get; set; }
        public string? Target { get; set; }
        public string? TargetSpoken { get; set; }
        public List<string> Leads { get; set; } = new List<string>();
    }
}

