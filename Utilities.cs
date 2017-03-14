using System;
using System.Data;
using System.IO;
using System.ServiceProcess;

namespace AppScheduler
{
    internal class Utilities
    {
        public static void WriteLogError(Exception ex)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString("g") + ": " + ex.Source + "; " + ex.Message);
                sw.Flush();
                sw.Close();
            }
            catch
            {
                // ignored
            }
        }

        public static void WriteLogError(string message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString("g") + ": " + message);
                sw.Flush();
                sw.Close();
            }
            catch(Exception ex)
            {
                // ignored
                Utilities.WriteLogError("Error:"+ex);
            }
        }

        public static void ReadTaskFile(string pathTaskFile, ref DataSet dsTasks)
        {
            try
            {
                FileStream fileStream = new FileStream(pathTaskFile, FileMode.Open);
                try
                {
                    dsTasks.ReadXml(fileStream);
                    fileStream.Close();
                }
                catch
                {

                }
                finally
                {
                    fileStream.Close();
                }
            }
            catch (Exception)
            {
                Utilities.WriteLogError("Error read file task");
            }

        }
    }
}