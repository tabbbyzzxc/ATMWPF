using ATMLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ATMWPF
{
    /// <summary>
    /// Interaction logic for TransactionPage.xaml
    /// </summary>
    public partial class TransactionPage : Page
    {
        private MainWindow _mainWindow = (MainWindow)Application.Current.MainWindow;
        private Account _senderAccount;
        private Account _recipientAccount;
        private ATM _ATMService = new();

        public TransactionPage(Account account)
        {
            InitializeComponent();
            _senderAccount = account;
            _ATMService.ATMOperationHandler += _ATMService_ATMOperationHandler;
        }

        private void _ATMService_ATMOperationHandler(object sender, ATMOperationArgs args)
        {
            if (args.IsSuccess == true)
            {
                MessageBox.Show(args.OperationMessage, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(args.OperationMessage, "Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CardCheckContinueBtn_Click(object sender, RoutedEventArgs e)
        {
            var cardNumber = cardTextBox.Text;
            if (cardNumber.Length < 16 || string.IsNullOrEmpty(cardNumber))
            {
                cardTextBox.Clear();
                MessageBox.Show("Card number must be 16 digit long", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var recipientAccount = _ATMService.FindCard(cardNumber);
            if (recipientAccount == null)
            {
                cardTextBox.Clear();
                MessageBox.Show("Card doesn`t exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _recipientAccount = recipientAccount;
            TogglePages();
        }

        private void TogglePages()
        {
            cardInfo.Visibility = cardInfo.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
            fundsInput.Visibility = fundsInput.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
        }

        private void cardTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string cardNumber = textBox.Text;
            if (cardNumber.StartsWith("51") || cardNumber.StartsWith("52") ||
                    cardNumber.StartsWith("53") || cardNumber.StartsWith("54"))
            {
                mastercardImage.Visibility = Visibility.Visible;
                visaImage.Visibility = Visibility.Hidden;
            }
            else if (cardNumber.StartsWith("4"))
            {
                visaImage.Visibility = Visibility.Visible;
                mastercardImage.Visibility = Visibility.Hidden;

            }
            else if (string.IsNullOrEmpty(cardNumber))
            {
                visaImage.Visibility = Visibility.Hidden;
                mastercardImage.Visibility = Visibility.Hidden;

            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.ChangeFrameContent(new CardMenuPage(_senderAccount));
        }

        private void MakeATransactionBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ValueTextBox.Text))
            {
                return;
            }
            var value = int.Parse(ValueTextBox.Text);

            _ATMService.MakeATransaction(value, _senderAccount, _recipientAccount);
            _mainWindow.ChangeFrameContent(new CardMenuPage(_senderAccount));
        }
    }
}
