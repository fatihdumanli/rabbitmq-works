using System;

namespace LogEmitter
{
    public class LogRandomizerFactory 
    {
        public LogItem GetRandomLogItem() 
        {
            LogItem item = new LogItem();

            Random r = new Random();
            int num = r.Next(50);

            if(num % 3 == 0)
            {
                item.Severity = LogSeverity.Info;
            } 
            else if(num % 3 == 1) 
            {
                item.Severity = LogSeverity.Warning;
            } 
            else if(num % 3 == 2)
            {
                item.Severity = LogSeverity.Error;
            }

            item.TimeStamp = DateTime.Now;
            item.Message = "This is a sample log message created by a randomizer.";

            return item;
        }

    }

}