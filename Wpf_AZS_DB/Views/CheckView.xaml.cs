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

namespace Wpf_AZS_DB.Views
{
    /// <summary>
    /// Логика взаимодействия для CheckView.xamltxtCheck.Text
    /// </summary>
    public partial class CheckView : Window
    {
        private string myValue;
        public CheckView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in MyData.MyValue)
            {
                myValue += $"{item}\r\n";
            }
            txtCheck.Text = myValue;
            MyData.MyValue.Clear();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
