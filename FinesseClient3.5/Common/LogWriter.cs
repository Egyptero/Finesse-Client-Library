using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace FinesseClient.Common
{
    public class LogWriter
    {
        private string m_Path = string.Empty;
        public LogWriter(string logMessage, string logLocation)
        {
            LogWrite(logMessage,logLocation);
        }

        public LogWriter(string logMessage)
        {
            LogWrite(logMessage);
        }
        public void LogWrite(string logMessage,string logLocation)
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() =>
            //System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() =>
            {
                try
                {
                    DateTime dateTime = DateTime.Now;
                    string todayName = dateTime.ToShortDateString().Replace('/', '_');
                    if (logLocation == null)
                        logLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

                    var fileName = Path.Combine(logLocation, "FinesseClient." + todayName + ".log");


                    using (StreamWriter w = File.AppendText(fileName))
                    {
                        Log(logMessage, w);
                    }
                }
                catch (Exception)
                {
                }

            }));
        }

        public void LogWrite(string logMessage)
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() =>
            //System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() =>
            {
                try
                {
                    DateTime dateTime = DateTime.Now;
                    string todayName = dateTime.ToShortDateString().Replace('/', '_');
                    var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FinesseClient." + todayName + ".log");


                    using (StreamWriter w = File.AppendText(fileName))
                    {
                        Log(logMessage, w);
                    }
                }
                catch (Exception)
                {
                }

            }));
        }

        public void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.Write("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                txtWriter.Write("  :");
                txtWriter.Write("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception)
            {
            }
        }

        public static void DeleteLog()
        {
            var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FinConnectorLog.log");
            File.Delete(fileName);
        }

    }
}
