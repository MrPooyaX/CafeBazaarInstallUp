using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CafeBazaarInstallCountUpper
{
    public partial class Form1 : Form
    {
        string clientVersion = "10.1.0";
        string clientVersionCode = "1000100";

        string androidId = "";
        string clientID = "";
        string deviceID = "";
        string packageName = "";
        string vCode = "";
        string adId = "";

        string manufacturer = "";
        string model = "";

        public static string accessToken = "";
        Random r = new Random();
        List<string> devices = new List<string>();
        string sdkversion = "";
        string[] sdkversions = { "24", "25", "26", "27", "28", "29" };
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }
        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public string authorize()
        {
            string data = "{\"properties\":{\"androidClientInfo\":{\"adId\":\"" + adId + "\",\"adOptOut\":false,\"androidId\":\"" + androidId + "\",\"city\":\"NA\",\"country\":\"NA\",\"cpu\":\"arm64-v8a,armeabi-v7a,armeabi\",\"device\":\"\",\"deviceType\":0,\"dpi\":320,\"hardware\":\"\",\"height\":1600,\"locale\":\"\",\"manufacturer\":\"" + manufacturer + "\",\"mcc\":432,\"mnc\":35,\"mobileServiceType\":1,\"model\":\"" + model + "\",\"osBuild\":\"\",\"product\":\"" + model + "\",\"province\":\"NA\",\"sdkVersion\":"+ sdkversion + ",\"width\":900},\"appThemeState\":0,\"clientID\":\"" + clientID + "\",\"clientVersion\":\"" + clientVersion + "\",\"clientVersionCode\":" + clientVersionCode + ",\"isKidsEnabled\":false,\"language\":2},\"singleRequest\":{\"registerDeviceRequest\":{\"gcmID\":\"\",\"hmsID\":\"\",\"systemLanguage\":\"fa\"}}}";
            string requestRes = Request.POSTRequest(data, Request.RegisterDeviceRequest);
            JObject json = JObject.Parse(requestRes);
            deviceID = json["singleReply"]["registerDeviceReply"]["deviceID"].ToString();
            if (requestRes.Contains("\"statusCode\":200"))
            {
                return "success";
            }
            return "error";
        }
     
        public string appdetail()
        {
            string data = "{\"properties\":{\"androidClientInfo\":{\"adId\":\"" + adId + "\",\"adOptOut\":false,\"androidId\":\"" + androidId + "\",\"city\":\"NA\",\"country\":\"NA\",\"cpu\":\"arm64-v8a,armeabi-v7a,armeabi\",\"device\":\"\",\"deviceType\":0,\"dpi\":320,\"hardware\":\"\",\"height\":1600,\"locale\":\"\",\"manufacturer\":\"" + manufacturer + "\",\"mcc\":432,\"mnc\":35,\"mobileServiceType\":1,\"model\":\"" + model + "\",\"osBuild\":\"\",\"product\":\"" + model + "\",\"province\":\"NA\",\"sdkVersion\":"+ sdkversion + ",\"width\":900},\"appThemeState\":0,\"clientID\":\"" + clientID + "\",\"clientVersion\":\"" + clientVersion + "\",\"clientVersionCode\":" + clientVersionCode + ",\"isKidsEnabled\":false,\"language\":2},\"singleRequest\":{\"appDetailsV2Request\":{\"packageName\":\"" + packageName + "\",\"referrers\":" + "[]" + " }}}";
            string requestRes = Request.POSTRequest(data, Request.AppDetailsV2Request);
            JObject json = JObject.Parse(requestRes);
            if (requestRes.Contains("\"statusCode\":200"))
            {
                vCode = json["singleReply"]["appDetailsV2Reply"]["package"]["versionCode"].ToString();
                return "success";
            }
            return "error";
        }
        public string UpgradableApps()
        {
            
            string data = "{\"properties\":{\"androidClientInfo\":{\"adId\":\"\",\"adOptOut\":false,\"androidId\":\"" + androidId + "\",\"city\":\"NA\",\"country\":\"NA\",\"cpu\":\"arm64-v8a,armeabi-v7a,armeabi\",\"device\":\"\",\"dpi\":480,\"hardware\":\"\",\"height\":1794,\"locale\":\"\",\"manufacturer\":\"HUAWEI\",\"mcc\":432,\"mnc\":35,\"model\":\"EVA-L19\",\"osBuild\":\"\",\"product\":\"EVA-L19\",\"province\":\"NA\",\"sdkVersion\":"+ sdkversion + ",\"width\":1080},\"clientID\":\"" + clientID + "\",\"clientVersion\":\"" + clientVersion + "\",\"clientVersionCode\":" + clientVersionCode + ",\"isKidsEnabled\":false,\"language\":2},\"singleRequest\":{\"getUpgradableAppsRequest\":{\"deviceID\":"+ deviceID + ",\"installedAppsInfo\":[{\"installDelta\":105411,\"isPreInstall\":false,\"packageName\":\""+packageName+"\",\"signs\":[\"1D1886AB\"],\"updateDelta\":105411,\"versionCode\":"+vCode+"}],\"sdkVersion\":"+ sdkversion + "}}}";
            string requestRes = Request.POSTRequest(data, Request.GetUpgradableAppsRequest);
            if (requestRes.Contains("\"statusCode\":200"))
            {
                return "success";
            }
            return "error";
        }
        private void loadDevices()
        {
            StreamReader sr = new StreamReader("devices.txt");
            while (!sr.EndOfStream)
            {
                try
                {
                    string line = sr.ReadLine();
                    if (!line.Equals(""))
                        devices.Add(line);
                }
                catch { }
            }
            sr.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(hamle))
            {
                IsBackground = true
            }.Start();
        }
        int success = 0;
        int bad = 0;
        private void hamle()
        {
            sdkversion = sdkversions[r.Next(sdkversions.Count())];
            loadDevices();
            packageName = pkgName.Text;
            appdetail();
            VersionCode.Text = vCode;
            StreamWriter swsa = new StreamWriter("apps.txt", true);
            swsa.WriteLine(packageName);
            swsa.Close();

        re:
            androidId = RandomString(16);
            clientID = RandomString(15) + "-" + RandomString(6);

            string dev = devices[r.Next(devices.Count)];
            manufacturer = dev.Split(':')[0];
            model = dev.Split(':')[1];
            sdkversion = sdkversions[r.Next(sdkversions.Count())];
            //vCode = versionCode.Text;
            int sleep = int.Parse(txtSleep.Text);
            int count = int.Parse(txtCount.Text);

            try
            {
                if (authorize().Equals("success"))
                {
                hhheee:
                    try
                    {
                        StreamWriter sw = new StreamWriter(packageName + ".txt", true);
                        sw.WriteLine(androidId + "|" + clientID + "|" + manufacturer + "|" + model);
                        sw.Close();
                    }
                    catch
                    {
                        Thread.Sleep(r.Next(50, 300));
                        goto hhheee;
                    }
                    if (UpgradableApps().Equals("success"))
                    {
                        success++;
                        label3.Text = success.ToString();
                        if (success >= count)
                        {
                            MessageBox.Show("finish");
                            return;
                        }
                        if (sleep > 0)
                        {
                            Thread.Sleep(sleep);
                        }
                        goto re;
                    }
                }
            }
            catch { }
            bad++;
            label4.Text = bad.ToString();
            goto re;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            if (!File.Exists("apps.txt"))
            {
                File.Create("apps.txt").Close();
            }
        }
    }
}
