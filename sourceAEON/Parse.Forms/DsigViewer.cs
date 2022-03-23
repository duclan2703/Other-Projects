using EInvoice.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

namespace Parse.Forms
{
    public class DsigViewer
    {
        public static string GetHtml(XmlDocument xdoc, string xsltData)
        {
            XmlDocument xmlDocTemplate = new XmlDocument();
            xmlDocTemplate.LoadXml(xsltData);
            return TransformXMLToHTML(xdoc.InnerXml, xmlDocTemplate.InnerXml);
        }

        public static string GetHtml(string xmlData, out int products)
        {
            products = 1;
            XmlDocument xdoc = new XmlDocument();
            xdoc.PreserveWhitespace = true;
            xdoc.LoadXml(xmlData);
            xdoc = QRCode.GetHtml(xmlData);
            XmlNodeList elemList = xdoc.GetElementsByTagName("Product");
            products = elemList.Count / 10 + 1;
            XmlProcessingInstruction instruction = xdoc.SelectSingleNode("processing-instruction('xml-stylesheet')") as XmlProcessingInstruction;
            if (instruction != null)
            {
                XmlElement dummyPi = instruction.OwnerDocument.ReadNode(XmlReader.Create(new StringReader("<pi " + instruction.Value + "/>"))) as XmlElement;
                string href = dummyPi.GetAttribute("href");
                string tempName = "";
                string templatePath = UtilsViewer.getInvoiceFolder(href, out tempName);
                if (null == templatePath)
                {
                    return null;
                }
                //edit xslt file
                string xsltFile = templatePath + tempName + ".xslt";
                XmlDocument xmlDocTemplate = new XmlDocument();
                xmlDocTemplate.Load(xsltFile);

                if (xmlDocTemplate != null)
                {
                    return TransformXMLToHTML(xdoc.InnerXml, xmlDocTemplate.InnerXml);
                }
            }
            return null;
        }

        public static string TransformXMLToHTML(string inputXml, string xsltString)
        {
            XslCompiledTransform transform = new XslCompiledTransform();
            using (XmlReader reader = XmlReader.Create(new StringReader(xsltString)))
            {
                transform.Load(reader);
            }
            StringWriter results = new StringWriter();
            using (XmlReader reader2 = XmlReader.Create(new StringReader(inputXml)))
            {
                transform.Transform(reader2, null, results);
            }
            return results.ToString();
        }
    }
}
