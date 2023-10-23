using Microsoft.IdentityModel.Tokens;
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

        if (string.IsNullOrWhiteSpace(SignInLoginBox.Text) && string.IsNullOrWhiteSpace(SignInPasswordBox.Text))
        {
            MessageBox.Show($"Пароль и логин должны быть заполнены!");
            return;
        }

        var context = new SkyDbContext();
        var user = context.Users.SingleOrDefault(x => x.Username == login && x.Password == password);
        if (user is not null)
        {
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


        if (string.IsNullOrWhiteSpace(SignUpLoginBox.Text) && string.IsNullOrWhiteSpace(SignUpPasswordBox.Text))
        {
            MessageBox.Show($"Пароль и логин должны быть заполнены!");
            return;
        }

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
