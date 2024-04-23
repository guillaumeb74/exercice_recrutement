using System.IO;

namespace ExerciceRecrutementRestaurant
{
    // Manager used to store logs in a file and show it in the console
    public static class LogManager
    {

        // Append new log line in the log file
        public static void AppendLogLine(string status, string logLine)
        {
            DateTime dateTimeNow = DateTime.Now;
            string filePath = "log/" + dateTimeNow.Year + "-" + dateTimeNow.Month + "-" + dateTimeNow.Day + "-D4E_DCS_LoadBalancer.log";
            Directory.CreateDirectory("log");
            File.AppendAllText(filePath, DateTime.Now.ToShortTimeString() + " [" + status + "] " + logLine + Environment.NewLine);
            Console.WriteLine(logLine);
        }

    }
}
