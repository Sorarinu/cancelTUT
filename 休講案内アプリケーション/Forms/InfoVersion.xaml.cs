using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.VisualBasic.ApplicationServices;

namespace 休講案内アプリケーション
{
    /// <summary>
    /// Information.xaml の相互作用ロジック
    /// </summary>
    public partial class InfoVersion : Window
    {
        public InfoVersion()
        {
            InitializeComponent();
        }

        /// <summary>
        /// アセンブリ情報を読み込む
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new AssemblyInfo( System.Reflection.Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// 閉じる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Twitterページを開く
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void twitterlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.AbsoluteUri); 
            e.Handled = true; 
        }

        /// <summary>
        /// ホームページを開く
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void homepagelink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true; 
        }
    }
}
