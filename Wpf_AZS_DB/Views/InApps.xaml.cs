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
using System.Windows.Shapes;
using Wpf_AZS_DB.Models;
using Wpf_AZS_DB.ViewModels;

namespace Wpf_AZS_DB.Views
{
    /// <summary>
    /// Логика взаимодействия для InApps.xaml
    /// </summary>
    public partial class InApps : Window
    {
        public MainWindowViewModel ViewModel { get; set; }
        List<Sellers> tempUser;
        public InApps()
        {
            InitializeComponent();
            ViewModel = new MainWindowViewModel();
            DataContext = ViewModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (ViewModels.AppContext db = new ViewModels.AppContext())
            {
                int count = db.Sellerses.Count();
                if (count == 0)
                {
                    foreach (var item in ViewModel.sellers)
                    {
                        db.Sellerses.Add(item);
                    }
                    //Сохранение изменений в БД
                    db.SaveChanges();

                }
                tempUser = db.Sellerses.ToList();
                var numberCheck = db.ArchiveSaleProductes
                .OrderByDescending(p => p.Numbercheck)
                .Select(p => p.Numbercheck)
                .FirstOrDefault();
                MyData.NumCheck = numberCheck;
            }
        }

        private void btnIn_Click(object sender, RoutedEventArgs e)
        {
            var users = tempUser;
            string us = userBox.Text;
            string pas = pasBox.Password;
            // Поиск пользователя по логину и паролю
            var user = users.FirstOrDefault(u => u.Login == us && u.Password == pas);

            // Если пользователь найден, выводим его имя
            if (user != null)
            {
                // MessageBox.Show("Имя пользователя: " + user.Name);
                MyData.Names = user.Name;
                MyData.Isadmin = user.IsAdmin;

                MainWindow window = new MainWindow();
                window.Show();

                this.Close();
            }
            else
            {
                MessageBox.Show("Пользователь не найден");
            }
        }
    }
}
