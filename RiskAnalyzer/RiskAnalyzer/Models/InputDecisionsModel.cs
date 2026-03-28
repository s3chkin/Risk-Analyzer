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

        // Потребителят въвежда оценка (напр. от 1 до 10)
        public int Score { get; set; }
        public string? ScenarioTitle { get; set; }
        public string? CriterionName { get; set; }
        public double CalculatedValue { get; set; }
    }
}
