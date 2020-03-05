using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;
using Alloy.Utility;

namespace Alloy.User
{
    public static class UserCodeDatabase
    {
        private static List<Assembly> loadedAssemblies = new List<Assembly>();
        private static List<string> assemblyPaths = new List<string>();
        public static void LoadAssembly(string path)
        {
            try
            {
                loadedAssemblies.Add(Assembly.LoadFrom(path));
                assemblyPaths.Add(path);
            }
            catch(Exception e)
            {
                Logging.LogError("User Code Database", e.Message);
            }
        }

        public static void Init()
        {
            Logging.LogInfo("User Code Database", "Loading User Code Database!");
            XMLAbstraction data = new XMLAbstraction("UserCodeDatabase", "UserCodeDatabase.xml");
            var assemblyNodes = data.GetNodes("//UserCodeDatabase/Assemblies/Assembly");
            foreach (var a in assemblyNodes)
                LoadAssembly(a.InnerText);
        }

        public static void WriteDatabase()
        {
            XMLAbstraction data = new XMLAbstraction("UserCodeDatabase");
            var assembliesNode = data.AddNode("Assemblies");
            foreach (string s in assemblyPaths)
                assembliesNode.AddNode("Assembly", s);
            data.Save("UserCodeDatabase.xml");
        }

        public static Type GetType(string fullName)
        {
            foreach (Assembly a in loadedAssemblies)
                foreach (Type t in a.GetTypes())
                    if (t.FullName == fullName)
                        return t;
            return Type.GetType(fullName);
        }
    }
}
