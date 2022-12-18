namespace Bhasha.Web.Domain.Pages;

[GenerateSerializer]
public record Cloze(string[] Words, int[] Gaps);

