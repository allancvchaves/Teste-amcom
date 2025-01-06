namespace Questao5.Domain.Exceptions
{
    public class BusinessException : Exception
    {
        public string ErrorCode { get; private set; }

        public BusinessException(string message, string errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
