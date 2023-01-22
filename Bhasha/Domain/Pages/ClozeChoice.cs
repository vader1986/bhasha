namespace Bhasha.Domain.Pages;

[GenerateSerializer]
public record ClozeChoice(string[] Text, int[] Gaps, string[] Choices);
