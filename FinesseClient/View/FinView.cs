using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinesseClient.Model;

namespace FinesseClient.View
{
    public interface FinView : IView
    {
        void FireLoadingMessage(string msg);
        void FireLoadLoginScreen();
        void FireErrorMessage(string msg);
        void FireLogMessage(string msg);
    }
}
