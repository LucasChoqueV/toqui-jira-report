namespace TJR.JiraReport.Services.Proxies.ViewModels
{
    public class JiraAssigneeViewModel
    {
        public IEnumerable<JiraAssignee> Actors { get; set; } = new List<JiraAssignee>();
    }

    public class JiraAssignee
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
    }
}
