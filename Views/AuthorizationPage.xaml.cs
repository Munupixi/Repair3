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

namespace Repair3.Views
{
    public partial class AuthorizationPage : Page
    {
        public AuthorizationPage()
        {
            InitializeComponent();
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            Repair3Context context = new Repair3Context();
            User? user = context.Users.FirstOrDefault(u => u.Login == LoginTextBox.Text);
            if (user == null)
            {
                MessageBox.Show("Пользователь не найден");
            }
            else if (user.Password != PasswordTextBox.Text)
            {
                MessageBox.Show("Пароль не подходит");
            }
            else
            {
                MessageBox.Show("Авторизация прошла успешно");
                User.ActiveUser = user;
                MainWindow.Frame.Content = new ViewRequests();
            }
        }
    }
}
