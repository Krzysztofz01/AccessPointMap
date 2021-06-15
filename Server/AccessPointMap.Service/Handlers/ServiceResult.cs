using Balto.Service.Exceptions;

namespace AccessPointMap.Service.Handlers
{
    public class ServiceResult : IServiceResult
    {
        private ResultStatus _status;

        public ServiceResult(ResultStatus status)
        {
            _status = status;
        }

        public ResultStatus Status()
        {
            return _status;
        }
    }

    public class ServiceResult<T> : IServiceResult where T : class
    {
        private T _result;
        private ResultStatus _status;

        public ServiceResult(T result)
        {
            _result = result;
            _status = ResultStatus.Sucess;
        }

        public ServiceResult(ResultStatus status)
        {
            if (status == ResultStatus.Sucess) throw new ServiceResultException("You can not set the success result for failed task!");

            _result = null;
            _status = status;
        }

        public T Result()
        {
            if (_status != ResultStatus.Sucess) throw new ServiceResultException("No results. You need to correctly handle the result that indicates a failure.");
            return _result;
        }

        public ResultStatus Status()
        {
            return _status;
        }
    }
}
