using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Bridge
{
    public class XMLManager
    {
        public string MakeXML(List<Tuple<string, string>> listItems)
        {
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(createReading(doc, listItems));
            StringWriter stringWriter = new StringWriter();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
            doc.WriteTo(xmlTextWriter);
            return stringWriter.ToString();
        }

        public XmlElement createReading(XmlDocument doc, List<Tuple<string, string>> listItems)
        {
            XmlElement reading = doc.CreateElement("reading");

            for (int i = 0; i < listItems.Count; i++)
            {
                XmlElement element = doc.CreateElement(listItems[i].Item1);
                if (listItems[i].Item1.Equals("timestamp"))
                {
                    element.InnerText = DateTimeOffset.FromUnixTimeSeconds(Convert.ToUInt32(listItems[i].Item2)).DateTime.ToString();
                }
                else
                {
                    element.InnerText = listItems[i].Item2;
                }
                reading.AppendChild(element);
            }
            return reading;
        }
    }
}
