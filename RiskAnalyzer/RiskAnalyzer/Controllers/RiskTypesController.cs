using Microsoft.AspNetCore.Mvc;
using RiskAnalyzer.Data;
using RiskAnalyzer.Data.Models;
using RiskAnalyzer.Models;

namespace RiskAnalyzer.Controllers
{
    public class RiskTypesController : Controller
    {
        public readonly ApplicationDbContext db;
        public RiskTypesController(ApplicationDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            var riskTypes = db.RiskTypes.Select(rt => new InputRiskTypesModel
            {
                Id = rt.Id,
                Name = rt.Name,
                Description = rt.Description
            }).ToList();
            return View(riskTypes);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(InputRiskTypesModel model)
        {
            if (ModelState.IsValid)
            {
                var riskType = new RiskType
                {
                    Name = model.Name,
                    Description = model.Description
                };
                db.RiskTypes.Add(riskType);
                db.SaveChanges();

            }
            return this.RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var riskType = db.RiskTypes.FirstOrDefault(rt => rt.Id == id);
            if (riskType == null)
            {
                return NotFound();
            }
            var model = new InputRiskTypesModel
            {
                Id = riskType.Id,
                Name = riskType.Name,
                Description = riskType.Description
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(InputRiskTypesModel model)
        {
            if (ModelState.IsValid)
            {
                var riskType = db.RiskTypes.FirstOrDefault(rt => rt.Id == model.Id);
                if (riskType == null)
                {
                    return NotFound();
                }
                riskType.Name = model.Name;
                riskType.Description = model.Description;
                db.SaveChanges();
            }
            return this.RedirectToAction("Index");
        }

        public IActionResult Delete(int id) { 
            var riskType = db.RiskTypes.FirstOrDefault(rt => rt.Id == id);
            db.RiskTypes.Remove(riskType);
            db.SaveChanges();
            return this.RedirectToAction("Index");

        }
    }
}
