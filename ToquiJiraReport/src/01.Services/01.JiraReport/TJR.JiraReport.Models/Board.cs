namespace TJR.JiraReport.Models
{
    public class Board
    {
        public int BoardId { get; set; }
        public string BoardName { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
    }
}
