using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RiskAnalyzer.Data.Models;
using RiskAnalyzer;

namespace RiskAnalyzer.Data;

/// <summary>
/// Примерни данни: по 5 типа риск, критерии, сценарии и оценки (за отчетност).
/// Пуска се само при SeedDemoData=true. Първо премахва стари записи с префикс „[Демо]“,
/// после добавя комплекта, ако маркерният сценарий още липсва.
/// </summary>
public static class DemoDataSeeder
{
    /// <summary>Уникално заглавие за проверка дали сийдърът вече е изпълнен (текуща версия).</summary>
    private const string SeedPresenceScenarioTitle = "Покачване на ниво — р. Девня";

    private const string LegacyDemoPrefix = "[Демо]";

    /// <summary>Старо заглавие на първия сценарий от сийдъра с префикс „[Демо] “.</summary>
    private const string LegacySeedScenarioTitle = "[Демо] Покачване на ниво — р. Девня";

    public static async Task SeedAsync(
        ApplicationDbContext db,
        UserManager<AppUser> userManager,
        IConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        if (!configuration.GetValue("SeedDemoData", false))
            return;

        var email = configuration["SeedAdmin:Email"] ?? "admin@admin.com";
        var admin = await userManager.FindByEmailAsync(email);
        if (string.IsNullOrEmpty(admin?.Id))
            return;

        var ownerId = admin.Id;

        await PurgeLegacyDemoRowsAsync(db, cancellationToken);

        if (await db.Scenarios.AnyAsync(
                s => s.Title == SeedPresenceScenarioTitle || s.Title == LegacySeedScenarioTitle,
                cancellationToken))
            return;

        var riskTypes = new List<RiskType>
        {
            new()
            {
                Name = "Наводнение",
                Description = "Риск от високи води и преливане на реки.",
                CreatedByUserId = ownerId
            },
            new()
            {
                Name = "Пожар",
                Description = "Горски и полски пожари при сухо време.",
                CreatedByUserId = ownerId
            },
            new()
            {
                Name = "Свлачище",
                Description = "Масиви с нестабилен терен и интензивни валежи.",
                CreatedByUserId = ownerId
            },
            new()
            {
                Name = "Силен вятър",
                Description = "Щети по инфраструктура и сгради.",
                CreatedByUserId = ownerId
            },
            new()
            {
                Name = "Градушка",
                Description = "Селскостопански и покривни щети.",
                CreatedByUserId = ownerId
            }
        };

        db.RiskTypes.AddRange(riskTypes);
        await db.SaveChangesAsync(cancellationToken);

        var criteria = new List<Criteria>
        {
            new() { Name = "Засегнато население", Weight = 8, CreatedByUserId = ownerId },
            new() { Name = "Критична инфраструктура", Weight = 10, CreatedByUserId = ownerId },
            new() { Name = "Скорост на разпространение", Weight = 6, CreatedByUserId = ownerId },
            new() { Name = "Достъпност за екипи", Weight = 4, CreatedByUserId = ownerId },
            new() { Name = "Прогнозна тежест на щетите", Weight = 7, CreatedByUserId = ownerId }
        };

        db.Criteria.AddRange(criteria);
        await db.SaveChangesAsync(cancellationToken);

        var baseDate = DateTime.UtcNow.AddDays(-14);
        var scenarios = new List<Scenario>
        {
            new()
            {
                Title = SeedPresenceScenarioTitle,
                Description = "Пороен дъжд, нивото на реката нараства за 6 часа.",
                Location = "Варна област, Девня",
                CreatedAt = baseDate.AddDays(1),
                Status = "В процес",
                RiskTypeId = riskTypes[0].Id,
                CreatedByUserId = ownerId
            },
            new()
            {
                Title = "Суха трева край автомагистрала",
                Description = "Палене на стърнище, дим към пътното платно.",
                Location = "Тракия, км 210",
                CreatedAt = baseDate.AddDays(2),
                Status = "Нов",
                RiskTypeId = riskTypes[1].Id,
                CreatedByUserId = ownerId
            },
            new()
            {
                Title = "Пукнатина по пътен откос",
                Description = "Седмици валежи, появи се пропадане на част от откоса.",
                Location = "Проход Ришки",
                CreatedAt = baseDate.AddDays(3),
                Status = "Приключен",
                RiskTypeId = riskTypes[2].Id,
                CreatedByUserId = ownerId
            },
            new()
            {
                Title = "Оранжев код — пориви 90 km/h",
                Description = "Прогноза за 12 ч, риск за кранове и табели.",
                Location = "Бургас — пристанище",
                CreatedAt = baseDate.AddDays(4),
                Status = "В процес",
                RiskTypeId = riskTypes[3].Id,
                CreatedByUserId = ownerId
            },
            new()
            {
                Title = "Клетка градушка — овощна градина",
                Description = "Радарно ядро 15 km, очакван диаметър на леда до 3 cm.",
                Location = "Пловдивско, Карлово",
                CreatedAt = baseDate.AddDays(5),
                Status = "Нов",
                RiskTypeId = riskTypes[4].Id,
                CreatedByUserId = ownerId
            }
        };

        db.Scenarios.AddRange(scenarios);
        await db.SaveChangesAsync(cancellationToken);

        var decisions = new[]
        {
            (scenarioIdx: 0, criterionIdx: 0, score: 6, action: "Осигурете евакуационни маршрути."),
            (scenarioIdx: 1, criterionIdx: 1, score: 8, action: "Координация с пожарна и АПИ."),
            (scenarioIdx: 2, criterionIdx: 2, score: 4, action: "Ограничаване на движението до обследване."),
            (scenarioIdx: 3, criterionIdx: 3, score: 7, action: "Временно затваряне на опасни зони."),
            (scenarioIdx: 4, criterionIdx: 4, score: 5, action: "Застрахователен оглед в рамките на 48 ч.")
        };

        var decisionEntities = new List<Decision>();
        for (var i = 0; i < decisions.Length; i++)
        {
            var d = decisions[i];
            var scenario = scenarios[d.scenarioIdx];
            var criterion = criteria[d.criterionIdx];
            decisionEntities.Add(new Decision
            {
                ScenarioId = scenario.Id,
                CriterionId = criterion.Id,
                Score = d.score,
                CalculatedValue = DecisionInputRules.CalculatedRiskValue(d.score, criterion.Weight),
                RecommendedAction = d.action,
                Notes = null,
                Timestamp = baseDate.AddDays(6 + i).AddHours(10 + i),
                DecidedByUserId = ownerId
            });
        }

        db.Decisions.AddRange(decisionEntities);
        await db.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Премахва стария комплект с префикс „[Демо]“, за да не се дублира след смяна на сийдъра.
    /// </summary>
    private static async Task PurgeLegacyDemoRowsAsync(ApplicationDbContext db, CancellationToken cancellationToken)
    {
        var legacyRtIds = await db.RiskTypes.AsNoTracking()
            .Where(r => r.Name.StartsWith(LegacyDemoPrefix))
            .Select(r => r.Id)
            .ToListAsync(cancellationToken);

        var legacyCritIds = await db.Criteria.AsNoTracking()
            .Where(c => c.Name.StartsWith(LegacyDemoPrefix))
            .Select(c => c.Id)
            .ToListAsync(cancellationToken);

        var legacyScenIds = await db.Scenarios.AsNoTracking()
            .Where(s => s.Title.StartsWith(LegacyDemoPrefix) || legacyRtIds.Contains(s.RiskTypeId))
            .Select(s => s.Id)
            .ToListAsync(cancellationToken);

        if (legacyRtIds.Count == 0 && legacyCritIds.Count == 0 && legacyScenIds.Count == 0)
            return;

        var decisions = await db.Decisions
            .Where(d => legacyScenIds.Contains(d.ScenarioId) || legacyCritIds.Contains(d.CriterionId))
            .ToListAsync(cancellationToken);
        if (decisions.Count > 0)
            db.Decisions.RemoveRange(decisions);

        var scenarios = await db.Scenarios
            .Where(s => legacyScenIds.Contains(s.Id))
            .ToListAsync(cancellationToken);
        if (scenarios.Count > 0)
            db.Scenarios.RemoveRange(scenarios);

        var criteria = await db.Criteria
            .Where(c => legacyCritIds.Contains(c.Id))
            .ToListAsync(cancellationToken);
        if (criteria.Count > 0)
            db.Criteria.RemoveRange(criteria);

        var riskTypes = await db.RiskTypes
            .Where(r => legacyRtIds.Contains(r.Id))
            .ToListAsync(cancellationToken);
        if (riskTypes.Count > 0)
            db.RiskTypes.RemoveRange(riskTypes);

        await db.SaveChangesAsync(cancellationToken);
    }
}
