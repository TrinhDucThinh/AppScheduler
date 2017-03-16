using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;


namespace AppScheduler.Helper
{
    public class XmlHelper
    {
        private string _filePath;

        public XmlHelper(string filePath)
        {
            _filePath = filePath;
        }

        public  List<Task> GetAll()
        {
            XDocument doc = XDocument.Load(_filePath);
            try
            {
                var list = (from c in doc.Descendants("Task")
                            select new Task
                            {
                                TaskId = (int)c.Element("TaskId"),
                                Name = (string)c.Element("Name"),
                                Action = (string)c.Element("Action"),
                                StartDate = (DateTime)c.Element("StartDate"),
                                RunTime = (string)c.Element("RunTime"),
                                NextTime = (DateTime)c.Element("NextTime"),
                                Type = (string)c.Element("Type"),
                                Recur = (int)c.Element("Recur"),
                                Detail = (string)c.Element("Detail"),
                                Status = (int)c.Element("Status") ==1?true:false,
                            }
                            ).OrderByDescending(x => x.TaskId).ToList();
                return list;
            }
            catch(Exception ex)
            {
                string exception = ex.ToString();
                return new List<Task>();
            }
        }

        public Task Get(int id)
        {
            var doc = XDocument.Load(_filePath);
            try
            {
                var note = (from c in doc.Descendants("Task")
                            where c.Element("TaskId").Value == id.ToString()
                            select new Task
                            {
                                TaskId = (int)c.Element("TaskId"),
                                Name = (string)c.Element("Name"),
                                Action = (string)c.Element("Action"),
                                StartDate = (DateTime)c.Element("Repeat"),
                                RunTime = (string)c.Element("RunTime"),
                                NextTime = (DateTime)c.Element("NextTime"),
                                Type = (string)c.Element("Type"),
                                Recur = (int)c.Element("Recur"),
                                Detail = (string)c.Element("Detail"),
                                Status = (int)c.Element("Status") == 1 ? true : false,
                            }
                            ).SingleOrDefault();
                return note;
            }
            catch
            {
                return null;
            }
        }

        public void Insert(Task task)
        {
            var doc = XDocument.Load(_filePath);
            try
            {
                var lastId = (from c in doc.Descendants("Task")
                              select (int)c.Element("TaskId")
                              ).OrderBy(x => x).Max();
                task.TaskId = lastId + 1;
            }
            catch
            {
                task.TaskId = 1;
            }

            var noteNode =
                    new XElement("Task",
                        new XElement("Id", task.TaskId),
                        new XElement("Name", task.Name),
                        new XElement("Action",task.Action),
                        new XElement("StartDate", task.StartDate.ToShortDateString()),
                        new XElement("RunTime", task.RunTime),
                        new XElement("NextTime", task.NextTime.ToLongDateString()),
                        new XElement("Type", task.StartDate.ToShortDateString()),
                        new XElement("Repeat", task.Recur),
                        new XElement("Detail", task.Detail),
                        new XElement("Status", task.Status)
                    );
            doc.Element("Config").Add(noteNode);
            doc.Save(_filePath);
        }

        public bool Update(Task task)
        {
            try
            {
                var doc = XDocument.Load(_filePath);
                var noteNode = doc.Elements("Config").Elements("Task").Where(x => x.Element("TaskId").Value == task.TaskId.ToString()).Take(1);
                noteNode.Elements("Name").SingleOrDefault().Value = task.Name;
                noteNode.Elements("Action").SingleOrDefault().Value = task.Action;
                noteNode.Elements("StartDate").SingleOrDefault().Value = task.StartDate.ToString("yyyy-MM-dd");
                noteNode.Elements("RunTime").SingleOrDefault().Value = task.RunTime;
                noteNode.Elements("NextTime").SingleOrDefault().Value = task.NextTime.ToString("yyyy-MM-dd HH:mm:ss");
                noteNode.Elements("Type").SingleOrDefault().Value = task.Type;
                noteNode.Elements("Recur").SingleOrDefault().Value = task.Recur.ToString();
                noteNode.Elements("Detail").SingleOrDefault().Value = task.Detail;
                noteNode.Elements("Status").SingleOrDefault().Value = task.Status==true?"1":"0";

                doc.Save(_filePath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //public bool Delete(int id)
        //{
        //    try
        //    {
        //        var doc = XDocument.Load(_filePath);
        //        doc.Elements("Notes").Elements("Note").Where(x => x.Element("Id").Value == id.ToString()).Remove();
        //        doc.Save(_filePath);
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
    }
}