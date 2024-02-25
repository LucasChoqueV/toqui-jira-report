using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Text.Json;
using TJR.Common.Extensions;
using TJR.JiraReport.Models;
using TJR.JiraReport.Services.Proxies.ViewModels;

namespace TJR.JiraReport.Services.Proxies
{
    public interface IJiraProxy
    {
        Task<Sprint> GetIssuesBySprintIdAsync(int sprintId);
        Task<JiraBoardViewModel> GetJiraBoardByNameAsync(string boardName);
        Task<JiraSprintViewModel> GetJiraSprintsByBoardIdAsync(int boardId);
        Task<JiraBoardConfigurationViewModel> GetJiraBoardConfigurationAsync(int boardId);
        Task<IEnumerable<JiraRoleViewModel>> GetJiraRolesByProjectIdAsync(int projectId);
        Task<JiraAssigneeViewModel> GetJiraRoleAsync(int projectId, int roleId);
        Task<JiraProjectViewModel> GetJiraProjectAsync(int projectId);
    }
    public class JiraProxy : IJiraProxy
    {
        private readonly HttpClient _httpClient;
        private readonly ApiUrls _apiUrls;

        public JiraProxy(
            HttpClient httpClient,
            IOptions<ApiUrls> apiUrls,
            IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddToken(httpContextAccessor);

            _apiUrls = apiUrls.Value ?? throw new ArgumentNullException(nameof(apiUrls));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<Sprint> GetIssuesBySprintIdAsync(int sprintId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiUrls.JiraApi}/rest/agile/1.0/sprint/{sprintId}/issue?maxResults=500");
                response.EnsureSuccessStatusCode();

                var result = JsonSerializer.Deserialize<Sprint>(
                    await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<JiraBoardViewModel> GetJiraBoardByNameAsync(string boardName)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiUrls.JiraApi}/rest/agile/1.0/board?maxResults=1&name={boardName}");
                response.EnsureSuccessStatusCode();

                var result = JsonSerializer.Deserialize<JiraBoardViewModel>(
                    await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<JiraBoardConfigurationViewModel> GetJiraBoardConfigurationAsync(int boardId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiUrls.JiraApi}/rest/agile/1.0/board/{boardId}/configuration");
                response.EnsureSuccessStatusCode();

                var result = JsonSerializer.Deserialize<JiraBoardConfigurationViewModel>(
                    await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<JiraProjectViewModel> GetJiraProjectAsync(int projectId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiUrls.JiraApi}/rest/api/2/project/{projectId}");
                response.EnsureSuccessStatusCode();

                var result = JsonSerializer.Deserialize<JiraProjectViewModel>(
                    await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<JiraAssigneeViewModel> GetJiraRoleAsync(int projectId, int roleId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiUrls.JiraApi}/rest/api/2/project/{projectId}/role/{roleId}");
                response.EnsureSuccessStatusCode();

                var result = JsonSerializer.Deserialize<JiraAssigneeViewModel>(
                    await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<JiraRoleViewModel>> GetJiraRolesByProjectIdAsync(int projectId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiUrls.JiraApi}/rest/api/2/project/{projectId}/roledetails");
                response.EnsureSuccessStatusCode();

                var result = JsonSerializer.Deserialize<IEnumerable<JiraRoleViewModel>>(
                    await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<JiraSprintViewModel> GetJiraSprintsByBoardIdAsync(int boardId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiUrls.JiraApi}/rest/agile/1.0/board/{boardId}/sprint");
                response.EnsureSuccessStatusCode();

                var result = JsonSerializer.Deserialize<JiraSprintViewModel>(
                    await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
