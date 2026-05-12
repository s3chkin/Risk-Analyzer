using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace RiskAnalyzer.Models
{
    public class InputCriteriaModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Името е задължително.")]
        public string Name { get; set; } = string.Empty;
        [Range(1, 10, ErrorMessage = "Тежестта трябва да е между 1 и 10.")]
        public int Weight { get; set; }

        [BindNever]
        public string? CreatedByUserId { get; set; }

        [BindNever]
        public bool CanDelete { get; set; }

        [BindNever]
        public bool CanEdit { get; set; }
    }
}
