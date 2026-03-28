using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using RiskAnalyzer.Data.Models;

namespace RiskAnalyzer.Models
{
    public class InputScenariosModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Location { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Нов";
        public int RiskTypeId { get; set; }
        [Required]
        public string RiskTypeName { get; internal set; }
        public List<SelectListItem> RiskTypes { get; set; } = new List<SelectListItem>();
    }
}
