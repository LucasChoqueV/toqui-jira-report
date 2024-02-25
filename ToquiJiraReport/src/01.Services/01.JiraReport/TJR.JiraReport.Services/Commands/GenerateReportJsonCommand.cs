using MediatR;
using TJR.Common.Results;
using TJR.JiraReport.Services.ViewModels;

namespace TJR.JiraReport.Services.Commands
{
    public class GenerateReportJsonCommand : IRequest<ValueResult<JiraReportViewModel>>
    {
        public string SprintName { get; set; }
        public string BoardName { get; set; }
        public IEnumerable<string> Queries { get; set; } = new List<string>();
    }

    public class OrderQueryCommand
    {
        public string QueryType { get; set; }
        public List<string> Values { get; set; } = new List<string>();
        public OrderQueryCommand Child { get; set; }
    }
}
