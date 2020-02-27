using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alloy
{
    public static class Logging
    {
        public static void LogInfo(object source, object message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Info");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"::{source}::{message}\n");
        }
        public static void LogInfo(string source, object message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Info");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"::{source}::{message}\n");
        }

        public static void LogWarning(object source, object message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Warning");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"::{source}::{message}\n");
        }
        public static void LogWarning(string source, object message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Warning");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"::{source}::{message}\n");
        }

        public static void LogError(object source, object message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Error");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"::{source}::{message}\n");
        }
        public static void LogError(string source, object message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Error");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"::{source}::{message}\n");
        }

        public static void LogSimple(object message)
        {
            Console.WriteLine($"{message}");
        }
    }
}
