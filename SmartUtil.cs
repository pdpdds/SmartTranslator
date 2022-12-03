using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace SmartTran
{
    internal class SmartUtil
    {
        public static string RequestTranslation(string clientId, string clientSecret, string languageSource, string languageTarget, string query)
        {

            string url = "https://openapi.naver.com/v1/papago/n2mt";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);


            request.Headers.Add("X-Naver-Client-Id", clientId);
            request.Headers.Add("X-Naver-Client-Secret", clientSecret);
            request.Method = "POST";

            byte[] byteDataParams = Encoding.UTF8.GetBytes("source=" + languageSource + "&target=" + languageTarget + "&Text=" + query);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteDataParams.Length;

            try
            {
                Stream rqstream = request.GetRequestStream();
                rqstream.Write(byteDataParams, 0, byteDataParams.Length);
                rqstream.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream rpstream = response.GetResponseStream();
                StreamReader reader = new StreamReader(rpstream, Encoding.UTF8);

                string text = reader.ReadToEnd();
                response.Close();
                rpstream.Close();
                reader.Close();

                JObject ret = JObject.Parse(text);

                return ret["message"]["result"]["translatedText"].ToString();

            }
            catch
            {
                MessageBox.Show("HTTP Request or Response Time Out.", "Time Out");
                return "";
            }
            
        }
    }
}
