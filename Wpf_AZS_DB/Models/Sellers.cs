using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_AZS_DB.Models
{
    /// <summary>
    /// класс Продавцы
    /// </summary>
    public class Sellers : BaseNotify
    {
        /// <summary>
        /// Id продавца
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Имя продавца
        /// </summary>
        private string? name;
        /// <summary>
        /// логин продавца
        /// </summary>
        private string login;
        /// <summary>
        /// пароль продавца
        /// </summary>
        private string password;
        /// <summary>
        /// является ли продавец администратором
        /// </summary>
        private bool isAdmin;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                OnPropertyChanged("Login");
            }
        }
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }
        public bool IsAdmin
        {
            get { return isAdmin; }
            set
            {
                isAdmin = value;
                OnPropertyChanged("IsAdmin");
            }
        }
    }
}
