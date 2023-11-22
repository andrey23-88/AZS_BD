using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf_AZS_DB.Models;

namespace Wpf_AZS_DB.Views
{
    // Статический класс для хранения передаваемых данных
    public static class MyData
    {
        public static List<string> MyValue = new List<string>();
        public static List<Sellers> MySeller = new List<Sellers>();
        public static string? Names;
        public static bool Isadmin;
        public static int NumCheck = 0;
    }

}