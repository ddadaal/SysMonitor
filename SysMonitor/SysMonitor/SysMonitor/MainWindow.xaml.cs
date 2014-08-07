using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace SysMonitor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        NetworkNotifier netNotifier = null;
        CPUNotifier cpuNotifier = null;
        RAMNotifier ramNotifier = null;

        public MainWindow()
        {
            InitializeComponent();

            //set initial size

            //set startuplocation
            this.Left = SystemParameters.WorkArea.Width - this.Width;
            this.Top = SystemParameters.WorkArea.Height - this.Height;

            netNotifier = new NetworkNotifier(0,0,"KiB/s  ");
            cpuNotifier = new CPUNotifier(0);
            ramNotifier = new RAMNotifier(0);

            tbUlSpeed.DataContext = netNotifier;
            tbDlSpeed.DataContext = netNotifier;
            tbCPU.DataContext = cpuNotifier;
            tbRAM.DataContext = ramNotifier;

            NetworkMonitor netMonitor = new NetworkMonitor(NetworkUpdate);
            netMonitor.Start();
            CPUMonitor cpuMonitor = new CPUMonitor(CPUUpdate);
            cpuMonitor.Start();
            RAMMonitor ramMonitor = new RAMMonitor(RAMUpdate);
            ramMonitor.Start();
        
        }

        private void NetworkUpdate(object sender,NetworkEventArgs e)
        {
            long ul = (long)e.UpdateSpeed / 1024;
            long dl = (long)e.DownloadSpeed / 1024;

            netNotifier.EditData(ul, dl);
        }
        private void CPUUpdate(object sender, UsageEventArgs e)
        {
            int usage = e.Usage;
            cpuNotifier.EditData(usage);
        }
        private void RAMUpdate(object sender, UsageEventArgs e)
        {
            int usage = e.Usage;
            ramNotifier.EditData(usage);
        }

        private void refreshLayout(object sender,SizeChangedEventArgs e)
        {
            
            
        }


        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            
               
                this.DragMove(); 
        }

        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
