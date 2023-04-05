using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VdfConverter;
using VdfParser;

namespace Steam_Cats
{
    internal class ValveDataFormatManager
    {
        public static string INVALID_FILE = "This is not a VDF file!";
        public static string TYPICAL_FILE_PATH = @"C:\Program Files (x86)\Steam\userdata\<steam id>\7\remote\sharedconfig.vdf";

        private string _filepath;
        private string Filepath
        {
            get
            {
                return _filepath;
            }
            set
            {
                _filepath = value.Trim();
            }
        }

        private List<string> _categoriesParsed;

        public ValveDataFormatManager()
        {
            _filepath = "";
            _categoriesParsed = new List<string>();
        }

        public bool ParseCategories(ref List<string> catsParsed, bool forceReparse = false)
        {

            // add file.close to this method
            if (_categoriesParsed.Count > 0)
            {
                if (forceReparse == true)
                {
                    _categoriesParsed = new List<string>();
                }
                else if (forceReparse == false)
                {
                    catsParsed = _categoriesParsed;
                    return true;
                }
            }

            FileStream sharedConfig = File.OpenRead(_filepath);
            VdfDeserializer parser = new VdfDeserializer();
            dynamic result;

            try
            {
                result = parser.Deserialize(sharedConfig);
            }
            catch (Exception ex)
            {
                sharedConfig.Close();
                return false;
            }

            var apps = result.UserRoamingConfigStore.Software.Valve.Steam.Apps as IDictionary<string, dynamic>;

            if (apps == null)
            {
                return false;
            }

            foreach (string appKey in apps.Keys)
            {
                var tags = apps[appKey].tags as IDictionary<string, dynamic>;

                if (tags != null)
                {
                    foreach (string tagIndex in tags.Keys)
                    {
                        if (_categoriesParsed.Contains(tags[tagIndex]) == false)
                        {
                            _categoriesParsed.Add(tags[tagIndex]);
                        }
                    }
                }
            }

            sharedConfig.Close();
            catsParsed = _categoriesParsed;
            return true;
        }

        public bool GetAppsForCategory(ref List<int> appIds, string steamCat)
        {
            List<int> catAppIds = new List<int>();

            FileStream sharedConfig = File.OpenRead(_filepath);
            VdfDeserializer parser = new VdfDeserializer();
            int numericId = 0;
            dynamic result;

            try
            {
                result = parser.Deserialize(sharedConfig);
            }
            catch (Exception ex)
            {
                sharedConfig.Close();
                return false;
            }

            var apps = result.UserRoamingConfigStore.Software.Valve.Steam.Apps as IDictionary<string, dynamic>;

            if (apps == null)
            {
                return false;
            }

            foreach (string appKey in apps.Keys)
            {
                var tags = apps[appKey].tags as IDictionary<string, dynamic>;

                if (tags != null)
                {
                    foreach (string tagIndex in tags.Keys)
                    {
                        if (steamCat == tags[tagIndex])
                        {
                            Int32.TryParse(appKey, out numericId);
                            catAppIds.Add(numericId);
                            numericId = 0;
                        }
                    }
                }
            }

            sharedConfig.Close();
            return true;
        }

        public void SetFilePath(string filepath)
        {
            Filepath = filepath;
        }

        public static bool IsValidVDFFile(string filepath)
        {
            if (File.Exists(filepath) == false)
            {
                return false;
            }

            FileStream sharedConfig = File.OpenRead(filepath);
            VdfDeserializer parser = new VdfDeserializer();

            try
            {
                dynamic result = parser.Deserialize(sharedConfig);
            }
            catch (Exception ex)
            {
                sharedConfig.Close();
                return false;
            }

            sharedConfig.Close();
            return true;
        }
    }
}
