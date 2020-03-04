using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;

namespace Alloy.User
{
    public static class UserCodeDatabase
    {
        private static List<Assembly> loadedAssemblies = new List<Assembly>();
        public static void LoadAssembly(string path)
        {         
            loadedAssemblies.Add(Assembly.LoadFrom(path));
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
