using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RiskAnalyzer.Data;
using RiskAnalyzer.Data.Models;
using RiskAnalyzer.Models;

namespace RiskAnalyzer.Controllers
{
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
                    CalculatedValue = d.CalculatedValue
                }).ToList();

            return View(model);
        }

        public IActionResult Add()
        {
            var model = new InputDecisionsModel
            {
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
            var criterion = db.Criteria.FirstOrDefault(c => c.Id == model.CriterionId);

            if (criterion != null)
            {
                var decision = new Decision
                {
                    ScenarioId = model.ScenarioId,
                    CriterionId = model.CriterionId,
                    Score = model.Score,
                    // СМЕТКАТА: Оценка * Тежест
                    CalculatedValue = model.Score * criterion.Weight
                };

                db.Decisions.Add(decision);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var decision = db.Decisions.FirstOrDefault(d => d.Id == id);
            if (decision != null)
            {
                db.Decisions.Remove(decision);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}