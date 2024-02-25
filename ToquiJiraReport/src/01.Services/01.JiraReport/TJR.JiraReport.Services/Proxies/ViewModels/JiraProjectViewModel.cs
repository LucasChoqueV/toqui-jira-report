namespace TJR.JiraReport.Services.Proxies.ViewModels
{
    public class JiraProjectViewModel
    {
        public IEnumerable<ProxyProjectIssueType> IssueTypes { get; set; } = new List<ProxyProjectIssueType>();
    }

    public class ProxyProjectIssueType
    {
        public string Name { get; set; }
        public bool Subtask { get; set; }
    }
}
