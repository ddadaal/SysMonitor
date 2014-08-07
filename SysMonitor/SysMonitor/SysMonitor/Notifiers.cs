using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SysMonitor
{
    class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }


    class NetworkNotifier : NotifyPropertyChanged
    {
        #region Properties
        private string unit;
        private double uploadSpeed;
        private double downloadSpeed;
        private string strUlSpeed;
        private string strDlSpeed;

        public double UploadSpeed
        {
            get { return uploadSpeed; }
            private set
            {
                this.uploadSpeed = value;
                RaisePropertyChanged("UploadSpeed");
            }
        }
        public double DownloadSpeed
        {
            get { return downloadSpeed; }
            private set
            {
                this.downloadSpeed = value;
                RaisePropertyChanged("DownloadSpeed");
            }
        }

        public string StrUlSpeed
        {
            get { return strUlSpeed; }
            private set
            {
                this.strUlSpeed = value;
                RaisePropertyChanged("StrUlSpeed");
            }
        }
        public string StrDlSpeed
        {
            get { return strDlSpeed; }
            private set
            {
                this.strDlSpeed = value;
                RaisePropertyChanged("StrDlSpeed");
            }
        }

        #endregion

        #region Construction Method
        public NetworkNotifier (double ulspeed,double dlspeed,string Unit)
        {
            EditData(ulspeed, dlspeed);
            this.unit = Unit;
        }
        #endregion

        #region Edit
        public void EditData(double ulspeed,double dlspeed)
        {
            this.UploadSpeed = ulspeed;
            this.DownloadSpeed = dlspeed;

            this.StrDlSpeed = dlspeed.ToString() + unit;
            this.StrUlSpeed = ulspeed.ToString() + unit;
        }
        #endregion

  
    }

    class CPUNotifier : NotifyPropertyChanged
    {
        #region Properties
        private string unit = "%";
        private int usage;
        private string strCPU;

        public int Usage
        {
            get { return usage; }
            set { usage = value; RaisePropertyChanged("Usage"); }
        }
        public string StrCPU
        {
            get { return usage.ToString() + unit; }
            set { this.strCPU = value; RaisePropertyChanged("StrCPU"); }
        }

        #endregion

        #region Construction Method
        public CPUNotifier(int usage)
        {
            EditData(usage);
        }
        #endregion

        #region EditData
        public void EditData(int usage)
        {
            this.usage = usage;
            this.StrCPU = usage.ToString() + " " + unit;
        }
        #endregion
    }
    class RAMNotifier : NotifyPropertyChanged
    {
        #region Properties
        private string unit = "%";
        private int usage;
        private string strRAM;

        public int Usage
        {
            get { return usage; }
            set { usage = value; RaisePropertyChanged("Usage"); }
        }
        public string StrRAM
        {
            get { return usage.ToString() + unit; }
            set { this.strRAM = value; RaisePropertyChanged("StrRAM"); }
        }

        #endregion

        #region Construction Method
        public RAMNotifier(int usage)
        {
            EditData(usage);
        }
        #endregion

        #region EditData
        public void EditData(int usage)
        {
            this.usage = usage;
            this.StrRAM = usage.ToString() + " " + unit;
        }
        #endregion
    }
}
