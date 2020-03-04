using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alloy;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.IO;

namespace AlloyCompiler
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length <= 0)
                Logging.LogWarning("AlloyCompiler", "No input files were provided!");
            else
            {
                foreach (string s in args)
                    Compile(s);
            }

            Console.Read();
        }

        public static void Compile(string path)
        {
            if (Path.GetExtension(path) == ".cs")
            {
                string code = string.Empty;
                using (StreamReader r = new StreamReader(path, Encoding.UTF8))
                    while (r.Peek() >= 0)
                        code += r.ReadLine();
                CSharpCodeProvider provider = new CSharpCodeProvider();
                CompilerParameters parameters = new CompilerParameters();
                parameters.ReferencedAssemblies.Add((new Uri(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase))).LocalPath + @"\AlloyEngine.dll");
                parameters.ReferencedAssemblies.Add("System.dll");
                parameters.ReferencedAssemblies.Add("System.Core.dll");
                parameters.ReferencedAssemblies.Add("System.Runtime.dll");
                parameters.ReferencedAssemblies.Add("mscorlib.dll");
                parameters.ReferencedAssemblies.Add("netstandard.dll");
                parameters.GenerateInMemory = false;
                parameters.GenerateExecutable = false;
                parameters.OutputAssembly = Path.GetFileNameWithoutExtension(path) + ".dll";
                Logging.LogInfo("Alloy Compiler", $"Compiling {path}...");
                CompilerResults results = provider.CompileAssemblyFromSource(parameters, code);
                if (results.Errors.HasErrors)
                {
                    Logging.LogError("Alloy Compiler", "Compilation unsuccessful!");
                    foreach (CompilerError e in results.Errors)
                        Logging.LogError("Alloy Compiler", $"\t{e.ErrorNumber} - {e.ErrorText}");
                }
                else
                    Logging.LogInfo("Alloy Compiler", "Compilation successful!");
            }
            else
                Logging.LogError("Alloy Compiler", $"{path} is not a valid source file!");
        }
    }
}
