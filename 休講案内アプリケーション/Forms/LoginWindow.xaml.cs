using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace 休講案内アプリケーション
{
    /// <summary>
    /// LoginWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class LoginWindow : Window
    {
        private string userid = "";
        private string passwd = "";

        public LoginWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 保存されている情報を出力
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.UserID != "")
            {
                txtUserID.Text = Properties.Settings.Default.UserID;
                txtPassword.Password = Properties.Settings.Default.Password;

                SaveCheckBox.IsChecked = true;
            }
            else
            {
                SaveCheckBox.IsChecked = false;
            }
        }

        /// <summary>
        /// ログイン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string save_password;

            lblError.Content = "";

            if (txtUserID.Text != "" && txtPassword.Password != "")
            {
                userid = txtUserID.Text;
                passwd = txtPassword.Password;

                //パスワードをハッシュ・ストレッチング化
                string hash = "";
                hash = SafePassword.GetStretchedPassword(passwd, userid);

                using (global.cn = new MySqlConnection(global.gDataSource))
                {
                    try
                    {
                        global.cn.Open();

                        //情報なし
                        if (Properties.Settings.Default.Password == "")
                        {
                            global.sql = "select * from User where UserID='" + userid + "' and Password='" + hash + "'";
                            save_password = hash;
                        }
                        else
                        {
                            //情報あり、入力変更なし
                            if (txtPassword.Password == Properties.Settings.Default.Password)
                            {
                                global.sql = "select * from User where UserID='" + userid + "' and Password='" + Properties.Settings.Default.Password + "'";
                                save_password = Properties.Settings.Default.Password;
                            }
                            //情報あり、入力変更あり
                            else
                            {
                                global.sql = "select * from User where UserID='" + userid + "' and Password='" + hash + "'";
                                save_password = hash;
                            }
                        }

                        global.cm = new MySqlCommand(global.sql, global.cn);
                        global.rd = global.cm.ExecuteReader();

                        while (global.rd.Read())
                        {
                            //DB照合確認
                            if (global.rd.GetValue(0).ToString() != "")
                            {
                                MessageBox.Show("ログインしました", "お知らせ", MessageBoxButton.OK, MessageBoxImage.Information);

                                if (SaveCheckBox.IsChecked == true)
                                {
                                    Properties.Settings.Default.UserID = txtUserID.Text;
                                    Properties.Settings.Default.Password = save_password;
                                    Properties.Settings.Default.Save();
                                }
                                else
                                {
                                    Properties.Settings.Default.UserID = "";
                                    Properties.Settings.Default.Password = "";
                                    Properties.Settings.Default.Save();
                                }

                                //メイン画面に遷移
                                //MainWindow frmMain = new MainWindow(global.rd.GetValue(3).ToString(), (SaveCheckBox.IsChecked == true) ? txtUserID.Text : "", (SaveCheckBox.IsChecked == true) ? save_password : "");
                                //frmMain.Show();

                                global.cn.Close();

                                this.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblError.Content = "ログインに失敗しました\r\n" + ex.Message;
                        global.cn.Close();


                        Properties.Settings.Default.UserID = "";
                        Properties.Settings.Default.Password = "";

                        Properties.Settings.Default.Save();

                        return;
                    }

                    lblError.Content = "ログインに失敗しました\r\nユーザ名またはパスワードが違います";
                    global.cn.Close();

                    Properties.Settings.Default.UserID = "";
                    Properties.Settings.Default.Password = "";

                    Properties.Settings.Default.Save();
                }
            }
            else
            {
                lblError.Content = "ログインに失敗しました\r\nユーザ名またはパスワードが違います";

                Properties.Settings.Default.UserID = "";
                Properties.Settings.Default.Password = "";

                Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        /// 終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// アカウント登録
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewAccount_Click(object sender, RoutedEventArgs e)
        {
            NewAccountWindow frm = new NewAccountWindow();
            frm.ShowDialog();
        }
    }
}
