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
        private static readonly char[] pwdEncChars = { 'A', 'v', 'i', 's', ' ', 'D', 'u', 'r', 'g', 'a', 'n' };
        private static readonly byte[] pwdEncBytes = { 0x41, 0x76, 0x69, 0x73, 0x20, 0x44, 0x75, 0x72, 0x67, 0x61, 0x6e };

        public class Gameinfo
        {
            public string Version { get; set; }
            public string GameTitle { get; set; }
            public string GameUID { get; set; }
        }

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

        static byte[] CharsToBytes(char[] chars)
        {
            byte[] bytes = new byte[chars.Length];
            int x = 0;
            foreach (char c in chars)
            {
                bytes[x] = (byte)c;
                x++;
            }

            return bytes;
        }

        private static int SwapEndianness(int value)
        {
            var b1 = (value >> 0) & 0xff;
            var b2 = (value >> 8) & 0xff;
            var b3 = (value >> 16) & 0xff;
            var b4 = (value >> 24) & 0xff;

            return b1 << 24 | b2 << 16 | b3 << 8 | b4 << 0;
        }

        public static void EncryptBytes(byte[] toenc)
        {
            int adx = 0;
            int toencx = 0;

            while (toencx < toenc.Length)
            {
                toenc[toencx] += pwdEncBytes[adx];
                adx++;
                toencx++;

                if (adx > 10)
                    adx = 0;
            }
        }

        public static void CreateTraFile(Gameinfo info, string filename, Dictionary<string, string> entryList)
        {

            Encoding encoding = Encoding.Default; //GetEncoding(1252); //Encoding.UTF8;
            bool encrypt = true;

            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                //Tail
                byte[] tail =
                {
                0x01, 0x00, 0x00, 0x00, 0x41, 0x01, 0x00, 0x00, 0x00, 0x41, 0x03, 0x00, 0x00, 0x00, 0x0C, 0x00,
                0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00,
                };

                //Write always header "AGSTranslation\0"
                byte[] agsHeader =
                {0x41, 0x47, 0x53, 0x54, 0x72, 0x61, 0x6E, 0x73, 0x6C, 0x61, 0x74, 0x69, 0x6F, 0x6E, 0x00,};
                fs.Write(agsHeader, 0, agsHeader.Length);

                //Padding not sure what exactly this is
                byte[] paddingBytes = { 0x02, 0x00, 0x00, 0x00, 0x16, 0x00, 0x00, 0x00, };
                fs.Write(paddingBytes, 0, paddingBytes.Length);

                //Write GameUID important or Translation does not load properly!
                string sGameUID = info.GameUID;
                int decAgain = int.Parse(sGameUID, System.Globalization.NumberStyles.HexNumber);
                byte[] bGameUID = BitConverter.GetBytes(SwapEndianness(decAgain));
                fs.Write(bGameUID, 0, bGameUID.Length);

                //Encrypt and write the Title
                string GameTitle = info.GameTitle + "\0";
                char[] cGameTitle = GameTitle.ToCharArray();

                //Write GameTitle Length
                byte[] bGameTitleLength = BitConverter.GetBytes(GameTitle.Length);
                fs.Write(bGameTitleLength, 0, bGameTitleLength.Length);

                //Write the encrypted GameTitle
                byte[] bGameTitle = CharsToBytes(cGameTitle);
                if (encrypt)
                {
                    EncryptBytes(bGameTitle);
                }
                fs.Write(bGameTitle, 0, bGameTitle.Length);

                //dummy write
                byte[] bDummy = { 0x01, 0x00, 0x00, 0x00, };
                fs.Write(bDummy, 0, bDummy.Length);

                //Write Length translation
                long translationLengthPosition = fs.Position;
                //Dummy write for later
                fs.Write(bDummy, 0, bDummy.Length);

                long translationLength = 0;

                if (entryList.Count > 0)
                {
                    foreach (KeyValuePair<string, string> pair in entryList)
                    {
                        if (!string.Equals(pair.Value, ""))
                        {
                            //Get original string
                            string entry1 = pair.Key;
                            entry1 = entry1 + "\0";

                            //Write original string length
                            byte[] bEntry1Length = BitConverter.GetBytes(entry1.Length);
                            fs.Write(bEntry1Length, 0, bEntry1Length.Length);

                            //Write original string bytes
                            char[] cEntry1 = entry1.ToCharArray();
                            byte[] bEntry1 = CharsToBytes(cEntry1);
                            if (encrypt)
                            {
                                EncryptBytes(bEntry1);
                            }
                            fs.Write(bEntry1, 0, bEntry1.Length);

                            //Get translation string  
                            string entry2 = pair.Value;
                            entry2 = entry2 + "\0";

                            //Write translation string length
                            int count = Encoding.Default.GetByteCount(entry2);
                            byte[] bEntry2Length = BitConverter.GetBytes(count);
                            //byte[] bEntry2Length = BitConverter.GetBytes(entry2.Length);
                            fs.Write(bEntry2Length, 0, bEntry2Length.Length);

                            //Write translation string bytes
                            char[] cEntry2 = entry2.ToCharArray();
                            byte[] bEntry2 = Encoding.Default.GetBytes(cEntry2);
                            if (encrypt)
                            {
                                EncryptBytes(bEntry2);
                            }
                            fs.Write(bEntry2, 0, bEntry2.Length);

                            long lengthTemp = BitConverter.ToInt32(bEntry1Length, 0) + 4 +
                                              BitConverter.ToInt32(bEntry2Length, 0) + 4;
                            translationLength = translationLength + lengthTemp;
                        }

                    }

                    //Write Tail
                    fs.Write(tail, 0, tail.Length);

                    //Write Translation length + 10
                    byte[] b = BitConverter.GetBytes((int)(translationLength + 10));
                    fs.Position = translationLengthPosition;
                    fs.Write(b, 0, b.Length);

                    fs.Close();
                }
            }
        }
    }
}
