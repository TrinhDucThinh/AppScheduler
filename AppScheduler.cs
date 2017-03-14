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

namespace AppScheduler
{
	public class AppScheduler : System.ServiceProcess.ServiceBase
	{
		string configPath;
        private static int time = 0;
		System.Timers.Timer _timer=new System.Timers.Timer();
		DataSet dsTasks=new DataSet();
		string formatString="MM/dd/yyyy HH:mm:ss";
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        /// <summary>
        /// Class that launches applications on demand.
        /// </summary>
		
        public void OnDebug()
        {
            OnStart(null);
        }

		void timeElapsed(object sender, ElapsedEventArgs args)
		{
            //Read task file to update new tasks
            Utilities.ReadTaskFile(configPath, ref dsTasks);

            //Loop task to excute task ;
           
            DateTime currTime = DateTime.Now;
            foreach (DataRow dRow in dsTasks.Tables["task"].Rows)
            {
                DateTime runTime = Convert.ToDateTime(dRow["time"]);
                string action = dRow["action"].ToString();

                if (currTime >= runTime)
                {
                    //Code process action in here
                    Utilities.WriteLogError(action);
                    // Update the next run time
                    string strInterval = dRow["repeat"].ToString().ToUpper();
                    switch (strInterval)
                    {
                        case "D":
                            runTime = runTime.AddDays(1);
                            break;
                        case "W":
                            runTime = runTime.AddDays(7);
                            break;
                        case "M":
                            runTime = runTime.AddMonths(1);
                            break;
                    }

                    dRow.Delete();

                    dRow["time"] = runTime.ToString(formatString);
                    dsTasks.AcceptChanges();
                    StreamWriter sWrite = new StreamWriter(configPath);
                    XmlTextWriter xWrite = new XmlTextWriter(sWrite);
                    dsTasks.WriteXml(xWrite, XmlWriteMode.WriteSchema);
                    xWrite.Close();
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
            //System.ServiceProcess.ServiceBase[] ServicesToRun;
            //ServicesToRun = new System.ServiceProcess.ServiceBase[] { new AppScheduler() };
            //System.ServiceProcess.ServiceBase.Run(ServicesToRun);
#if DEBUG
            AppScheduler appScheduler = new AppScheduler();
            appScheduler.OnDebug();
            System.Threading.Thread.Sleep(Timeout.Infinite);

#else
#endif
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
			configPath=ConfigurationSettings.AppSettings["configpath"];
			
         
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
