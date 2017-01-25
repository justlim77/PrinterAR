using UnityEngine;
using System.Collections;
using System;

public static class Converter
{
    public static string ToMinutesAndSeconds(int seconds)
    {
        //string time = "";
        //int s = seconds % 60;
        //int m = seconds / 60;
        //TimeSpan.FromSeconds(seconds);
        TimeSpan timeSpan = new TimeSpan(0, 0, seconds);
        return string.Format("{0:D2}m:{1:D2}s",
            timeSpan.Minutes,
            timeSpan.Seconds);        
    }

    public static TimeSpan ToTimeSpanInMinsAndSecs(int seconds)
    {
        TimeSpan timeSpan = new TimeSpan(0, 0, seconds);
        return timeSpan;
    }

}
