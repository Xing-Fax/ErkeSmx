using ErkeSmx.Request;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.SessionState;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Animation;
using static System.Net.WebRequestMethods;

namespace ErkeSmx
{
    public struct MeInfo
    {
        public int code { get; set; }
        public string msg { get; set; }
        public MeInfoRes res { get; set; }
    }

    public struct MeInfoRes
    {
        public string content { get; set; }
        public string create_by { get; set; }
        public string create_time { get; set; }
        public string message { get; set; }
        public string title { get; set; }
    }

    public struct MyInfo
    {
        public int code { get; set; }
        public string msg { get; set; }
        public InfoList res { get; set; }
    }

    public struct InfoList
    {
        public string name { get; set; }
        public string sex { get; set; }
        public string s_number { get; set; }
        public string ruxue_date { get; set; }
        public string id_number { get; set; }
        public string phonenumber { get; set; }
        public string xueyuan_name { get; set; }
        public string zhuanye_name { get; set; }
        public string banji_name { get; set; }
        public string xuezhi { get; set; }
        public string is_leaguemember { get; set; }
        public string is_migrant { get; set; }
        public string entryPartyTime { get; set; }
        public string recordDept { get; set; }
        public string partyOrg { get; set; }
        public string is_flow { get; set; }

        public string avatar { get; set; }
        //public string banji_id { get; set; }
        //public string erke_count { get; set; }
        //public string tw_name { get; set; }
        //public string xueyuan_id { get; set; }
        //public string zhuanye_id { get; set; }
    }

    public struct SocietyS
    {
        public int code { get; set; }
        public string msg { get; set; }
        public List<SocietyList> res { get; set; }
    }

    public struct SocietyList
    {
        ///
    }

    public struct PaymMney
    {
        public int code { get; set; }
        public string msg { get; set; }
        public List<PayList> res { get; set; }
    }

    public struct PayList
    {
        ///
    }

    public struct GroupInfo
    {
        public int code { get; set; }
        public string msg { get; set; }
        public string ty_num { get; set; }
        public GroupData res { get; set; }
    }

    public struct GroupData
    {
        public string deptBreifIntroduction { get; set; }
        public string leader { get; set; }
        public string phone { get; set; }
        public string tg_num { get; set; }
        public string tw_name { get; set; }
        public string ty_num { get; set; }
        public List<GroupList> ty_list { get; set; }
    }

    public struct GroupList
    {
        public string avatar { get; set; }
        public string name { get; set; }
        public string phonenumber { get; set; }
        public string s_number { get; set; }
    }

    //成绩单
    public struct Transcript
    {
        public int code { get; set; }
        public string msg { get; set; }
        public TranscriptRes res { get; set; }
    }

    public struct TranscriptRes
    {
        public string dept_name { get; set; }
        public string endTime { get; set; }
        //public string erke_count { get; set; }
        //public string erke_jifen { get; set; }
        public string name { get; set; }
        public string ruxue_date { get; set; }
        public string nstris_number { get; set; }
        public string s_number { get; set; }
        public string sort  { get; set; }
        public string totalScore { get; set; }
        public string xymc { get; set; }
        public string zymc { get; set; }

        //public List<TranscriptList> erke_list { get; set; }
        public List<indicator> indicator { get; set; }
        public List<indicators> indicators { get; set; }
        //public List<typeScores> typeScores { get; set; }
    }

    public struct indicator
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public struct indicators
    {
        public float max { get; set; }
        public string name { get; set; }
        public float score { get; set; }
    }


    //public struct TranscriptList
    //{

    //    public string erke_date { get; set; }
    //    public string erke_length { get; set; }
    //    public string erke_name { get; set; }
    //}

    public struct Mail
    {
        public int code { get; set; }
        public int totalNum { get; set; }
        public string msg { get; set; }
        public List<MailInfo> res { get; set;}
    }

    public struct MailInfo
    {
        public string content { get; set; }
        public string create_by { get; set; }
        public string create_time { get; set; }
        public string message_id { get; set; }
        public string title { get; set; }
    }

    public struct ErkeInfo
    {
        public int code { get; set; }
        public int sum { get; set; }
        public int totalPages { get; set; }
        public string msg { get; set; }
        public List<ErkeDate> res { get; set; }
    }

    public struct ErkeDate
    {
        public string img { get; set; }
        public string bm_content { get; set; }
        public string qj_flag_remark { get; set; }
        public string address { get; set; }
        public string bm_flag_remark { get; set; }
        public string end_time { get; set; }
        public string dept_name { get; set; }
        public string bm_flag { get; set; }
        public string start_time { get; set; }
        public string qj_content { get; set; }
        public string t_name { get; set; }
        public string name { get; set; }
        public string erke_type { get; set; }
        public string id { get; set; }
        public string max_num { get; set; }
        public string qj_flag { get; set; }
        public string bm_num { get; set; }
        public string t_time { get; set; }
        public string zj_content { get; set; }
        public string zj_flag { get; set; }
        public string zj_flag_remark { get; set; }

        public string qd_content { get; set; }
        public string qd_flag { get; set;}
    }

    public struct DetailInfo
    {
        public int code { get; set; }
        public string msg { get; set; }
        public DetaiData res { get; set; }
    }

    public struct DetaiData
    {
        public string address { get; set; }
        public string bm_num { get; set; }
        public string content { get; set; }
        public string createTime { get; set; }
        public string dept_name { get; set; }
        public string endTime { get; set; }
        public string erkeResult { get; set; }
        public string erke_type { get; set; }
        public string id { get; set; }
        public string img { get; set; }
        public string max_num { get; set; }
        public string name { get; set; }
        public string startTime { get; set; }
        public string status { get; set; }
        public string status_remark { get; set; }
        public string t_name { get; set; }
        public string zj_content { get; set; }
        public string zj_flag { get; set; }
        public string zj_flag_remark { get; set; }
    }

    public struct PublicKey
    {
        public int code { get; set; }
        public string publicKeyString { get; set; }
    }

    public struct LoginInfo
    {
        public int code { get; set; }
        public string msg { get; set; }
        public string userId { get; set; }
        public string token { get; set; }
    }

    public struct GsList
    {
        public int code { get; set; }
        public string msg { get; set; }
        public List<DsListData> res;
    }

    public struct DsListData 
    {
        public string erkeId { get; set; }
        public string deptName { get; set; }
        public string Img { get; set; }
        public string address { get; set; }
        public string erkeName { get; set; }
        public string startTime { get; set;}
        public string endTime { get; set;}
    }

    public static class IntAddr
    {
        public static string Url = "https://erke.smxpt.cn";
        /// <summary>
        /// 正在进行的活动列表请求
        /// </summary>
        public static string EventsUrl = "https://erke.smxpt.cn/lencon/app/erke/erke_list";
        /// <summary>
        /// 报名指定活动请求
        /// </summary>
        public static string SignUrl = "https://erke.smxpt.cn/lencon/app/erke/baoming";
        /// <summary>
        /// 登录公钥请求
        /// </summary>
        public static string PublicKey = "https://erke.smxpt.cn/lencon/app/getPublicKey";
        /// <summary>
        /// 登录请求
        /// </summary>
        public static string LoginLog = "https://erke.smxpt.cn/lencon/app/login";
        /// <summary>
        /// 个人信息获取
        /// </summary>
        public static string Info = "https://erke.smxpt.cn/lencon/app/person/get_info";
        /// <summary>
        /// 首页活动列表获取
        /// </summary>
        public static string MainList = "https://erke.smxpt.cn/lencon/app/index/erkeGSList";
        /// <summary>
        /// 活动详细信息
        /// </summary>
        public static string Detail = "https://erke.smxpt.cn/lencon/app/erke/detail";
        /// <summary>
        /// 个人信箱列表获取
        /// </summary>
        public static string InfoList = "https://erke.smxpt.cn/lencon/app/index/message_list";
        /// <summary>
        /// 二课成绩单获取
        /// </summary>
        public static string Score = "https://erke.smxpt.cn/lencon/app/erke/chengjidan";
        /// <summary>
        /// 团组织信息获取
        /// </summary>
        public static string Group = "https://erke.smxpt.cn/lencon/app/person/get_tw_info";
        /// <summary>
        /// 团费缴纳记录
        /// </summary>
        public static string PayFee = "https://erke.smxpt.cn/lencon/app/person/dues_info";
        /// <summary>
        /// 上传数据到服务器
        /// </summary>
        public static string Updata = "https://erke.smxpt.cn/lencon/app/common/upload";
        /// <summary>
        /// 更新用户头像
        /// </summary>
        public static string Avatar = "https://erke.smxpt.cn/lencon/app/modify/avatar";
        /// <summary>
        /// 消息内容获取
        /// </summary>
        public static string MeData = "https://erke.smxpt.cn/lencon/app/index/message_detail";
    }

    public static class MainHint
    {
        public static MainWindow Window;
    }

    public static class PublicVar
    {
        public static string Token;
        public static string LogName;
        public static string UserID;
        public static PublicKey  KeyData      = new PublicKey();
        public static LoginInfo  InfoData     = new LoginInfo();
        public static MyInfo     UserInfoData = new MyInfo();
        public static GsList     GsList       = new GsList();
        public static DetailInfo Detail       = new DetailInfo();
        public static ErkeInfo   ErkeData     = new ErkeInfo();
        public static Mail       MeInfo       = new Mail();
        public static Transcript TScore       = new Transcript();
        public static GroupInfo  GroupData    = new GroupInfo();
        public static PaymMney   PaymMney     = new PaymMney();
        public static SocietyS   Society      = new SocietyS();
        public static MyInfo     MyInfo       = new MyInfo();
        public static MeInfo     MeData       = new MeInfo();

    }

    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static string GlobalColor = "#FFFF9999";
        public static string ImageBack = "pack://application:,,,/Image/Back.png";
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Timeline.DesiredFrameRateProperty.OverrideMetadata(typeof(Timeline),new FrameworkPropertyMetadata { DefaultValue = 120 });
        }
    }
}
