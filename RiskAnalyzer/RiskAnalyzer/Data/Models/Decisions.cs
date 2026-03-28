namespace RiskAnalyzer.Data.Models
{
    public class Decisions
    {
        public int Id { get; set; }
        public string RecommendedAction { get; set; }
        public double TotalRiskScore { get; set; }
        public DateTime Timestamp { get; set; }
        public string Notes { get; set; }

        public int ScenarioId { get; set; } 
        public virtual Scenarios Scenario { get; set; } 

        public string DecidedByUserId { get; set; } 
        public virtual AppUser DecidedByUser { get; set; } 
    }
}

