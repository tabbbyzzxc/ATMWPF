﻿using ATMLib;
using System;
using System.Linq;

namespace ATMWPF
{
    public class CardService
    {

        public Account FindCard(string cardNumber)
        {
            ATMDbContext db = new ATMDbContext();
            var account = db.Accounts.FirstOrDefault(x => x.CardNumber == cardNumber);

            return account;

        }

        public bool CheckPin(string pinCode, Account _account)
        {
            ATMDbContext db = new ATMDbContext();
            var account = db.Accounts.First(x => x.Id == _account.Id);
            
            if(account.CardPIN == pinCode)
            {
                return true;
            }
                
            return false;
        }

        public bool Withdraw(int value, Account account)
        {
            if(account.Balance < value)
            {
                return false;
            }

            ATMDbContext db = new ATMDbContext();
            account.Balance -= value;
            db.Update(account);
            db.SaveChanges();
            return true;
        }
    }
}