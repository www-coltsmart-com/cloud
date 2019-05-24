namespace ColtSmart.Service
{
    public class ErrorResult<T> : BaseResult<T>
    {
        public override string Type { get; } = "Error";

        public ErrorResult() { }

        public ErrorResult(T result) : base(result) { }
    }
}
