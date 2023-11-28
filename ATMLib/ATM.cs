
using System.Security.Principal;

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

        public event ATMOperation ATMOperationHandler;

        public Account FindCard(string cardNumber)
        {
            var account = db.Accounts.FirstOrDefault(x => x.CardNumber == cardNumber);

            return account;

        }

        public bool CheckPin(string pinCode, Account _account)
        {
            var account = db.Accounts.First(x => x.Id == _account.Id);

            if (account.CardPIN == pinCode)
            {
                return true;
            }

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
