using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Xml;
using System.Timers;
using System.Threading;
using System.Configuration;
using System.IO;
using AppScheduler.Helper;
using System.Collections.Generic;

namespace AppScheduler
{
	public class AppScheduler : System.ServiceProcess.ServiceBase
	{
        private static string configPath = AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["ConfigPath"];
        private static string LogPath= AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["LogPath"];
        
		private System.Timers.Timer _timer=new System.Timers.Timer();
		private DataSet dsTasks=new DataSet();
		private System.ComponentModel.Container components = null;
       

        //Using for test Window Service		
        public void OnDebug()
        {
            string filepath = AppDomain.CurrentDomain.BaseDirectory ;
            OnStart(null);
        }

        //Event Occur each timer elapse
		void timeElapsed(object sender, ElapsedEventArgs args)
		{

            //Read task file to update new tasks
            XmlHelper xmlHelper = new XmlHelper(configPath);
            TextHelper txtHelper = new TextHelper(LogPath);
            List<Task> listTask = xmlHelper.GetAll();
            //Loop task to excute task ;
            DateTime currTime = DateTime.Now;
            foreach (Task task in listTask)
            {
                if (currTime >= task.StartTime)
                {
                    //Excute task
                    txtHelper.WriteText(currTime+" Excuted "+task.Action);
                    //Update Repeat
                    switch (task.Repeat)
                    {
                        case "D":
                            task.StartTime = task.StartTime.AddDays(1);
                            break;
                        case "W":
                            task.StartTime = task.StartTime.AddDays(7);
                            break;
                        case "M":
                            task.StartTime = task.StartTime.AddMonths(1);
                            break;
                    }
                    xmlHelper.Update(task);
                }
            }
        }

		public AppScheduler()
		{
			// This call is required by the Windows.Forms Component Designer.
			InitializeComponent();
			// TODO: Add any initialization after the InitComponent call
		}

		// The main entry point for the process
		static void Main()
		{
            System.ServiceProcess.ServiceBase[] ServicesToRun;
            ServicesToRun = new System.ServiceProcess.ServiceBase[] { new AppScheduler() };
            System.ServiceProcess.ServiceBase.Run(ServicesToRun);
//#if DEBUG
//            AppScheduler appScheduler = new AppScheduler();
//            appScheduler.OnDebug();
//            System.Threading.Thread.Sleep(Timeout.Infinite);

//#else
//#endif
        }

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
		{
			// 
			// AppScheduler
			// 
			this.CanPauseAndContinue = true;
			this.ServiceName = "Application Scheduler";

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// Set things in motion so your service can do its work.
		/// </summary>
		protected override void OnStart(string[] args)
		{
			// TODO: Add code here to start your service.
			_timer.Interval=60000;
			_timer.Elapsed+=new ElapsedEventHandler(timeElapsed);
			_timer.Start();
		}
 
		/// <summary>
		/// Stop this service.
		/// </summary>
		protected override void OnStop()
		{
			// TODO: Add code here to perform any tear-down necessary to stop your service.
		}
	}
}
