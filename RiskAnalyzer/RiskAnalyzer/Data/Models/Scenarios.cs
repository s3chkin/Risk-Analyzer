namespace RiskAnalyzer.Data.Models
{
    public class Scenarios
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; } //какво точно се случва.
        public string Location{ get; set; }
        public DateTime CreatedAt { get; set; } //кога е регистриран сигналът
        public string Status { get; set; }

        public int RiskTypeId { get; set; } 
        public virtual RiskTypes RiskType { get; set; } 
    }
}
