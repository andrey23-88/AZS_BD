using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf_AZS_DB;

namespace Wpf_AZS_DB.Models
{
    /// <summary>
    /// Класс проданных продуктов
    /// </summary>
    public class ArchiveSaleProducts : BaseNotify
    {
        /// <summary>
        /// Id проданного продукта
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Имя проданного продукта
        /// </summary>
        private string? name;
        /// <summary>
        /// Количество проданногопродукта
        /// </summary>
        private int count;
        /// <summary>
        /// Цена проданного продукта
        /// </summary>
        private decimal price;
        /// <summary>
        /// Стоимость проданного продукта
        /// </summary>
        private decimal allprice;
        /// <summary>
        /// Время продажи
        /// </summary>
        private string timecheck;
        /// <summary>
        /// Имя продавца
        /// </summary>
        private string? seller;
        /// <summary>
        /// Номер операции
        /// </summary>
        public int numbercheck;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public int Count
        {
            get { return count; }
            set
            {
                count = value;
                OnPropertyChanged("Count");
            }
        }
        public decimal Price
        {
            get { return price; }
            set
            {
                price = value;
                OnPropertyChanged("Price");
            }
        }

        public decimal AllPrice
        {
            get { return allprice; }
            set
            {
                allprice = value;
                OnPropertyChanged("AllPrice");
            }
        }

        public string Timecheck
        {
            get { return timecheck; }
            set
            {
                timecheck = value;
                OnPropertyChanged("Timecheck");
            }
        }

        public string? Seller
        {
            get { return seller; }
            set
            {
                seller = value;
                OnPropertyChanged("Seller");
            }
        }
        public int Numbercheck
        {
            get { return numbercheck; }
            set
            {
                numbercheck = value;
                OnPropertyChanged("Numbercheck");
            }
        }

    }
}

    /// <summary>
    /// Класс проданных продуктов
    /// </summary>
    public class ArchiveSaleProducts : BaseNotify
{
    /// <summary>
    /// Id проданного продукта
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Имя проданного продукта
    /// </summary>
    private string? name;
    /// <summary>
    /// Количество проданногопродукта
    /// </summary>
    private int count;
    /// <summary>
    /// Цена проданного продукта
    /// </summary>
    private decimal price;
    /// <summary>
    /// Стоимость проданного продукта
    /// </summary>
    private decimal allprice;
    /// <summary>
    /// Время продажи
    /// </summary>
    private string timecheck;
    /// <summary>
    /// Имя продавца
    /// </summary>
    private string? seller;
    /// <summary>
    /// Номер операции
    /// </summary>
    public int numbercheck;
    public string Name
    {
        get { return name; }
        set
        {
            name = value;
            OnPropertyChanged("Name");
        }
    }

    public int Count
    {
        get { return count; }
        set
        {
            count = value;
            OnPropertyChanged("Count");
        }
    }
    public decimal Price
    {
        get { return price; }
        set
        {
            price = value;
            OnPropertyChanged("Price");
        }
    }

    public decimal AllPrice
    {
        get { return allprice; }
        set
        {
            allprice = value;
            OnPropertyChanged("AllPrice");
        }
    }

    public string Timecheck
    {
        get { return timecheck; }
        set
        {
            timecheck = value;
            OnPropertyChanged("Timecheck");
        }
    }

    public string? Seller
    {
        get { return seller; }
        set
        {
            seller = value;
            OnPropertyChanged("Seller");
        }
    }
    public int Numbercheck
    {
        get { return numbercheck; }
        set
        {
            numbercheck = value;
            OnPropertyChanged("Numbercheck");
        }
    }

}

