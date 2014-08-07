using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Management;
using System.Timers;

namespace SysMonitor
{
    class RAMMonitor
    {
        #region Properties
        private ManagementClass manager2;
        private event EventHandler<UsageEventArgs> update;
        private Timer timer;

        private long totalRAM;
        
        #endregion

        #region Construction Method
        public RAMMonitor(EventHandler<UsageEventArgs> updateEvent)
        {
            int interval=1000;
            this.update += updateEvent;
            timer = new Timer(interval);
            timer.Elapsed += timer_Elapsed;

            ManagementClass manager1 = new ManagementClass("Win32_PhysicalMemory");
            foreach (ManagementObject o in manager1.GetInstances())
                    totalRAM += long.Parse(o.Properties["Capacity"].Value.ToString());

           manager2 = new ManagementClass("Win32_PerfFormattedData_PerfOS_Memory");
           


        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Refresh();
        }

        #endregion

        #region Method
        public void Start()
        {
            if (manager2 != null)
            {
                timer.Start();
                Refresh();
            }
          
        }
        public void Refresh()
        {
            long freeRAM = 0;
            foreach (ManagementObject o in manager2.GetInstances())
                freeRAM += long.Parse(o.Properties["AvailableBytes"].Value.ToString());

            double usedRAM =(double)totalRAM - (double) freeRAM;
            double rate = usedRAM / totalRAM;
            int usage = (int)(rate * 100);
            update(this, new UsageEventArgs(usage));
        }
        #endregion
    }
}
