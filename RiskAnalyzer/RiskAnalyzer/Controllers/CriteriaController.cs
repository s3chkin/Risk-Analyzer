using Microsoft.AspNetCore.Mvc;
using RiskAnalyzer.Data;
using RiskAnalyzer.Data.Models;
using RiskAnalyzer.Models;

namespace RiskAnalyzer.Controllers
{
    public class CriteriaController : Controller
    {
        private readonly ApplicationDbContext db;

        public CriteriaController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            var model = db.Criteria.Select(c => new InputCriteriaModel
            {
                Id = c.Id,
                Name = c.Name,
                Weight = c.Weight
            }).ToList();

            return View(model);
        }

        public IActionResult Add()
        {
            return View(new InputCriteriaModel());
        }

        [HttpPost]
        public IActionResult Add(InputCriteriaModel model)
        {
            var criterion = new Criteria
            {
                Name = model.Name,
                Weight = model.Weight
            };

            db.Criteria.Add(criterion);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var model = db.Criteria
                .Where(c => c.Id == id)
                .Select(c => new InputCriteriaModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Weight = c.Weight
                }).FirstOrDefault();

            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var criterion = db.Criteria.FirstOrDefault(c => c.Id == id);
            if (criterion == null) return NotFound();

            var model = new InputCriteriaModel
            {
                Id = criterion.Id,
                Name = criterion.Name,
                Weight = criterion.Weight
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(InputCriteriaModel model)
        {
            var criterion = db.Criteria.FirstOrDefault(c => c.Id == model.Id);
            if (criterion != null)
            {
                criterion.Name = model.Name;
                criterion.Weight = model.Weight;

                db.Criteria.Update(criterion);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var criterion = db.Criteria.FirstOrDefault(c => c.Id == id);
            if (criterion != null)
            {
                db.Criteria.Remove(criterion);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}