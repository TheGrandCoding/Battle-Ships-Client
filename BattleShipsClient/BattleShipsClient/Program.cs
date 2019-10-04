using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;

namespace BattleShipsClient
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
         static string LogName;
        public static void Log(string message)
        {
            StreamWriter swAppend = File.AppendText(LogName);
            swAppend.WriteLine("["+DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString()+":"+ DateTime.Now.Second.ToString()+"]"+" - " +message);
            swAppend.Close();
        }
        public static void MakeLog()
        {
            if (!Directory.Exists("Logs/"))
            {
                Directory.CreateDirectory("Logs");
            }
            LogName = $"logs/" + DateTime.Today.Day.ToString() + " " + DateTime.Today.Month.ToString() + " & " + DateTime.Now.Hour.ToString() + ";" + DateTime.Now.Minute.ToString() + ".txt";
            StreamWriter swNew = File.CreateText(LogName);
            swNew.Close();
        }
    }
}
