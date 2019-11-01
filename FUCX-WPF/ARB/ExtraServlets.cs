using FinesseClient;
using FinesseClient.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static FinesseClient.Model.Dialog.MediaPropertiesClass;

namespace FUCX_WPF.ARB
{
    public class ExtraServlets
    {
        private FinAgent _FinAgent;
        private Dialog _Dialog;
        private string _CTIWebServer;
        public ExtraServlets(FinAgent finAgent, string dialogID, string ctiwebserver)
        {
            _FinAgent = finAgent;
            _Dialog = finAgent.FindDialog(dialogID);
            _CTIWebServer = ctiwebserver;
        }
        public ExtraServlets(FinAgent finAgent, Dialog dialog, string ctiwebserver)
        {
            _FinAgent = finAgent;
            _Dialog = dialog;
            _CTIWebServer = ctiwebserver;
        }

        public void OnCallStartEvent()
        {
            string cic = string.Empty;
            string dialedNumber = string.Empty;
            string type = string.Empty;

            if (_Dialog == null)
                return;
            if (_Dialog.MediaProperties == null)
                return;
            if (_Dialog.MediaProperties.CallVariables == null)
                return;
            if (_Dialog.MediaProperties.CallVariables.Count >= 4)
            {
                if (_Dialog.MediaProperties.CallVariables[3] != null)
                    cic = ((CallVariableClass)_Dialog.MediaProperties.CallVariables[3]).Value;
            }
            if (_Dialog.MediaProperties.CallVariables.Count >= 7)
            {
                if (_Dialog.MediaProperties.CallVariables[6] != null)
                    dialedNumber = ((CallVariableClass)_Dialog.MediaProperties.CallVariables[6]).Value;
            }
            // Remove Zeros from CIC as per Dosoky request as the DB is not getting CIC of 16 digits
            while (cic != null && cic.Length > 1 && cic.StartsWith("0"))
                cic = cic.Substring(1);
            if (dialedNumber.Equals("VIP") || dialedNumber.Equals("VIP-Intl") || dialedNumber.Equals("Retail") || dialedNumber.Equals("Retail-Int"))
                type = "retail";
            else if (dialedNumber.Equals("2727") || dialedNumber.Equals("5858"))
                type = "tadawul";
            else if (dialedNumber.Equals("KWTRETAIL"))
                type = "kuwaitRetail";
            else if (dialedNumber.Equals("JRDNRETAIL"))
                type = "jrdnRetail";
            else if (dialedNumber.Equals("POS"))
                type = "pos";
            else if (dialedNumber.Equals("SME"))
                type = "SME";
            else if (dialedNumber.Equals("MARKETING"))
                type = "marketing";
            else
                type = dialedNumber;
            string url = _CTIWebServer + "CheckCicOnCtiStart?cic=" + cic + "&type=" + type;
            SendHTTPRequest(url, "GET");
        }
        public void OnCallEndEvent()
        {
            string reason = string.Empty;
            string cic = string.Empty;
            string dialedNumber = string.Empty;
            string callDuration = string.Empty;

            if (_Dialog == null)
                return;
            if (_Dialog.MediaProperties == null)
                return;
            if (_Dialog.MediaProperties.CallVariables == null)
                return;
            if (_Dialog.MediaProperties.CallVariables.Count >= 4)
            {
                if (_Dialog.MediaProperties.CallVariables[3] != null)
                    cic = ((CallVariableClass)_Dialog.MediaProperties.CallVariables[3]).Value;
            }
            if (_Dialog.MediaProperties.CallVariables.Count >= 7)
            {
                if (_Dialog.MediaProperties.CallVariables[6] != null)
                    dialedNumber = ((CallVariableClass)_Dialog.MediaProperties.CallVariables[6]).Value;
            }
            Dialog.Participant myParticipantObject = null;
            foreach (Dialog.Participant participant in _Dialog.Participants)
            {
                if (_FinAgent.AgentInformation.Extension.Equals(participant.MediaAddress))
                {
                    myParticipantObject = participant;
                    TimeSpan timeSpan = DateTime.Now - Convert.ToDateTime(myParticipantObject.StartTime);
                    callDuration = timeSpan.TotalHours.ToString("00") + ":" + timeSpan.TotalMinutes.ToString("00") + ":" + timeSpan.TotalSeconds.ToString("00");
                }
            }
            // Remove Zeros from CIC as per Dosoky request as the DB is not getting CIC of 16 digits
            while (cic != null && cic.Length > 1 && cic.StartsWith("0"))
                cic = cic.Substring(1);
            cic = cic.Trim();

            string url = _CTIWebServer + "CallHistory?cic=" + cic +
                "&agentId=" + _FinAgent.AgentInformation.AgentID +
                "&callTime=" + callDuration +
                "&reason=" + reason +
                "&calltype=" + dialedNumber;
            SendHTTPRequest(url, "GET");
        }
        public void OnCallSkillTransfer(string currentSkill, string targetSkill)
        {
            string cic = string.Empty;
            string ciscoguid = "NA";
            if (_Dialog == null)
                return;
            if (_Dialog.MediaProperties == null)
                return;
            if (_Dialog.MediaProperties.CallVariables == null)
                return;
            if (_Dialog.MediaProperties.CallVariables.Count >= 10)
            {
                if (_Dialog.MediaProperties.CallVariables[9] != null)
                    ciscoguid = ((CallVariableClass)_Dialog.MediaProperties.CallVariables[9]).Value;
            }
            if (_Dialog.MediaProperties.CallVariables.Count >= 4)
            {
                if (_Dialog.MediaProperties.CallVariables[3] != null)
                    cic = ((CallVariableClass)_Dialog.MediaProperties.CallVariables[3]).Value;
            }
            // Remove Zeros from CIC as per Dosoky request as the DB is not getting CIC of 16 digits
            while (cic != null && cic.Length > 1 && cic.StartsWith("0"))
                cic = cic.Substring(1);
            cic = cic.Trim();
            string url = _CTIWebServer + "LogTransfer?agentID=" + _FinAgent.AgentInformation.AgentID +
                "&Instrument=" + _FinAgent.AgentInformation.Extension +
                "&currentSkill=" + currentSkill +
                "&targetSkill=" + targetSkill +
                "&ciscoguid=" + ciscoguid +
                "&cic=" + cic;
            SendHTTPRequest(url, "GET");
        }
        private string SendHTTPRequest(string uri, string method)
        {

            //WebClient myWebClient = new WebClient();

            //// Download the Web resource and save it into a data buffer.
            //byte[] myDataBuffer = myWebClient.DownloadData(@uri);
            //// Display the downloaded data.
            //string download = Encoding.ASCII.GetString(myDataBuffer);

            //return "";


            string result = "CTIWEB Error";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = method;
                // Log the response from Redmine RESTful service
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    reader.Close();
                    response.Close();
                }
            }
            catch (WebException e)
            {
                if (_FinAgent.TraceStatus.Equals("true"))
                {
                    System.Diagnostics.Trace.Write("CXConnect: Extra Servlet CTI WEB Server Error:" + e.Message);
                    System.Diagnostics.Trace.Write(e);
                }
                _FinAgent.FireErrorMessage("CTIWEB Unreachable. Can not call extra urls");
                return e.Message;
            }
            catch (Exception e)
            {
                if (_FinAgent.TraceStatus.Equals("true"))
                {
                    System.Diagnostics.Trace.Write("CXConnect: Extra Servlet CTI WEB Server Error:" + e.Message);
                    System.Diagnostics.Trace.Write(e);
                }
                _FinAgent.FireErrorMessage("CTIWEB Unreachable. Can not call extra urls");
                return e.Message;
            }
            finally
            {

            }
            return result;
        }
    }
}
