using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartTran
{
    internal class SmartTranslation
    {
        private static Dictionary<string, string> translationLines;


        public static Dictionary<string, string> ParseTranslation(string filename)
        {
           
            string[] list = File.ReadAllLines(filename, Encoding.Default);
            translationLines = new Dictionary<string, string>();

            //Look for comments and remove them
            var result = Array.FindAll(list, s => !s.StartsWith("//") && !s.Equals(""));

            for (int i = 0; i < result.Length;)
            {
                string sSourceText = result[i];
                i++;
                string sTranslationText = "";
                if (i < result.Length)
                {
                    sTranslationText = result[i];
                    i++;
                }

                if (!translationLines.ContainsKey(sSourceText))
                {
                    translationLines.Add(sSourceText, sTranslationText);
                }
                else
                {
                    MessageBox.Show("Entry already in Dictionary!",string.Format("Key already available: {0}", sSourceText));
                }
            }
            return translationLines;
        }

        public static Dictionary<string, string> ParseTranslationTxt(string filename)
        {

            string[] list = File.ReadAllLines(filename, Encoding.Default);
            translationLines = new Dictionary<string, string>();

            //Look for comments and remove them
            var result = Array.FindAll(list, s => !s.StartsWith("//") && !s.Equals(""));

            for (int i = 0; i < result.Length;)
            {
                string sSourceText = result[i];
                i++;
               
                if (!translationLines.ContainsKey(sSourceText))
                {
                    translationLines.Add(sSourceText, sSourceText);
                }
                else
                {
                    //MessageBox.Show("Entry already in Dictionary!", string.Format("Key already available: {0}", sSourceText));
                }
            }
            return translationLines;
        }
    }
}
