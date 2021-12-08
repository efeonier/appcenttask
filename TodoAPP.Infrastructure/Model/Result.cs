namespace TodoAPP.Infrastructure.Model
{
    public class Result : BaseResult
    {
        public Result() { }
        public Result(object data)
        {
            Data = data;
        }
        public object Data { get; set; }
    }
}