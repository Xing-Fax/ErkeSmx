using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ErkeSmx
{
    internal class Related
    {
        public static string POST(string Url, string PostData)
        {
            try
            {
                string strURL = Url;
                HttpWebRequest request;
                request = (HttpWebRequest)WebRequest.Create(strURL);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                string paraUrlCoded = PostData;
                byte[] payload;
                payload = Encoding.UTF8.GetBytes(paraUrlCoded);
                request.ContentLength = payload.Length;
                Stream writer = request.GetRequestStream();
                writer.Write(payload, 0, payload.Length);
                writer.Close();
                HttpWebResponse response;
                response = (HttpWebResponse)request.GetResponse();
                StreamReader myreader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string responseText = myreader.ReadToEnd();
                myreader.Close();
                return responseText;
            }
            catch
            {
                return "{\"code\":0}";
            }
        }

        public static string PostGetKey(string url)
        {
            try
            {
                string result = string.Empty;
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                Stream stream = resp.GetResponseStream();
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    result = reader.ReadToEnd();
                return result;
            }
            catch
            {
                return "{\"code\":0}";
            }
        }
    }
}
