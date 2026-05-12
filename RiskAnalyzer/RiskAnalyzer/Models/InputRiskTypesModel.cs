using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace RiskAnalyzer.Models
{
    public class InputRiskTypesModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Въведи име на типа.")]
        [Display(Name = "Име")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Въведи описание.")]
        [Display(Name = "Описание")]
        public string Description { get; set; } = string.Empty;

        [BindNever]
        public string? CreatedByUserId { get; set; }

        [BindNever]
        public bool CanDelete { get; set; }

        [BindNever]
        public bool CanEdit { get; set; }
    }
}
