using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.IO;
using System.Collections.Specialized;
using Microsoft.Win32;
using System.Drawing;
using System.Web;
using ErkeSmx.Request;
using Newtonsoft.Json;

namespace ErkeSmx
{
    /// <summary>
    /// HomePage.xaml 的交互逻辑
    /// </summary>
    public partial class HomePage : UserControl
    {
        JavaScriptSerializer JSON = new JavaScriptSerializer();

        readonly System.Timers.Timer Load = new System.Timers.Timer(300);
        public HomePage()
        {
            InitializeComponent();
            ErkeGroup.Visibility = Visibility.Collapsed;
            ErkePay.Visibility = Visibility.Collapsed;
            ErkeSociety.Visibility = Visibility.Collapsed;
            ErkeInfo.Visibility = Visibility.Collapsed;

            HideHint();

            ComBox[] Box = new ComBox[5];

            Box[0] = new ComBox();
            Box[0].Pack.Kind = MaterialDesignThemes.Wpf.PackIconKind.StarShootingOutline;
            Box[0].Title.Text = "二课成绩单";

            Box[1] = new ComBox();
            Box[1].Pack.Kind = MaterialDesignThemes.Wpf.PackIconKind.HumanQueue;
            Box[1].Title.Text = "我的团组织";

            Box[2] = new ComBox();
            Box[2].Pack.Kind = MaterialDesignThemes.Wpf.PackIconKind.Cash;
            Box[2].Title.Text = "团费缴纳";

            Box[3] = new ComBox();
            Box[3].Pack.Kind = MaterialDesignThemes.Wpf.PackIconKind.AccountSupervisor;
            Box[3].Title.Text = "我的社团";

            Box[4] = new ComBox();
            Box[4].Pack.Kind = MaterialDesignThemes.Wpf.PackIconKind.InformationVariant;
            Box[4].Title.Text = "个人信息";

            for (int i = 0; i < Box.Count(); i++)
                Select.Items.Add(Box[i]);
                

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

        private void Error(bool Flag)
        {
            if (Flag)
            {
                LoadIng.Visibility = Visibility.Visible;
                TextPage.Text = "肥肠抱歉，加载失败辣~";
                Progr.Value = 100;
                Index = -1;
            }
            else
            {
                MailInfo.Visibility = Visibility.Visible;
                MailText.Text = "呀! 这里木有任何数据 ＞﹏＜";
            }
        }

        private void ScoreStart(object sender, DoWorkEventArgs e)
        {
            string Form = "token=" + PublicVar.Token;
            PublicVar.TScore = JSON.Deserialize<Transcript>(Related.POST(IntAddr.Score, Form));
        }

        private void ScoreComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            Load.Stop();
            if (PublicVar.TScore.code != 1)
            {
                Error(true);
                return;
            }

            BeginStoryboard((Storyboard)FindResource("Page" + Index + "_S"));
            LoadIng.Visibility = Visibility.Collapsed;

            Score.Name.Text = PublicVar.TScore.res.name;
            Score.ID.Text = PublicVar.TScore.res.s_number;
            Score.InTime.Text = PublicVar.TScore.res.ruxue_date;
            Score.Class.Text = PublicVar.TScore.res.dept_name;

            Score.Credit.Text = "共计学分：" + PublicVar.TScore.res.erke_jifen + " 分";
            Score.Hours.Text = "共计学时：" + PublicVar.TScore.res.erke_count + " 时";
            Score.Time.Text = "成绩点生成时间：" + PublicVar.TScore.res.endTime;

            if (PublicVar.TScore.res.erke_list.Count == 0)
            {
                MailInfo.Visibility = Visibility.Visible;
                MailText.Text = "你尚未参加过任何项目(＃°Д°)";
                return;
            }

            for (int i = 0; i < PublicVar.TScore.res.erke_list.Count; i++)
            {
                ScoreInfo ScoreList = new ScoreInfo();
                ScoreList.Title.Text = PublicVar.TScore.res.erke_list[i].erke_name;
                ScoreList.Time.Text = PublicVar.TScore.res.erke_list[i].erke_date;
                ScoreList.Count.Text = PublicVar.TScore.res.erke_list[i].erke_length + " 小时";
                Score.ErkeList.Children.Add(ScoreList);
            }
        }

        private void GetScore()
        {
            Score.Name.Text = string.Empty;
            Score.ID.Text = string.Empty;
            Score.InTime.Text = string.Empty;
            Score.Class.Text = string.Empty;
            Score.Credit.Text = string.Empty;
            Score.Hours.Text = string.Empty;
            Score.Time.Text = string.Empty;

            Score.ErkeList.Children.Clear();

            Load.Start();
            using (BackgroundWorker Score = new BackgroundWorker())
            {
                Score.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ScoreComplete);
                Score.DoWork += new DoWorkEventHandler(ScoreStart);
                Score.RunWorkerAsync();
            }
        }

        private void GroupStart(object sender, DoWorkEventArgs e)
        {
            string Form = "token=" + PublicVar.Token;
            PublicVar.GroupData = JSON.Deserialize<GroupInfo>(Related.POST(IntAddr.Group, Form));
        }

        private void GroupComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            Load.Stop();
            if(PublicVar.GroupData.code != 1)
            {
                Error(true);
                return;
            }

            BeginStoryboard((Storyboard)FindResource("Page" + Index + "_S"));
            LoadIng.Visibility = Visibility.Collapsed;
            Group.ClassName.Text = PublicVar.GroupData.res.tw_name;

            Group.Name.Text = PublicVar.GroupData.res.deptBreifIntroduction;
            Group.Address.Text = PublicVar.GroupData.res.leader;
            Group.Phone.Text = PublicVar.GroupData.res.phone;

            Group.Count.Text = "团干人数：" + PublicVar.GroupData.res.tg_num + " 人";
            Group.Number.Text = "团员人数：" + PublicVar.GroupData.res.ty_num + " 人";

            if (PublicVar.GroupData.res.ty_list.Count == 0)
            {
                Error(false);
                return;
            }

            for(int i = 0;i < PublicVar.GroupData.res.ty_list.Count;i++)
            {
                People Info = new People();
                Info.Name.Text = PublicVar.GroupData.res.ty_list[i].name;
                Info.ID.Text = PublicVar.GroupData.res.ty_list[i].s_number;
                Info.Phone.Text = PublicVar.GroupData.res.ty_list[i].phonenumber;
                if (PublicVar.GroupData.res.ty_list[i].avatar != null)
                    Info.Image.Source = new BitmapImage(new Uri(IntAddr.Url + PublicVar.GroupData.res.ty_list[i].avatar));
                Group.GroupList.Children.Add(Info);
            }
        }

        private void GetGroup()
        {
            Group.Name.Text = string.Empty;
            Group.Phone.Text = string.Empty;
            Group.Address.Text = string.Empty;

            Group.Count.Text = string.Empty;
            Group.Number.Text = string.Empty;

            Group.GroupList.Children.Clear();
            Load.Start();
            using (BackgroundWorker Group = new BackgroundWorker())
            {
                Group.RunWorkerCompleted += new RunWorkerCompletedEventHandler(GroupComplete);
                Group.DoWork += new DoWorkEventHandler(GroupStart);
                Group.RunWorkerAsync();
            }
        }

        private void PayStart(object sender, DoWorkEventArgs e)
        {
            string Form = "token=" + PublicVar.Token;
            PublicVar.PaymMney = JSON.Deserialize<PaymMney>(Related.POST(IntAddr.PayFee, Form));
        }

        private void PayComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            Load.Stop();
            if(PublicVar.PaymMney.code != 1)
            {
                Error(true);
                return;
            }

            BeginStoryboard((Storyboard)FindResource("Page" + Index + "_S"));
            LoadIng.Visibility = Visibility.Collapsed;

            if (PublicVar.PaymMney.res.Count == 0)
            {
                Error(false);
                return;
            }

            for(int i = 0;i < PublicVar.PaymMney.res.Count;i++)
            {
                ///
            }
        }

        private void PayFee()
        {
            Pay.PayList.Children.Clear();
            Load.Start();
            using (BackgroundWorker Pay = new BackgroundWorker())
            {
                Pay.RunWorkerCompleted += new RunWorkerCompletedEventHandler(PayComplete);
                Pay.DoWork += new DoWorkEventHandler(PayStart);
                Pay.RunWorkerAsync();
            }
        }

        private void SocietyStart(object sender, DoWorkEventArgs e)
        {
            string Form = "token=" + PublicVar.Token + "&page=" + e.Argument;
            PublicVar.Society = JSON.Deserialize<SocietyS>(Related.POST(IntAddr.PayFee, Form));
        }

        private void SocietyComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            Load.Stop();
            if (PublicVar.Society.code != 1)
            {
                Error(true);
                return;
            }

            BeginStoryboard((Storyboard)FindResource("Page" + Index + "_S"));
            LoadIng.Visibility = Visibility.Collapsed;

            if (PublicVar.Society.res.Count == 0)
            {
                Error(false);
                return;
            }

            for (int i = 0; i < PublicVar.Society.res.Count; i++)
            {
                ///
            }
        }

        private void InfoStart(object sender, DoWorkEventArgs e)
        {
            string Form = "token=" + PublicVar.Token;
            PublicVar.MyInfo = JSON.Deserialize<MyInfo>(Related.POST(IntAddr.Info, Form));
        }

        private static string[] NameList = { "姓名", "性别", "学号", "入学时间", "身份证号", "联系方式", "所在学院", "所在专业", "所在班级", "学制", "是否团员", "是否团干", "入团时间", "团员编号", "团组织关系是否转接", "是否录入智慧团建" };
        private void InfoComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            Load.Stop();
            if (PublicVar.MyInfo.code != 1)
            {
                Error(true);
                return;
            }

            BeginStoryboard((Storyboard)FindResource("Page" + Index + "_S"));
            LoadIng.Visibility = Visibility.Collapsed;

            int i = 0;
            foreach (PropertyInfo P in PublicVar.MyInfo.res.GetType().GetProperties())
            {
                if (P.Name != "avatar")
                {
                    InfoDisplay Disp = new InfoDisplay();
                    Disp.InfoName.Text = NameList[i++];
                    Disp.Info.Text = (string)P.GetValue(PublicVar.MyInfo.res);
                    PersInfo.InfoList.Items.Add(Disp);
                }
            }
        }

        private void SocietyClass()
        {
            Society.SocietyList.Children.Clear();
            Load.Start();
            using (BackgroundWorker Society = new BackgroundWorker())
            {
                Society.RunWorkerCompleted += new RunWorkerCompletedEventHandler(SocietyComplete);
                Society.DoWork += new DoWorkEventHandler(SocietyStart);
                Society.RunWorkerAsync("1");
            }
        }

        
        private void ShowInfo()
        {
            PersInfo.InfoList.Items.Clear();
            Load.Start();
            using (BackgroundWorker Info = new BackgroundWorker())
            {
                Info.RunWorkerCompleted += new RunWorkerCompletedEventHandler(InfoComplete);
                Info.DoWork += new DoWorkEventHandler(InfoStart);
                Info.RunWorkerAsync();
            }
        }

        private void HideHint()
        {
            LoadIng.Visibility = Visibility.Collapsed;
            MailInfo.Visibility = Visibility.Collapsed;
        }

        private static int Index = 1;
        private void Select_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Select.SelectedIndex != -1 && IsLoaded)
            {
                HideHint();
                if(Index != -1)
                    BeginStoryboard((Storyboard)FindResource("Page" + Index + "_C"));
                Index = Select.SelectedIndex + 1;
                switch (Index)
                {
                    case 1: GetScore();break;
                    case 2: GetGroup();break;
                    case 3: PayFee();break;
                    case 4: SocietyClass();break;
                    case 5: ShowInfo();break;
                }
            }
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            BeginStoryboard((Storyboard)FindResource("Replace_S"));
        }

        private void Canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            BeginStoryboard((Storyboard)FindResource("Replace_C"));
        }

        public struct Upload
        {
            public int code { get; set; }
            public string msg { get; set; }
            public List<string> data { get; set; }
        }

        public struct State
        {
            public int code { get; set; }
            public string msg { get; set; }
        }

        private static State ReturnState = new State();
        private static Upload DataInfo = new Upload();
        private void UpdateStart(object sender, DoWorkEventArgs e)
        {

            NameValueCollection Data = new NameValueCollection
            {
                { "file_type", "image" }
            };

            FileStream Files = new FileStream(e.Argument.ToString(), FileMode.Open);
            byte[] ImgByte = new byte[Files.Length];
            Files.Read(ImgByte, 0, ImgByte.Length);
            MemoryStream InputStream = new MemoryStream(ImgByte);

            DataInfo = JsonConvert.DeserializeObject<Upload>(Operate.HttpPostData(IntAddr.Updata, Data, System.IO.Path.GetFileName(e.Argument.ToString()), InputStream));

            Files.Close();
            InputStream.Close();

            if (DataInfo.code != 1)
            {
                MainHint.Window.HintText.Content = "图片上传失败惹~";
                MainHint.Window.BeginStoryboard((Storyboard)FindResource("Hint"));
                return;
            }

            string Form = "token=" + PublicVar.Token + "&avatar=" + HttpUtility.UrlEncode(DataInfo.data[0]);
            ReturnState = JSON.Deserialize<State>(Related.POST(IntAddr.Avatar, Form));

            if (DataInfo.code != 1)
            {
                MainHint.Window.HintText.Content = "头像修改失败惹~";
                MainHint.Window.BeginStoryboard((Storyboard)FindResource("Hint"));
                return;
            }
        }

        private void UpdateComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if(ReturnState.code == 1 && DataInfo.code == 1)
            {
                MainHint.Window.HintText.Content = "头像修改成功啦~";
                MainHint.Window.BeginStoryboard((Storyboard)FindResource("Hint"));
                Image.Source = new BitmapImage(new Uri(IntAddr.Url + DataInfo.data[0]));
            }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Multiselect = false;
            OpenFile.RestoreDirectory = true;
            OpenFile.Filter = "JPG|*.jpg|BMP|*.bmp|PNG|*.png";
            OpenFile.FilterIndex = 0;

            if ((bool)OpenFile.ShowDialog())
            {
                using (BackgroundWorker Update = new BackgroundWorker())
                {
                    Update.RunWorkerCompleted += new RunWorkerCompletedEventHandler(UpdateComplete);
                    Update.DoWork += new DoWorkEventHandler(UpdateStart);
                    Update.RunWorkerAsync(OpenFile.FileName);
                }
            }
        }
    }
}
