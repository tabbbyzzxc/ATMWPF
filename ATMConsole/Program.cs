using ATMLib;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using System;

namespace MyApp // Note: actual namespace depends on the project name.
{
    public class Program
    {
        private static ATM _ATMService = new();
        static void Main(string[] args)
        {
            _ATMService.AccountOperationHandler += _ATMService_AccountOperationHandler;
            _ATMService.ATMOperationHandler += _ATMService_ATMOperationHandler;

            Console.Write("Card number:");
            var account = _ATMService.FindCard(Console.ReadLine());
            Console.Write("Card PIN:");
            var pinCode = _ATMService.CheckPin(Console.ReadLine(), account);

            int menuOption;
            do
            {
                Console.WriteLine("Enter - \"1\" to check card balance");
                Console.WriteLine("Enter - \"2\" to withdraw funds");
                Console.WriteLine("Enter - \"3\" to deposit funds");
                Console.WriteLine("Enter - \"4\" to make a transaction");
                Console.WriteLine("Enter - \"0\" to exit");
                menuOption = int.Parse(Console.ReadLine());

                switch (menuOption)
                {
                    case 0:
                        break;

                    case 1:
                        Console.WriteLine($"{account.Name}'s account balance: {account.Balance}");
                        break;

                    case 2:
                        Console.Write("Amount of money you would like to withdraw: ");
                        _ATMService.Withdraw(int.Parse(Console.ReadLine()), account);
                        break;

                    case 3:
                        Console.Write("Amount of money you would like to deposit into your account: ");
                        _ATMService.AddFunds(int.Parse(Console.ReadLine()), account);
                        break;

                    case 4:
                        Console.Write("Recipients card number: ");
                        var recipientAccount = _ATMService.FindCard(Console.ReadLine());
                        Console.WriteLine($"How much do you want to deposit into {recipientAccount.Name}'s account: ");
                        _ATMService.MakeATransaction(int.Parse(Console.ReadLine()), account, recipientAccount);
                        break;
                }

            }while(menuOption != 0);
            Console.ReadKey();
        }

        private static void _ATMService_ATMOperationHandler(object sender, ATMOperationArgs args)
        {
            if (args.IsSuccess == true)
            {
                WriteSystemMessage(args.OperationMessage, 2);
            }
            else
            {
                WriteSystemMessage(args.OperationMessage, 0);
            }
        }

        private static void _ATMService_AccountOperationHandler(object sender, AccountOperationArgs args)
        {
            if (args.Error == ErrorType.User)
            {
                WriteSystemMessage(args.OperationMessage, 0);
            }
            else
            {
                WriteSystemMessage(args.OperationMessage, 1);
            }

        }

        private static void WriteSystemMessage(string message, int messageType)
        {
            switch (messageType)
            {
                case 0://user
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(message);
                    break;

                case 1://sys
                    Console.BackgroundColor = ConsoleColor.Magenta;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(message);
                    break;

                case 2://success
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(message);
                    break;
            }

            Console.ResetColor();
        }

        
    }
}