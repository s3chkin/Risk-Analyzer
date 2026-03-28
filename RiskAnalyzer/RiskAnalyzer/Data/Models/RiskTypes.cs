namespace RiskAnalyzer.Data.Models
{
    public class RiskTypes
    {
        public int Id{ get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Scenarios> Scenarios { get; set; } = new List<Scenarios>();
    }
}
