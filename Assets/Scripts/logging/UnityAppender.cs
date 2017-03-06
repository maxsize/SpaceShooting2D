using UnityEngine;
using System;
using log4net.Appender;
using log4net.Core;

public class UnityAppender : AppenderSkeleton
{
    protected override void Append(LoggingEvent loggingEvent)
    {
        switch (loggingEvent.Level.Name)
        {
            case "Error":
                Debug.LogError(loggingEvent.MessageObject.ToString());
                break;
            case "All":
            case "Debug":
            default:
                Debug.LogFormat(loggingEvent.MessageObject.ToString(), new object[]{});
                break;
        }
    }
}