using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using RiskAnalyzer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RiskAnalyzer.Authorization;
using RiskAnalyzer.Data;
using RiskAnalyzer.Data.Models;
using RiskAnalyzer.Models;

namespace RiskAnalyzer.Controllers
{
    [Authorize]
    public class DecisionsController : Controller
    {
        private readonly ApplicationDbContext db;

        public DecisionsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            var model = db.Decisions
                .Select(d => new InputDecisionsModel
                {
                    Id = d.Id,
                    ScenarioTitle = d.Scenario.Title,
                    CriterionName = d.Criterion.Name,
                    Score = d.Score,
                    CalculatedValue = d.CalculatedValue,
                    RecommendedAction = d.RecommendedAction,
                    Notes = d.Notes,
                    Timestamp = d.Timestamp,
                    DecidedByUserName = d.DecidedByUser != null ? d.DecidedByUser.UserName : null,
                    DecidedByUserId = d.DecidedByUserId
                })
                .OrderByDescending(d => d.Timestamp)
                .ToList();

            foreach (var row in model)
                row.CanDelete = DeleteAuthorization.UserMayDelete(User, row.DecidedByUserId);

            return View(model);
        }

        public IActionResult Add()
        {
            var model = new InputDecisionsModel
            {
                Score = 5,
                Scenarios = db.Scenarios.Select(s => new SelectListItem
                { Value = s.Id.ToString(), Text = s.Title }).ToList(),
                Criteria = db.Criteria.Select(c => new SelectListItem
                { Value = c.Id.ToString(), Text = c.Name }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(InputDecisionsModel model)
        {
            if (model.ScenarioId <= 0)
            {
                ModelState.AddModelError(nameof(model.ScenarioId), "Избери сценарий.");
            }

            if (model.CriterionId <= 0)
            {
                ModelState.AddModelError(nameof(model.CriterionId), "Избери критерий.");
            }

            if (!DecisionInputRules.ScoreIsValid(model.Score))
            {
                ModelState.AddModelError(nameof(model.Score), "Оценката трябва да е между 1 и 10.");
            }

            var scenario = db.Scenarios.FirstOrDefault(s => s.Id == model.ScenarioId);
            if (model.ScenarioId > 0 && scenario == null)
            {
                ModelState.AddModelError(nameof(model.ScenarioId), "Невалиден сценарий.");
            }

            var criterion = db.Criteria.FirstOrDefault(c => c.Id == model.CriterionId);
            if (model.CriterionId > 0 && criterion == null)
            {
                ModelState.AddModelError(nameof(model.CriterionId), "Невалиден критерий.");
            }

            if (!ModelState.IsValid)
            {
                model.Scenarios = db.Scenarios.Select(s => new SelectListItem
                { Value = s.Id.ToString(), Text = s.Title }).ToList();
                model.Criteria = db.Criteria.Select(c => new SelectListItem
                { Value = c.Id.ToString(), Text = c.Name }).ToList();
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var decision = new Decision
            {
                ScenarioId = model.ScenarioId,
                CriterionId = model.CriterionId,
                Score = model.Score,
                CalculatedValue = DecisionInputRules.CalculatedRiskValue(model.Score, criterion!.Weight),
                RecommendedAction = model.RecommendedAction,
                Notes = model.Notes,
                Timestamp = DateTime.Now,
                DecidedByUserId = userId
            };

            db.Decisions.Add(decision);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var decision = db.Decisions.FirstOrDefault(d => d.Id == id);
            if (decision == null)
                return RedirectToAction(nameof(Index));

            if (!DeleteAuthorization.UserMayDelete(User, decision.DecidedByUserId))
                return Forbid();

            db.Decisions.Remove(decision);
            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}