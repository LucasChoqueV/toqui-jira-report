using AutoMapper;
using TJR.Common.Results;
using TJR.JiraReport.Models;
using TJR.JiraReport.Services.Proxies;
using TJR.JiraReport.Services.Proxies.ViewModels;

namespace TJR.JiraReport.Services.Repositories
{
    public interface IJiraRepository
    {
        Task<ValueResult<Board>> GetBoardByNameAsync(string boardName);
        Task<ValueResult<Sprint>> GetSprintAsync(int boardId, string sprintName);
        Task LoadBoardStatusesAsync(int boardId);
        Task LoadBoardAssigneesAsync(int projectId);
        Task LoadBoardIssueTypesAsync(int projectId);
        List<string> JiraStatus { get; set; }
        List<string> JiraAssignees { get; set; }
        List<string> JiraIssueTypes { get; set; }
        List<string> JiraSubtasks { get; set; }
    }
    public class JiraRepository : IJiraRepository
    {
        private readonly IJiraProxy _proxy;
        public List<string> JiraStatus { get; set; } = new List<string>();
        public List<string> JiraAssignees { get; set; } = new List<string>();
        public List<string> JiraIssueTypes { get; set; } = new List<string>();
        public List<string> JiraSubtasks { get; set; } = new List<string>() { "Subtask" };

        public JiraRepository(
            IJiraProxy proxy)
        {
            _proxy = proxy ?? throw new ArgumentNullException(nameof(proxy));
        }
        public async Task<ValueResult<Board>> GetBoardByNameAsync(string boardName)
        {
            try
            {
                var response = await _proxy.GetJiraBoardByNameAsync(boardName);

                if (response == null || !response.Values.Any())
                {
                    return new ValueResult<Board>();
                }

                return new ValueResult<Board>()
                {
                    Result = new Board()
                    {
                        BoardId = response.Values.First().Id,
                        BoardName = boardName,
                        ProjectId = response.Values.First().Location.ProjectId,
                        ProjectName = response.Values.First().Location.ProjectName
                    }
                };

            }
            catch (HttpRequestException hre)
            {
                var status = ResultStatus.Error;
                if (!Enum.TryParse(hre.StatusCode.ToString(), out status))
                {
                    status = ResultStatus.Error;
                }

                return new ValueResult<Board>()
                {
                    Status = status,
                    ErrorMessages = new List<string>() { hre.Message, hre.InnerException?.Message }
                };
            }
            catch (Exception ex)
            {
                return new ValueResult<Board>()
                {
                    Status = ResultStatus.Error,
                    ErrorMessages = new List<string>() { ex.Message, ex.InnerException?.Message }
                };
            }
        }

        public async Task<ValueResult<Sprint>> GetSprintAsync(int boardId, string sprintName)
        {
            try
            {
                var sprints = await _proxy.GetJiraSprintsByBoardIdAsync(boardId);
                var sprintId = sprints.Values.Where(x => x.Name == sprintName).First().Id;
                var sprintIssues = await _proxy.GetIssuesBySprintIdAsync(sprintId);


                return new ValueResult<Sprint>()
                {
                    Result = sprintIssues
                };

            }
            catch (HttpRequestException hre)
            {
                var status = ResultStatus.Error;
                if (!Enum.TryParse(hre.StatusCode.ToString(), out status))
                {
                    status = ResultStatus.Error;
                }

                return new ValueResult<Sprint>()
                {
                    Status = status,
                    ErrorMessages = new List<string>() { hre.Message, hre.InnerException?.Message }
                };
            }
            catch (Exception ex)
            {
                return new ValueResult<Sprint>()
                {
                    Status = ResultStatus.Error,
                    ErrorMessages = new List<string>() { ex.Message, ex.InnerException?.Message }
                };
            }
        }

        public async Task LoadBoardStatusesAsync(int boardId)
        {
            try
            {
                var boarConfiguration = await _proxy.GetJiraBoardConfigurationAsync(boardId);
                if (boarConfiguration != null && boarConfiguration.ColumnConfig != null)
                {
                    JiraStatus = boarConfiguration.ColumnConfig.Columns.Select(x => x.Name).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task LoadBoardAssigneesAsync(int projectId)
        {
            try
            {
                var roles = await _proxy.GetJiraRolesByProjectIdAsync(projectId);
                if (roles != null)
                {
                    foreach (var role in roles)
                    {
                        if (role.Name == "Administrator" || role.Name == "Member")
                        {
                            var assignees = await _proxy.GetJiraRoleAsync(projectId, role.Id);
                            JiraAssignees.AddRange(assignees.Actors.Select(x => x.DisplayName).ToList());
                        }
                    }
                }

                JiraAssignees.Add("Unassigned");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task LoadBoardIssueTypesAsync(int projectId)
        {
            try
            {
                var project = await _proxy.GetJiraProjectAsync(projectId);
                if (project != null)
                {
                    JiraIssueTypes = project.IssueTypes.Where(x => !x.Subtask).Select(y => y.Name).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
