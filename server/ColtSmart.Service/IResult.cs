namespace ColtSmart.Service
{
    public interface IResult
    {
        string Type { get; }
    }

    public class BaseResult<T> : IResult
    {
        public virtual string Type { get; } = "Success";

        public T Result { get; set; }

        public BaseResult(T result)
        {
            this.Result = result;
        }

        public BaseResult() { }
    }

    public enum ResultType
    {
        Error,
        Sucess,
        Message
    }
}
