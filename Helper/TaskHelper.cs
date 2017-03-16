using System;
using System.Collections.Generic;
using System.Text;
using AppScheduler.Helper;
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
                startDate = task.NextTime.AddDays(7*((int)task.Recur))
                .AddDays(-(((task.NextTime.DayOfWeek - DayOfWeek.Monday) + 7) % 7));
            }else
            {
                startDate = task.NextTime
                .AddDays(-(((task.NextTime.DayOfWeek - DayOfWeek.Monday) + 7) % 7));
            }
           
            List<DateTime> listDateTime = DateTimeHelper.GetListDateTime(startDate, startDate.AddDays(7));
            foreach(DateTime tempDate in listDateTime)
            {
                //get date > nextTime Date
                if (tempDate>task.NextTime)
                {
                    int i = (int)tempDate.DayOfWeek;

                    if (DateTimeHelper.CheckDayOfWeek(task.Detail,tempDate))
                    {
                        result=tempDate;
                        break;
                    }
                   
                }
            }
            return result;
        }
    }
}
