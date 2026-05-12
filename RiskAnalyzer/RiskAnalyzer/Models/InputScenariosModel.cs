using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RiskAnalyzer.Models
{
    public class InputScenariosModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Location { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Нов";
        [Range(1, int.MaxValue, ErrorMessage = "Избери валиден тип риск.")]
        public int RiskTypeId { get; set; }
        public string? RiskTypeName { get; set; }
        public List<SelectListItem> RiskTypes { get; set; } = new List<SelectListItem>();

        // Полета за отчетност в детайли на сценарий
        public int DecisionCount { get; set; }
        public double AverageCalculatedRisk { get; set; }
        public DateTime? LastDecisionAt { get; set; }

        [BindNever]
        public string? CreatedByUserId { get; set; }

        [BindNever]
        public bool CanDelete { get; set; }

        [BindNever]
        public bool CanEdit { get; set; }
    }
}
