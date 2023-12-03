using Bhasha.Domain;
using Bhasha.Infrastructure.EntityFramework.Extensions;
using FluentAssertions;
using Xunit;

namespace Bhasha.Tests.Infrastructure.EntityFramework.Extensions;

public class CompactifyExtensionsTests
{
    [Fact]
    public void CompactifyAndDecompactifyArrayOfEnumValues()
    {
        // arrange
        var array = new[]
        {
            ValidationResult.Correct,
            ValidationResult.PartiallyCorrect,
            ValidationResult.Wrong
        };
        
        // act
        var result = array
            .Compactify()
            .Decompactify(x => (ValidationResult)x);
        
        // assert
        result
            .Should()
            .BeEquivalentTo(array);
    }
}