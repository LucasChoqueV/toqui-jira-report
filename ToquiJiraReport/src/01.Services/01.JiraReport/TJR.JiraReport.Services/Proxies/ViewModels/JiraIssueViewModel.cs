namespace TJR.JiraReport.Services.Proxies.ViewModels
{
    public class JiraIssueViewModel
    {
        public int Total { get; set; }
        public IEnumerable<ProxyIssue> Issues { get; set; } = new List<ProxyIssue>();
    }

    public class ProxyIssue
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public ProxyField Fields { get; set; }
    }

    public class ProxyField
    {
        public string Summary { get; set; }
        public string Description { get; set; }
        public ProxyAssignee Assignee { get; set; }
        public ProxyIssueType IssueType { get; set; }
        public ProxyPriority Priority { get; set; }
        public ProxyStatus Status { get; set; }
        public IEnumerable<ProxyIssue> SubTasks { get; set; } = new List<ProxyIssue>();
    }

    public class ProxyIssueType
    {
        public string Name { get; set; }
        public bool SubTask { get; set; }
    }

    public class ProxyPriority
    {
        public string Name { get; set; }
    }

    public class ProxyAssignee
    {
        public string EmailAddress { get; set; }
        public string DisplayName { get; set; }
    }
    public class ProxyStatus
    {
        public string Name { get; set; }
    }
}
