using System;
using System.Collections.Generic;
using System.Text;
using AppScheduler.Helper;
using System.Text.RegularExpressions;

namespace AppScheduler.Helper
{
    public class TaskHelper
    {
        public static DateTime GetNextTimeForWeek(Task task)
        {
            DateTime result = task.NextTime;
            DateTime startDate = new DateTime();
            //Kiem tra ngay cuoi tuan de lay 

            int test = (int)task.NextTime.DayOfWeek;
            if ((int)task.NextTime.DayOfWeek == DateTimeHelper.GetMaxNeedDay(task.Detail))
            {
                //Get day the first of week
                startDate = task.NextTime.AddDays(7 * (Convert.ToInt32(task.Recur)))
                .AddDays(-(((task.NextTime.DayOfWeek - DayOfWeek.Monday) + 7) % 7));
            } else
            {
                //Get day the first of week
                startDate = task.NextTime
                .AddDays(-(((task.NextTime.DayOfWeek - DayOfWeek.Monday) + 7) % 7));
            }

            List<DateTime> listDateTime = DateTimeHelper.GetListDateTime(startDate, startDate.AddDays(7));
            foreach (DateTime tempDate in listDateTime)
            {
                //get date > nextTime Date
                if (tempDate > task.NextTime)
                {
                    int i = (int)tempDate.DayOfWeek;

                    if (DateTimeHelper.CheckDayOfWeek(task.Detail, tempDate))
                    {
                        result = tempDate;
                        break;
                    }
                }
            }
            return result;
        }

        public static DateTime GetNextTimeForMonth(Task task)
        {
            DateTime result = task.NextTime;
            string[] arrMonth = task.Recur.ToString().Split(';');
            string[] arrDayTh = task.Detail.ToString().Split(';');

            List<int> listIntMonth = ConvertToInt(arrMonth);
            List<int> listIntDayTh = ConvertToInt(arrDayTh);

            int hh = task.NextTime.Hour;
            int mm = task.NextTime.Minute;
            int ss = task.NextTime.Second;
            int year = task.NextTime.Year;

            List<DateTime> ListDateOfMonth=GetListDateOfMonth(year,listIntMonth, listIntDayTh,hh,mm,ss);

            if(task.NextTime > ListDateOfMonth[ListDateOfMonth.Count-1])
            {
                ListDateOfMonth= GetListDateOfMonth(year+1, listIntMonth, listIntDayTh, hh, mm, ss);
            }

            foreach (DateTime date in ListDateOfMonth)
            {
                if (date > task.NextTime)
                {
                    result = date;
                    break;
                }
            }

            return result;
        }

        private static List<DateTime> GetListDateOfMonth(int year,List<int> listIntMonth, List<int> listIntDayTh, int hh, int mm, int ss)
        {
            List<DateTime> listResult = new List<DateTime>();
            for(int i = 0; i < listIntMonth.Count; i++)
            {
                for(int j = 0; j < listIntDayTh.Count; j++)
                {
                    DateTime temp = new DateTime(year, listIntMonth[i], listIntDayTh[j], hh, mm, ss);
                    listResult.Add(temp);
                }
            }
            return listResult;
        }

        public static  List<int> ConvertToInt(string[] arrString)
        {
            List<int> listResult = new List<int>();
            foreach (string str in arrString)
            {
                listResult.Add(Convert.ToInt32(str));
            }
            return listResult;
        }

    }
}
