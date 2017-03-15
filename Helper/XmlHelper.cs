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
                var list = (from c in doc.Descendants("task")
                            select new Task
                            {
                                Id = (int)c.Element("Id"),
                                Action = (string)c.Element("Action"),
                                StartTime = (DateTime)c.Element("StartTime"),
                                Repeat = (string)c.Element("Repeat")
                            }
                            ).OrderByDescending(x => x.Id).ToList();
                return list;
            }
            catch
            {
                return new List<Task>();
            }
        }

        public Task Get(int id)
        {
            var doc = XDocument.Load(_filePath);
            try
            {
                var note = (from c in doc.Descendants("Task")
                            where c.Element("Id").Value == id.ToString()
                            select new Task
                            {
                                Id = (int)c.Element("Id"),
                                Action = (string)c.Element("Action"),
                                StartTime = (DateTime)c.Element("Content"),
                                Repeat = (string)c.Element("Repeat")

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
                              select (int)c.Element("Id")
                              ).OrderBy(x => x).Max();
                task.Id = lastId + 1;
            }
            catch
            {
                task.Id = 1;
            }

            var noteNode =
                    new XElement("Task",
                        new XElement("Id", task.Id),
                        new XElement("Action", task.Action),
                        new XElement("StartTime", task.StartTime.ToLongDateString()),
                        new XElement("Repeat", task.Repeat)
                    );
            doc.Element("Tasks").Add(noteNode);
            doc.Save(_filePath);
        }

        public bool Update(Task task)
        {
            try
            {
                var doc = XDocument.Load(_filePath);
                var noteNode = doc.Elements("Tasks").Elements("task").Where(x => x.Element("Id").Value == task.Id.ToString()).Take(1);
                noteNode.Elements("Action").SingleOrDefault().Value = task.Action;
                noteNode.Elements("StartTime").SingleOrDefault().Value = task.StartTime.ToString("yyyy-MM-dd HH:mm:ss");
                noteNode.Elements("Repeat").SingleOrDefault().Value = task.Repeat;
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