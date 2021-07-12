namespace AccessPointMap.Service.Handlers
{
    public interface IServiceResult
    {
        ResultStatus Status();
    }

    public enum ResultStatus
    {
        Sucess,
        Failed,
        NotFound,
        NotPermited,
        Conflict
    }
}
