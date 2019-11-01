using FinesseClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinesseClient.Model
{
    public class MessageEvent
    {
        #region Fields
        private string _messageType;
        private Dialog _dialog;
        private string _event;
        private string _errorCode;
        private string _errorMsg;
        private string _errorType;
        #endregion

        #region Properties
        public String MessageType { get { return _messageType; } set { _messageType = value; } }
        public Dialog Dialog { get { return _dialog; } set { _dialog = value; } }
        public String Event { get { return _event; } set { _event = value; } }
        public String ErrorCode { get { return _errorCode; } set { _errorCode = value; } }
        public String ErrorMsg { get { return _errorMsg; } set { _errorMsg = value; } }
        public String ErrorType { get { return _errorType; } set { _errorType = value; } }
        #endregion
    }
}
