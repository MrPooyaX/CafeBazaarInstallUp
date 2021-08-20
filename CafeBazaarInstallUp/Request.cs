using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeBazaarInstallCountUpper
{
    class Request
    {

        public static string MainUrl = "https://api.cafebazaar.ir/rest-v1/process/";
        public static string RegisterDeviceRequest = MainUrl + "RegisterDeviceRequest";
        public static string GetOtpTokenRequest = MainUrl + "GetOtpTokenRequest";
        public static string VerifyOtpTokenRequest = MainUrl + "VerifyOtpTokenRequest";
        public static string AppDetailsV2Request = MainUrl + "AppDetailsV2Request";
        public static string SubmitReviewRequest = MainUrl + "SubmitReviewRequest";
        public static string getAccessTokenRequest = MainUrl + "getAccessTokenRequest";

        public static string RegisterDeviceAndGetInfo = MainUrl + "RegisterDeviceAndGetInfo";
        public static string GetPageByPathRequest = MainUrl + "GetPageByPathRequest";
        public static string SendActionLogRequest = MainUrl + "SendActionLogRequest";
        public static string GetUpgradableAppsRequest = MainUrl + "GetUpgradableAppsRequest";
        public static string AppDownloadInfoRequest = MainUrl + "AppDownloadInfoRequest";
        public static string userAgent = "okhttp/3.12.12";
        public static string POSTRequest(string POST_DATA, string post_url)
        {
            var client = new RestClient(post_url);
            var request = new RestRequest("", Method.POST);
            client.UserAgent = userAgent;
            client.FollowRedirects = false;
            client.Timeout = 5000;
            client.ReadWriteTimeout = 5000;
            request.AddParameter("application/json; charset=utf-8", POST_DATA, ParameterType.RequestBody);
            request.AddHeader("User-Agent", userAgent);
            if (!Form1.accessToken.Equals(""))
            {
                request.AddHeader("authorization", "Bearer " + Form1.accessToken);
            }
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            IRestResponse response1 = client.Execute(request);
            return response1.Content;
        }
        public static string GETRequest(string post_url)
        {
            var client = new RestClient(post_url);
            var request = new RestRequest("", Method.GET);
            client.Timeout = 5000;
            client.ReadWriteTimeout = 5000;
            client.UserAgent = userAgent;
            request.AddHeader("User-Agent", userAgent);
            if (!Form1.accessToken.Equals(""))
            {
                request.AddHeader("authorization", "Bearer " + Form1.accessToken);
            }
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            IRestResponse response1 = client.Execute(request);
            return response1.Content;
        }
    }
}
