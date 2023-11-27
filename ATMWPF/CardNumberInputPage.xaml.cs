using ATMLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
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
    /// Interaction logic for CardNumberInputPage.xaml
    /// </summary>
    public partial class CardNumberInputPage : Page
    {
        private ATM _ATMservice = new();

        private Account _account;
        public CardNumberInputPage()
        {
            InitializeComponent();
            

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
            else if(string.IsNullOrEmpty(cardNumber))
            {
                visaImage.Visibility = Visibility.Hidden;
                mastercardImage.Visibility = Visibility.Hidden;

            }
            

        }

        private void ProceedBtn_Click(object sender, RoutedEventArgs e)
        {
            var cardNumber = cardTextBox.Text;
            if (cardNumber.Length < 16 || string.IsNullOrEmpty(cardNumber))
            {
                cardTextBox.Clear();
                MessageBox.Show("Card number must be 16 digit long", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var account = _ATMservice.FindCard(cardNumber);
            if(account == null)
            {
                cardTextBox.Clear();
                MessageBox.Show("Card doesn`t exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _account = account;
            TogglePages();

        }

        private void VerifyPINBtn_Click(object sender, RoutedEventArgs e)
        {
            var pin = ConvertSecureStringToString(PINTextBox.SecurePassword);
            if (pin.Length < 4 || string.IsNullOrEmpty(pin))
            {
                PINTextBox.Clear();
                MessageBox.Show("PIN code must be 4 digit long", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(!_ATMservice.CheckPin(pin, _account))
            {
                PINTextBox.Clear();
                MessageBox.Show("Incorrect PIN", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.ChangeFrameContent(new CardMenuPage(_account));
            }
        }

        private void TogglePages()
        {
            cardNumberInput.Visibility = cardNumberInput.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
            PINInput.Visibility = PINInput.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
        }

        private string ConvertSecureStringToString(SecureString secureString)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }
    }
}
