namespace RiskAnalyzer.Data.Models
{
    public class RiskType
    {
        public int Id{ get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Scenario> Scenarios { get; set; } = new List<Scenario>();
    }
}
