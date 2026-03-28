using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RiskAnalyzer.Data;
using RiskAnalyzer.Data.Models;
using RiskAnalyzer.Models;

namespace RiskAnalyzer.Controllers
{
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
                Status = s.Status

            }).ToList();

            return View(model);
        }

        public IActionResult Add()
        {
            var riskTpyes = db.RiskTypes.Select(rt => new SelectListItem
            {
                Value = rt.Id.ToString(),
                Text = rt.Name.ToString()
            }).ToList();
            var model = new InputScenariosModel
            {
                RiskTypes = riskTpyes
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(InputScenariosModel model)
        {
            
                var scenario = new Scenarios
                {
                    Title = model.Title,
                    Description = model.Description,
                    Location = model.Location,
                    CreatedAt = model.CreatedAt,
                    Status = model.Status,
                    RiskTypeId = model.RiskTypeId
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
                RiskTypeName = s.RiskType.Name
            }).FirstOrDefault();
            return View(model);
        }


        public IActionResult Edit(int id)
        {
            var scenario = db.Scenarios.FirstOrDefault(s => s.Id == id);
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

            var riskTpyes = db.RiskTypes.Select(rt => new SelectListItem
            {
                Value = rt.Id.ToString(),
                Text = rt.Name
            }).ToList();
            model.RiskTypes = riskTpyes;
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(InputScenariosModel model)
        {
           
                var scenario = db.Scenarios.FirstOrDefault(s => s.Id == model.Id);
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
            if (scenario != null)
            {
                db.Scenarios.Remove(scenario);
                db.SaveChanges();
            }
            return this.RedirectToAction("Index");
        }
    }
}
