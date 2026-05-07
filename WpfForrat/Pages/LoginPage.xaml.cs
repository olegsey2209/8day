using Microsoft.EntityFrameworkCore;
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
using WpfForrat.Models;

namespace WpfForrat.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private readonly Day6RubForContext _db = new Day6RubForContext();
        public string Login { get; set; }
        public string Password { get; set; }
        public LoginPage()
        {
            InitializeComponent();
            DataContext = this;
            Loaded += (s, e) =>
            {
                Window.GetWindow(this).Title = "Страница входа";
            };
        }

        private void VhodPolzovatel(object sender, RoutedEventArgs e)
        {
            var user = _db.Users.FirstOrDefault(p => p.Login == Login);

            if (user != null)
            {
                if (user.Password == Password)
                {
                    NavigationService.Navigate(new MainPage(user));
                   
                }
                else MessageBox.Show("Неверный пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else MessageBox.Show("Пользователь не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        private void VhodGost(object sender, RoutedEventArgs e)
        {
            var guest = new User
            {
                IdRole = 4,
                Fio = "Гость"

            };
            NavigationService.Navigate(new MainPage(guest));
        }
    }
}
