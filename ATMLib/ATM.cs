
using System.Net.NetworkInformation;
using System.Security.Principal;
using static System.Net.Mime.MediaTypeNames;

namespace ATMLib
{

    public class ATM
    {
        private ATMDbContext db = new ATMDbContext();

        
        public long Id { get; set; }

        public decimal ATMBalance { get; set; }

        public string Adress { get; set; }

        public Bank Bank { get; set; }

        public long BankId { get; set; }
            
        public delegate void ATMOperation(object sender, ATMOperationArgs args);

        public delegate void AccountOperation(object sender, AccountOperationArgs args);

        public event ATMOperation ATMOperationHandler;

        public event AccountOperation AccountOperationHandler;

        public Account FindCard(string cardNumber)
        {
            if (cardNumber.Length < 16 || string.IsNullOrEmpty(cardNumber))
            {
                AccountOperationHandler?.Invoke(this, new AccountOperationArgs { OperationMessage = "Card number must be 16 digit long", Error = ErrorType.User});
                return null;
            }

            var account = db.Accounts.FirstOrDefault(x => x.CardNumber == cardNumber);
            if (account == null)
            {
                AccountOperationHandler?.Invoke(this, new AccountOperationArgs { OperationMessage = "Card doesn`t exists", Error = ErrorType.System });
                return null;
            }
            
            return account;
        }
        public bool CheckPin(string pinCode, Account _account)
        {

            if (pinCode.Length < 4 || string.IsNullOrEmpty(pinCode))
            {
                AccountOperationHandler?.Invoke(this, new AccountOperationArgs { OperationMessage = "PIN code must be 4 digit long", Error = ErrorType.User });
                return false;
            }

            var account = db.Accounts.First(x => x.Id == _account.Id);

            if (account.CardPIN == pinCode)
            {
                return true;
            }

            AccountOperationHandler?.Invoke(this, new AccountOperationArgs { OperationMessage = "Incorrect PIN", Error = ErrorType.User });
            return false;
        }

        public void Withdraw(int value, Account account)
        {
            ATM atm = db.ATMs.Where(x => x.Id == 1).FirstOrDefault();
            if (value > atm.ATMBalance)
            {
                ATMOperationHandler?.Invoke(this, new ATMOperationArgs { OperationMessage = "The ATM doesn`t have enough funds to complete this operation", IsSuccess = false });
                return;
            }

            if (account.Balance < value)
            {
                ATMOperationHandler?.Invoke(this, new ATMOperationArgs { OperationMessage = "Insufficient funds", IsSuccess = false });
                return;
            }

            account.Balance -= value;
            atm.ATMBalance -= value;
            db.UpdateRange(account, atm);
            db.SaveChanges();
            ATMOperationHandler?.Invoke(this, new ATMOperationArgs { OperationMessage = $"You have successfully withdrawn {value} UAH", IsSuccess = true });
            return;
        }

        public void AddFunds(int value, Account account)
        {
            ATM atm = db.ATMs.Where(x => x.Id == 1).FirstOrDefault();
            if (value <= 0)
            {
                ATMOperationHandler?.Invoke(this, new ATMOperationArgs { OperationMessage = "The value you want to deposit to your account is incorrect.", IsSuccess = false });
                return;
            }

            account.Balance += value;
            atm.ATMBalance += value;
            db.UpdateRange(account, atm);
            db.SaveChanges();
            ATMOperationHandler?.Invoke(this, new ATMOperationArgs { OperationMessage = $"You have successfully deposited {value} UAH into your account", IsSuccess = true });
        }


        public void MakeATransaction(int value, Account senderAccount, Account recipientAccount)
        {
            if (value <= 0)
            {
                ATMOperationHandler?.Invoke(this, new ATMOperationArgs { OperationMessage = $"The value you want to deposit to {recipientAccount.Name} {recipientAccount.Surname} is incorrect.", IsSuccess = false });
                return;
            }

            senderAccount.Balance -= value;
            recipientAccount.Balance += value;
            db.UpdateRange(senderAccount, recipientAccount);
            db.SaveChanges();
            ATMOperationHandler?.Invoke(this, new ATMOperationArgs { OperationMessage = $"You have successfully sent {recipientAccount.Name} {recipientAccount.Surname} {value} UAH", IsSuccess = true });
        }
    }



}
