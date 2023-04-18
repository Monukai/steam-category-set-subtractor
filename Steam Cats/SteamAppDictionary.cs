using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Steam_Cats
{
    internal class SteamAppDictionary : Dictionary<int, string>
    {
        private bool _hasLoaded;
        private bool _isLoading;
        private string _apiPath = @"https://api.steampowered.com/ISteamApps/GetAppList/v2/";

        public SteamAppDictionary() : base()
        {
            _hasLoaded = false;
            _isLoading = false;
        }

        public async Task LoadSteamAppData()
        {
            _isLoading = true;
            var asyncResult = await TryDownloadString(_apiPath);

            if (asyncResult.Success == true)
            {
                // parse logic
                JObject jsonResult = JObject.Parse(asyncResult.Result);
                string applistText = "applist";
                string appsText = "apps";


                if (jsonResult == null || jsonResult[applistText] == null || jsonResult[applistText][appsText] == null)
                {
                    _isLoading = false;
                    return;
                }

                JToken appData = jsonResult[applistText][appsText];
                int appId = 0;

                foreach (JObject app in appData)
                {
                    Int32.TryParse(app["appid"].ToString(), out appId);
                    if (this.ContainsKey(appId) == false)
                    {
                        this.Add(appId, app["name"].ToString());
                    }
                    appId = 0;
                }

                _isLoading = false;
                _hasLoaded = true;
            }

            return;
        }

        private static async Task<(bool Success, string Result)> TryDownloadString(string url)
        {
            String result = String.Empty;
            HttpClient httpClient = new HttpClient();
            //httpClient.Encoding = System.Text.Encoding.UTF8;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            try
            {
                result = await httpClient.GetStringAsync(url);
            }
            catch (WebException e)
            {
                result = String.Empty;
                return (false, result);
            }
            catch (HttpRequestException e)
            {
                result = String.Empty;
                return (false, result);
            }

            return (true, result);
        }

        public bool HasLoaded()
        {
            return _hasLoaded;
        }

        public bool IsLoading()
        {
            return _isLoading;
        }
    }
}
