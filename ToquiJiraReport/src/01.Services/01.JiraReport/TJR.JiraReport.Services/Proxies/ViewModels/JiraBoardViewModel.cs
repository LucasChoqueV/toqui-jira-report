namespace TJR.JiraReport.Services.Proxies.ViewModels
{
    public class JiraBoardViewModel
    {
        public int Total { get; set; }
        public IEnumerable<ProxyBoardValues> Values { get; set; } = new List<ProxyBoardValues>();
    }

    public class ProxyBoardValues
    {
        public int Id { get; set; }
        public ProxyBoardProject Location { get; set; }
    }

    public class ProxyBoardProject
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
    }
}
