using SkyWalker.Data;
using SkyWalker.Models;
using SkyWalker.Windows;
using System.Linq;
using System.Windows;

namespace SkyWalker;

public partial class MainWindow : Window
{
    public MainWindow() => InitializeComponent();

    private void AuthClick(object sender, RoutedEventArgs e)
    {
        string login = SignInLoginBox.Text;
        string password = SignInPasswordBox.Text;

        var context = new SkyDbContext();
        var user = context.Users.SingleOrDefault(x => x.Username == login && x.Password == password);
        if (user is not null)
        {
            MessageBox.Show($"Вы успешно вошли в аккаунт {user.Username}");

            Cabinet cab = new Cabinet(user);
            cab.Show();
            this.Hide();

            return;
        }

        MessageBox.Show($"Логин или пароль неверные!");
    }

    private void RegisterClick(object sender, RoutedEventArgs e)
    {
        string login = SignUpLoginBox.Text;
        string password = SignUpPasswordBox.Text;

        var user = new User 
        {
            Username = login,
            Password = password
        };

        var context = new SkyDbContext();
        context.Users.Add(user);
        context.SaveChanges();
    }
}
