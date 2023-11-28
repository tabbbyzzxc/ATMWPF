using System.Windows;
using System.Windows.Controls;

namespace ATMWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            mainFrame.Content = new StartPage();

        }

        public void ChangeFrameContent(Page page)
        {
            mainFrame.Content = page;
        }
    }
}
