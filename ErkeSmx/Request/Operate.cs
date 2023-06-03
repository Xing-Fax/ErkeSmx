using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using System.Management;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ErkeSmx.Request
{
    internal class Operate
    {
        public static string GetID()
        {
            ManagementClass Sys = new ManagementClass("Win32_OperatingSystem");
            string ReID = string.Empty;
            foreach (ManagementObject Info in Sys.GetInstances())
                ReID = (string)Info["SerialNumber"];
            return ReID.Replace("-", "").Substring(0, 16);
        }

        public static string AesEncrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);

            RijndaelManaged rm = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = rm.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray);
        }

        public static string AesDecrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Convert.FromBase64String(str);

            RijndaelManaged rm = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = rm.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);
        }

        public static string HttpPostData(string url, NameValueCollection stringDict, string fileName, MemoryStream file, string fileKeyName = "image", int timeOut = 20)
        {

            //System.GC.Collect();

            timeOut = timeOut * 1000;
            string responseContent;

            MemoryStream memStream = null;
            Stream requestStream = null;
            HttpWebResponse httpWebResponse = null;
            HttpWebRequest webRequest = null;

            try
            {
                memStream = new MemoryStream();
                webRequest = (HttpWebRequest)WebRequest.Create(url);
                var boundary = "---------------" + DateTime.Now.Ticks.ToString("x");
                var beginBoundary = Encoding.ASCII.GetBytes("--" + boundary + "\r\n"); 
                var endBoundary = Encoding.ASCII.GetBytes("--" + boundary + "--\r\n");

                webRequest.Method = "POST";
                webRequest.Timeout = timeOut;
                webRequest.ContentType = "multipart/form-data; boundary=" + boundary;

                var stringKeyHeader = "--" + boundary +
                                        "\r\nContent-Disposition: form-data; name=\"{0}\"" +
                                        "\r\n\r\n{1}\r\n";

                foreach (byte[] formitembytes in from string key in stringDict.Keys
                                                 select string.Format(stringKeyHeader, key, stringDict[key])
                                                        into formitem
                                                 select Encoding.UTF8.GetBytes(formitem))
                {
                    memStream.Write(formitembytes, 0, formitembytes.Length);
                }

                const string filePartHeader =
                    "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
                        "Content-Type: application/octet-stream\r\n\r\n";
                var header = string.Format(filePartHeader, fileKeyName, fileName);
                var headerbytes = Encoding.UTF8.GetBytes(header);

                memStream.Write(beginBoundary, 0, beginBoundary.Length);
                memStream.Write(headerbytes, 0, headerbytes.Length);

                var buffer = new byte[1024];
                int bytesRead; // =0  
                file.Seek(0, SeekOrigin.Begin);
                while ((bytesRead = file.Read(buffer, 0, buffer.Length)) != 0)
                {
                    memStream.Write(buffer, 0, bytesRead);
                }

                var contentLine = Encoding.ASCII.GetBytes("\r\n");
                memStream.Write(contentLine, 0, contentLine.Length);

                memStream.Write(endBoundary, 0, endBoundary.Length);

                webRequest.ContentLength = memStream.Length;

                requestStream = webRequest.GetRequestStream();

                memStream.Position = 0;
                var tempBuffer = new byte[memStream.Length];
                memStream.Read(tempBuffer, 0, tempBuffer.Length);

                requestStream.Write(tempBuffer, 0, tempBuffer.Length);

                httpWebResponse = (HttpWebResponse)webRequest.GetResponse();

                using (var httpStreamReader = new StreamReader(httpWebResponse.GetResponseStream(),
                                                                Encoding.GetEncoding("utf-8")))
                {
                    responseContent = httpStreamReader.ReadToEnd();
                }
            }
            finally
            {
                if (memStream != null) { memStream.Close(); }
                if (requestStream != null) { requestStream.Close(); }
                if (httpWebResponse != null) { httpWebResponse.Close(); }
                if (webRequest != null) { webRequest.Abort(); }
            }

            return responseContent;
        }

        public static bool CompressImage(string sFile, string dFile, int flag = 90, int size = 200, bool sfsc = true)
        {
            FileInfo firstFileInfo = new FileInfo(sFile);
            if (sfsc == true && firstFileInfo.Length < size * 1024)
            {
                firstFileInfo.CopyTo(dFile); return true;
            }
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;
            int dHeight = iSource.Height / 2;
            int dWidth = iSource.Width / 2;
            int sW = 0, sH = 0;
            System.Drawing.Size tem_size = new System.Drawing.Size(iSource.Width, iSource.Height);
            if (tem_size.Width > dHeight || tem_size.Width > dWidth)
            {
                if ((tem_size.Width * dHeight) > (tem_size.Width * dWidth))
                {
                    sW = dWidth; sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }
            Bitmap ob = new Bitmap(dWidth, dHeight);
            Graphics g = Graphics.FromImage(ob);
            g.Clear(System.Drawing.Color.WhiteSmoke);
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);
            g.Dispose();
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x]; break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    ob.Save(dFile, jpegICIinfo, ep);
                    FileInfo fi = new FileInfo(dFile);
                    if (fi.Length > 1024 * size)
                    {
                        flag = flag - 10;
                        CompressImage(sFile, dFile, flag, size, false);
                    }
                }
                else
                {
                    ob.Save(dFile, tFormat);
                }
                return true;
            }
            catch { return false; }
            finally { iSource.Dispose(); ob.Dispose(); }
        }


        public static string Greetings()
        {
            string Str = "你好 ";
            var now = DateTime.Now;
            int times = now.Hour;
            if (times >= 6  && times < 9 ) { Str = "早上好 "; }
            if (times >= 9  && times < 11) { Str = "上午好 "; }
            if (times >= 11 && times < 13) { Str = "中午好 "; }
            if (times >= 13 && times < 17) { Str = "下午好 "; }
            if (times >= 19 && times < 24) { Str = "晚上好 "; }
            return Str;
        }

        public static string Execute(string str)
        {
            JsonSerializer serializer = new JsonSerializer();
            TextReader tr = new StringReader(str);
            JsonTextReader jtr = new JsonTextReader(tr);
            object obj = serializer.Deserialize(jtr);
            if (obj != null)
            {
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                serializer.Serialize(jsonWriter, obj);
                return textWriter.ToString();
            }
            else
            {
                return str;
            }
        }

        public static string Character(string sourse, string startstr, string endstr)
        {
            string result = string.Empty;
            int startindex, endindex;
            try
            {
                startindex = sourse.IndexOf(startstr);
                if (startindex == -1)
                    return result;
                string tmpstr = sourse.Substring(startindex + startstr.Length);
                endindex = tmpstr.IndexOf(endstr);
                if (endindex == -1)
                    return result;
                result = tmpstr.Remove(endindex);
            }
            catch { }
            return result;
        }

        public static void Theme(string Sixteen)
        {
            App.GlobalColor = Sixteen;
            Properties.Settings.Default.Color = App.GlobalColor;
            var paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();
            System.Windows.Media.Color color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Sixteen);
            theme.SetPrimaryColor(color);
            paletteHelper.SetTheme(theme);
        }

        public static string NoHTML(string Htmlstring)
        {
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<img[^>]*>;", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            return Htmlstring;
        }

        public static void Personalise()
        {
            if (Properties.Settings.Default.ImagePath.Substring(0, 4) != "Pack")
            {
                if (File.Exists(Properties.Settings.Default.ImagePath))
                {
                    MainHint.Window.BackImage.Source = new BitmapImage(new Uri(Properties.Settings.Default.ImagePath));
                }
                else
                {
                    MainHint.Window.AppSet.Set.Color4.IsChecked = true;
                }
            }
            switch (Properties.Settings.Default.Color)
            {
                case "#FF10AEC2": MainHint.Window.AppSet.Set.Color1.IsChecked = true; break;
                case "#FFED556A": MainHint.Window.AppSet.Set.Color2.IsChecked = true; break;
                case "#FF2CCFA0": MainHint.Window.AppSet.Set.Color3.IsChecked = true; break;
                case "#FFFF9999": MainHint.Window.AppSet.Set.Color4.IsChecked = true; break;
                case "#FF9E9E9E": MainHint.Window.AppSet.Set.Color5.IsChecked = true; break;
                case "#FFFE624C": MainHint.Window.AppSet.Set.Color6.IsChecked = true; break;
                case "#FF673AB7": MainHint.Window.AppSet.Set.Color7.IsChecked = true; break;
                case "#FF323232": MainHint.Window.AppSet.Set.Color8.IsChecked = true; break;
                case "#FFE07629": MainHint.Window.AppSet.Set.Color9.IsChecked = true; break;
                case "#FF009688": MainHint.Window.AppSet.Set.Color10.IsChecked = true; break;
                default: MainHint.Window.AppSet.Set.Color1.IsChecked = true; break;
            }

            switch (Properties.Settings.Default.Filling)
            {
                case 1:MainHint.Window.AppSet.Set.Filling1.IsChecked = true;break;
                case 2:MainHint.Window.AppSet.Set.Filling2.IsChecked = true;break;
                case 3:MainHint.Window.AppSet.Set.Filling3.IsChecked = true;break;
                case 4:MainHint.Window.AppSet.Set.Filling4.IsChecked = true;break;
                default:MainHint.Window.AppSet.Set.Filling1.IsChecked = true;break;
            }

            MainHint.Window.BackMask.Opacity = Properties.Settings.Default.Brigh / 100;
            MainHint.Window.ImageBlur.Radius = Properties.Settings.Default.Blur;
        }
    }
}
