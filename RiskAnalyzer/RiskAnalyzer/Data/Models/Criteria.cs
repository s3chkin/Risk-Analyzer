namespace RiskAnalyzer.Data.Models
{
    public class Criteria
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; } //тежестта на критериите (колко е опасно)

        public string? CreatedByUserId { get; set; }
        public virtual AppUser? CreatedByUser { get; set; }
    }
}
