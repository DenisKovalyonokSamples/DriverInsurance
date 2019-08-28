using Foundation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.iOS.Managers
{
    public static class DataConverter
    {
        public static DateTime NSDateToDateTime(NSDate date)
        {
            DateTime reference = new DateTime(2001, 1, 1, 0, 0, 0);
            DateTime currentDate = reference.AddSeconds(date.SecondsSinceReferenceDate);
            DateTime localDate = currentDate.ToLocalTime();

            return localDate;
        }

        public static NSDate DateTimeToNSDate(DateTime date)
        {
            DateTime reference = TimeZone.CurrentTimeZone.ToLocalTime(
                new DateTime(2001, 1, 1, 0, 0, 0));
            return NSDate.FromTimeIntervalSinceReferenceDate(
                (date - reference).TotalSeconds);
        }
    }
}
