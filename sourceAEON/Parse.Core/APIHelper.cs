using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core
{
    public class APIHelper
    {        
        public static string PublishInv(string xmlData, string pattern, string serial = null)
        {
            try
            {
                var company = AppContext.Current.company;
                string apiurl = company.Domain;
                string username = company.UserName;
                string password = company.PassWord;
                string data = "{'xmlData':'" + xmlData + "','pattern':'" + pattern + "','serial':'" + serial + "'}";
                string dt = callApi(apiurl, "api/publish/importAndPublishInv", data, username, password);
                return dt;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string AdjustInv(string xmlData, string fkey, string pattern, string serial, string no)
        {
            try
            {
                var company = AppContext.Current.company;
                string apiurl = company.Domain;
                string username = company.UserName;
                string password = company.PassWord;
                string data = "{'xmlData':'" + xmlData + "','fkey':'" + fkey + "','pattern':'" + pattern + "','serial':'" + serial + "', 'invNo':'" + no + "'}";
                string result = callApi(apiurl, "api/business/adjustInv", data, username, password);
                return result;

            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }
        public static string ReplaceInv(string xmlData, string fkey, string pattern, string serial, string no)
        {
            try
            {
                var company = AppContext.Current.company;
                string apiurl = company.Domain;
                string username = company.UserName;
                string password = company.PassWord;
                string data = "{'xmlData':'" + xmlData + "','fkey':'" + fkey + "','pattern':'" + pattern + "','serial':'" + serial + "', 'invNo':'" + no + "'}";
                string result = callApi(apiurl, "api/business/replaceInv", data, username, password);
                return result;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string ConvertInv(string fkey,string pattern, string serial, string no)
        {
            try
            {
                var company = AppContext.Current.company;
                string apiurl = company.Domain;
                string username = company.UserName;
                string password = company.PassWord;
                string data = "{'fkey':'" + fkey + "', 'pattern':'" + pattern + "','serial':'" + serial + "', 'invNo':'" + no + "'}";
                string result = callApi(apiurl, "api/convertinv/convertForStore", data, username, password);
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public static string callApi(string apiUri, string action, string data = null, string username = null, string password = null, Method method = Method.POST)
        {

            var client = new RestClient(apiUri);
            // using Proxy
            client.Proxy = WebRequest.DefaultWebProxy;
            var request = new RestRequest(action);
            request.Method = method;
            request.AddHeader("Content-Type", "application/json");
            if (data != null)
                request.AddParameter("application/json", data, ParameterType.RequestBody);
            //Tạo dữ liệu Authentication
            string value = makeAuthenticationString(request.Method, username, password);
            request.AddHeader("Authentication", value);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return "ERR:1";
            if (response.StatusCode != HttpStatusCode.OK)
            {                
                return "ERR:2";
            }
            return response.Content;
        }

        public static string makeAuthenticationString(Method method, string username, string password)
        {
            //Calculate UNIX time     
            DateTime epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = DateTime.UtcNow - epochStart;
            string Timestamp = Convert.ToUInt64(timeSpan.TotalSeconds).ToString();
            //Mã duy nhất
            string nonce = Guid.NewGuid().ToString("N").ToLower();
            //Tạo dữ liệu mã hóa
            string signatureRawData = String.Format("{0}{1}{2}", method.ToString().ToUpper(), Timestamp, nonce);
            MD5 md5 = MD5.Create();
            var hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(signatureRawData));
            var signature = Convert.ToBase64String(hash);
            return string.Format("{0}:{1}:{2}:{3}:{4}", signature, nonce, Timestamp, username, password);
        }
    }
}
