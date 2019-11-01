using CXDE.Server_Side;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static CXDE.Server_Side.Dialog.MediaProperties;

namespace CXDE.RJB_Customization
{
    public class ExtraServlets
    {
        private FinAgent _FinAgent;
        private Dialog _Dialog;
        private string _CTIWebServer;
        public ExtraServlets(FinAgent finAgent, string dialogID,string ctiwebserver)
        {
            _FinAgent = finAgent;
            _Dialog = finAgent.FindDialog(dialogID);
            _CTIWebServer = ctiwebserver;
        }
        public ExtraServlets(FinAgent finAgent, Dialog dialog,string ctiwebserver)
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
            if (_Dialog._MediaProperties == null)
                return;
            if (_Dialog._MediaProperties._CallVariables == null)
                return;
            if (_Dialog._MediaProperties._CallVariables.Count >= 4)
            {
            if (_Dialog._MediaProperties._CallVariables[3] != null)
                cic = ((CallVariable)_Dialog._MediaProperties._CallVariables[3])._Value;
            }
            if (_Dialog._MediaProperties._CallVariables.Count >= 7)
            {
            if (_Dialog._MediaProperties._CallVariables[6] != null)
                dialedNumber = ((CallVariable)_Dialog._MediaProperties._CallVariables[6])._Value;
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
            string url = _CTIWebServer+"CheckCicOnCtiStart?cic=" + cic + "&type="+type;
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
            if (_Dialog._MediaProperties == null)
                return;
            if (_Dialog._MediaProperties._CallVariables == null)
                return;
            if (_Dialog._MediaProperties._CallVariables.Count >= 4)
            {
            if (_Dialog._MediaProperties._CallVariables[3] != null)
                cic = ((CallVariable)_Dialog._MediaProperties._CallVariables[3])._Value;
            }
            if (_Dialog._MediaProperties._CallVariables.Count >= 7)
            {
            if (_Dialog._MediaProperties._CallVariables[6] != null)
                dialedNumber = ((CallVariable)_Dialog._MediaProperties._CallVariables[6])._Value;
            }
            Dialog.Participant myParticipantObject = null;
            foreach (Dialog.Participant participant in _Dialog._Participants)
            {
                if (_FinAgent._agentInformation.Extension.Equals(participant._MediaAddress))
                {
                    myParticipantObject = participant;
                    TimeSpan timeSpan = DateTime.Now - Convert.ToDateTime(myParticipantObject._StartTime);
                    callDuration = timeSpan.TotalHours.ToString("00") + ":" + timeSpan.TotalMinutes.ToString("00") + ":" + timeSpan.TotalSeconds.ToString("00");
                }
            }
            // Remove Zeros from CIC as per Dosoky request as the DB is not getting CIC of 16 digits
            while (cic != null && cic.Length > 1 && cic.StartsWith("0"))
                cic = cic.Substring(1);
            cic = cic.Trim();

            string url = _CTIWebServer+"CallHistory?cic=" + cic +
                "&agentId=" + _FinAgent._agentInformation.AgentID +
                "&callTime=" + callDuration +
                "&reason="+ reason +
                "&calltype=" + dialedNumber;
            SendHTTPRequest(url, "GET");
        }
        public void OnCallSkillTransfer(string currentSkill, string targetSkill)
        {
            string cic = string.Empty;
            string ciscoguid = "NA";
            if (_Dialog == null)
                return;
            if (_Dialog._MediaProperties == null)
                return;
            if (_Dialog._MediaProperties._CallVariables == null)
                return;
            if (_Dialog._MediaProperties._CallVariables.Count >= 10)
            {
            if (_Dialog._MediaProperties._CallVariables[9] != null)
                ciscoguid = ((CallVariable)_Dialog._MediaProperties._CallVariables[9])._Value;
            }
            if (_Dialog._MediaProperties._CallVariables.Count >= 4)
            {
            if (_Dialog._MediaProperties._CallVariables[3] != null)
                cic = ((CallVariable)_Dialog._MediaProperties._CallVariables[3])._Value;
            }
            // Remove Zeros from CIC as per Dosoky request as the DB is not getting CIC of 16 digits
            while (cic != null && cic.Length > 1 && cic.StartsWith("0"))
                cic = cic.Substring(1);
            cic = cic.Trim();
            string url = _CTIWebServer + "LogTransfer?agentID=" + _FinAgent._agentInformation.AgentID +
                "&Instrument=" + _FinAgent._agentInformation.Extension +
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
