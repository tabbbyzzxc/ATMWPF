using ATMLib;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for WithdrawPage.xaml
    /// </summary>
    public partial class WithdrawPage : Page
    {
        private MainWindow _mainWindow = (MainWindow)Application.Current.MainWindow;
        private Account _account;
        private CardService _cardService = new CardService();

        public WithdrawPage(Account account)
        {
            InitializeComponent();
            _account = account;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.ChangeFrameContent(new CardMenuPage(_account));
        }

        private void WithdrawBtn_Click(object sender, RoutedEventArgs e)
        {

            var value = ((Button)sender).Content.ToString();
            var numericValue = int.Parse(value.Substring(0, 3));
            if (!_cardService.Withdraw(numericValue, _account))
            {
                MessageBox.Show("Insufficient funds", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            _mainWindow.ChangeFrameContent(new CardMenuPage(_account));
        }

        private void SwitchCustomValue_Click(object sender, RoutedEventArgs e)
        {
            OptionsContent.Visibility = OptionsContent.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
            CustomValue.Visibility = CustomValue.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible; 
        }

        private void CustomWithDraw_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ValueTextBox.Text))
            {
                return;
            }
            var value = int.Parse(ValueTextBox.Text);
            if (!_cardService.Withdraw(value, _account))
            {
                MessageBox.Show("Insufficient funds", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            _mainWindow.ChangeFrameContent(new CardMenuPage(_account));
        }
    }
}
