namespace CloudDataProtection.Core.Result
{
    public class BusinessResult<TResult>
    {
        public bool Success { get; private set; }
        public TResult Data { get; private set; }
        public string Message { get; private set; }

        public static BusinessResult<TResult> Error(string message)
        {
            return new BusinessResult<TResult>()
            {
                Success = false,
                Message = message
            };
        }

        public static BusinessResult<TResult> Ok(TResult data)
        {
            return new BusinessResult<TResult>()
            {
                Success = true,
                Data = data
            };
        }

        public static BusinessResult<TResult> Ok()
        {
            return new BusinessResult<TResult>()
            {
                Success = true
            };
        }
    }
}