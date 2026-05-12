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
                Description = rt.Description,
                CreatedByUserId = rt.CreatedByUserId
            }).ToList();

            foreach (var row in riskTypes)
            {
                row.CanDelete = DeleteAuthorization.UserMayDelete(User, row.CreatedByUserId);
                row.CanEdit = DeleteAuthorization.UserMayEdit(User, row.CreatedByUserId);
            }

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
                    Description = model.Description,
                    CreatedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
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

            if (!DeleteAuthorization.UserMayEdit(User, riskType.CreatedByUserId))
                return Forbid();

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

                if (!DeleteAuthorization.UserMayEdit(User, riskType.CreatedByUserId))
                    return Forbid();

                riskType.Name = model.Name;
                riskType.Description = model.Description;
                db.SaveChanges();
            }
            return this.RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var riskType = db.RiskTypes.FirstOrDefault(rt => rt.Id == id);
            if (riskType == null)
                return RedirectToAction(nameof(Index));

            if (!DeleteAuthorization.UserMayDelete(User, riskType.CreatedByUserId))
                return Forbid();

            db.RiskTypes.Remove(riskType);
            db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
