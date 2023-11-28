using ATMLib;
using System.Windows;
using System.Windows.Controls;

namespace ATMWPF
{
    /// <summary>
    /// Interaction logic for AddFundsPage.xaml
    /// </summary>
    public partial class AddFundsPage : Page
    {
        private MainWindow _mainWindow = (MainWindow)Application.Current.MainWindow;
        private Account _account;
        private ATM _ATMService = new();
        public AddFundsPage(Account account)
        {
            InitializeComponent();
            _account = account;
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

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.ChangeFrameContent(new CardMenuPage(_account));
        }

        private void AddFunds_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ValueTextBox.Text))
            {
                return;
            }
            var value = int.Parse(ValueTextBox.Text);

            _ATMService.AddFunds(value, _account);

            _mainWindow.ChangeFrameContent(new CardMenuPage(_account));
        }
    }
}
