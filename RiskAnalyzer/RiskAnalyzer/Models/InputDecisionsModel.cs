using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RiskAnalyzer.Models
{
    public class InputDecisionsModel
    {
        public int Id { get; set; }

        public int ScenarioId { get; set; }
        public List<SelectListItem>? Scenarios { get; set; }

        public int CriterionId { get; set; }
        public List<SelectListItem>? Criteria { get; set; }

        /// <summary>Сила на въздействие по избрания критерий за този сценарий (1–10).</summary>
        public int Score { get; set; } = 5;
        public string? ScenarioTitle { get; set; }
        public string? CriterionName { get; set; }
        public double CalculatedValue { get; set; }

        public string? RecommendedAction { get; set; }
        public string? Notes { get; set; }
        public DateTime Timestamp { get; set; }
        public string? DecidedByUserName { get; set; }

        [BindNever]
        public string? DecidedByUserId { get; set; }

        [BindNever]
        public bool CanDelete { get; set; }
    }
}
