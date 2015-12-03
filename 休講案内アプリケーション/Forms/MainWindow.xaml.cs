using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using Codeplex.Data;

namespace 休講案内アプリケーション
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// メニュー＞終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 休講情報取得・出力
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Loaded(object sender, RoutedEventArgs e)
        {
            menuViewFree.IsChecked = true;

            getKyukou();
        }

        /// <summary>
        /// 全学部、全学年の休講情報を取得して出力
        /// </summary>
        public void getKyukou()
        {
            string url = "http://konome.org/junk/95.php?mode=json";
            var req = WebRequest.Create(url);
            Info[] info = new Info[256];
            var res = req.GetResponse();
            Encoding enc = Encoding.GetEncoding("utf-8");
            Stream st = res.GetResponseStream();
            StreamReader sr = new StreamReader(st, enc);
            string str = sr.ReadToEnd();
            var obj = DynamicJson.Parse(str);
            int i = 0;

            clear();

            foreach (var objct in obj)
            {
                if (objct.school.@string != "デザイン学科" && objct.school.@string != "看護学科" && objct.school.@string != "臨床工学科" && objct.school.@string != "作業療法学科" && objct.school.@string != "理学療法学科" && objct.school.@string != "バイオ・情報メディア研究科")
                {
                    info[i] = new Info(objct.title, objct.teacher, objct.school.@string, objct.date.@string + "　" + objct.period.@string + "限目", objct.@class, objct.grade.ToString(), objct.note, objct.update.year.ToString() + "年" + objct.update.month.ToString() + "月" + objct.update.day.ToString() + "日　" + objct.update.hour.ToString() + "時" + objct.update.min.ToString() + "分");
                    i++;
                }
            }

            try
            {
                for (int j = 0; j < 256; j++)
                {
                    WrapP.Children.Add(info[j]);
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// 指定された学部、学年の休講情報を取得して出力
        /// </summary>
        /// <param name="param">URLパラメータ</param>
        public void getKyukou(string param)
        {
            string url = "http://konome.org/junk/95.php?mode=json&" + param;
            var req = WebRequest.Create(url);
            Info[] info = new Info[256];
            var res = req.GetResponse();
            Encoding enc = Encoding.GetEncoding("utf-8");
            Stream st = res.GetResponseStream();
            StreamReader sr = new StreamReader(st, enc);
            string str = sr.ReadToEnd();
            var obj = DynamicJson.Parse(str);
            int i = 0;

            clear();

            foreach (var objct in obj)
            {
                if (objct.school.@string != "デザイン学科" && objct.school.@string != "看護学科" && objct.school.@string != "臨床工学科" && objct.school.@string != "作業療法学科" && objct.school.@string != "理学療法学科" && objct.school.@string != "バイオ・情報メディア研究科")
                {
                    info[i] = new Info(objct.title, objct.teacher, objct.school.@string, objct.date.@string + "　" + objct.period.@string + "限目", objct.@class, objct.grade.ToString(), objct.note, objct.update.year.ToString() + "年" + objct.update.month.ToString() + "月" + objct.update.day.ToString() + "日　" + objct.update.hour.ToString() + "時" + objct.update.min.ToString() + "分");
                    i++;
                }
            }

            try
            {
                for (int j = 0; j < 256; j++)
                {
                    WrapP.Children.Add(info[j]);
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// 休講情報をクリア
        /// </summary>
        public void clear()
        {
            WrapP.Children.Clear();
        }

        /// <summary>
        /// フリー表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuViewFree_Checked(object sender, RoutedEventArgs e)
        {
            menuViewOne.IsChecked = false;
            menuViewTwo.IsChecked = false;
            menuViewThree.IsChecked = false;
            menuViewFour.IsChecked = false;

            this.MaxWidth = 100000;
            this.MinWidth = 396;
        }

        /// <summary>
        /// 1カラム表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuViewOne_Checked(object sender, RoutedEventArgs e)
        {
            menuViewFree.IsChecked = false;
            menuViewTwo.IsChecked = false;
            menuViewThree.IsChecked = false;
            menuViewFour.IsChecked = false;

            this.WindowState = WindowState.Normal;

            this.Width = 397;
            this.MaxWidth = 397;
            this.MinWidth = 397;
        }

        /// <summary>
        /// 2カラム表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuViewTwo_Checked(object sender, RoutedEventArgs e)
        {
            menuViewFree.IsChecked = false;
            menuViewOne.IsChecked = false;
            menuViewThree.IsChecked = false;
            menuViewFour.IsChecked = false;

            this.WindowState = WindowState.Normal;

            this.Width = 763;
            this.MaxWidth = 763;
            this.MinWidth = 763;
        }

        /// <summary>
        /// 3カラム表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuViewThree_Checked(object sender, RoutedEventArgs e)
        {
            menuViewOne.IsChecked = false;
            menuViewTwo.IsChecked = false;
            menuViewFree.IsChecked = false;
            menuViewFour.IsChecked = false;

            this.WindowState = WindowState.Normal;

            this.Width = 1128;
            this.MaxWidth = 1128;
            this.MinWidth = 1128;
        }

        /// <summary>
        /// 4カラム表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuViewFour_Checked(object sender, RoutedEventArgs e)
        {
            menuViewOne.IsChecked = false;
            menuViewTwo.IsChecked = false;
            menuViewThree.IsChecked = false;
            menuViewFree.IsChecked = false;

            this.WindowState = WindowState.Normal;

            this.Width = 1493;
            this.MaxWidth = 1493;
            this.MinWidth = 1493;
        }

        /// <summary>
        /// バージョン情報
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuInfoVersion_Click(object sender, RoutedEventArgs e)
        {
            InfoVersion frm = new InfoVersion();
            frm.ShowDialog();
        }

        /// <summary>
        /// 絞り込み：CS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkCS_Checked(object sender, RoutedEventArgs e)
        {
            chkMS.IsChecked = false;
            chkBS.IsChecked = false;
        }

        /// <summary>
        /// 絞り込み：MS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkMS_Checked(object sender, RoutedEventArgs e)
        {
            chkCS.IsChecked = false;
            chkBS.IsChecked = false;
        }

        /// <summary>
        /// 絞り込み：BS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkBS_Checked(object sender, RoutedEventArgs e)
        {
            chkCS.IsChecked = false;
            chkMS.IsChecked = false;
        }

        /// <summary>
        /// 絞り込み：1年
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkOne_Checked(object sender, RoutedEventArgs e)
        {
            chkTwo.IsChecked = false;
            chkThree.IsChecked = false;
            chkFour.IsChecked = false;
        }

        /// <summary>
        /// 絞り込み：2年
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkTwo_Checked(object sender, RoutedEventArgs e)
        {
            chkOne.IsChecked = false;
            chkThree.IsChecked = false;
            chkFour.IsChecked = false;
        }

        /// <summary>
        /// 絞り込み：3年
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkThree_Checked(object sender, RoutedEventArgs e)
        {
            chkOne.IsChecked = false;
            chkTwo.IsChecked = false;
            chkFour.IsChecked = false;
        }

        /// <summary>
        /// 絞り込み：4年
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkFour_Checked(object sender, RoutedEventArgs e)
        {
            chkOne.IsChecked = false;
            chkTwo.IsChecked = false;
            chkThree.IsChecked = false;
        }

        /// <summary>
        /// 絞り込み開始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            string param = "";

            if (chkCS.IsChecked == true) param += (param == "") ? "department=cs" : "&department=cs";
            if (chkMS.IsChecked == true) param += (param == "") ? "department=ms" : "&department=ms";
            if (chkBS.IsChecked == true) param += (param == "") ? "department=bs" : "&department=bs";

            if (chkOne.IsChecked == true) param += (param == "") ? "grade=1" : "&grade=1";
            if (chkTwo.IsChecked == true) param += (param == "") ? "grade=2" : "&grade=2";
            if (chkThree.IsChecked == true) param += (param == "") ? "grade=3" : "&grade=3";
            if (chkFour.IsChecked == true) param += (param == "") ? "grade=4" : "&grade=4";

            getKyukou(param);
        }

        /// <summary>
        /// ログイン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow frm = new LoginWindow();
            frm.ShowDialog();
        }
    }
}

