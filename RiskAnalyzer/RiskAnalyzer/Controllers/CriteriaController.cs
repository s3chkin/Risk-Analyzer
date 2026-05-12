using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RiskAnalyzer.Authorization;
using RiskAnalyzer.Data;
using RiskAnalyzer.Data.Models;
using RiskAnalyzer.Models;

namespace RiskAnalyzer.Controllers
{
    [Authorize]
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
                Weight = c.Weight,
                CreatedByUserId = c.CreatedByUserId
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
            return View(new InputCriteriaModel());
        }

        [HttpPost]
        public IActionResult Add(InputCriteriaModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var criterion = new Criteria
            {
                Name = model.Name,
                Weight = model.Weight,
                CreatedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
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
                    Weight = c.Weight,
                    CreatedByUserId = c.CreatedByUserId
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
            var criterion = db.Criteria.FirstOrDefault(c => c.Id == id);
            if (criterion == null) return NotFound();

            if (!DeleteAuthorization.UserMayEdit(User, criterion.CreatedByUserId))
                return Forbid();

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
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var criterion = db.Criteria.FirstOrDefault(c => c.Id == model.Id);
            if (criterion == null)
            {
                return NotFound();
            }

            if (!DeleteAuthorization.UserMayEdit(User, criterion.CreatedByUserId))
                return Forbid();

            criterion.Name = model.Name;
            criterion.Weight = model.Weight;

            db.Criteria.Update(criterion);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var criterion = db.Criteria.FirstOrDefault(c => c.Id == id);
            if (criterion == null)
                return RedirectToAction(nameof(Index));

            if (!DeleteAuthorization.UserMayDelete(User, criterion.CreatedByUserId))
                return Forbid();

            db.Criteria.Remove(criterion);
            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}