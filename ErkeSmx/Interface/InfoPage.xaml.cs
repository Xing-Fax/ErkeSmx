using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ErkeSmx
{
    /// <summary>
    /// InfoPage.xaml 的交互逻辑
    /// </summary>
    public partial class InfoPage : UserControl
    {
        public InfoPage()
        {
            InitializeComponent();
            CanvasPage.Visibility = Visibility.Collapsed;
            LoadIng.Visibility = Visibility.Collapsed;
        }

        private void ClearContent(object source, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                TitleText.Text = "空";
                Organize.Text = "空";
                Time.Text = "空";
                Type.Text = "空";
                StartTime.Text = "空";
                EndTime.Text = "空";
                Place.Text = "空";
                Teacher.Text = "空";
                Contents.Text = "无内容...";
                Result.Text = "无内容 w(ﾟДﾟ)w！！！";
                Image.Source = null;
                CanvasPage.Visibility = Visibility.Collapsed;
                LoadIng.Visibility = Visibility.Collapsed;
                TextPage.Text = "玩命加载中ING...";
                Progr.Value = 0;
                MainHint.Window.MainInfo.Children.Clear();
            }));
        }

        readonly System.Timers.Timer Clear = new System.Timers.Timer(300);
        public void Button_Click(object sender, RoutedEventArgs e)
        {
            if(e != null)
                MainHint.Window.BackThis_MouseUp(null, null);
            Clear.Elapsed += new System.Timers.ElapsedEventHandler(ClearContent);
            Clear.AutoReset = false;
            Clear.Enabled = true;
        }
    }
}
