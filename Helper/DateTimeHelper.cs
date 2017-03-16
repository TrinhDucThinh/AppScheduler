using System;
using System.Collections.Generic;
using System.Linq;

namespace AppScheduler.Helper
{
    public static class DateTimeHelper
    {
        public static List<DateTime> GetListDateTime(DateTime startDate, DateTime endDate)
        {
            //the number of days in our range of dates
            var numDays = (int)((endDate - startDate).TotalDays);
            List<DateTime> myDates = Enumerable
                       //creates an IEnumerable of ints from 0 to numDays
                       .Range(0, numDays)
                       //now for each of those numbers (0..numDays),
                       //select startDate plus x number of days
                       .Select(x => startDate.AddDays(x))
                       //and make a list
                       .ToList();
            return myDates;
        }
        public static string[] SplitNeedDay(string needDays)
        {
            string[] arrDay = needDays.Split(';');
            for (int i = 0; i < arrDay.Length; i++)
            {
                switch (arrDay[i])
                {
                    case "D0":
                        arrDay[i] = "0";
                        break;
                    case "D1":
                        arrDay[i] = "1";
                        break;
                    case "D2":
                        arrDay[i] = "2";
                        break;
                    case "D3":
                        arrDay[i] = "3";
                        break;
                    case "D4":
                        arrDay[i] = "4";
                        break;
                    case "D5":
                        arrDay[i] = "5";
                        break;
                    case "D6":
                        arrDay[i] = "6";
                        break;
                    default:
                        arrDay[i] = "0";
                        break;
                }
            }
            return arrDay;
        }

        public static int GetMaxNeedDay(string needDays)
        {
            int max = -1;
            string[] arrDay = SplitNeedDay(needDays);
            foreach(string str in arrDay)
            {
                if (Convert.ToInt32(str) > max) max = Convert.ToInt32(str);
            }
            return max;
        }

        public static bool CheckDayOfWeek(string needDays,DateTime dayCheck)
        {
            string[] arrDay = SplitNeedDay(needDays);
            string test = ((int)dayCheck.DayOfWeek).ToString();
            if (arrDay.Contains(((int)dayCheck.DayOfWeek).ToString()))
            {
                return true;
            }
            return false;
        }
    }
}