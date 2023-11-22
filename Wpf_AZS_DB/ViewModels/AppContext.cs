using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Media;
using Wpf_AZS_DB.Models;

namespace Wpf_AZS_DB.ViewModels
{
    public class AppContext : DbContext
    {
        public AppContext()
        {
            //Database.EnsureDeleted(); //Удаляет базу с указанным названием
            Database.EnsureCreated(); //Создает базу с указанным названием
        }
        //Класс сущностей: сколько таблиц в базе данных, столько же строк с сущностями
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Sellers> Sellerses { get; set; } = null!;
        public DbSet<ArchiveSaleProducts> ArchiveSaleProductes { get; set; } = null!;

        //Класс отвечает за установку параметров подключения
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=LAPTOP-QVBLQU7F\\SQLEXPRESS; Initial Catalog=AutoGlass; Integrated Security=True; Connect Timeout=30; Encrypt=False; Trust Server Certificate=False; Application Intent=ReadWrite; Multi Subnet Failover=False;");
        }
    }
}
//Data Source = LAPTOP - QVBLQU7F\SQLEXPRESS; Initial Catalog = AutoGlass; Integrated Security = True; Connect Timeout = 30; Encrypt = False; Trust Server Certificate=False; Application Intent = ReadWrite; Multi Subnet Failover=False
