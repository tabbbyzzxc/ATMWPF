using ATMLib;
using System.Windows;
using System.Windows.Controls;

namespace ATMWPF
{
    /// <summary>
    /// Interaction logic for WithdrawPage.xaml
    /// </summary>
    public partial class WithdrawPage : Page
    {
        private MainWindow _mainWindow = (MainWindow)Application.Current.MainWindow;
        private Account _account;
        private ATM _ATMService = new();

        public WithdrawPage(Account account)
        {
            InitializeComponent();
            _account = account;
            _ATMService.ATMOperationHandler += _ATMService_ATMOperationHandler;
        }

        private void _ATMService_ATMOperationHandler(object sender, ATMOperationArgs args)
        {
            if( args.IsSuccess == true)
            {
                MessageBox.Show(args.OperationMessage, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(args.OperationMessage, "Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.ChangeFrameContent(new CardMenuPage(_account));
        }

        private void WithdrawBtn_Click(object sender, RoutedEventArgs e)
        {
            var value = ((Button)sender).Content.ToString();
            var numericValue = int.Parse(value.Substring(0, 3));
            _ATMService.Withdraw(numericValue, _account);
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

            _ATMService.Withdraw(value, _account);

            
            _mainWindow.ChangeFrameContent(new CardMenuPage(_account));
        }

    }
}
