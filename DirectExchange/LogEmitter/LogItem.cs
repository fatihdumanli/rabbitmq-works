using System;

namespace LogEmitter
{
    public static class LogSeverity 
    {
        public static string Info = "Info";
        public static string Warning = "Warning";

        public static string Error = "Error";

    }
    public class LogItem
    {
        public string Severity { get; set; }
        public DateTime TimeStamp { get; set; } 
        public string Message { get; set; }
               
    }
}