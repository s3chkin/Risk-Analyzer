using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RiskAnalyzer.Data;
using RiskAnalyzer.Models;

namespace RiskAnalyzer.Controllers
{
    [Authorize]
    public class ReportingController : Controller
    {
        private readonly ApplicationDbContext db;

        public ReportingController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index(string? status, int? riskTypeId)
        {
            var scenariosQuery = db.Scenarios.AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
            {
                scenariosQuery = scenariosQuery.Where(s => s.Status == status);
            }

            if (riskTypeId.HasValue && riskTypeId.Value > 0)
            {
                scenariosQuery = scenariosQuery.Where(s => s.RiskTypeId == riskTypeId.Value);
            }

            var scenarioRows = scenariosQuery
                .Select(s => new ScenarioReportRow
                {
                    Id = s.Id,
                    Title = s.Title,
                    Status = s.Status,
                    RiskTypeName = s.RiskType.Name,
                    DecisionsCount = db.Decisions.Count(d => d.ScenarioId == s.Id),
                    AverageRisk = db.Decisions
                        .Where(d => d.ScenarioId == s.Id)
                        .Select(d => (double?)d.CalculatedValue)
                        .Average() ?? 0
                })
                .OrderByDescending(s => s.AverageRisk)
                .ThenBy(s => s.Title)
                .ToList();

            var recentDecisions = db.Decisions
                .OrderByDescending(d => d.Timestamp)
                .Take(10)
                .Select(d => new DecisionReportRow
                {
                    Timestamp = d.Timestamp,
                    ScenarioTitle = d.Scenario.Title,
                    CriterionName = d.Criterion.Name,
                    Score = d.Score,
                    CalculatedValue = d.CalculatedValue,
                    DecidedByUserName = d.DecidedByUser != null ? d.DecidedByUser.UserName : null,
                    RecommendedAction = d.RecommendedAction
                })
                .ToList();

            var model = new ReportingViewModel
            {
                SelectedStatus = status,
                SelectedRiskTypeId = riskTypeId,
                Statuses = GetStatuses(),
                RiskTypes = db.RiskTypes
                    .OrderBy(r => r.Name)
                    .Select(r => new SelectListItem
                    {
                        Value = r.Id.ToString(),
                        Text = r.Name
                    })
                    .ToList(),
                TotalScenarios = db.Scenarios.Count(),
                TotalCriteria = db.Criteria.Count(),
                TotalDecisions = db.Decisions.Count(),
                AverageRisk = db.Decisions.Select(d => (double?)d.CalculatedValue).Average() ?? 0,
                ScenarioRows = scenarioRows,
                RecentDecisions = recentDecisions
            };

            return View(model);
        }

        private static List<SelectListItem> GetStatuses()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "Нов", Text = "Нов" },
                new SelectListItem { Value = "В процес", Text = "В процес" },
                new SelectListItem { Value = "Приключен", Text = "Приключен" }
            };
        }
    }
}
