using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Navigation;
using WpfForrat.Models;
using System.IO;

namespace WpfForrat.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private readonly Day6RubForContext _db = new Day6RubForContext();
        public ObservableCollection<Tovar> Tovars { get; set; } = new();
        public User CurrentUser { get; set; }
        public Tovar Tovar { get; set; }

        public CollectionView view;
        public List<Postavshik> GroupNameList { get; set; } = new();
        public List<Tovar> ListProduct { get; set; } = new();
        
        public string SearchText { get; set; }
        public MainPage(User user=null)
        {
            InitializeComponent();
            DataContext = this;
            CurrentUser = user;
            GetAll();
            Loaded += (s, e) => GetAll();
            Loaded += (s, e) =>
            {
                Window.GetWindow(this).Title = "Страница товаров";
            };

        }
        public void GetAll()
        {
            _db.ChangeTracker.Clear(); // Обновление данных из БД
            Tovars.Clear();
            var tovars = _db.Tovars
             .Include(m => m.IdCategoryNavigation)
             .Include(m => m.IdProizvodNavigation)
             .Include(m => m.IdPostNavigation)
             .Include(m => m.IdEdIzmNavigation)
             .ToList();

            foreach (var all in tovars)
            {
                if (!string.IsNullOrWhiteSpace(all.Fhoto))
                {
                    all.Fhoto = $"..\\Images\\{all.Fhoto}";
                }
                else
                {
                    all.Fhoto = $"..\\Images\\picture1.png";
                }


                all.PriceWithDiscount = all.Money - (all.Money * all.Sale / 100);
                Tovars.Add(all);
                _db.ChangeTracker.Clear();

            }
            var postList = _db.Postavshiks.ToList();

            GroupNameList.Clear();

            GroupNameList.Add(new Postavshik
            {
                IdPost = -1,
                Name = "Все поставщики"
            });

            foreach (var post in postList)
            {
                GroupNameList.Add(post);
            }

            view = (CollectionView)CollectionViewSource.GetDefaultView(Tovars);

        }


        private void AddClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditPage());
        }

        private void DeleteCLick(object sender, RoutedEventArgs e)
        {
            if (TovarListView.SelectedItem is not Tovar selectedTovar)
            {
                MessageBox.Show("Выберите товар.");
                return;
            }
            if (_db.SostavZakazs.Any(x => x.IdTovar == selectedTovar.IdTovar))
            {
                MessageBox.Show("Нельзя удалить товар, который есть в заказе.");
                return;
            }
            if (MessageBox.Show("Удалить товар?", "Подтверждение", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;

            var tovar = _db.Tovars.First(x => x.IdTovar == selectedTovar.IdTovar);
            _db.Tovars.Remove(tovar);
            _db.SaveChanges();

            GetAll();
        }

      
     
        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CurrentUser?.IdRole != 1)
                return;
            if (TovarListView.SelectedItem is Tovar selectedTovar)
            {
                NavigationService.Navigate(new AddEditPage(selectedTovar));
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = (ComboBox)sender;
            var selected = cb.SelectedItem as Postavshik;
            if (selected == null || selected.IdPost == -1)
            {
                view.Filter = null;
            }
            else
            {
                view.Filter = obj =>
                {
                    if (obj is Tovar product)
                    {
                        return product.IdPostNavigation.IdPost == selected.IdPost;
                    }
                    return false;
                };
            }

            view.Refresh();
        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            var cb = (ComboBox)sender;
            var selected = (ComboBoxItem)cb.SelectedItem;
            view.SortDescriptions.Clear();

            switch (selected.Tag)
            {
                case "Desc":
                    view.SortDescriptions.Add( new SortDescription("KolSklad", ListSortDirection.Descending));
                    break;

                case "Asc":
                    view.SortDescriptions.Add(new SortDescription("KolSklad", ListSortDirection.Ascending));
                    break;
            }

            view.Refresh();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            view.Filter = obj =>
            {
                if (obj is Tovar product)
                {
                    string search = SearchText.ToLower();
                    return
                        product.NameTovar.ToLower().Contains(search)
                        || product.Articul.ToLower().Contains(search)
                        || product.Opisanie.ToLower().Contains(search)
                        || product.IdCategoryNavigation.Category.ToLower().Contains(search)
                        || product.IdPostNavigation.Name.ToLower().Contains(search)
                        || product.IdProizvodNavigation.Name.ToLower().Contains(search);
                }
                return false;
            };

            view.Refresh();
        }

        private void ZakaziClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ZakaziPage(CurrentUser));
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
