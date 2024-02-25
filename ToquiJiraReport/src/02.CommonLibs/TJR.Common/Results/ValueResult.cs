namespace TJR.Common.Results
{
    public class ValueResult<T> : BaseResult
    {
        public ValueResult() { }

        public T Result { get; set; }
    }
}
