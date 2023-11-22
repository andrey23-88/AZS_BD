using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_AZS_DB.Models
{
    public class SaleProduct : BaseNotify
    {
        public int Id { get; set; }
        private string? name;
        private int count;
        private decimal price;
        private decimal allprice;


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
    }
}
