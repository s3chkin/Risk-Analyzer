using Microsoft.AspNetCore.Mvc.Rendering;

namespace RiskAnalyzer.Models
{
    public class ReportingViewModel
    {
        public string? SelectedStatus { get; set; }
        public int? SelectedRiskTypeId { get; set; }

        public List<SelectListItem> Statuses { get; set; } = new();
        public List<SelectListItem> RiskTypes { get; set; } = new();

        public int TotalScenarios { get; set; }
        public int TotalCriteria { get; set; }
        public int TotalDecisions { get; set; }
        public double AverageRisk { get; set; }

        public List<ScenarioReportRow> ScenarioRows { get; set; } = new();
        public List<DecisionReportRow> RecentDecisions { get; set; } = new();
    }

    public class ScenarioReportRow
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string RiskTypeName { get; set; } = string.Empty;
        public int DecisionsCount { get; set; }
        public double AverageRisk { get; set; }
    }

    public class DecisionReportRow
    {
        public DateTime Timestamp { get; set; }
        public string ScenarioTitle { get; set; } = string.Empty;
        public string CriterionName { get; set; } = string.Empty;
        public int Score { get; set; }
        public double CalculatedValue { get; set; }
        public string? DecidedByUserName { get; set; }
        public string? RecommendedAction { get; set; }
    }
}
