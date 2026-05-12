using System.ComponentModel.DataAnnotations;
using RiskAnalyzer.Models;
using Xunit;

namespace RiskAnalyzer.Tests;

public sealed class InputCriteriaModelValidationTests
{
    private static bool TryValidate(InputCriteriaModel model, out IList<ValidationResult> results)
    {
        results = new List<ValidationResult>();
        var ctx = new ValidationContext(model, null, null);
        return Validator.TryValidateObject(model, ctx, results, validateAllProperties: true);
    }

    [Fact]
    public void Valid_Name_And_Weight_Passes()
    {
        var model = new InputCriteriaModel { Name = "Критерий", Weight = 5 };
        Assert.True(TryValidate(model, out var results), string.Join("; ", results.Select(r => r.ErrorMessage)));
    }

    [Fact]
    public void Empty_Name_Fails_Required()
    {
        var model = new InputCriteriaModel { Name = "", Weight = 5 };
        Assert.False(TryValidate(model, out var results));
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(InputCriteriaModel.Name)));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(11)]
    public void Weight_Outside_1_To_10_Fails(int weight)
    {
        var model = new InputCriteriaModel { Name = "Ок", Weight = weight };
        Assert.False(TryValidate(model, out var results));
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(InputCriteriaModel.Weight)));
    }
}
