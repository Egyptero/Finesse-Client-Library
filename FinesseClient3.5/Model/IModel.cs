using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FinesseClient.Model
{
    public interface IModel
    {
        void SetFinAgent(FinAgent finAgent);
        void SetDialogID(String dialogID);
    }
}
