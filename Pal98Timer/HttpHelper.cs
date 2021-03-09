using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Pal98Timer
{
    public class HttpHelper
    {

        public static string DoGet(string url)
        {
            HttpWebRequest hwrq = null;
            HttpWebResponse hwrp = null;
            StreamReader MyStreamR = null;
            try
            {
                hwrq = (HttpWebRequest)WebRequest.Create(url);
                hwrq.UserAgent = "Mozilla/4.0 (compatible; Windows NT 5.1; SV1; .NET CLR 2.0.50727;)";
                hwrq.KeepAlive = false;
                hwrq.Method = "GET";
                hwrq.Timeout = 15000;
                //创建HttpWebResponse实例
                hwrp = (HttpWebResponse)hwrq.GetResponse();
                //hwrp.Cookies = _cc.GetCookies(hwrq.RequestUri);
                MyStreamR = new StreamReader(hwrp.GetResponseStream(), Encoding.UTF8);
                string result = MyStreamR.ReadToEnd();
                return result;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (MyStreamR != null) MyStreamR.Close();
                if (hwrp != null) hwrp.Close();
            }
        }

        public static string PostByUtf8(String url, String post)
        {
            string ret = "err";
            Encoding dataEncode = System.Text.Encoding.GetEncoding("utf-8");
            StreamReader sr = null;
            WebResponse response = null;
            Stream newStream = null;
            try
            {
                byte[] byteArray = dataEncode.GetBytes(post); //转化
                WebRequest webReq = WebRequest.Create(url);
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";

                webReq.ContentLength = byteArray.Length;
                newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                //newStream.Close();
                response = webReq.GetResponse();
                sr = new StreamReader(response.GetResponseStream(), dataEncode);
                ret = sr.ReadToEnd();

            }
            catch (Exception ex)
            {
                ret = "err";
            }
            finally
            {
                if (sr != null)
                    sr.Close();
                if (response != null)
                    response.Close();
                if (newStream != null)
                    newStream.Close();
            }
            return ret;
        }
    }
}
