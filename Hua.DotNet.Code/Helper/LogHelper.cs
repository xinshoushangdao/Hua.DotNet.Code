using System;
using System.Diagnostics;
using System.IO;

namespace Hua.DotNet.Code.Helper
{
    /// <summary>
    /// 简单日志帮助类
    /// </summary>
    public class LogHelper
    {
        private static string LogFileName => $"Log{DateTime.Now:yyyy_MM_dd}.log";

        public static void Log(string title)
        {
            Log(title, string.Empty);
        }

        public static void Log(Exception e)
        {
            Log(e.Message, e.StackTrace);
        }


        public static void Log<T>(string title)
        {
            Log<T>(title, string.Empty);
        }

        public static void Log<T>(string title, string? msg)
        {
            Log(title, $"{typeof(T).FullName} \t {msg}");
        }


        public static void Log(string title, string? msg)
        {
            using var fs = new FileStream(LogFileName, FileMode.OpenOrCreate);
            var textWriterTraceListener = new TextWriterTraceListener(fs);
            textWriterTraceListener.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss:FFF}\t [{title}]\t{msg}");
            textWriterTraceListener.Close();
            fs.Close();
        }
    }
}