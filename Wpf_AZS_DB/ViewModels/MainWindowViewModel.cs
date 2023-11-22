using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Wpf_AZS_DB.Models;

namespace Wpf_AZS_DB.ViewModels
{
    public class MainWindowViewModel : BaseNotify
    {
        /// <summary>
        /// коллекция продавцов
        /// </summary>
        public ObservableCollection<Sellers> sellers { get; set; }
        private Sellers? selectedsellers;
        public Sellers Selectedsellers
        {
            get => selectedsellers;
            set
            {
                selectedsellers = value;
                OnPropertyChanged("SelectedProduct");
            }
        }

        /// <summary>
        /// Коллекция товаров на витрине
        /// </summary>
        public ObservableCollection<Product>? productsObserv { get; set; }

        /// <summary>
        /// выбранный товар
        /// </summary>
        private Product? selectedProduct;

        /// <summary>
        /// Свойство выбранный товар
        /// </summary>
        public Product SelectedProduct
        {
            get => selectedProduct;
            set
            {
                selectedProduct = value;
                OnPropertyChanged("SelectedProduct");
            }
        }

        public ObservableCollection<ArchiveSaleProducts>? ArchproductsObserv { get; set; }
        private ArchiveSaleProducts? archselectedProduct;
        public ArchiveSaleProducts ArchselectedProduct
        {
            get => archselectedProduct;
            set
            {
                archselectedProduct = value;
                OnPropertyChanged("ArchselectedProduct");
            }
        }


        /// <summary>
        /// коллекция купленного товара
        /// </summary>
        public ObservableCollection<SaleProduct>? saleproductsObserv { get; set; }

        /// <summary>
        /// выбранный из купленного товар
        /// </summary>
        private SaleProduct? saleselectedProduct;

        /// <summary>
        /// Свойство выбранный из купленного товар
        /// </summary>
        public SaleProduct SaleSelectedProduct
        {
            get => saleselectedProduct;
            set
            {
                saleselectedProduct = value;
                OnPropertyChanged("SaleSelectedProduct");
            }
        }

        /// <summary>
        /// коллекция марок бензина
        /// </summary>
        public Dictionary<string, decimal> comboSource { get; set; }

        public ObservableCollection<string>? Query { get; set; }


        /// <summary>
        /// конструктор MainWindowViewModel
        /// </summary>
        public MainWindowViewModel()
        {
            using (ViewModels.AppContext db = new ViewModels.AppContext())
            {

                int count = db.Products.Count();
                if (count == 0)
                {
                    productsObserv = new ObservableCollection<Product>
                    {
                        new Product{Name = "Хот-Дог", Count = 1, Price = 70.10M},
                        new Product{Name = "Гамбургер", Count = 6, Price = 85.70M},
                        new Product{Name = "Кока кола", Count = 4, Price = 40.30M},
                        new Product{Name = "Чизбургер", Count = 28, Price = 86.40M},
                        new Product{Name = "Картофель Фри", Count = 29, Price = 55.30M},
                        new Product{Name = "Чай", Count = 30, Price = 20.10M}

                    };
                    sellers = new ObservableCollection<Sellers>
                    {
                        new Sellers{Name="Александр", Login="Agreat", Password="12345",IsAdmin=true },
                        new Sellers{Name="Анна", Login="Anuta", Password="67890",IsAdmin=false },
                        new Sellers{Name="User", Login="user", Password="user",IsAdmin=false }
                    };

                }
                else
                {
                    var tempProduct = db.Products.ToList();
                    productsObserv = new ObservableCollection<Product>();
                    foreach (var item in tempProduct)
                    {
                        Product product = new Product { Name = item.Name, Price = item.Price, Count = item.Count };
                        productsObserv.Add(product);
                    }

                    var tempUser = db.Sellerses.ToList();
                    sellers = new ObservableCollection<Sellers>();
                    foreach (var el in tempUser)
                    {
                        Sellers seller = new Sellers
                        {
                            Name = el.Name,
                            Login = el.Login,
                            Password = el.Password,
                            IsAdmin = el.IsAdmin
                        };
                        sellers.Add(seller);
                    }
                }

            }

            saleproductsObserv = new ObservableCollection<SaleProduct> { };
            comboSource = new Dictionary<string, decimal>();
            comboSource.Add("АИ-76", 45.20M);
            comboSource.Add("АИ-92", 47.70M);
            comboSource.Add("АИ-95", 50.40M);
            comboSource.Add("АИ-100", 55.80M);



            ArchproductsObserv = new ObservableCollection<ArchiveSaleProducts>();

            Query = new ObservableCollection<string> { "Выбрать всю таблицу", "Item 2", "Item 3" };
        }



    }
}

