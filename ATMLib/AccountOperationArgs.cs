
namespace ATMLib
{
    public enum ErrorType
    {
        User = 0,
        System
    }

    public class AccountOperationArgs : EventArgs
    {
        public string OperationMessage { get; set; }

        public ErrorType Error { get; set; }
    }
}
