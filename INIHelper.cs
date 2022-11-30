using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SmartTran
{
    internal class INIHelper
    {
        string _iniPath;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, UInt32 size,string FilePath);

        public INIHelper(string IniPath)
        {
            _iniPath = new FileInfo(IniPath).FullName;
        }

        public string Read(string Key, string Section)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section, Key, "", RetVal, 255, _iniPath);
            return RetVal.ToString();
        }

        public void Write(string Key, string Value, string Section)
        {
            WritePrivateProfileString(Section, Key, Value, _iniPath);
        }

        public void DeleteKey(string Key, string Section)
        {
            Write(Key, null, Section);
        }

        public void DeleteSection(string Section)
        {
            Write(null, null, Section);
        }

        public bool KeyExists(string Key, string Section = null)
        {
            return Read(Key, Section).Length > 0;
        }
    }
}
