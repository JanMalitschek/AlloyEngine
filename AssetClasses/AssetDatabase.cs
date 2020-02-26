using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Alloy.Assets
{
    public class AssetDatabase
    {
        private static List<Asset> loadedAssets = new List<Asset>();
        private static List<Tuple<string, string>> assetsInProject = new List<Tuple<string, string>>();
        public static void Init()
        {
            Logging.LogInfo("Asset Database", "Loading Asset Database!");
            if (System.IO.File.Exists("AssetDatabase.xml"))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load("AssetDatabase.xml");
                XmlNode root = xml.SelectSingleNode("//AssetDatabase");
                foreach(XmlNode asset in root.ChildNodes)
                    assetsInProject.Add(new Tuple<string, string>(asset.Attributes["path"].Value, asset.Attributes["type"].Value));
            }
            else
            {
                Logging.LogError("Asset Database", "Could not find AssetDatabase.xml!");
            }
        }
    }
}
