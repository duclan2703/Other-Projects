using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Parse.Forms
{
    public class UtilsViewer
    {
        /// <summary>
        /// lay duong dan thu muc chua mau xslt, image
        /// </summary>
        /// <param name="invoiceLink"></param>
        /// <returns></returns>
        public static string getInvoiceFolder(string invoiceLink, out string tempName)
        {
            string rv = "";
            tempName = "";
            string folderName;
            rv = getTempName(invoiceLink, out folderName);
            tempName = rv;
            if (rv != null)
            {
                rv = AppDomain.CurrentDomain.BaseDirectory + "/Template/" + folderName + "/" + rv;
            }
            if (Directory.Exists(rv))
            {
                return rv + "/";
            }
            downloadTemplate(invoiceLink);
            if (Directory.Exists(rv))
            {
                return rv + "/";
            }
            return null;
        }

        static string getTempName(string invoiceLink, out string folderName)
        {
            string[] _template = new string[] { "GetXSLTbyPattern", "GetXSLTbyTempName/" };
            string template = invoiceLink.IndexOf(_template[0]) > 0 ? _template[0] : (invoiceLink.IndexOf(_template[1]) > 0 ? _template[1] : "");
            string rv = "";
            folderName = "";
            if (!string.IsNullOrEmpty(template))
            {
                folderName = invoiceLink.Substring(0, invoiceLink.IndexOf(template));
                rv = invoiceLink.Substring(invoiceLink.IndexOf(template), invoiceLink.Length - invoiceLink.IndexOf(template));
                rv = rv.Replace("?", "").Replace("/", "").Replace("=", "");
                folderName = folderName.Replace("/", "").Replace(":", "");
            }
            return rv;
        }

        /// <summary>
        /// download template inv from .inv url
        /// </summary>
        /// <param name="tempLink"></param>
        static void downloadTemplate(string tempLink)
        {
            string folderName;
            string tempName = getTempName(tempLink, out folderName);
            string path = AppDomain.CurrentDomain.BaseDirectory + "/Template";
            DirectoryInfo di;
            if (!Directory.Exists(path))
            {
                di = Directory.CreateDirectory(path);
            }
            path = path + "/" + folderName + "/" + tempName;
            di = Directory.CreateDirectory(path);
            //download file
            try
            {
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(tempLink);
                httpRequest.Method = WebRequestMethods.Http.Get;
                HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                Stream httpResponseStream = httpResponse.GetResponseStream();
                int bufferSize = 1024;
                byte[] buffer = new byte[bufferSize];
                int bytesRead = 0;

                // Read from response and write to file
                StreamReader reader = new StreamReader(httpResponseStream);
                FileStream fileStream = File.Create(path + "/" + tempName + ".xslt");
                while ((bytesRead = httpResponseStream.Read(buffer, 0, bufferSize)) != 0)
                {
                    fileStream.Write(buffer, 0, bytesRead);
                } // end while
                fileStream.Close();
                editTemplateDownloadFile(path + "/" + tempName + ".xslt");
            }
            catch (Exception ex)
            {
            }
        }

        static void editTemplateDownloadFile(string templatePath)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(templatePath);
            XmlNodeList xmlNodes;
            xmlNodes = xdoc.GetElementsByTagName("script");
            foreach (XmlNode n in xmlNodes)
            {
                if (n.InnerXml.Contains("function displayCert(serialCert)"))
                {
                    n.InnerXml = "function displayCert(certName){window.external.displayCert(certName);}function htmlEncode(value) {return $('<div/>').text(value).html();}function htmlDecode(value) {return $('<div/>').html(value).text();}";
                    break;
                }
            }

            //edit onclick xslt
            xmlNodes = xdoc.GetElementsByTagName("div");
            foreach (XmlNode n in xmlNodes)
            {
                XmlAttributeCollection attrs = n.Attributes;
                if (null == attrs || attrs.Count == 0 || !attrs[0].Name.Equals("class") || !attrs[0].Value.Equals("bgimg")) continue;
                foreach (XmlAttribute attr in attrs)
                {
                    if (attr.Name.Equals("onclick") && attr.Value.Contains("showDialog('dialogClient'"))
                    {
                        attr.Value = "displayCert('client')";
                    }
                    if (attr.Name.Equals("onclick") && attr.Value.Contains("showDialog('dialogServer'"))
                    {
                        attr.Value = "displayCert('server')";
                    }
                }
            }
            xdoc.Save(templatePath);
        }

        #region addImage to xmld
        public static XmlDocument addImage(XmlDocument xd, int c)
        {
            try
            {
                if (c == 0)
                {
                    ImageGenerator.AddCompanyImage(xd, true);
                    ImageGenerator.AddCustomerImage(xd, true);
                }
                else if (c == 1)
                {
                    ImageGenerator.AddCompanyImage(xd, true);
                }
                else if (c == 2)
                {
                    ImageGenerator.AddCompanyImage(xd, true);
                    ImageGenerator.AddCustomerImage(xd, false);
                }
                else
                {
                    ImageGenerator.AddCompanyImage(xd, false);
                    ImageGenerator.AddCustomerImage(xd, false);
                }
                return xd;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        class ImageGenerator
        {
            static string TRUE_IMAGE = "iVBORw0KGgoAAAANSUhEUgAAACUAAAAlCAYAAADFniADAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyBpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMC1jMDYwIDYxLjEzNDc3NywgMjAxMC8wMi8xMi0xNzozMjowMCAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNSBXaW5kb3dzIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjMyMzFCNDVEQTU3ODExRTI4QzBDODg4NzgxNTlDNkI2IiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjMyMzFCNDVFQTU3ODExRTI4QzBDODg4NzgxNTlDNkI2Ij4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6MzIzMUI0NUJBNTc4MTFFMjhDMEM4ODg3ODE1OUM2QjYiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6MzIzMUI0NUNBNTc4MTFFMjhDMEM4ODg3ODE1OUM2QjYiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz429TazAAAG+klEQVR42uxYWWxUVRj+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";
            static string FALSE_IMAGE = "iVBORw0KGgoAAAANSUhEUgAAACUAAAAlCAYAAADFniADAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyBpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMC1jMDYwIDYxLjEzNDc3NywgMjAxMC8wMi8xMi0xNzozMjowMCAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNSBXaW5kb3dzIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjIxREU4QTdCQTU3ODExRTJCRDA1QzVBQkU4NEFCNTNGIiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjIxREU4QTdDQTU3ODExRTJCRDA1QzVBQkU4NEFCNTNGIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6MjFERThBNzlBNTc4MTFFMkJEMDVDNUFCRTg0QUI1M0YiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6MjFERThBN0FBNTc4MTFFMkJEMDVDNUFCRTg0QUI1M0YiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz4ZWG1TAAAHw0lEQVR42uSYa1BU5xnH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";

            public static void AddCompanyImage(XmlDocument xDoc, bool verifyStatus)
            {
                XmlElement xe = xDoc.CreateElement("image");
                if (verifyStatus)
                    xe.InnerText = "Signature Valid";
                else xe.InnerText = "Signature Invalid";
                xe.SetAttribute("URI", ImageEmbed(verifyStatus));
                xDoc.DocumentElement.AppendChild(xe);
            }
            public static void AddCustomerImage(XmlDocument xDoc, bool verifyStatus)
            {
                XmlElement xe = xDoc.CreateElement("imageClient");
                if (verifyStatus)
                    xe.InnerText = "Signature Valid";
                else xe.InnerText = "Signature Invalid";
                xe.SetAttribute("URI", ImageEmbed(verifyStatus));
                xDoc.DocumentElement.AppendChild(xe);
            }
            private static string ImageEmbed(bool verifyStatus)
            {
                string imageData;
                if (verifyStatus)
                    imageData = TRUE_IMAGE;
                else
                    imageData = FALSE_IMAGE;
                return "data:image/png;base64," + imageData;
            }
        }
        #endregion              
    }
}
