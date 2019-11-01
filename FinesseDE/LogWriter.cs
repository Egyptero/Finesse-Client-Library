using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace FinesseDE
{
    public class LogWriter
    {
        private string m_Path = string.Empty;
        public LogWriter(string logMessage)
        {
            LogWrite(logMessage);
        }
        public void LogWrite(string logMessage)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() =>
            {
                try
                {
                    DateTime dateTime = DateTime.Now;
                    string todayName = dateTime.ToShortDateString().Replace('/', '_');
                    var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FinesseDE." + todayName + ".log");


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
