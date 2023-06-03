using ErkeSmx.Request;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ErkeSmx.Interface
{
    /// <summary>
    /// SetItem.xaml 的交互逻辑
    /// </summary>
    public partial class SetItem : UserControl
    {
        public SetItem()
        {
            InitializeComponent();
            Brightness.Text = Properties.Settings.Default.Brigh.ToString();
            Blur.Text = Properties.Settings.Default.Blur.ToString();
            Synchronize.IsChecked = Properties.Settings.Default.Synchronize;
            ImagePath.Text = Properties.Settings.Default.ImagePath;
            App.ImageBack = Properties.Settings.Default.ImagePath;
        }

        private void BackSet(string Path,string Color)
        {
            Operate.Theme(Color);

            if ((bool)Synchronize.IsChecked)
            {
                MainHint.Window.BackImage.Source = new BitmapImage(new Uri(Path));
                Properties.Settings.Default.ImagePath = Path;
                App.ImageBack = Path;
                ImagePath.Text = Path;
            }
        }

        //荀子蓝
        private void Color1_Checked(object sender, RoutedEventArgs e)
        {
            BackSet("Pack://application:,,,/Image/Back/Blue.png", "#FF10AEC2");
        }

        //山茶红
        private void Color2_Checked(object sender, RoutedEventArgs e)
        {
            BackSet("Pack://application:,,,/Image/Back/Red.png", "#FFED556A");
        }

        //宝石绿
        private void Color3_Checked(object sender, RoutedEventArgs e)
        {
            BackSet("Pack://application:,,,/Image/Back/Green.png", "#FF2CCFA0");
        }

        //桃花粉
        private void Color4_Checked(object sender, RoutedEventArgs e)
        {
            BackSet("Pack://application:,,,/Image/Back/Pick.png", "#FFFF9999");
        }

        //古典灰
        private void Color5_Checked(object sender, RoutedEventArgs e)
        {
            BackSet("Pack://application:,,,/Image/Back/Grey.png", "#FF9E9E9E");
        }

        //活力橙
        private void Color6_Checked(object sender, RoutedEventArgs e)
        {
            BackSet("Pack://application:,,,/Image/Back/Orange.png", "#FFFE624C");
        }

        //葡萄紫
        private void Color7_Checked(object sender, RoutedEventArgs e)
        {
            BackSet("Pack://application:,,,/Image/Back/Purple.png", "#FF673AB7");
        }

        //夜间黑
        private void Color8_Checked(object sender, RoutedEventArgs e)
        {
            BackSet("Pack://application:,,,/Image/Back/Black.png", "#FF323232");
        }

        //咸蛋黄
        private void Color9_Checked(object sender, RoutedEventArgs e)
        {
            BackSet("Pack://application:,,,/Image/Back/Yellow.png", "#FFE07629");
        } 

        //衬衫青
        private void Color10_Checked(object sender, RoutedEventArgs e)
        {
            BackSet("Pack://application:,,,/Image/Back/Cblue.png", "#FF009688");
        }

        //原始
        private void Filling1_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Filling = 1;
            MainHint.Window.BackImage.Stretch = Stretch.None;
        }

        //缩放
        private void Filling2_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Filling = 2;
            MainHint.Window.BackImage.Stretch = Stretch.Fill;
        }

        //填充
        private void Filling3_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Filling = 3;
            MainHint.Window.BackImage.Stretch = Stretch.Uniform;
        }

        //适应
        private void Filling4_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Filling = 4;
            MainHint.Window.BackImage.Stretch = Stretch.UniformToFill;
        }

        private void Brightness_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(Brightness.Text, out _))
                Brightness.Text = "0";
            MainHint.Window.BackMask.Opacity = (float.Parse(Brightness.Text) / 100);
            Properties.Settings.Default.Brigh = float.Parse(Brightness.Text);
        }

        private void Blur_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(Blur.Text, out _))
                Blur.Text = "0";
            MainHint.Window.ImageBlur.Radius = (int.Parse(Blur.Text));
            Properties.Settings.Default.Blur = float.Parse(Blur.Text);
        }

        private void Synchronize_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Synchronize = (bool)Synchronize.IsChecked;

            switch (Properties.Settings.Default.Color)
            {
                case "#FF10AEC2": Color1_Checked(null,null); break;
                case "#FFED556A": Color2_Checked(null, null); break;
                case "#FF2CCFA0": Color3_Checked(null, null); break;
                case "#FFFF9999": Color4_Checked(null, null); break;
                case "#FF9E9E9E": Color5_Checked(null, null); break;
                case "#FFFE624C": Color6_Checked(null, null); break;
                case "#FF673AB7": Color7_Checked(null, null); break;
                case "#FF323232": Color8_Checked(null, null); break;
                case "#FFE07629": Color9_Checked(null, null); break;
                case "#FF009688": Color10_Checked(null, null); break;
                default: Color1_Checked(null, null); break;
            }
        }

        private void Remember_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.RePassw = (bool)Remember.IsChecked;
        }

        private void AutoLogin_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ReLoad = (bool)AutoLogin.IsChecked;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            AutoLogin.IsChecked = Properties.Settings.Default.ReLoad;
            Remember.IsChecked = Properties.Settings.Default.RePassw;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog Ofd = new OpenFileDialog();
            Ofd.Filter = "图像文件|*.jpg;*.png;*.bmp;*.jpeg";
            if (Ofd.ShowDialog() == true)
            {
                MainHint.Window.BackImage.Source = new BitmapImage(new Uri(Ofd.FileName));
                Synchronize.IsChecked = false;
                App.ImageBack = Ofd.FileName;
                ImagePath.Text = Ofd.FileName;
                Properties.Settings.Default.ImagePath = Ofd.FileName;
                Properties.Settings.Default.Synchronize = (bool)Synchronize.IsChecked;
            }
        }
    }
}
