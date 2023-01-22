namespace Bhasha.Domain.Pages;

[GenerateSerializer]
public record Cloze(string[] Words, int[] Gaps);

