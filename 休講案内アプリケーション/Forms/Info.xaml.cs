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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace 休講案内アプリケーション
{
    /// <summary>
    /// Info.xaml の相互作用ロジック
    /// </summary>
    public partial class Info : UserControl
    {
        public Info( string lessonName, string teacherName, string gakubu, string date, string className, string grade, string info, string updateDate)
        {
            InitializeComponent();

            lblLesson.Text = lessonName;
            lblLesson.TextAlignment = TextAlignment.Center;
            lblTeacher.Content = teacherName;
            lblGakubu.Text = gakubu;
            lblDate.Content = date;
            lblClass.Content = className;
            lblGrade.Content = grade;
            txtInfo.Text = info;
            lblUpdateDate.Content = updateDate;
        }
    }
}
