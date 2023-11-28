
namespace ATMLib
{
    public class ATMOperationArgs : EventArgs
    {
        public string OperationMessage { get; set; }

        public bool IsSuccess { get; set; }
    }
}
