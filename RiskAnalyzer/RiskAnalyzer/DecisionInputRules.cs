namespace RiskAnalyzer;

/// <summary>Правила за въвеждане на решение: валидна оценка и пресметнат риск (оценка × тежест).</summary>
public static class DecisionInputRules
{
    public static bool ScoreIsValid(int score) => score is >= 1 and <= 10;

    public static double CalculatedRiskValue(int score, int criterionWeight) =>
        score * criterionWeight;
}
