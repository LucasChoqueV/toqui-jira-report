using Microsoft.Extensions.Logging;
using TJR.Common.Application.CQRS;
using TJR.Common.Results;
using TJR.JiraReport.Models;
using TJR.JiraReport.Services.Commands;
using TJR.JiraReport.Services.Repositories;
using TJR.JiraReport.Services.ViewModels;

namespace TJR.JiraReport.Services.CommandHandlers
{
    public class GenerateReportJsonCommandHandler : CommandHandlerBase<GenerateReportJsonCommand, ValueResult<JiraReportViewModel>>
    {
        private readonly ILogger<GenerateReportJsonCommandHandler> _logger;
        private readonly IJiraRepository _jiraRepository;

        public GenerateReportJsonCommandHandler(
            IJiraRepository jiraRepository,
            ILogger<GenerateReportJsonCommandHandler> logger)
        {
            _jiraRepository = jiraRepository ?? throw new ArgumentNullException(nameof(jiraRepository)); ;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected async override Task<ValueResult<JiraReportViewModel>> HandleAsync()
        {
            _logger.LogInformation($"--- GenerateReportJsonCommandHandler started");
            try
            {
                var board = await _jiraRepository.GetBoardByNameAsync(CommandRequest.BoardName);
                if (board.Result.BoardId == 0)
                {
                    return new ValueResult<JiraReportViewModel>()
                    {
                        Status = ResultStatus.NotFound,
                        ErrorMessages = new List<string>() { $"{CommandRequest.BoardName} not found." }
                    };
                }

                if (!board.IsSuccess)
                {
                    return new ValueResult<JiraReportViewModel>()
                    {
                        ErrorMessages = board.ErrorMessages.Select(x => x.ToString()),
                        Status = board.Status
                    };
                }

                var sprint = await _jiraRepository.GetSprintAsync(board.Result.BoardId, CommandRequest.SprintName);

                if (sprint.Result == null)
                {
                    return new ValueResult<JiraReportViewModel>()
                    {
                        Status = ResultStatus.NotFound,
                        ErrorMessages = new List<string>() { $"{CommandRequest.SprintName} not found" }
                    };
                }

                if (!sprint.IsSuccess)
                {
                    return new ValueResult<JiraReportViewModel>()
                    {
                        ErrorMessages = sprint.ErrorMessages.Select(x => x.ToString()),
                        Status = sprint.Status
                    };
                }

                // load status, assignees, issuetypes
                await _jiraRepository.LoadBoardStatusesAsync(board.Result.BoardId);
                await _jiraRepository.LoadBoardIssueTypesAsync(board.Result.ProjectId);
                await _jiraRepository.LoadBoardAssigneesAsync(board.Result.ProjectId);

                var queries = new List<List<IssueViewModel>>();

                foreach (var query in CommandRequest.Queries)
                {
                    var queryResult = GetIssuesGeneric(GenerateRequest(query), CommandRequest.SprintName, sprint.Result.Issues);
                    if (queryResult != null)
                    {
                        queries.Add(queryResult);
                    }
                }

                return new ValueResult<JiraReportViewModel>()
                {
                    Result = new JiraReportViewModel()
                    {
                        SprintName = CommandRequest.SprintName,
                        Total = sprint.Result.Issues.Count(),
                        Queries = queries
                    }
                };

            }
            catch (Exception ex)
            {
                _logger.LogError($"--- GenerateReportJsonCommandHandler error {ex.Message} - {ex.InnerException?.Message}");
                return new ValueResult<JiraReportViewModel>
                {
                    Status = ResultStatus.Error,
                    ErrorMessages = new List<string> { ex.Message, ex.InnerException?.Message }
                };
            }
        }

        private OrderQueryCommand GenerateRequest(string query)
        {
            var splitted = query.Split('/');
            OrderQueryCommand request = null;

            foreach (var split in Reverse(splitted))
            {

                request = new OrderQueryCommand()
                {
                    QueryType = split,
                    Child = request
                };
                if (split == "Status")
                {
                    request.Values = _jiraRepository.JiraStatus;
                }
                if (split == "Assignee")
                {
                    request.Values = _jiraRepository.JiraAssignees;
                }
                if (split == "IssueType")
                {
                    request.Values = _jiraRepository.JiraIssueTypes;
                }
                if (split == "Subtask")
                {
                    request.Values = _jiraRepository.JiraSubtasks;
                }
            }

            return request;
        }

        static IEnumerable<T> Reverse<T>(T[] array)
        {
            for (int i = array.Length - 1; i >= 0; i--)
            {
                yield return array[i];
            }
        }

        private List<IssueViewModel> GetIssuesGeneric(OrderQueryCommand request, string parent, IEnumerable<Issue> parentIssues)
        {
            var result = new List<IssueViewModel>();
            if (request.Child == null)
            {
                foreach (var value in request.Values)
                {
                    var element = new IssueViewModel();
                    IEnumerable<Issue> filterIssues = new List<Issue>();
                    element.QueryType = request.QueryType;
                    element.Name = value;
                    element.Parent = parent;

                    if (request.QueryType == "Status")
                    {
                        filterIssues = FilterIssuesByStatus(parentIssues, value);
                    }

                    if (request.QueryType == "Assignee")
                    {
                        filterIssues = FilterIssuesByAssignee(parentIssues, value);

                    }

                    if (request.QueryType == "IssueType")
                    {
                        filterIssues = FilterIssuesByIssueType(parentIssues, value);
                    }

                    if (request.QueryType == "Subtask")
                    {
                        filterIssues = FilterIssuesBySubtask(parentIssues);
                    }

                    element.Total = filterIssues.Count();
                    result.Add(element);
                }
            }
            else
            {
                foreach (var value in request.Values)
                {
                    var element = new IssueViewModel();
                    IEnumerable<Issue> filterIssues = new List<Issue>();
                    element.QueryType = request.QueryType;
                    element.Name = value;
                    element.Parent = parent;

                    if (request.QueryType == "Status")
                    {
                        filterIssues = FilterIssuesByStatus(parentIssues, value);
                    }

                    if (request.QueryType == "Assignee")
                    {
                        filterIssues = FilterIssuesByAssignee(parentIssues, value);
                    }

                    if (request.QueryType == "IssueType")
                    {
                        filterIssues = FilterIssuesByIssueType(parentIssues, value);
                    }

                    if (request.QueryType == "Subtask")
                    {
                        filterIssues = FilterIssuesBySubtask(parentIssues);
                    }

                    element.Total = filterIssues.Count();
                    element.Child = GetIssuesGeneric(request.Child, request.QueryType, filterIssues);
                    result.Add(element);
                }
            }

            return result;
        }

        private IEnumerable<Issue> FilterIssuesByStatus(IEnumerable<Issue> issues, string value)
        {
            return FilterIssues(issues, x => x.Fields.Status.Name.ToUpper() == value.ToUpper());
        }

        private IEnumerable<Issue> FilterIssuesByAssignee(IEnumerable<Issue> issues, string value)
        {
            if (value == "Unassigned")
            {
                return FilterIssues(issues, x => x.Fields.Assignee == null);
            }
            else
            {
                return FilterIssues(issues, x => x.Fields.Assignee != null && x.Fields.Assignee.DisplayName.Equals(value));
            }
        }

        private IEnumerable<Issue> FilterIssuesBySubtask(IEnumerable<Issue> issues)
        {
            var issueWithSubtask = FilterIssues(issues, x => x.Fields.SubTasks.Any());
            var result = new List<Issue>();
            foreach (var subtask in issueWithSubtask)
            {
                result.AddRange(subtask.Fields.SubTasks);
            }

            return result;
        }

        private IEnumerable<Issue> FilterIssuesByIssueType(IEnumerable<Issue> issues, string value)
        {
            return FilterIssues(issues, x => x.Fields.IssueType.Name.ToUpper().Equals(value.ToUpper()));
        }

        private IEnumerable<Issue> FilterIssues(IEnumerable<Issue> issues, Func<Issue, bool> predicate = null)
        {
            var filter = issues.Where(predicate);
            if (filter.Any() && filter != null)
            {
                return filter;
            }

            return new List<Issue>();
        }
    }
}
