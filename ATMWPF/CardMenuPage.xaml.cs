using ATMLib;
using System.Windows;
using System.Windows.Controls;

namespace ATMWPF
{
    /// <summary>
    /// Interaction logic for CardMenuPage.xaml
    /// </summary>
    public partial class CardMenuPage : Page
    {
        private Account _account;
        private MainWindow _mainWindow = (MainWindow)Application.Current.MainWindow;
        public CardMenuPage(Account account)
        {
            InitializeComponent();
            _account = account;
            DataContext = account;
        }

        private void BalanceBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.ChangeFrameContent(new CardBalancePage(_account));
        }

        private void AddFundsBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.ChangeFrameContent(new AddFundsPage(_account));
        }

        private void CreateTransactionBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.ChangeFrameContent(new TransactionPage(_account));
        }

        private void WithdrawBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.ChangeFrameContent(new WithdrawPage(_account));
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.ChangeFrameContent(new StartPage());
        }
    }
}
