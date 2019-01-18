using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class Log
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("WebLogger");

        /// <summary>
        /// Print Info
        /// </summary>
        /// <param name="msg"></param>
        public static void Info(string msg)
        {
            string str = @DateTime.Now.ToString() + " " + "[Info]\t" + msg.ToString() + "\t";
            Console.WriteLine(str);
            log.Info(str);
        }

        /// <summary>
        /// Print Info
        /// </summary>
        /// <param name="msg"></param>
        public static void Info(string msg, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            string str = @DateTime.Now.ToString() + " " + "[Info]\t" + msg.ToString() + "\t";
            Console.WriteLine(str);
            Console.ResetColor();
            log.Info(str);
        }

        /// <summary>
        /// Debug Info
        /// </summary>
        /// <param name="msg"></param>
        public static void Debug(string msg)
        {
            string str = @DateTime.Now.ToString() + " " + "[Debug]\t" + msg.ToString() + "\t";
            Console.WriteLine(str);
            log.Info(str);
        }

        /// <summary>
        /// Print Error
        /// </summary>
        /// <param name="msg"></param>
        public static void Error(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            string str = @DateTime.Now.ToString() + " " + "[Debug]\t" + msg.ToString() + "\t";
            Console.WriteLine(str);
            Console.ResetColor();
            log.Info(str);
        }

    }
}
