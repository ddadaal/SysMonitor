using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Timers;
using System.ComponentModel;

namespace SysMonitor
{

    class NetworkMonitor
    {
        #region Properties
        private double timerInterval = 1000;
        private List<NetworkAdapter> listAdapters;
        private Timer timer;

        private event EventHandler<NetworkEventArgs> updateEvent;
        #endregion

        #region Construction Method
        public NetworkMonitor(EventHandler<NetworkEventArgs> UpdateEvent)
        {
            timer = new Timer();
            timer.Interval= timerInterval;
            timer.Elapsed += timer_Elapsed;
            listAdapters = new List<NetworkAdapter>();
            this.updateEvent = UpdateEvent;
        }
        #endregion

        #region Methods
        private void enumerateNetworkAdapters()
        {
            PerformanceCounterCategory category = new PerformanceCounterCategory("Network Interface");

            foreach (string name in category.GetInstanceNames())
            {
                // This one exists on every computer.  
                if (name == "MS TCP Loopback interface")
                    continue;

                // Create an instance of NetworkAdapter class, and create performance counters for it.  
                NetworkAdapter adapter = new NetworkAdapter(name);
                adapter.dlCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", name);
                adapter.ulCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", name);
                this.listAdapters.Add(adapter); 
            }
        }  
        public void Start()
        {
            enumerateNetworkAdapters();
            foreach (NetworkAdapter adapter in listAdapters)
            {
                adapter.Start();
            }
            timer.Start();
        }
        private void update()
        {
            double ulspeed=0, dlspeed=0;
            foreach (NetworkAdapter adapter in listAdapters)
            {
                ulspeed += adapter.UlSpeed;
                dlspeed += adapter.DlSpeed;
            }
            updateEvent(this, new NetworkEventArgs(ulspeed, dlspeed));
        }
        #endregion

        #region Event for Timer
        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            foreach (NetworkAdapter adapter in listAdapters)
            {
                adapter.Refresh();
                update();
            }
        }
        #endregion

       //#region Open for Read
       // private double totalUlSpeed;
       // private double totalDlSpeed;

       // public double TotalUlSpeed
       // {
       //     get { return totalUlSpeed; }
       // }
       // public double TotalDlSpeed
       // {
       //     get { return totalDlSpeed; }
       // }
       // #endregion
    }

    class NetworkAdapter
    {
        #region Properties
        public PerformanceCounter dlCounter
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public PerformanceCounter ulCounter
        {
            get;
            set;
        }

        private double dlValue, lastDlValue;
        private double ulValue, lastUlValue;
        private double dlSpeed, ulSpeed;
        #endregion

        #region Construction Method
        public NetworkAdapter(string name)
        {
            this.Name = name;
        }
        #endregion

        #region Method
        public void Start()
        {
            lastDlValue = dlCounter.NextSample().RawValue;
            lastUlValue = ulCounter.NextSample().RawValue;

        }

        public void Refresh()
        {
            dlValue = dlCounter.NextSample().RawValue;
            ulValue = ulCounter.NextSample().RawValue;

            dlSpeed = dlValue - lastDlValue;
            ulSpeed = ulValue - lastUlValue;

            lastDlValue = dlValue;
            lastUlValue = ulValue;
        }
        #endregion

        #region Open for Read
        public double DlSpeed
        {
            get { return dlSpeed; }
        }

        public double UlSpeed
        {
            get { return ulSpeed; }
        }
        #endregion
    }

    class NetworkEventArgs : EventArgs
    {  
        public NetworkEventArgs(double updateSpeed,double downloadSpeed)
        {
            this.UpdateSpeed = updateSpeed;
            this.DownloadSpeed = downloadSpeed;
        }

        public double UpdateSpeed
        {
            get;private set;
        }
        public double DownloadSpeed
        {
            get;private set;
        }
    }



}
