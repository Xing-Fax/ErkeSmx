using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ErkeSmx.Request;
using System.Web.Script.Serialization;
using System.Web;
using System.ComponentModel;
using System.Net;
using System.IO;

namespace ErkeSmx
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        /// <summary>
        /// 存放登录用户名
        /// </summary>
        private static string NameC = string.Empty;
        /// <summary>
        /// 存放登录密码
        /// </summary>
        private static string PassC = string.Empty;
        /// <summary>
        /// 标识是否登录成功
        /// </summary>
        private static bool LoginSign = false;
        /// <summary>
        /// 标识是否启用保存密码
        /// </summary>
        private static bool SaveInfo = false;
        /// <summary>
        /// 标识是否自动登录
        /// </summary>
        private static bool AutoLogin = false;

        private static string Path = Environment.CurrentDirectory;
        public Login()
        {
            InitializeComponent();
            Grid.Visibility = Visibility.Hidden;
            NextInfo.Visibility = Visibility.Hidden;
            HintInfo.Visibility = Visibility.Collapsed;

            Save.IsChecked = Properties.Settings.Default.RePassw;
            Log.IsChecked = Properties.Settings.Default.ReLoad;

            NameTextBox.Text = Properties.Settings.Default.UserName;

            if (Properties.Settings.Default.RePassw)
                PasswordBox.Password = Operate.AesDecrypt(Properties.Settings.Default.Password , Operate.GetID());

            if (!Directory.Exists(Path + @"\ImageCache\"))
                Directory.CreateDirectory(Path + @"\ImageCache\");
            if (!Directory.Exists(Path + @"\ImageCache\Compression\"))
                Directory.CreateDirectory(Path + @"\ImageCache\Compression\");
        }

        private void Rectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) { DragMove(); }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            if (Grid.Visibility == Visibility.Hidden)
                BeginStoryboard((Storyboard)FindResource("Login"));
        }

        private void Error(string Info)
        {
            Dispatcher.Invoke(new Action(() => { HintText.Content = "   " + Info + "~   "; }));
            Dispatcher.Invoke(new Action(() => { BeginStoryboard((Storyboard)FindResource("Hint")); }));
        }

        JavaScriptSerializer JSON = new JavaScriptSerializer();

        private void LoginStart(object sender, DoWorkEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>{ InfoText.Text = "请求公钥..."; }));
            PublicVar.KeyData = JSON.Deserialize<PublicKey>(Related.PostGetKey(IntAddr.PublicKey));
            if(PublicVar.KeyData.code != 1)
            {
                Error("登录失败惹! 公钥获取失败");
                return;
            }
            string RSA_LogName = LoginR.RSAEncrypt(LoginR.RSAPublicKey(PublicVar.KeyData.publicKeyString), NameC);
            string RSA_LogPass = LoginR.RSAEncrypt(LoginR.RSAPublicKey(PublicVar.KeyData.publicKeyString), PassC);
            string LoginParameter = "password=" + HttpUtility.UrlEncode(RSA_LogPass) + "&login_name=" + HttpUtility.UrlEncode(RSA_LogName) + "&publicKey=" + HttpUtility.UrlEncode(PublicVar.KeyData.publicKeyString);

            Dispatcher.Invoke(new Action(() => { InfoText.Text = "请求登录..."; }));
            PublicVar.InfoData = JSON.Deserialize<LoginInfo>(Related.POST(IntAddr.LoginLog,LoginParameter));
            if(PublicVar.InfoData.code != 1)
            {
                Error(PublicVar.InfoData.msg);
                return;
            }
            PublicVar.Token = PublicVar.InfoData.token;
            PublicVar.UserID = PublicVar.InfoData.userId;

            string UserParameter = "token=" + PublicVar.InfoData.token;
            Dispatcher.Invoke(new Action(() => { InfoText.Text = "获取信息..."; }));
            PublicVar.UserInfoData = JSON.Deserialize<MyInfo>(Related.POST(IntAddr.Info, UserParameter));
            if (PublicVar.UserInfoData.code != 1)
            {
                Error(PublicVar.InfoData.msg);
                return;
            }

            Dispatcher.Invoke(new Action(() => { InfoText.Text = "加载首页..."; }));
            PublicVar.GsList = JSON.Deserialize<GsList>(Related.PostGetKey(IntAddr.MainList));
            if(PublicVar.GsList.code != 1)
            {
                Error(PublicVar.GsList.msg);
                return;
            }

            LoginSign = true;
        }

        private void LoadImageStart(object sender, DoWorkEventArgs e)
        {
            using (WebClient WebClient = new WebClient())
            {
                WebClient.DownloadFile(IntAddr.Url + PublicVar.GsList.res[(int)e.Argument].Img, Path + @"\ImageCache\" + System.IO.Path.GetFileName(PublicVar.GsList.res[(int)e.Argument].Img));
                Operate.CompressImage(Path + @"\ImageCache\" + System.IO.Path.GetFileName(PublicVar.GsList.res[(int)e.Argument].Img), Path + @"\ImageCache\Compression\" + System.IO.Path.GetFileName(PublicVar.GsList.res[(int)e.Argument].Img), 20);
            }

            Dispatcher.Invoke(new Action(() => {
                ((InfoPane)MainHint.Window.ListBoxMain.Items[(int)e.Argument]).Image.Source = new BitmapImage(new Uri(Path + @"\ImageCache\Compression\" + System.IO.Path.GetFileName(PublicVar.GsList.res[(int)e.Argument].Img)));
            }));
        }

        private void FillingList()
        {
            for (int i = 0;i < PublicVar.GsList.res.Count;i++)
            {
                InfoPane Pane = new InfoPane();
                Pane.TitleText.Text = PublicVar.GsList.res[i].erkeName;
                Pane.TimeText.Text = PublicVar.GsList.res[i].startTime + " - " + PublicVar.GsList.res[i].endTime;
                Pane.PlaceText.Text = PublicVar.GsList.res[i].address;
                Pane.InbuText.Text = PublicVar.GsList.res[i].deptName;

                if (PublicVar.GsList.res[i].Img != "/img/profile.jpg" && PublicVar.GsList.res[i].Img != null)
                {
                    if (!File.Exists(Path + @"\ImageCache\" + System.IO.Path.GetFileName(PublicVar.GsList.res[i].Img)))
                    {
                        using (BackgroundWorker DowImage = new BackgroundWorker())
                        {
                            DowImage.DoWork += new DoWorkEventHandler(LoadImageStart);
                            DowImage.RunWorkerAsync(i);
                        }
                    }
                    else
                    {
                        Pane.Image.Source = new BitmapImage(new Uri(Path + @"\ImageCache\Compression\" + System.IO.Path.GetFileName(PublicVar.GsList.res[i].Img)));
                    }
                }
                MainHint.Window.ListBoxMain.Items.Add(Pane);
            }
        }

        private void SaveStart(object sender, DoWorkEventArgs e)
        {
            Properties.Settings.Default.UserName = NameC;
            if (SaveInfo)
            {
                Properties.Settings.Default.Password = Operate.AesEncrypt(PassC, Operate.GetID());
            }
            else
            {
                if (Properties.Settings.Default.Password != string.Empty)
                    Properties.Settings.Default.Password = string.Empty;
            }

            Properties.Settings.Default.ReLoad = AutoLogin;
            Properties.Settings.Default.RePassw = SaveInfo;
            Properties.Settings.Default.Save();
        }

        private void LoginComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (LoginSign)
            {
                Bar.Value = 100;
                InfoText.Text = "登录成功(●'◡'●)~  ";
                FillingList();
                MainHint.Window.Info.Text  = Operate.Greetings() + PublicVar.UserInfoData.res.name + " 同学！(●'◡'●)~";
                BeginStoryboard((Storyboard)FindResource("Fade"));
                MainHint.Window.Visibility = Visibility.Visible;


                SaveInfo = (bool)Save.IsChecked;
                AutoLogin = (bool)Log.IsChecked;
                using (BackgroundWorker Save = new BackgroundWorker())
                {
                    Save.DoWork += new DoWorkEventHandler(SaveStart);
                    Save.RunWorkerAsync();
                }
            }
            else
            {
                Start.Content = "登录";
                NextInfo.Visibility = Visibility.Hidden;
                LoginPanel.IsEnabled = true;

                Log.IsChecked = false;
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (NameTextBox.Text == string.Empty)
            {
                Error("请输入账号");
                NameTextBox.Focus();
                return;
            }

            if (PasswordBox.Password == string.Empty)
            {
                Error("请输入密码");
                PasswordBox.Focus();
                return;
            }

            if (NameTextBox.Text != string.Empty && PasswordBox.Password != string.Empty)
            {
                NameC = NameTextBox.Text;
                PassC = PasswordBox.Password;
                Start.Content = "登录中...";
                Bar.IsIndeterminate = true;
                LoginPanel.IsEnabled = false;
                NextInfo.Visibility = Visibility.Visible;

                using (BackgroundWorker Login = new BackgroundWorker())
                {
                    Login.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoginComplete);
                    Login.DoWork += new DoWorkEventHandler(LoginStart);
                    Login.RunWorkerAsync();
                }
            }
        }

        private void ExitClose(object source, System.Timers.ElapsedEventArgs e)
        {
            Environment.Exit(0);
        }

        new readonly System.Timers.Timer Close = new System.Timers.Timer(300);
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BeginStoryboard((Storyboard)FindResource("Close"));
            Close.Elapsed += new System.Timers.ElapsedEventHandler(ExitClose);
            Close.AutoReset = false;
            Close.Enabled = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            if (NameTextBox.Text == string.Empty)
            {
                NameTextBox.Focus();
                return;
            }
            if(PasswordBox.Password == string.Empty)
            {
                PasswordBox.Focus();
                return;
            }
            if (Properties.Settings.Default.ReLoad)
                Start_Click(null, null);
        }

        private void Network_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
