using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinesseClient.Model
{
    public interface IModel
    {
        void SetFinAgent(FinAgent finAgent);
        void SetDialogID(String dialogID);
    }
}
