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
using Microsoft.Win32;
using System.IO;
using Path = System.IO.Path;

namespace WpfForrat.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private readonly Day6RubForContext _db = new();

        public Tovar Tovar { get; set; }

        public List<Categoty> Categories { get; set; }
        public List<EdIzm> EdIzms { get; set; }
        public List<Postavshik> Postavshiki { get; set; }
        public List<Proizvoditel> Proizvoditeli { get; set; }

        private bool _isEdit;

        private string? _selectedPhotoPath;
        public AddEditPage(Tovar? tovar = null)
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                Window.GetWindow(this).Title = "Страница добавления/редактирования товаров";
            };
            Categories = _db.Categoties.ToList();
            EdIzms = _db.EdIzms.ToList();
            Postavshiki = _db.Postavshiks.ToList();
            Proizvoditeli = _db.Proizvoditels.ToList();

            if (tovar == null)
            {
                IdTextBlock.Visibility = Visibility.Collapsed;
                _isEdit = false;
                Tovar = new Tovar
                {
                    IdTovar = _db.Tovars.Any() ? _db.Tovars.Max(t => t.IdTovar) + 1 : 1,
                    Articul = "",
                    NameTovar = "",
                    Opisanie = "",
                    Money = 0,
                    Sale = 0,
                    KolSklad = 0,
                    Fhoto = "picture.jpg",
                    IdCategory = Categories.First().IdCategory,
                    IdEdIzm = EdIzms.First().IdEdIzm,
                    IdPost = Postavshiki.First().IdPost,
                    IdProizvod = Proizvoditeli.First().IdProizvod
                };
            }
            else
            {
                IdTextBlock.Visibility = Visibility.Visible;
                _isEdit = true;
                Tovar = _db.Tovars.First(t => t.IdTovar == tovar.IdTovar);
                if (!string.IsNullOrWhiteSpace(Tovar.Fhoto))
                {
                    Tovar.Fhoto = $"..\\Images\\{Tovar.Fhoto}";
                }
                else
                {
                    Tovar.Fhoto = $"..\\Images\\picture1.png";
                }
            }

            DataContext = this;
        }

        private void SaveButton(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Tovar.Articul))
            {
                MessageBox.Show("Введите артикул товара.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(Tovar.NameTovar))
            {
                MessageBox.Show("Введите наименование товара.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(Tovar.Opisanie))
            {
                MessageBox.Show("Введите описание товара.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Tovar.Money < 0)
            {
                MessageBox.Show("Цена не может быть отрицательной.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Tovar.KolSklad < 0)
            {
                MessageBox.Show("Количество на складе не может быть отрицательным.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Tovar.Sale < 0 || Tovar.Sale > 100)
            {
                MessageBox.Show("Скидка должна быть от 0 до 100.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Tovar.Money = Math.Round(Tovar.Money, 2);

            if (string.IsNullOrWhiteSpace(Tovar.Fhoto))
            {
                Tovar.Fhoto = "picture.jpg";
            }

            if (_selectedPhotoPath != null)
            {
                string imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");

                if (!Directory.Exists(imagesFolder))
                    Directory.CreateDirectory(imagesFolder);

                string fileName = Path.GetFileName(_selectedPhotoPath);
                string newPath = Path.Combine(imagesFolder, fileName);

                File.Copy(_selectedPhotoPath, newPath, true);

                Tovar.Fhoto = fileName;
            }

            if (!_isEdit)
                _db.Tovars.Add(Tovar);

            _db.SaveChanges();

            MessageBox.Show("Данные сохранены.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

            NavigationService.GoBack();
        
        }

        private void BackButton(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ChoosePhotoButton(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "фото товара|*.png;*.jpg;*.jpeg";

            if (dialog.ShowDialog() == true)
            {
                _selectedPhotoPath = dialog.FileName;
                ProductImage.Source = new BitmapImage(new Uri(_selectedPhotoPath));
            }
        }
    }
}
