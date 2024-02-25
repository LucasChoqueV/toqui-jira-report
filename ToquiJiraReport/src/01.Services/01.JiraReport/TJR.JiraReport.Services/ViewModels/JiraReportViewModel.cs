namespace TJR.JiraReport.Services.ViewModels
{
    public class JiraReportViewModel
    {
        public string SprintName { get; set; }
        public int Total { get; set; }
        public IEnumerable<IEnumerable<IssueViewModel>> Queries { get; set; } = new List<List<IssueViewModel>>();
    }

    public class IssueViewModel
    {
        public string QueryType { get; set; }
        public string Name { get; set; }
        public string Parent { get; set; }
        public int Total { get; set; }
        public IEnumerable<IssueViewModel> Child { get; set; } = new List<IssueViewModel>();
    }
}
