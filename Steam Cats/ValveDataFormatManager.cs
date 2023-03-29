using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steam_Cats
{
    internal class ValveDataFormatManager
    {
        public static string INVALID_FILE = "This is not a VDF file!";

        private string _filepath;

        public List<string> ParseCategories()
        {
            List<string> categoriesParsed = new List<string>();

            return categoriesParsed;
        }

        public void SetFilePath(string filepath)
        {
            _filepath = filepath;
        }

        public static bool IsValidVDFFile(string filepath)
        {
            throw new NotImplementedException();
        }
    }
}
