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

namespace ErkeSmx.Interface
{
    /// <summary>
    /// MeInfoData.xaml 的交互逻辑
    /// </summary>
    public partial class MeInfoData : UserControl
    {
        public MeInfoData()
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
                Content.Text = "Null";
                Name.Text = "Null";
                Time.Text = "Null";
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
            if (e != null)
                MainHint.Window.BackThis_MouseUp(null, null);
            Clear.Elapsed += new System.Timers.ElapsedEventHandler(ClearContent);
            Clear.AutoReset = false;
            Clear.Enabled = true;
        }
    }
}
