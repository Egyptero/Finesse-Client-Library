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

namespace FC_Mini.ARB
{
    public class AUDOption
    {
        private FinAgent _FinAgent;
        private Dialog _Dialog;
        private string _CTIWebServer;
        public AUDOption(FinAgent finAgent, Dialog dialog, string ctiwebserver)
        {
            _FinAgent = finAgent;
            _Dialog = dialog;
            _CTIWebServer = ctiwebserver;
        }
        public void EndCallAUDRecord()
        {
            if (_Dialog == null)
                return;
            if (_Dialog.MediaProperties == null)
                return;
            if (_Dialog.MediaProperties.CallVariables == null)
                return;

            string dialedNumber = ((CallVariableClass)_Dialog.MediaProperties.CallVariables[6]).Value;
            if (dialedNumber.Equals("VIP") || dialedNumber.Equals("VIP-Intl") || dialedNumber.Equals("Retail") || dialedNumber.Equals("Retail-Int") ||
                dialedNumber.Equals("MARKETING") || dialedNumber.Equals("POS") || dialedNumber.Equals("Corporate") ||
                dialedNumber.Equals("SI") || dialedNumber.Equals("SI92") || dialedNumber.Equals("Fraud_Line") || dialedNumber.Equals("REMIT") ||
                dialedNumber.Equals("SME") || dialedNumber.Equals("KWTRETAIL") || dialedNumber.Equals("CREDIT_ADV"))
            {
                string url = _CTIWebServer + "AUDEndCall?agentId=" + _FinAgent.AgentInformation.AgentID;
                SendHTTPRequest(url, "GET");
            }

        }
        public void StartCallAUDRecord()
        {
            if (_Dialog == null)
                return;
            if (_Dialog.MediaProperties == null)
                return;
            if (_Dialog.MediaProperties.CallVariables == null)
                return;

            string exitPoint = ((CallVariableClass)_Dialog.MediaProperties.CallVariables[2]).Value;
            string cic = ((CallVariableClass)_Dialog.MediaProperties.CallVariables[3]).Value;
            string dialedNumber = ((CallVariableClass)_Dialog.MediaProperties.CallVariables[6]).Value;
            string appName = ((CallVariableClass)_Dialog.MediaProperties.CallVariables[0]).Value;
            string audOptions = "";

            if (dialedNumber.Equals("VIP") || dialedNumber.Equals("VIP-Intl"))
            {
                if (appName != null && appName.Length > 0 && appName.IndexOf('-') > -1)
                {
                    if (exitPoint.Equals("AddBeneficiary"))
                        audOptions = appName.Substring(appName.IndexOf('-') + 1);
                }
            }

            if (dialedNumber.Equals("VIP") || dialedNumber.Equals("VIP-Intl") || dialedNumber.Equals("Retail") || dialedNumber.Equals("Retail-Int") ||
                dialedNumber.Equals("MARKETING") || dialedNumber.Equals("POS") || dialedNumber.Equals("Corporate") ||
                dialedNumber.Equals("SI") || dialedNumber.Equals("SI92") || dialedNumber.Equals("Fraud_Line") || dialedNumber.Equals("REMIT") || dialedNumber.Equals("SME"))
            {
                string url = _CTIWebServer + "AUDNewCall?cic=" + TrimLeftZeros(cic) + "&agentId=" + _FinAgent.AgentInformation.AgentID + "&options=" + audOptions;
                SendHTTPRequest(url, "GET");
            }
            if (dialedNumber.Equals("KWTRETAIL") || dialedNumber.Equals("CREDIT_ADV"))
            {
                string url = _CTIWebServer + "AUDNewCall?cic=" + "" + "&agentId=" + _FinAgent.AgentInformation.AgentID + "&options=" + audOptions;
                SendHTTPRequest(url, "GET");
            }


        }
        private string TrimLeftZeros(string input)
        {
            if (input == null || input.Equals(""))
                return null;
            string outcome = input;
            string ch = outcome.Substring(0, 1);
            while (ch == "0" && outcome.Length > 1)
            { // Check for zeros at the beginning of the string
                outcome = outcome.Substring(1, outcome.Length - 1);
                ch = outcome.Substring(0, 1);
            }
            return outcome;
        }
        private string SendHTTPRequest(string uri, string method)
        {

            //WebClient myWebClient = new WebClient();

            //// Download the Web resource and save it into a data buffer.
            //byte[] myDataBuffer = myWebClient.DownloadData(@uri);
            //// Display the downloaded data.
            //string download = Encoding.ASCII.GetString(myDataBuffer);

            //return "";


            string result;
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
                    System.Diagnostics.Trace.Write("CXConnect: AUD CTI WEB Server Error:" + e.Message);
                    System.Diagnostics.Trace.Write(e);
                }
                _FinAgent.FireErrorMessage("AUD is not reachable");
                return e.Message;
            }
            catch (Exception e)
            {
                if (_FinAgent.TraceStatus.Equals("true"))
                {
                    System.Diagnostics.Trace.Write("CXConnect: AUD CTI WEB Server Error:" + e.Message);
                    System.Diagnostics.Trace.Write(e);
                }
                _FinAgent.FireErrorMessage("AUD is not reachable");
                return e.Message;
            }
            finally
            {

            }
            return result;

        }

    }
}
