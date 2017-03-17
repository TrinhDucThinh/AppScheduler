using System;
using System.Collections.Generic;
using System.Linq;

namespace AppScheduler.Helper
{
    public static class DateTimeHelper
    {
        //get all day from startDate to endDate
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

        //get string day in weekend 
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

        //get day max in weekend choosed
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


        //get max day in month choosed
        public static int GetMaxDayTh(List<int> arrIntTh)
        {
            int max = -1;
            for(int i = 0; i < arrIntTh.Count; i++)
            {
                if (arrIntTh[i] > max) max = arrIntTh[i];
            }
            return max;
        }

        //Check DateTime has been contained in the day choosing 
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

        //get all dayth of month by year
        public static List<DateTime>GetDateOfMonth(int month, int year)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(year, month))  // Days: 1, 2 ... 31 etc.
                   .Select(day => new DateTime(year, month, day)) // Map each day to a date
                   .ToList(); // Load dates into a list
        }

        //get
        public static DateTime GetDayTh(int dayTh, int month, int year,int hh,int mm,int ss)
        {
            DateTime resultDate = new DateTime();
            List<DateTime> listDateOfMonth = GetDateOfMonth(month, year);
            foreach(DateTime date in listDateOfMonth)
            {
                if (date.Day == dayTh)
                {
                    resultDate = date;
                    break;
                }
            }
            string ResultDateTime = resultDate.ToString("yyyy-MM-dd HH:mm:ss");
            return resultDate;
        }
    }
}