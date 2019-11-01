using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace WSDEVC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hwc, IntPtr hwp);
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Process p = Process.Start(@"C:\Program Files (x86)\GCTI\Workspace Desktop Edition\InteractionWorkspace\InteractionWorkspace.exe");
            //Thread.Sleep(50000);
            p.WaitForInputIdle(500000);
            //SetParent(p.MainWindowHandle, );
        }
    }
}
