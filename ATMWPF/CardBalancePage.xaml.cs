using ATMLib;
using System.Windows;
using System.Windows.Controls;

namespace ATMWPF
{
    /// <summary>
    /// Interaction logic for CardBalancePage.xaml
    /// </summary>
    public partial class CardBalancePage : Page
    {
        private MainWindow _mainWindow = (MainWindow)Application.Current.MainWindow;
        private Account _account;
        public CardBalancePage(Account account)
        {
            InitializeComponent();
            DataContext = account;
            _account = account;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.ChangeFrameContent(new CardMenuPage(_account));
        }
    }
}
