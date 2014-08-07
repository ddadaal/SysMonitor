using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;

namespace SysMonitor
{
    class CPUMonitor
    {
        #region Properties
        public PerformanceCounter counter
        {
            get;
           private set;
        }

        private int cpuUsage;
        private Timer timer;
        private int timerInterval = 1000;
        private event EventHandler<UsageEventArgs> updateEvent;
        #endregion

        #region Construction Method
        public CPUMonitor(EventHandler<UsageEventArgs> updateEvent)
        {
            timer = new Timer(timerInterval);
            timer.Elapsed+=timer_Elapsed;
            counter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            this.updateEvent = updateEvent;
        }


        #endregion

        #region Method
        public void Start()
        {
            Refresh();
            timer.Start();

        }

        public void Refresh()
        {
            cpuUsage = (int)counter.NextValue();
            updateEvent(this, new UsageEventArgs(cpuUsage));
        }

       
        #endregion

        #region Event for Timer
        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Refresh();
        }
        #endregion

        #region Open for Read
        public int Usage
        {
            get { return cpuUsage; }
        }
        #endregion
    }

    public class UsageEventArgs : EventArgs
    {
        public int Usage
        {
            get;
            private set;
        }
        public UsageEventArgs(int usage)
        {
            this.Usage = usage;
        }
    }
}
