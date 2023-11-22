using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf_AZS_DB.Models;
using Wpf_AZS_DB.ViewModels;
using Wpf_AZS_DB.Views;

namespace Wpf_AZS_DB.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Глобальные переменные
        private int selectedIndex = 0;

        /// <summary>
        /// скидка в процентах
        /// </summary>
        private decimal AllDiscont = 0.05M;

        /// <summary>
        /// к оплате
        /// </summary>
        private decimal accuredTotal = 0.0M;

        /// <summary>
        /// итого за кафе
        /// </summary>
        private decimal accuredForCafe = 0.0M;

        /// <summary>
        /// объём бензина
        /// </summary>
        private int volumePatrol;

        /// <summary>
        /// цена бензина за литр
        /// </summary>
        private decimal pricePatrol = 0.0M;

        /// <summary>
        /// скидка в рублях
        /// </summary>
        private decimal discont = 0.0M;

        /// <summary>
        /// начислено
        /// </summary>
        private decimal accured = 0.0M;

        private decimal costOfPatrol = 0.0M;
        /// <summary>
        /// Продавец
        /// </summary>
        private string SellerUser;

        private int myVariable;
        private int myVariable2;

        //private bool IsTab = false;
        #endregion
        public MainWindowViewModel ViewModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainWindowViewModel();
            DataContext = ViewModel;
            Create_BD();
        }
        /// <summary>
        /// Создание БД и заполнение таблиц с проверками
        /// </summary>
        private void Create_BD()
        {
            string temp;
            SellerUser = MyData.Names;
            if (MyData.Isadmin)
            {
                temp = "Администратор";
                IaAdminPanel.IsEnabled = true;
                sellersBox.IsEnabled = true;
            }
            else { temp = "Продавец"; IaAdminPanel.IsEnabled = false; sellersBox.IsEnabled = false; }
            txtSeller.Text = $"{temp} {SellerUser}";
            txtSeller2.Text = $"{temp} {SellerUser}";
            ViewModel.ArchproductsObserv.Clear();
            LoadArchiv();
            LoadSellers();
        }

        /// <summary>
        /// Загрузка в DataGrid о статистике
        /// </summary>
        private void LoadArchiv()
        {
            using (ViewModels.AppContext db = new ViewModels.AppContext())
            {
                if (MyData.Isadmin)
                {
                    var tempUser = db.ArchiveSaleProductes.ToList();
                    foreach (var el in tempUser)//ViewModel.ArchproductsObserv
                    {
                        ViewModel.ArchproductsObserv.Add(new ArchiveSaleProducts
                        {
                            Name = el.Name,
                            Numbercheck = el.Numbercheck,
                            Count = el.Count,
                            Price = el.Price,
                            AllPrice = el.AllPrice,
                            Timecheck = el.Timecheck,
                            Seller = el.Seller
                        });
                    }
                }
                else
                {
                    var tempUser = db.ArchiveSaleProductes.ToList();
                    foreach (var el in tempUser)//ViewModel.ArchproductsObserv
                    {
                        if (el.Seller != SellerUser) continue;
                        ViewModel.ArchproductsObserv.Add(new ArchiveSaleProducts
                        {
                            Name = el.Name,
                            Numbercheck = el.Numbercheck,
                            Count = el.Count,
                            Price = el.Price,
                            AllPrice = el.AllPrice,
                            Timecheck = el.Timecheck,
                            Seller = el.Seller
                        });
                    }
                }
            }
        }

        /// <summary>
        /// Подключение ListBox к коллекции запросов
        /// </summary>
        private void LoadSellers()
        {
            query.ItemsSource = ViewModel.Query;
        }


        #region Вкладка Основная
        /// <summary>
        /// Метод формирования и вывода информации в разделе Оплата
        /// </summary>
        private void СompletionAllPrices()
        {
            decimal Prices = accuredForCafe;//decimal.Parse(TxtCafe.Text);//за кафе
            decimal Prices2 = decimal.Parse(TxtCena.Text);//за бензин
            accured = Prices + Prices2;
            discont = AllDiscont * accured;//скидка
            discont = Math.Round(discont, 2);//округление 
            if (accured < 1000) discont = 0;// условия скидки
            accuredTotal = accured - discont;//к оплате 
            if (accured == 0m) btnPay.IsEnabled = false;
            else btnPay.IsEnabled = true;
            TxtAllPrice.Text = accured.ToString();//вывод начислено
            Txtdiscount.Text = discont.ToString();//вывод скидки
            TxtAlldiscount.Text = accuredTotal.ToString();//вывод к оплате

        }

        /// <summary>
        /// Обрабока события нажатий кнопки вниз
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUP_Click(object sender, RoutedEventArgs e)
        {
            int temp = int.Parse(numericUpDown.Text);
            if (selectedIndex >= 0 && selectedIndex < ViewModel.productsObserv.Count)
            {
                Product product = ViewModel.productsObserv[selectedIndex];
                if (product.Count > 0)
                {
                    temp++;
                    numericUpDown.Text = temp.ToString();
                    product.Count = product.Count - 1;
                }
                var tempProduct = ViewModel.saleproductsObserv.FirstOrDefault(p => p.Name == product.Name);
                if (tempProduct != null)
                {
                    tempProduct.Count = temp;
                    tempProduct.AllPrice = temp * tempProduct.Price;
                }
                decimal totalCost = 0.0m;
                foreach (SaleProduct products in ViewModel.saleproductsObserv)
                {
                    totalCost += products.AllPrice;
                }
                accuredForCafe = totalCost;
                TxtCafe.Text = totalCost.ToString();

                СompletionAllPrices();
            }
        }

        /// <summary>
        /// Обрабока события нажатий кнопки вверх
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDOWN_Click(object sender, RoutedEventArgs e)
        {
            int temp = int.Parse(numericUpDown.Text);
            if (selectedIndex >= 0 && selectedIndex < ViewModel.productsObserv.Count)
            {
                Product product = ViewModel.productsObserv[selectedIndex];
                if (temp > 0)
                {
                    temp--;
                    numericUpDown.Text = temp.ToString();
                    product.Count = product.Count + 1;
                }
                var tempProduct = ViewModel.saleproductsObserv.FirstOrDefault(p => p.Name == product.Name);
                if (tempProduct != null)
                {
                    tempProduct.Count = temp;
                    tempProduct.AllPrice = temp * tempProduct.Price;
                }
                decimal totalCost = 0.0m;
                foreach (SaleProduct products in ViewModel.saleproductsObserv)
                {
                    totalCost += products.AllPrice;
                }
                accuredForCafe = totalCost;
                TxtCafe.Text = totalCost.ToString();
                СompletionAllPrices();
            }
        }

        /// <summary>
        /// Обрабока события на выбор строки в DataGrid  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedIndex = dataGrid.SelectedIndex;

            SPNumeric.IsEnabled = true;

            var selectedRow = dataGrid.SelectedItem as Product;

            if (selectedRow != null)
            {
                // Создать новый экземпляр объекта для второго DataGrid
                var newRow = new Product
                {
                    Name = selectedRow.Name,
                    Count = selectedRow.Count,
                    Price = selectedRow.Price
                    // Заполните остальные свойства значениями из выбранной строки
                };

                // Добавить новую строку во второй DataGrid

                int temp = int.Parse(numericUpDown.Text);

                if (!ViewModel.saleproductsObserv.Any(item => item.Name == newRow.Name))
                {
                    ViewModel.saleproductsObserv.Add(new SaleProduct { Name = newRow.Name, Count = 0, Price = newRow.Price, AllPrice = 0 });
                    numericUpDown.Text = "0";
                }
                else
                {
                    var tempProduct = ViewModel.saleproductsObserv.FirstOrDefault(p => p.Name == newRow.Name);
                    if (tempProduct != null)
                    {
                        temp = tempProduct.Count;
                        numericUpDown.Text = temp.ToString();
                    }
                }
            }
        }

        /// <summary>
        /// Обрабока события на выбор строки в DataGrid1 - купленных товаров
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Получить доступ к DataGrid

            var selectedRow = dataGrid1.SelectedItem as SaleProduct;

            // Проверить, что DataGrid не пустой
            if (dataGrid != null)
            {
                // Найти строку с именем "Иван"
                foreach (var item in dataGrid.Items)
                {
                    if (item is Product dataItem && dataItem.Name == selectedRow.Name)
                    {
                        // Выбрать найденную строку в DataGrid
                        dataGrid.SelectedItem = item;
                        dataGrid.ScrollIntoView(item);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Проверка введенного значения на цифры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBValues_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Проверка на введенный знак пробела
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBValues_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Обработка изменения значения в TextBlock Объём бензина
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBValues_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(txtBValues.Text, out int result)) myVariable = result;
            else myVariable = 0;

            if (Txtlitr == null) return;
            else
            {
                if (txtBValues.Text == "") txtBValues.Text = "0";
                //var tempTxtPrice = decimal.Parse(TxtPrice.Text);//цена бензина за литр
                var temptxtBValues = int.Parse(txtBValues.Text);//количество литров
                var tempItog = pricePatrol * temptxtBValues;//стоимость
                TxtCena.Text = tempItog.ToString();
                Txtlitr.Text = temptxtBValues.ToString();
                volumePatrol = temptxtBValues;
                СompletionAllPrices();
            }
        }

        /// <summary>
        /// Обработка изменения значения в TextBlock Сумма за бензин
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBPrices_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(txtBPrices.Text, out int result)) myVariable2 = result;
            else myVariable2 = 0;

            if (TxtCena == null) return;
            else
            {
                if (TxtCena.Text == "") TxtCena.Text = "0";
                //var tempTxtPrice = decimal.Parse(TxtPrice.Text);//цена бензина за литр 
                var temptxtBPrices = int.Parse(txtBPrices.Text);//Сумма 
                if (pricePatrol != 0)
                {
                    var tempItog = temptxtBPrices / pricePatrol;//количество литров
                    decimal floorNumber = Math.Round(tempItog, 2);
                    TxtCena.Text = temptxtBPrices.ToString();
                    Txtlitr.Text = floorNumber.ToString();
                    volumePatrol = (int)floorNumber;
                }


                СompletionAllPrices();
            }
        }


        /// <summary>
        /// Обработка выбора Radiobutton1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtn1_Checked(object sender, RoutedEventArgs e)
        {
            txtBValues.IsEnabled = true;
            txtBValues.Text = "0";
            txtBPrices.Text = "0";
            txtBPrices.IsEnabled = false;
        }


        /// <summary>
        /// Обработка выбора Radiobutton2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtn2_Checked(object sender, RoutedEventArgs e)
        {
            txtBValues.IsEnabled = false;
            txtBPrices.Text = "0";
            txtBValues.Text = "0";
            txtBPrices.IsEnabled = true;
        }

        /// <summary>
        /// Обработка кнопок с марками бензина
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ai92_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            var content = button.Content.ToString();
            pricePatrol = ViewModel.comboSource[content];
            TxtPrice.Text = pricePatrol.ToString();
            rbtn1.IsChecked = false;
            rbtn2.IsChecked = false;
            txtBPrices.Text = "0";
            txtBValues.Text = "0";
            txtBValues.IsEnabled = false;
            txtBPrices.IsEnabled = false;
        }


        /// <summary>
        /// Обработка кнопки оплатить, генерация чека
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            decimal PricePatrol = decimal.Parse(TxtCena.Text);//Итого за бензин
            //decimal ValuePricePatrol = decimal.Parse(TxtPrice.Text);//Значение марки выбранного бензина

            if (PricePatrol != 0)
            {
                var key = ViewModel.comboSource.FirstOrDefault(x => x.Value == pricePatrol).Key;
                string PatrolCheck = $"{key}  {pricePatrol} X {volumePatrol} = {PricePatrol} руб.";
                MyData.MyValue.Add(PatrolCheck);
            }
            if (accuredForCafe != 0)
            {
                foreach (var el in ViewModel.saleproductsObserv)
                {
                    if (el.Count != 0)
                    {
                        string PriceCafe = $"{el.Name}  {el.Price} X {el.Count} = {el.AllPrice} руб.";
                        MyData.MyValue.Add(PriceCafe);
                    }
                }
            }

            MyData.MyValue.Add("-------------------------");
            MyData.MyValue.Add($"Начислено: {accured} руб.");
            MyData.MyValue.Add($"Скидка 5%: {discont} руб.");
            MyData.MyValue.Add($"Итого: {accuredTotal} руб.");
            MyData.MyValue.Add("-------------------------");
            MyData.MyValue.Add($"Продавец: {SellerUser} ");
            MyData.MyValue.Add($"Время: {DateTime.Now} ");
            MyData.MyValue.Add($"Номер операции: {++MyData.NumCheck}");

            SaveArchiveSaleProducts();
            CheckView checkView = new CheckView();
            checkView.Show();
            ZeroForms();
            using (ViewModels.AppContext db = new ViewModels.AppContext())
            {
                var tempUser = db.Products.ToList();
                db.Products.RemoveRange(tempUser);
                db.SaveChanges();
                foreach (var item in ViewModel.productsObserv)
                {
                    Product product = new Product
                    {
                        Name = item.Name,
                        Count = item.Count,
                        Price = item.Price
                    };
                    db.Products.Add(product);
                }
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Метод сохранения купленного в архивную таблицу
        /// </summary>
        private void SaveArchiveSaleProducts()
        {
            var key = ViewModel.comboSource.FirstOrDefault(x => x.Value == pricePatrol).Key;
            using (ViewModels.AppContext db = new ViewModels.AppContext())
            {
                if (volumePatrol != 0)
                {
                    ArchiveSaleProducts archiveSaleProducts = new ArchiveSaleProducts
                    {
                        Name = key,
                        Price = pricePatrol,
                        Count = volumePatrol,
                        AllPrice = volumePatrol * pricePatrol,
                        Timecheck = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                        Seller = SellerUser,
                        Numbercheck = MyData.NumCheck
                    };
                    db.ArchiveSaleProductes.Add(archiveSaleProducts);
                    ViewModel.ArchproductsObserv.Add(new ArchiveSaleProducts
                    {
                        Name = key,
                        Numbercheck = MyData.NumCheck,
                        Count = volumePatrol,
                        Price = pricePatrol,
                        AllPrice = volumePatrol * pricePatrol,
                        Timecheck = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                        Seller = SellerUser
                    });
                }
                foreach (var el in ViewModel.saleproductsObserv)
                {
                    if (el.Count != 0)
                    {
                        ArchiveSaleProducts archiveSaleProducts = new ArchiveSaleProducts
                        {
                            Name = el.Name,
                            Price = el.Price,
                            Count = el.Count,
                            AllPrice = el.Price * el.Count,
                            Timecheck = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                            Seller = SellerUser,
                            Numbercheck = MyData.NumCheck
                        };
                        db.ArchiveSaleProductes.Add(archiveSaleProducts);

                        ViewModel.ArchproductsObserv.Add(new ArchiveSaleProducts
                        {
                            Name = el.Name,
                            Numbercheck = MyData.NumCheck,
                            Count = el.Count,
                            Price = el.Price,
                            AllPrice = el.AllPrice,
                            Timecheck = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                            Seller = SellerUser
                        });
                    }
                }
                db.SaveChanges();
            }
            //LoadArchiv();
        }

        /// <summary>
        /// Метод очистки после формирования чека
        /// </summary>
        private void ZeroForms()
        {
            TxtPrice.Text = "0";
            rbtn1.IsChecked = false;
            rbtn2.IsChecked = false;
            txtBValues.IsEnabled = false;
            txtBPrices.Text = "0";
            txtBValues.Text = "0";
            txtBPrices.IsEnabled = false;
            TxtAllPrice.Text = "0";
            Txtdiscount.Text = "0";
            TxtAlldiscount.Text = "0";
            ViewModel.saleproductsObserv.Clear();
            TxtCafe.Text = "0";
            selectedIndex = 0;
            AllDiscont = 0.05M;
            accuredTotal = 0.0M;
            accured = 0.0M;
            SPNumeric.IsEnabled = false;
            numericUpDown.Text = "0";
            accuredForCafe = 0.0M;
            discont = 0.0M;
            pricePatrol = 0.0M;
            volumePatrol = 0;
            accuredForCafe = 0.0M;
        }
#endregion

        #region Вкладка статистика


        /// <summary>
        /// Кнопка Сменить пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ZeroForms();
            this.Opacity = 0.5;
            InApps inApps = new InApps();
            inApps.ShowDialog();
            this.Close();
        }

        /// <summary>
        /// Обрабока события на выбор строки в DataGridStat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridStat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedIndex = dataGridStat.SelectedIndex;

            var selectedRow = dataGridStat.SelectedItem as Product;

            if (selectedRow != null)
            {
                // Создать новый экземпляр объекта для второго DataGrid
                var newRow = new Product
                {
                    Name = selectedRow.Name,
                    Count = selectedRow.Count,
                    Price = selectedRow.Price
                    // Заполните остальные свойства значениями из выбранной строки
                };
                txbName.Text = selectedRow.Name;
                txbCount.Text = selectedRow.Count.ToString();
                txbPrice.Text = selectedRow.Price.ToString();
            }
            else
            {
                txbName.Text = "---";
                txbCount.Text = "---";
                txbPrice.Text = "---";
            }
        }

        /// <summary>
        /// Обработка кнопки Добавить продукт
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string tempName = txbName.Text;
            int tempCount = int.Parse(txbCount.Text);
            decimal tempPrice = decimal.Parse(txbPrice.Text);
            if (!ViewModel.productsObserv.Any(item => item.Name == tempName))
            {
                ViewModel.productsObserv.Add(new Product { Name = tempName, Count = tempCount, Price = tempPrice });
                txbName.Text = "---";
                txbCount.Text = "---";
                txbPrice.Text = "---";
                using (ViewModels.AppContext db = new ViewModels.AppContext())
                {
                    db.Products.Add(new Product
                    {
                        Name = tempName,
                        Count = tempCount,
                        Price = tempPrice

                    });
                    db.SaveChanges();
                }
            }
            else
            {
                MessageBox.Show("Нельзя добавить существующий продукт!");
            }
        }

        /// <summary>
        /// Обработка кнопки Обновить продукт
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnlUpdate_Click(object sender, RoutedEventArgs e)
        {
            string tempName = txbName.Text;
            int tempCount = int.Parse(txbCount.Text);
            decimal tempPrice = decimal.Parse(txbPrice.Text);
            if ((ViewModel.productsObserv.Any(item => item.Name == tempName)) || (tempName != "---"))
            {

                using (ViewModels.AppContext db = new ViewModels.AppContext())
                {
                    var product = db.Products.FirstOrDefault(p => p.Name == tempName);

                    if (product != null)
                    {
                        // обработка результата
                        product.Count = tempCount;
                        product.Price = tempPrice;
                        db.SaveChanges();
                        var ItemToUpdate = ViewModel.productsObserv.FirstOrDefault(item => item.Name == tempName);
                        if (ItemToUpdate != null)
                        {
                            // элемент найден
                            ItemToUpdate.Price = tempPrice;
                            ItemToUpdate.Count = tempCount;
                        }
                    }
                    else
                    {
                        // обработка отсутствия результата
                        MessageBox.Show("Такого продукта не существует!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Такого продукта в таблице не существует!");
            }
        }

        /// <summary>
        /// Обработка кнопки Удалить продукт
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            string tempName = txbName.Text;
            int tempCount = int.Parse(txbCount.Text);
            decimal tempPrice = decimal.Parse(txbPrice.Text);
            if (ViewModel.productsObserv.Any(item => item.Name == tempName))
            {
                int index = ViewModel.productsObserv.IndexOf(ViewModel.productsObserv.FirstOrDefault(item => item.Name == tempName));
                if (index != -1)
                {
                    ViewModel.productsObserv.RemoveAt(index);
                }
                else
                {
                    MessageBox.Show("Не могу удалить, та как такого продукта в таблице не существует!");
                }
                using (ViewModels.AppContext db = new ViewModels.AppContext())
                {
                    var product = db.Products.FirstOrDefault(p => p.Name == tempName);

                    if (product != null)
                    {
                        // обработка результата
                        db.Products.Remove(product);
                        db.SaveChanges();
                    }
                    else
                    {
                        // обработка отсутствия результата
                        MessageBox.Show("Такого продукта не существует!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Нет такого продукта в таблице !");
            }
        }

        /// <summary>
        /// Проверка на ввод только цифр и десятичной точки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9,]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// Проверка на ввод только цифр
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbCount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// Обработка события выбора вкладки на TabControl
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Получаем индекс выбранной вкладки
            int selectedIndex = tabControl1.SelectedIndex;

            // Выполняем действие в зависимости от выбранной вкладки
            switch (selectedIndex)
            {
                case 0:
                    // Действие для первой вкладки

                    break;
                case 1:
                    // Действие для второй вкладки

                    ZeroForms();
                    break;
                    // добавьте дополнительные case в зависимости от количества вкладок
            }
        }

        /// <summary>
        /// Обрабока события на выбор строки в DataGridSellers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridSellers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedIndex = dataGridSellers.SelectedIndex;

            var selectedRow = dataGridSellers.SelectedItem as Sellers;

            if (selectedRow != null)
            {
                // Создать новый экземпляр объекта для второго DataGrid
                var newRow = new Sellers
                {
                    Name = selectedRow.Name,
                    Login = selectedRow.Login,
                    Password = selectedRow.Password,
                    IsAdmin = selectedRow.IsAdmin
                    // Заполните остальные свойства значениями из выбранной строки
                };
                txbNameS.Text = selectedRow.Name;
                txbLoginS.Text = selectedRow.Login.ToString();
                txbPassS.Text = selectedRow.Password.ToString();
                txbAdminS.Text = selectedRow.IsAdmin.ToString();
            }
            else
            {
                txbNameS.Text = "---";
                txbLoginS.Text = "---";
                txbPassS.Text = "---";
                txbAdminS.Text = "---";
            }
        }

        /// <summary>
        /// Обработка кнопки Добавить сотрудника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddS_Click(object sender, RoutedEventArgs e)
        {
            bool tempAdmin = false;
            string tempName = txbNameS.Text;
            string tempLogin = txbLoginS.Text;
            string tempPass = txbPassS.Text;
            if (txbAdminS.Text == "True" || txbAdminS.Text == "False")
            { tempAdmin = bool.Parse(txbAdminS.Text); }
            else { MessageBox.Show($" {tempLogin} - не верно! Выберите True или False"); }
            if (!ViewModel.sellers.Any(item => item.Login == tempLogin))
            {
                ViewModel.sellers.Add(new Sellers
                {
                    Name = tempName,
                    Login = tempLogin,
                    Password = tempPass,
                    IsAdmin = tempAdmin
                });
                txbNameS.Text = "---";
                txbLoginS.Text = "---";
                txbPassS.Text = "---";
                txbAdminS.Text = "---";
                using (ViewModels.AppContext db = new ViewModels.AppContext())
                {
                    db.Sellerses.Add(new Sellers
                    {
                        Name = tempName,
                        Login = tempLogin,
                        Password = tempPass,
                        IsAdmin = tempAdmin

                    });
                    db.SaveChanges();
                }
            }
            else
            {
                MessageBox.Show($"Сотрудник с логином {tempLogin} существует!");
            }
        }

        /// <summary>
        /// Обработка кнопки Обновить сотрудника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnlUpdateS_Click(object sender, RoutedEventArgs e)
        {
            bool tempAdmin = false;
            string tempName = txbNameS.Text;
            string tempLogin = txbLoginS.Text;
            string tempPass = txbPassS.Text;
            if (txbAdminS.Text == "True" || txbAdminS.Text == "False")
            { tempAdmin = bool.Parse(txbAdminS.Text); }
            else { MessageBox.Show($" {tempLogin} - не верно! Выберите True или False"); }
            if ((!ViewModel.sellers.Any(item => item.Login == tempLogin)) || (tempLogin != "---"))
            {

                using (ViewModels.AppContext db = new ViewModels.AppContext())
                {
                    var seller = db.Sellerses.FirstOrDefault(p => p.Login == tempLogin);

                    if (seller != null)
                    {
                        // обработка результата
                        seller.Name = tempName;
                        seller.Login = tempLogin;
                        seller.Password = tempPass;
                        seller.IsAdmin = tempAdmin;
                        db.SaveChanges();
                        var ItemToUpdate = ViewModel.sellers.FirstOrDefault(item => item.Login == tempLogin);
                        if (ItemToUpdate != null)
                        {
                            // элемент найден
                            ItemToUpdate.Name = tempName;
                            ItemToUpdate.Login = tempLogin;
                            ItemToUpdate.Password = tempPass;
                            ItemToUpdate.IsAdmin = tempAdmin;
                        }
                    }
                    else
                    {
                        // обработка отсутствия результата
                        MessageBox.Show("Такого сотрудника не существует!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Такого сотрудника в таблице не существует!");
            }
        }

        /// <summary>
        /// Обработка кнопки Удалить сотрудника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelS_Click(object sender, RoutedEventArgs e)
        {
            string tempName = txbNameS.Text;
            string tempLogin = txbLoginS.Text;
            string tempPass = txbPassS.Text;
            bool tempAdmin = bool.Parse(txbAdminS.Text);
            if (ViewModel.sellers.Any(item => item.Login == tempLogin))
            {
                int index = ViewModel.sellers.IndexOf(ViewModel.sellers.FirstOrDefault(item => item.Login == tempLogin));
                if (index != -1)
                {
                    ViewModel.sellers.RemoveAt(index);
                }
                else
                {
                    MessageBox.Show("Не могу удалить, та как такого сотрудника в таблице не существует!");
                }
                using (ViewModels.AppContext db = new ViewModels.AppContext())
                {
                    var seller = db.Sellerses.FirstOrDefault(p => p.Login == tempLogin);

                    if (seller != null)
                    {
                        // обработка результата
                        db.Sellerses.Remove(seller);
                        db.SaveChanges();
                    }
                    else
                    {
                        // обработка отсутствия результата
                        MessageBox.Show("Такого сотрудника не существует!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Нет такого сотрудника в таблице !");
            }
        }

        /// <summary>
        /// Обработка события выбора строки в ListBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void query_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = query.SelectedIndex;
            // Используем индекс для выполнения нужных операций
            switch (selectedIndex)
            {
                case 0: MessageBox.Show("Первый запрос!"); break;
                case 1: MessageBox.Show("Второй запрос!"); break;
                case 2: MessageBox.Show("Третий запрос!"); break;
                default:
                    break;
            }
        }
        #endregion

    }
}
