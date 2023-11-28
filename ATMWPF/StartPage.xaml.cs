using System.Windows;
using System.Windows.Controls;

namespace ATMWPF
{
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        public StartPage()
        {
            InitializeComponent();
            
        }

        private void WithoutCardBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.ChangeFrameContent(new CardNumberInputPage());
        }

    }
}
