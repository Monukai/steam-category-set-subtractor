using System.Net;
using Newtonsoft.Json.Linq;

namespace Steam_Cats
{
    public class SteamAPIFacilitator
    {
        // using a Steam App ID, queries the storefront API to get the app name
        public bool GetAppName(int appId, out string gameName)
        {
            gameName = string.Empty;
            string result = String.Empty;
            bool success = false;

            success = TryDownloadString(String.Format("https://store.steampowered.com/api/appdetails?appids={0}&l=english", appId), out result);
            SteamAPICount.Instance.IncremementCounter();

            if (success == false)
            {
                return success;
            }

            JObject jsonResult = JObject.Parse(result);

            string appID = appId.ToString();

            if (jsonResult == null || jsonResult[appID] == null || jsonResult[appID]["data"] == null)
            {
                return false;
            }

            JToken appData = jsonResult[appID]["data"];

            gameName = (string)appData["name"] ?? "";

            return true;
        }

        private static bool TryDownloadString(string url, out string result)
        {
            WebClient webClient = new WebClient();
            webClient.Encoding = System.Text.Encoding.UTF8;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            try
            {
                result = webClient.DownloadString(url);
            }
            catch (WebException e)
            {
                result = String.Empty;
                return false;
            }

            return true;
        }
    }

    public sealed class SteamAPICount
    {
        private SteamAPICount()
        {
            apiCounter = 0;
        }

        private static SteamAPICount steamApiCount = null;
        public static SteamAPICount Instance
        {
            get
            {
                if (steamApiCount == null)
                {
                    steamApiCount = new SteamAPICount();
                }

                return steamApiCount;
            }
        }

        private int apiCounter;
        // timeout in milliseconds
        private const int STEAMAPI_TIMEOUT = 5 * 60 * 1000;

        public void IncremementCounter()
        {
            apiCounter++;

            if (apiCounter >= 200)
            {
                System.Threading.Thread.Sleep(STEAMAPI_TIMEOUT);
                ResetCount();
            }
        }

        public void ResetCount()
        {
            apiCounter = 0;
        }
    }
}