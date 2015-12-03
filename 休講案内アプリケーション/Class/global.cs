using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace 休講案内アプリケーション
{
    public class global
    {
        public static MySqlConnection cn;
        public static MySqlCommand cm;
        public static MySqlDataReader rd;
        public static string gDataSource = "server=" + Properties.Resources.ServerHost + ";user id=" + Properties.Resources.UserID + ";Password=" + Properties.Resources.Password + ";persist security info=True;database=" + Properties.Resources.Database;
        public static string sql = "";
    }
}
