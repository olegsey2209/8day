using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для ZakaziPage.xaml
    /// </summary>
    public partial class ZakaziPage : Page
    {
        private readonly Day6RubForContext _db = new Day6RubForContext();
        public ObservableCollection<Zakaz> Zakazs { get; set; } = new();
        public User CurrentUser { get; set; }
        public ZakaziPage(User user)
        {
            InitializeComponent();
            CurrentUser = user;
            DataContext = this;
            GetAll();
            Loaded += (s, e) => GetAll();
            Loaded += (s, e) =>
            {
                Window.GetWindow(this).Title = "Заказы";
            };
        }
        public void GetAll()
        {
            _db.ChangeTracker.Clear();
            Zakazs.Clear();
            var zakazs = _db.Zakazs
                .Include(z => z.IdStatusNavigation)
                .Include(z => z.IdAdresNavigation)
                .Include(z => z.IdUserNavigation)
                .ToList();

            foreach (var zakaz in zakazs)
            {
                Zakazs.Add(zakaz);
            }

        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditZakazi());
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            if (ZakazListView.SelectedItem is not Zakaz selectedZakaz)
            {
                MessageBox.Show("Выберите заказ для удаления", "Удаление заказа", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить заказ?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                _db.ChangeTracker.Clear();

                var sostav = _db.SostavZakazs
                    .Where(s => s.IdZakaz == selectedZakaz.IdZakaz)
                    .ToList();

                _db.SostavZakazs.RemoveRange(sostav);

                var zakaz = _db.Zakazs
                    .FirstOrDefault(z => z.IdZakaz == selectedZakaz.IdZakaz);

                if (zakaz == null)
                {
                    MessageBox.Show("Заказ не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                _db.Zakazs.Remove(zakaz);
                _db.SaveChanges();

                GetAll();

                MessageBox.Show("Заказ удален", "Удаление заказа", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch 
            {
                MessageBox.Show("Не удалось удалить заказ" , "Ошибка удаления", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        
        }

        private void ZakazListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CurrentUser?.IdRole != 1)
                return;
            if (ZakazListView.SelectedItem is Zakaz selectedZakaz)
            {
                NavigationService.Navigate(new AddEditZakazi(selectedZakaz));
            }
        }
    }
}
