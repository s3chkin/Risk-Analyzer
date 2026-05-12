using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace RiskAnalyzer.Models
{
    public class InputRiskTypesModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [BindNever]
        public string? CreatedByUserId { get; set; }

        [BindNever]
        public bool CanDelete { get; set; }

        [BindNever]
        public bool CanEdit { get; set; }
    }
}
