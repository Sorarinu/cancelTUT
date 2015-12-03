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
    /// NewAccountWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class NewAccountWindow : Window
    {
        private bool UserID_Enable = false;
        private bool Password_Enable = false;
        private bool PasswordChk_Enable = false;
        private bool NickName_Enable = false;
        private bool EMail_Enable = false;

        public NewAccountWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// キャンセル
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 全角・半角判定
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private bool isOneByteChar(string str)
        {
            byte[] byte_data = System.Text.Encoding.GetEncoding(932).GetBytes(str);
            if (byte_data.Length == str.Length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (UserID_Enable && Password_Enable && NickName_Enable && EMail_Enable && PasswordChk_Enable)
            {
                DateTime dt = DateTime.Now;

                //パスワードをハッシュ・ストレッチング化
                string hash = "";
                hash = SafePassword.GetStretchedPassword(txtPassword.Password, txtUserID.Text);

                using (global.cn = new MySqlConnection(global.gDataSource))
                {
                    try
                    {
                        global.cn.Open();

                        global.sql = "insert into User (UserID, Password, UserName, E_Mail) values ( " +
                                     "'" + txtUserID.Text + "'," +
                                     "'" + hash + "'," +
                                     "'" + txtUserName.Text + "'," +
                                     "'" + txtEmail.Text + "'" +
                                     ")";

                        global.cm = new MySqlCommand(global.sql, global.cn);
                        global.cm.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error : " + ex.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                        //Notify.SendMailError(DateTime.Now, ex.Message, "NewAccount - 235～247");
                    }
                    finally
                    {
                        global.cn.Close();
                        this.Close();
                    }

                    MessageBox.Show("登録が完了しました", "お知らせ", MessageBoxButton.OK, MessageBoxImage.Information);
                    //Notify.SendMailSuccess(DateTime.Now, txtUserID.Text, txtUserName.Text, txtEmail.Text);
                }
            }
            else
            {
                MessageBox.Show("入力情報が不正です", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        /// <summary>
        /// ユーザIDが使用可能か判定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUserID_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (!isOneByteChar(txtUserID.Text))
            {
                lblNG_ID_MSG.Content = "全角文字は使用できません";
                lblNG_ID_MSG.Foreground = Brushes.Red;
                lblOK_ID.Visibility = System.Windows.Visibility.Hidden;
                lblNG_ID.Visibility = System.Windows.Visibility.Visible;
                lblNG_ID_MSG.Visibility = System.Windows.Visibility.Visible;

                UserID_Enable = false;
            }
            else
            {
                using (global.cn = new MySqlConnection(global.gDataSource))
                {
                    try
                    {
                        global.cn.Open();
                        global.sql = "select count(*) from User where UserID='" + txtUserID.Text + "'";
                        global.cm = new MySqlCommand(global.sql, global.cn);
                        global.rd = global.cm.ExecuteReader();

                        while (global.rd.Read())
                        {
                            if (global.rd.GetValue(0).ToString() != "0")    //NG
                            {
                                lblNG_ID_MSG.Content = "既に利用されています";
                                lblNG_ID_MSG.Foreground = Brushes.Red;
                                lblOK_ID.Visibility = System.Windows.Visibility.Hidden;
                                lblNG_ID.Visibility = System.Windows.Visibility.Visible;
                                lblNG_ID_MSG.Visibility = System.Windows.Visibility.Visible;

                                UserID_Enable = false;

                                global.cn.Close();

                                return;
                            }
                            else
                            {
                                if (txtUserID.Text != "")   //OK
                                {
                                    lblOK_ID.Visibility = System.Windows.Visibility.Visible;
                                    lblNG_ID.Visibility = System.Windows.Visibility.Hidden;
                                    lblNG_ID_MSG.Visibility = System.Windows.Visibility.Hidden;

                                    UserID_Enable = true;

                                    global.cn.Close();

                                    return;
                                }
                                else
                                {
                                    //NG
                                    lblNG_ID_MSG.Content = "文字列を入力してください";
                                    lblNG_ID_MSG.Foreground = Brushes.Red;
                                    lblOK_ID.Visibility = System.Windows.Visibility.Hidden;
                                    lblNG_ID.Visibility = System.Windows.Visibility.Visible;
                                    lblNG_ID_MSG.Visibility = System.Windows.Visibility.Visible;

                                    UserID_Enable = false;

                                    global.cn.Close();

                                    return;
                                }
                            }
                        }
                    }
                    catch (Exception) { }

                    global.cn.Close();
                }
            }
        }

        /// <summary>
        /// パスワードが使用可能か判定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPassword_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (txtPassword.Password.Count() >= 8)
            {
                lblOK_PW.Visibility = System.Windows.Visibility.Visible;
                lblNG_PW.Visibility = System.Windows.Visibility.Hidden;
                lblNG_PW_MSG.Visibility = System.Windows.Visibility.Hidden;

                Password_Enable = true;
            }
            else
            {
                lblNG_PW_MSG.Content = "8文字以上で入力してください";
                lblNG_PW_MSG.Foreground = Brushes.Red;
                lblOK_PW.Visibility = System.Windows.Visibility.Hidden;
                lblNG_PW.Visibility = System.Windows.Visibility.Visible;
                lblNG_PW_MSG.Visibility = System.Windows.Visibility.Visible;

                Password_Enable = false;
            }
        }

        /// <summary>
        /// パスワード確認
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPassword_Check_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (txtPassword.Password.Count() >= 8)
            {
                if (txtPassword.Password == txtPassword_Check.Password)
                {
                    lblOK_PWC.Visibility = System.Windows.Visibility.Visible;
                    lblNG_PWC.Visibility = System.Windows.Visibility.Hidden;
                    lblNG_PWC_MSG.Visibility = System.Windows.Visibility.Hidden;

                    PasswordChk_Enable = true;
                }
                else
                {
                    lblNG_PWC_MSG.Content = "入力されたパスワードが違います";
                    lblNG_PWC_MSG.Foreground = Brushes.Red;
                    lblOK_PWC.Visibility = System.Windows.Visibility.Hidden;
                    lblNG_PWC.Visibility = System.Windows.Visibility.Visible;
                    lblNG_PWC_MSG.Visibility = System.Windows.Visibility.Visible;

                    PasswordChk_Enable = false;
                }
            }
            else
            {
                lblNG_PWC_MSG.Content = "8文字以上で入力してください";
                lblNG_PWC_MSG.Foreground = Brushes.Red;
                lblOK_PWC.Visibility = System.Windows.Visibility.Hidden;
                lblNG_PWC.Visibility = System.Windows.Visibility.Visible;
                lblNG_PWC_MSG.Visibility = System.Windows.Visibility.Visible;

                PasswordChk_Enable = false;
            }
        }

        /// <summary>
        /// ニックネームが使用可能か判定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUserName_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (txtUserName.Text != "")
            {
                lblOK_NN.Visibility = System.Windows.Visibility.Visible;
                lblNG_NN.Visibility = System.Windows.Visibility.Hidden;
                lblNG_NN_MSG.Visibility = System.Windows.Visibility.Hidden;

                NickName_Enable = true;
            }
            else
            {
                lblNG_NN_MSG.Content = "ニックネームを入力してください";
                lblNG_NN_MSG.Foreground = Brushes.Red;
                lblOK_NN.Visibility = System.Windows.Visibility.Hidden;
                lblNG_NN.Visibility = System.Windows.Visibility.Visible;
                lblNG_NN_MSG.Visibility = System.Windows.Visibility.Visible;

                NickName_Enable = false;
            }
        }

        /// <summary>
        /// メールアドレスが使用可能か判定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtEmail_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (txtEmail.Text != "")
            {
                lblOK_AD.Visibility = System.Windows.Visibility.Visible;
                lblNG_AD.Visibility = System.Windows.Visibility.Hidden;
                lblNG_AD_MSG.Visibility = System.Windows.Visibility.Hidden;

                EMail_Enable = true;
            }
            else
            {
                lblNG_AD_MSG.Content = "メールアドレスを入力してください";
                lblNG_AD_MSG.Foreground = Brushes.Red;
                lblOK_AD.Visibility = System.Windows.Visibility.Hidden;
                lblNG_AD.Visibility = System.Windows.Visibility.Visible;
                lblNG_AD_MSG.Visibility = System.Windows.Visibility.Visible;

                EMail_Enable = false;
            }
        }
    }
}
