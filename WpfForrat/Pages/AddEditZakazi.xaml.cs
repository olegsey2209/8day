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
    /// Логика взаимодействия для AddEditZakazi.xaml
    /// </summary>
    /// using System;
    
    public partial class AddEditZakazi : Page
    {
        private readonly Day6RubForContext _db = new Day6RubForContext();
        public Zakaz Zakaz { get; set; }
        public List<Status> Statuses { get; set; }
        public List<PunktVidachi> PunktVidachis { get; set; }
        public DateTime? DateZakazPicker { get; set; }
        public DateTime? DateDostavPicker { get; set; }
        public List<User> Users { get; set; }
        
        private bool _isEdit;
        public AddEditZakazi(Zakaz? zakaz = null)
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                Window.GetWindow(this).Title = "Страница добавления и редактирования заказов";
            };
            Statuses = _db.Statuses.ToList();
            Users = _db.Users.ToList();
            PunktVidachis = _db.PunktVidachis.ToList();

            if (zakaz == null)
            {
                _isEdit = false;
                Zakaz = new Zakaz
                {
                    IdZakaz = _db.Zakazs.Any() ? _db.Zakazs.Max(z => z.IdZakaz) + 1 : 1
                   
                };
                DateZakazPicker = DateTime.Today;
                DateDostavPicker = DateTime.Today;
            }
            else
            {
                _isEdit = true;
                Zakaz = _db.Zakazs.First(z => z.IdZakaz == zakaz.IdZakaz);
                DateZakazPicker = Zakaz.DateZakaz.ToDateTime(TimeOnly.MinValue);
                DateDostavPicker = Zakaz.DateDostav.ToDateTime(TimeOnly.MinValue);
            }
           
            DataContext = this;
        }

        private void BackButton(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (Zakaz.IdUser == 0)
            {
                MessageBox.Show("Выберите клиента.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Zakaz.IdStatus == 0)
            {
                MessageBox.Show("Выберите статус заказа.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Zakaz.IdAdres == 0)
            {
                MessageBox.Show("Выберите адрес пункта выдачи.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (DateZakazPicker == null)
            {
                MessageBox.Show("Выберите дату заказа.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (DateDostavPicker == null)
            {
                MessageBox.Show("Выберите дату выдачи.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (DateDostavPicker < DateZakazPicker)
            {
                MessageBox.Show("Дата выдачи не может быть раньше даты заказа.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Zakaz.DateZakaz = DateOnly.FromDateTime(DateZakazPicker.Value);
            Zakaz.DateDostav = DateOnly.FromDateTime(DateDostavPicker.Value);

            if (!_isEdit)
                _db.Zakazs.Add(Zakaz);

            _db.SaveChanges();

            MessageBox.Show("Данные сохранены.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

            NavigationService.GoBack();
        
    }
    }
}
