using FinesseClient.Common;
using FinesseClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinesseClient.View
{
    public interface IView
    {
        Screen GetLocation();
        void SetContext(IModel model,FinView finView);
        void FireNewEvent();
        void FireReLoginEvent();
        void FireDisconnectEvent();
        void FireCallEvent(Dialog dialog);
    }
}
