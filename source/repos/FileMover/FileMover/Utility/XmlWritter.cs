using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Linq;

namespace FileMover.Utility
{
    public class XmlWritter
    {
        XDocument document = new XDocument();
        string path = @"C:\FileMover\";

        public void createXml()
        {
            if (!Directory.Exists(path))
            {
                DirectoryInfo di = Directory.CreateDirectory(path);

                XmlTextWriter textWriter = new XmlTextWriter(path+"config.xml", null);
 
                textWriter.WriteStartDocument();
                textWriter.WriteStartElement("Settings");

                textWriter.WriteStartElement("files");
                textWriter.WriteStartElement("type", "");
                textWriter.WriteString(".txt");
                textWriter.WriteEndElement();
                // Write one more element  
                textWriter.WriteStartElement("type", "");
                textWriter.WriteString(".tmp");
                textWriter.WriteEndElement();
                // Write one more element  
                textWriter.WriteStartElement("type", "");
                textWriter.WriteString(".doc");
                textWriter.WriteEndElement();
                // Write one more element  
                textWriter.WriteStartElement("type", "");
                textWriter.WriteString(".xls");
                textWriter.WriteEndElement();
                // Write one more element  
                textWriter.WriteStartElement("type", "");
                textWriter.WriteString(".ppt");
                textWriter.WriteEndElement();
                // Write one more element  
                textWriter.WriteStartElement("type", "");
                textWriter.WriteString(".xml");
                textWriter.WriteEndElement();
                textWriter.WriteEndElement();

                textWriter.WriteStartElement("time");
                textWriter.WriteStartElement("duration", "");
                textWriter.WriteString("1 hr");
                textWriter.WriteEndElement();

                textWriter.WriteStartElement("duration", "");
                textWriter.WriteString("2 hr");
                textWriter.WriteEndElement();

                textWriter.WriteStartElement("duration", "");
                textWriter.WriteString("3 hr");
                textWriter.WriteEndElement();

                textWriter.WriteStartElement("duration", "");
                textWriter.WriteString("4 hr");
                textWriter.WriteEndElement();

                textWriter.WriteStartElement("duration", "");
                textWriter.WriteString("5 hr");
                textWriter.WriteEndElement();

                textWriter.WriteStartElement("duration", "");
                textWriter.WriteString("6 hr");
                textWriter.WriteEndElement();

                textWriter.WriteStartElement("duration", "");
                textWriter.WriteString("7 hr");
                textWriter.WriteEndElement();

                textWriter.WriteStartElement("duration", "");
                textWriter.WriteString("8 hr");
                textWriter.WriteEndElement();
                // Ends the document.  
               
                textWriter.WriteEndDocument();
                textWriter.Close();
            }
        }
    }
}
