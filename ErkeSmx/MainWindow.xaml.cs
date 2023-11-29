using ErkeSmx.Interface;
using ErkeSmx.Request;
using Org.BouncyCastle.Crypto.Fpe;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace ErkeSmx
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        JavaScriptSerializer JSON = new JavaScriptSerializer();

        readonly Login Screen = new Login();

        readonly System.Timers.Timer Load = new System.Timers.Timer(300);
        public MainWindow()
        {

            InitializeComponent();
            Main.Visibility = Visibility.Hidden;
            MainHint.Window = this;
            Operate.Personalise();
            Screen.Effect.Radius = Properties.Settings.Default.Blur;
            Screen.BackImg.Opacity = Properties.Settings.Default.Brigh / 100;
            if (Properties.Settings.Default.ImagePath.Substring(0, 4) != "Pack")
            {
                if (File.Exists(Properties.Settings.Default.ImagePath))
                    Screen.Image.Source = new BitmapImage(new Uri(Properties.Settings.Default.ImagePath));
            }
            else
            {
                Screen.Image.Source = new BitmapImage(new Uri(Properties.Settings.Default.ImagePath));
            }
            Screen.Show();

            Erke.Visibility = Visibility.Collapsed;
            MeInfo.Visibility = Visibility.Collapsed;
            Individual.Visibility = Visibility.Collapsed;
            SetTing.Visibility = Visibility.Collapsed;

            MainInfo.Visibility = Visibility.Collapsed;
            BackThis.Visibility = Visibility.Collapsed;

            LoadIng.Visibility = Visibility.Collapsed;

            PopUp.Visibility = Visibility.Collapsed;

            Load.Elapsed += new System.Timers.ElapsedEventHandler(StartLoad);
            Load.AutoReset = false;
        }

        private void StartLoad(object source, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => {
                TextPage.Text = "玩命加载中ING...";
                LoadIng.Visibility = Visibility.Visible;
            }));
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) { DragMove(); }
        }

        private void Main_Activated(object sender, EventArgs e)
        {
            Main.Visibility = Visibility.Hidden;
        }

        private void Main_ContentRendered(object sender, EventArgs e)
        {
            BeginStoryboard((Storyboard)FindResource("Start"));
            using (BackgroundWorker Preloading = new BackgroundWorker())
            {
                Preloading.RunWorkerCompleted += new RunWorkerCompletedEventHandler(PreloadComplete);
                Preloading.DoWork += new DoWorkEventHandler(PreloadStart);
                Preloading.RunWorkerAsync();
            }
        }

        private void ExitBox_MouseLeave(object sender, MouseEventArgs e)
        {
            ExitBox.SelectedIndex = -1;
        }

        private void ExitClose(object source, System.Timers.ElapsedEventArgs e)
        {
            Environment.Exit(0);
        }

        new readonly System.Timers.Timer Close = new System.Timers.Timer(400);
        private void ExitBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ExitBox.SelectedIndex == 0)
            {
                BeginStoryboard((Storyboard)FindResource("Close"));

                Properties.Settings.Default.Save();

                Close.Elapsed += new System.Timers.ElapsedEventHandler(ExitClose);
                Close.AutoReset = false;
                Close.Enabled = true;
            }
        }

        private void PreloadStart(object sender, DoWorkEventArgs e)
        {
            string Form = "token=" + PublicVar.Token;
            PublicVar.TScore = JSON.Deserialize<Transcript>(Related.POST(IntAddr.Score, Form));
            PublicVar.MyInfo = JSON.Deserialize<MyInfo>(Related.POST(IntAddr.Info, Form));
            
        }

        private void PreloadComplete(object sender, RunWorkerCompletedEventArgs e)
        {

            if (PublicVar.TScore.code != 1)
            {
                LoadIng.Visibility = Visibility.Visible;
                HomeInfo.TextPage.Text = "肥肠抱歉，加载失败辣~";
                Progr.Value = 100;
                HomeInfo.Score.Visibility = Visibility.Collapsed;
                return;
            }

            HomeInfo.Score.Name.Text = PublicVar.TScore.res.name;

            HomeInfo.Score.ID.Text = PublicVar.TScore.res.s_number;
            HomeInfo.Score.InTime.Text = PublicVar.TScore.res.ruxue_date;
            HomeInfo.Score.Class.Text = PublicVar.TScore.res.dept_name;

            HomeInfo.Score.Credit.Text = "获得学分：" + PublicVar.TScore.res.totalScore + " 分";
            HomeInfo.Score.Hours.Text = "分数排名：" + PublicVar.TScore.res.sort;
            HomeInfo.Score.Time.Text = "成绩点生成时间：" + PublicVar.TScore.res.endTime;

            HomeInfo.MeName.Text = PublicVar.TScore.res.name;
            HomeInfo.Image.Source = new BitmapImage(new Uri(IntAddr.Url + PublicVar.MyInfo.res.avatar));

            //if (PublicVar.TScore.res..Count == 0)
            //{
            //    HomeInfo.MailInfo.Visibility = Visibility.Visible;
            //    HomeInfo.MailText.Text = "你尚未参加过任何项目(＃°Д°)";
            //    return;
            //}

            for (int i = 0; i < PublicVar.TScore.res.indicators.Count; i++)
            {
                ScoreInfo ScoreList = new ScoreInfo();
                ScoreList.Title.Text = PublicVar.TScore.res.indicators[i].name;
                ScoreList.Count.Text = PublicVar.TScore.res.indicators[i].score + "/" +
                                       PublicVar.TScore.res.indicators[i].max;

                ScoreList.Bar.Maximum = PublicVar.TScore.res.indicators[i].max;
                ScoreList.Bar.Value = PublicVar.TScore.res.indicators[i].score;


                //ScoreList.Title.Text = PublicVar.TScore.res.erke_list[i].erke_name;
                //ScoreList.Time.Text = PublicVar.TScore.res.erke_list[i].erke_date;
                //ScoreList.Count.Text = PublicVar.TScore.res.erke_list[i].erke_length + " 小时";
                HomeInfo.Score.ErkeList.Children.Add(ScoreList);
            }
        }

        private void HideHint()
        {
            LoadIng.Visibility = Visibility.Collapsed;
            MeInfo.Visibility = Visibility.Collapsed;
            MailInfo.Visibility = Visibility.Collapsed;
            HintInfo.Visibility = Visibility.Collapsed;
        }

        private static int Index = 1;
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(IsLoaded)
            {
                HideHint();
                BeginStoryboard((Storyboard)FindResource("Page" + (TabControl.SelectedIndex + 1) + "_S"));
                BeginStoryboard((Storyboard)FindResource("Page" + Index + "_C"));
                Index = TabControl.SelectedIndex + 1;
                if(Index == 2 && TabErkeIndex != -1)
                    Control_SelectionChanged(null,null);
                if (Index == 3)
                    GetInfo();
            }
        }

        private int Options = -1;
        private void InfoStart(object sender, DoWorkEventArgs e)
        {
            Options = int.Parse(e.Argument.ToString());
            string Form = "token=" + PublicVar.Token + "&erkeId=" + PublicVar.GsList.res[Options].erkeId;
            PublicVar.Detail = JSON.Deserialize<DetailInfo>(Related.POST(IntAddr.Detail, Form));
        }

        InfoPage InfoText = new InfoPage();
        private void InfoComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (PublicVar.Detail.code == 1)
            {
                InfoText.TitleText.Text = PublicVar.Detail.res.name;
                InfoText.Organize.Text = PublicVar.Detail.res.dept_name;
                InfoText.Time.Text = PublicVar.Detail.res.createTime;
                InfoText.Type.Text = PublicVar.Detail.res.erke_type;
                InfoText.StartTime.Text = PublicVar.Detail.res.startTime;
                InfoText.EndTime.Text = PublicVar.Detail.res.endTime;
                InfoText.Place.Text = PublicVar.Detail.res.address;
                InfoText.Teacher.Text = PublicVar.Detail.res.t_name;
                if (PublicVar.Detail.res.content != null)
                    InfoText.Contents.Text = Operate.NoHTML(PublicVar.Detail.res.content);
                if (PublicVar.Detail.res.erkeResult != null)
                    InfoText.Result.Text = Operate.NoHTML(PublicVar.Detail.res.erkeResult);
                if(File.Exists(Environment.CurrentDirectory + @"\ImageCache\Compression\" + Path.GetFileName(PublicVar.GsList.res[Options].Img)))
                    InfoText.Image.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\ImageCache\Compression\" + Path.GetFileName(PublicVar.GsList.res[Options].Img)));
                else
                    InfoText.Image.Source = new BitmapImage(new Uri("pack://application:,,,/Image/Null.png"));
                InfoText.CanvasPage.Visibility = Visibility.Visible;
                InfoText.LoadIng.Visibility = Visibility.Collapsed;
            }
            else
            {
                InfoText.TextPage.Text = "肥肠抱歉，加载失败辣~";
                InfoText.Progr.Value = 100; 
            }
        }

        public void BackThis_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if(BackThis.Opacity < 1) return;
            BeginStoryboard((Storyboard)FindResource("MainInfo_C"));
            if(Index == 1)
                if (InfoText.TitleText.Text != "Null")
                    InfoText.Button_Click(null, null);
            if(Index == 3)
                if(Infos.TitleText.Text != "Null")
                    Infos.Button_Click(null, null);

        }

        private void ListBoxMain_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ListBoxMain.SelectedIndex == -1)
                return;
            InfoText.LoadIng.Visibility = Visibility.Visible;
            MainInfo.Children.Add(InfoText);
            if (InfoText.InfoImage.Source.ToString() != App.ImageBack)
                InfoText.InfoImage.Source = new BitmapImage(new Uri(App.ImageBack));
            if(InfoText.Effect.Radius != ImageBlur.Radius)
                InfoText.Effect.Radius = ImageBlur.Radius;
            InfoText.BackImg.Opacity = Properties.Settings.Default.Brigh / 100;
            BeginStoryboard((Storyboard)FindResource("MainInfo_S"));
            using (BackgroundWorker PageInfo = new BackgroundWorker())
            {
                PageInfo.RunWorkerCompleted += new RunWorkerCompletedEventHandler(InfoComplete);
                PageInfo.DoWork += new DoWorkEventHandler(InfoStart);
                PageInfo.RunWorkerAsync(ListBoxMain.SelectedIndex);
            }
        }

        static T FindChildOfType<T>(DependencyObject root) where T : class
        {
            var queue = new Queue<DependencyObject>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                DependencyObject current = queue.Dequeue();
                for (int i = VisualTreeHelper.GetChildrenCount(current) - 1; 0 <= i; i--)
                {
                    var child = VisualTreeHelper.GetChild(current, i);
                    var typedChild = child as T;
                    if (typedChild != null)
                    {
                        return typedChild;
                    }
                    queue.Enqueue(child);
                }
            }
            return null;
        }

        private void NotStart_MouseMove(object sender, MouseEventArgs e)
        {
            ScrollViewer scrollViewer = FindChildOfType<ScrollViewer>(NotStart);
            if (scrollViewer == null)
            {
                throw new InvalidOperationException("erro");
            }
            else
            {
                if (scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight)
                {
                    if(NotStart.Items.Count > 4)
                    {
                        ///存在问题：如何判断重复加载，判断下一页是否是空数据
                        //TabErkeIndex = Control.SelectedIndex;

                        //using (BackgroundWorker Login = new BackgroundWorker())
                        //{
                        //    Login.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ErkeComplete);
                        //    Login.DoWork += new DoWorkEventHandler(ErkeStart);
                        //    Login.RunWorkerAsync(new int[2] { TabErkeIndex, 2 });
                        //}
                    }
                }
            }
        }

        private void ErkeStart(object sender, DoWorkEventArgs e)
        {
            string Form = "page=" + ((int[])e.Argument)[1] + "&token=" + PublicVar.Token + "&status=" + ((int[])e.Argument)[0];
            PublicVar.ErkeData = JSON.Deserialize<ErkeInfo>(Related.POST(IntAddr.EventsUrl, Form));
        }

        private void ErkeComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            Load.Stop();

            if (PublicVar.ErkeData.code != 1)
            {
                LoadIng.Visibility = Visibility.Visible;
                TextPage.Text = "肥肠抱歉，加载失败辣~";
                Progr.Value = 100;
                return;
            }

            if (PublicVar.ErkeData.totalPages == 0)
            {
                HintInfo.Visibility = Visibility.Visible;
                TextThis.Text = "呀! 这里木有任何数据 ＞﹏＜";
                LoadIng.Visibility = Visibility.Collapsed;
                return;
            }

            LoadIng.Visibility = Visibility.Collapsed;
            ///不能清除掉之前的项目，否则第二页独占列表
            //NotStart.Items.Clear();
            FillingErkeS(TabErkeIndex);
        }

        private void FillingErkeS(int index)
        {
            for (int i = 0; i < PublicVar.ErkeData.res.Count; i++)
            {
                EvInfo Ev = new EvInfo();
                Ev.Title.Text = PublicVar.ErkeData.res[i].name;
                Ev.Time.Text = PublicVar.ErkeData.res[i].start_time + " - " + PublicVar.ErkeData.res[i].end_time;
                Ev.Address.Text = PublicVar.ErkeData.res[i].address;
                Ev.Type.Text = PublicVar.ErkeData.res[i].erke_type;
                if (PublicVar.ErkeData.res[i].img != "/img/profile.jpg" && PublicVar.ErkeData.res[i].img != null)
                {
                    if (!File.Exists(Environment.CurrentDirectory + @"\ImageCache\" + Path.GetFileName(PublicVar.ErkeData.res[i].img)))
                        using (BackgroundWorker DowImage = new BackgroundWorker())
                        {
                            DowImage.DoWork += new DoWorkEventHandler(LoadImageStart);
                            DowImage.RunWorkerAsync(i);
                        }
                    else
                        Ev.Image.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\ImageCache\Compression\" + Path.GetFileName(PublicVar.ErkeData.res[i].img)));
                }
                switch (index)
                {
                    case 0:Ev.Sum.Text = "#招募" + PublicVar.ErkeData.res[i].bm_num + "人#    [" + PublicVar.ErkeData.res[i].bm_content + "]";break;
                    case 1:Ev.Sum.Text = "#招募" + PublicVar.ErkeData.res[i].bm_num + "人#    [" + PublicVar.ErkeData.res[i].qd_content + "]";break;
                    case 2:Ev.Sum.Text = "#招募" + PublicVar.ErkeData.res[i].bm_num + "人#    [" + PublicVar.ErkeData.res[i].zj_content + "]";break;
                }
                NotStart.Items.Add(Ev);
            }
        }

        private void LoadImageStart(object sender, DoWorkEventArgs e)
        {
            using (WebClient WebClient = new WebClient())
            {
                WebClient.DownloadFile(IntAddr.Url + PublicVar.ErkeData.res[(int)e.Argument].img, Environment.CurrentDirectory + @"\ImageCache\" + Path.GetFileName(PublicVar.ErkeData.res[(int)e.Argument].img));
                Operate.CompressImage(Environment.CurrentDirectory + @"\ImageCache\" + Path.GetFileName(PublicVar.ErkeData.res[(int)e.Argument].img), Environment.CurrentDirectory + @"\ImageCache\Compression\" + Path.GetFileName(PublicVar.ErkeData.res[(int)e.Argument].img), 20);
            }

            Dispatcher.Invoke(new Action(() => {
                ((EvInfo)NotStart.Items[(int)e.Argument]).Image.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\ImageCache\Compression\" + Path.GetFileName(PublicVar.ErkeData.res[(int)e.Argument].img)));
            }));
        }

        private static int TabErkeIndex = -1;
        private void Control_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(Control.SelectedIndex != -1)
            {
                Load.Start();
                HintInfo.Visibility = Visibility.Collapsed;
                Progr.Value = 0;
                NotStart.Items.Clear();
                TabErkeIndex = Control.SelectedIndex;
                
                using (BackgroundWorker Login = new BackgroundWorker())
                {
                    Login.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ErkeComplete);
                    Login.DoWork += new DoWorkEventHandler(ErkeStart);
                    Login.RunWorkerAsync(new int[2] { TabErkeIndex, 1});
                }
            }
        }

        private void GetInfoStart(object sender, DoWorkEventArgs e)
        {
            string Form = "token=" + PublicVar.Token + "&page=" + e.Argument.ToString();
            PublicVar.MeInfo = JSON.Deserialize<Mail>(Related.POST(IntAddr.InfoList, Form));
        }

        private void GetInfoComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            Load.Stop();

            if (PublicVar.MeInfo.code != 1)
            {
                LoadIng.Visibility = Visibility.Visible;
                TextPage.Text = "肥肠抱歉，加载失败辣~";
                Progr.Value = 100;
                return;
            }

            if (PublicVar.MeInfo.totalNum == 0)
            {
                MeInfo.Visibility = Visibility.Visible;
                MailText.Text = "呀! 这里木有任何数据 ＞﹏＜";
                LoadIng.Visibility = Visibility.Collapsed;
                return;
            }

            LoadIng.Visibility = Visibility.Collapsed;

            for (int i = 0;i < PublicVar.MeInfo.res.Count;i++)
            {
                Notify InfoData = new Notify();
                InfoData.Title.Text = PublicVar.MeInfo.res[i].title;
                InfoData.Time.Text = PublicVar.MeInfo.res[i].create_time;
                NotifyS.Items.Add(InfoData);
            }
        }

        private void GetInfo()
        {
            Load.Start();
            MeInfo.Visibility = Visibility.Collapsed;
            MailInfo.Visibility = Visibility.Collapsed;
            Progr.Value = 0;
            NotifyS.Items.Clear();
            using (BackgroundWorker Login = new BackgroundWorker())
            {
                Login.RunWorkerCompleted += new RunWorkerCompletedEventHandler(GetInfoComplete);
                Login.DoWork += new DoWorkEventHandler(GetInfoStart);
                Login.RunWorkerAsync(1);
            }
        }

        private void ShowInfoStart(object sender, DoWorkEventArgs e)
        {
            string Form = "token=" + PublicVar.Token + "&messageId=" + e.Argument.ToString();
            PublicVar.MeData = JSON.Deserialize<MeInfo>(Related.POST(IntAddr.MeData, Form));
        }

        MeInfoData Infos = new MeInfoData();
        private void ShowInfoComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (PublicVar.MeData.code == 1)
            {
                Infos.TitleText.Text = PublicVar.MeData.res.title;
                Infos.Content.Text = PublicVar.MeData.res.content;
                Infos.Name.Text = PublicVar.MeData.res.create_by;
                Infos.Time.Text = PublicVar.MeData.res.create_time;
                Infos.CanvasPage.Visibility = Visibility.Visible;
                Infos.LoadIng.Visibility = Visibility.Collapsed;
            }
            else
            {
                Infos.TextPage.Text = "肥肠抱歉，加载失败辣~";
                Infos.Progr.Value = 100;
            }
        }

        private void NotifyS_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (NotifyS.SelectedIndex == -1)
                return;
            Infos.LoadIng.Visibility = Visibility.Visible;
            MainInfo.Children.Add(Infos);
            if (Infos.InfoImage.Source.ToString() != App.ImageBack)
                Infos.InfoImage.Source = new BitmapImage(new Uri(App.ImageBack));
            if (Infos.Effect.Radius != ImageBlur.Radius)
                Infos.Effect.Radius = ImageBlur.Radius;
            Infos.BackImg.Opacity = Properties.Settings.Default.Brigh / 100;
            BeginStoryboard((Storyboard)FindResource("MainInfo_S"));
            using (BackgroundWorker ShowInfo = new BackgroundWorker())
            {
                ShowInfo.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ShowInfoComplete);
                ShowInfo.DoWork += new DoWorkEventHandler(ShowInfoStart);
                ShowInfo.RunWorkerAsync(PublicVar.MeInfo.res[NotifyS.SelectedIndex].message_id);
            }
        }
    }
}
