namespace TJR.JiraReport.Services.Proxies.ViewModels
{
    public class JiraSprintViewModel
    {
        public IEnumerable<ProxySprintValues> Values { get; set; } = new List<ProxySprintValues>();
    }

    public class ProxySprintValues
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
