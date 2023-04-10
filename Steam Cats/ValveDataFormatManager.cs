using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VdfConverter;
using VdfParser;
using System.Text.RegularExpressions;

namespace Steam_Cats
{
    internal class ValveDataFormatManager
    {
        public static string INVALID_FILE = "This is not a VDF file, or the sharedconfig.vdf file selected has no Steam category data.";
        public static string TYPICAL_FILE_PATH = @"C:\Program Files (x86)\Steam\userdata\<steam id>\7\remote\sharedconfig.vdf";

        private string _filepath;
        private bool _pathParsed;
        public string Filepath
        {
            get
            {
                return _filepath;
            }
            private set
            {
                _filepath = value.Trim();
                _pathParsed = false;
            }
        }

        private List<string> _categoriesParsed;

        public ValveDataFormatManager()
        {
            _filepath = "";
            _categoriesParsed = new List<string>();
            _pathParsed = false;
        }

        public bool ParseCategories(ref List<string> catsParsed, bool forceReparse = false)
        {

            if (_categoriesParsed.Count > 0 && _pathParsed == true)
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

            // Some sharedconfig.vdf files have differing capitlization in their serialization keys, use GetTags method to resolve these descrepencies
            IDictionary<string, dynamic> apps;
            apps = GetTags(result);

            if (apps == null)
            {
                return false;
            }

            foreach (string appKey in apps.Keys)
            {
                if (((IDictionary<string, object>)apps[appKey]).ContainsKey("tags") == true)
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
            }

            sharedConfig.Close();
            catsParsed = _categoriesParsed;
            return true;
        }
        
        // TODO: Consider rewriting GetTags and DissectDes. with bool output for error checking, and making the returned IDict an out param
        private static IDictionary<string, dynamic>? GetTags(dynamic result)
        {
            result = DissectDeserializedVDF("userroamingconfigstore", result);
            result = DissectDeserializedVDF("software", result);
            result = DissectDeserializedVDF("valve", result);
            result = DissectDeserializedVDF("steam", result);
            result = DissectDeserializedVDF("apps", result);

            return result as IDictionary<string, dynamic>;
        }
        private static dynamic DissectDeserializedVDF(string vdfKey, dynamic parseResult)
        {
            foreach (string key in ((IDictionary<string, object>)parseResult).Keys)
            {
                if (Regex.IsMatch(key, vdfKey, RegexOptions.IgnoreCase) == true)
                {
                    var subset = parseResult as IDictionary<string, object>;
                    parseResult = subset[key];
                    return parseResult;
                }
            }

            return parseResult;
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

            IDictionary<string, dynamic> apps;
            apps = GetTags(result);

            if (apps == null)
            {
                return false;
            }

            foreach (string appKey in apps.Keys)
            {
                if (((IDictionary<string, object>)apps[appKey]).ContainsKey("tags") == true)
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
            }

            appIds = catAppIds;

            sharedConfig.Close();
            return true;
        }

        public void SetFilePath(string filepath)
        {
            Filepath = filepath;
        }

        public bool HasValidVDFFile()
        {
            return IsValidVDFFile(Filepath);
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
                //TODO: If GetTags is rewritting to return bool, reflect that here
                IDictionary<string, dynamic> apps;
                apps = GetTags(result);
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
