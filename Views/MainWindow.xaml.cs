using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Repair3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Frame Frame;
        public MainWindow()
        {
            InitializeComponent();
            RepairContext repairContext = new RepairContext();
            Frame = MainFrame;
            Frame.Content = new AuthorizationPage();
        }
    }
}