using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace blah
{
    class OutlierConverter
    {

        private static OutlierConverter _instance;

        public static OutlierConverter Instance { get { return _instance == null ? _instance = new OutlierConverter() : _instance; } }

        private OutlierConverter() { }


        public void Convert(String path)
        {
            XmlDocument doc = new XmlDocument();
           doc.Load(File.OpenRead(@path));

           

        }
    }
}
