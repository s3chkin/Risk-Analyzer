using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RiskAnalyzer.Authorization;
using RiskAnalyzer.Data;
using RiskAnalyzer.Data.Models;
using RiskAnalyzer.Models;

namespace RiskAnalyzer.Controllers
{
    [Authorize]
    public class ScenariosController : Controller
    {
        private readonly ApplicationDbContext db;
        public ScenariosController(ApplicationDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            var model = db.Scenarios.Select(s => new InputScenariosModel
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                Location = s.Location,
                RiskTypeName = s.RiskType.Name,
                Status = s.Status,
                CreatedByUserId = s.CreatedByUserId
            }).ToList();

            foreach (var row in model)
            {
                row.CanDelete = DeleteAuthorization.UserMayDelete(User, row.CreatedByUserId);
                row.CanEdit = DeleteAuthorization.UserMayEdit(User, row.CreatedByUserId);
            }

            return View(model);
        }

        public IActionResult Add()
        {
            var model = new InputScenariosModel
            {
                RiskTypes = GetRiskTypes(),
                Status = "Нов",
                CreatedAt = DateTime.Now
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(InputScenariosModel model)
        {
            if (!ModelState.IsValid)
            {
                model.RiskTypes = GetRiskTypes();
                return View(model);
            }

            var scenario = new Scenario
            {
                Title = model.Title,
                Description = model.Description,
                Location = model.Location,
                CreatedAt = model.CreatedAt,
                Status = model.Status,
                RiskTypeId = model.RiskTypeId,
                CreatedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };
            db.Scenarios.Add(scenario);
            db.SaveChanges();

            return this.RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var model = db.Scenarios.Where(s => s.Id == id).Select(s => new InputScenariosModel
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                Location = s.Location,
                CreatedAt = s.CreatedAt,
                Status = s.Status,
                RiskTypeName = s.RiskType.Name,
                CreatedByUserId = s.CreatedByUserId,
                DecisionCount = db.Decisions.Count(d => d.ScenarioId == s.Id),
                AverageCalculatedRisk = db.Decisions.Where(d => d.ScenarioId == s.Id).Select(d => (double?)d.CalculatedValue).Average() ?? 0,
                LastDecisionAt = db.Decisions.Where(d => d.ScenarioId == s.Id).Select(d => (DateTime?)d.Timestamp).Max()
            }).FirstOrDefault();
            if (model == null)
            {
                return NotFound();
            }

            model.CanEdit = DeleteAuthorization.UserMayEdit(User, model.CreatedByUserId);

            return View(model);
        }


        public IActionResult Edit(int id)
        {
            var scenario = db.Scenarios.FirstOrDefault(s => s.Id == id);
            if (scenario == null)
            {
                return NotFound();
            }

            if (!DeleteAuthorization.UserMayEdit(User, scenario.CreatedByUserId))
                return Forbid();

            var model = new InputScenariosModel
            {
                Id = scenario.Id,
                Title = scenario.Title,
                Description = scenario.Description,
                Location = scenario.Location,
                CreatedAt = scenario.CreatedAt,
                Status = scenario.Status,
                RiskTypeId = scenario.RiskTypeId
            };

            model.RiskTypes = GetRiskTypes();
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(InputScenariosModel model)
        {
            if (!ModelState.IsValid)
            {
                model.RiskTypes = GetRiskTypes();
                return View(model);
            }

            var scenario = db.Scenarios.FirstOrDefault(s => s.Id == model.Id);
            if (scenario == null)
            {
                return NotFound();
            }

            if (!DeleteAuthorization.UserMayEdit(User, scenario.CreatedByUserId))
                return Forbid();

            scenario.Title = model.Title;
            scenario.Description = model.Description;
            scenario.Location = model.Location;
            scenario.CreatedAt = model.CreatedAt;
            scenario.Status = model.Status;
            scenario.RiskTypeId = model.RiskTypeId;
            db.Scenarios.Update(scenario);
            db.SaveChanges();

            return this.RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var scenario = db.Scenarios.FirstOrDefault(s => s.Id == id);
            if (scenario == null)
                return RedirectToAction(nameof(Index));

            if (!DeleteAuthorization.UserMayDelete(User, scenario.CreatedByUserId))
                return Forbid();

            db.Scenarios.Remove(scenario);
            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private List<SelectListItem> GetRiskTypes()
        {
            return db.RiskTypes
                .Select(rt => new SelectListItem
                {
                    Value = rt.Id.ToString(),
                    Text = rt.Name
                })
                .ToList();
        }
    }
}
