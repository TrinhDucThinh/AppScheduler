using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AppScheduler.Helper
{
    public class TextHelper
    {
        private string _filePath;

        public TextHelper()
        {
            _filePath = null;
        }

        public TextHelper(string filePath)
        {
            _filePath = filePath;
        }

        public void WriteText(string message)
        {
            if (!File.Exists(_filePath))
            {
                File.Create(_filePath);
            }else
            {
                using (var tw= new StreamWriter(_filePath,true))
                {
                    tw.WriteLine(message);
                    tw.Close();
                }
            }
        }
    }
}
