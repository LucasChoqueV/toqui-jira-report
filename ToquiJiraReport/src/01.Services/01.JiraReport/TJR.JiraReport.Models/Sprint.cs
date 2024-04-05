namespace TJR.JiraReport.Models
{
    public class Sprint
    {
        public int Total { get; set; }
        public IEnumerable<Issue> Issues { get; set; } = new List<Issue>();
    }

    public class Issue
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public Field Fields { get; set; }
    }

    public class Field
    {
        public string Summary { get; set; }
        public string Description { get; set; }
        public Assignee Assignee { get; set; }
        public IssueType IssueType { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }
        public IEnumerable<Issue> SubTasks { get; set; } = new List<Issue>();
    }

    public class IssueType
    {
        public string Name { get; set; }
        public bool SubTask { get; set; }
    }

    public class Priority
    {
        public string Name { get; set; }
    }

    public class Assignee
    {
        public string EmailAddress { get; set; }
        public string DisplayName { get; set; }
    }

    public class Status
    {
        public string Name { get; set; }
    }
}
