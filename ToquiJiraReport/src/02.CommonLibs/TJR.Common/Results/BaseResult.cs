using System.Text.Json.Serialization;

namespace TJR.Common.Results
{
    public enum ResultStatus
    {
        Ok,
        Error,
        Forbidden,
        Invalid,
        NotFound,
        NoContent,
    }

    public interface IResult
    {
        ResultStatus Status { get; }
        bool IsSuccess { get; }
        string SuccessMessage { get; }
        IEnumerable<string> ErrorMessages { get; }
    }

    public class BaseResult : IResult
    {
        public BaseResult() { }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ResultStatus Status { get; init; } = ResultStatus.Ok;

        public bool IsSuccess => Status == ResultStatus.Ok;

        public string SuccessMessage { get; set; } = String.Empty;

        public IEnumerable<string> ErrorMessages { get; init; } = new List<string>();
    }
}
