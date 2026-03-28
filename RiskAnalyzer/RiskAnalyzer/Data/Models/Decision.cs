namespace RiskAnalyzer.Data.Models
{
    public class Decision
    {
        public int Id { get; set; }

        public int CriterionId { get; set; }
        public virtual Criteria Criterion { get; set; }

        public int Score { get; set; } // Оценката (1-10)
        public double CalculatedValue { get; set; } // Score * Weight

        public int ScenarioId { get; set; }
        public virtual Scenario Scenario { get; set; }

        public string? RecommendedAction { get; set; }
        public string? Notes { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;

        public string? DecidedByUserId { get; set; }
        public virtual AppUser? DecidedByUser { get; set; }
    }
}