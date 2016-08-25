using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Web;

namespace WebEditor.Utilities
{
    public enum FFXExportType
    {
        Text = 1,
        XML = 2,
        HTML = 3
    }

    public enum FFXExportLevel
    {
        Page = 1,
        Line = 2,
        Word = 3
    }

    public class Utilitiess
    {
        private static Utilitiess _instance;

        private Utilitiess() { }

        public static Utilitiess GetInstance()
        {
            if (_instance == null)
                _instance = new Utilitiess();
            return _instance;
        }

        public int ConvertToPx(float f)
        {
            return Convert.ToInt32(f * 1.77164021f);
        }

        public string GetFilePath(string sPath)
        {
            string sRoot = HttpRuntime.AppDomainAppId == null ? new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName : AppDomain.CurrentDomain.BaseDirectory;

            return Path.GetFullPath(Path.Combine(sRoot, sPath));
        }
    }
}
