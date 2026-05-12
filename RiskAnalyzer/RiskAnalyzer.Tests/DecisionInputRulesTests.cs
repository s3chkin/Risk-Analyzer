using Xunit;

namespace RiskAnalyzer.Tests;

public sealed class DecisionInputRulesTests
{
    [Theory]
    [InlineData(1, true)]
    [InlineData(5, true)]
    [InlineData(10, true)]
    [InlineData(0, false)]
    [InlineData(11, false)]
    [InlineData(-1, false)]
    public void ScoreIsValid_Respects_Range_1_To_10(int score, bool expected)
    {
        Assert.Equal(expected, DecisionInputRules.ScoreIsValid(score));
    }

    [Theory]
    [InlineData(3, 4, 12)]
    [InlineData(10, 10, 100)]
    [InlineData(1, 1, 1)]
    public void CalculatedRiskValue_Is_Score_Times_Weight(int score, int weight, double expected)
    {
        Assert.Equal(expected, DecisionInputRules.CalculatedRiskValue(score, weight));
    }
}
