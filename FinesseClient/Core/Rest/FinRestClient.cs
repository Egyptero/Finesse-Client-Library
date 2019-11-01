using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FinesseClient.Core.Rest
{
    public class FinRestClient
    {

        /*
         * Building the restAPI which the CTI cotroller will use it  
         * 
         */
        public string WebappPath { get; set; }
        private FinAgent _FinAgent;
        private RestClient restClient;
        public FinRestClient(FinAgent finAgent)
        {
            _FinAgent = finAgent;
        }
        /**
        * Sign in the user.
        *
        * @param {String} extension
        *      Agent's extension.
        * @param {Boolean} forcedFlag
        *      Set true to force the sign in.
        * @param {Function(Object)} handler
        *      Callback to handle response.
        * @param {Function(Object)} errHandler
        *      Callback to handle error response.
        * @memberOf cisco.desktop.services.Transporter#
        */
        public string SignIn(string userName, string agentID, string extension, string password)
        {
            string method = "PUT";
            string url = WebappPath + "/api/User/" + agentID;
            string xmlData = "<User><state>LOGIN</state><extension>" + extension + "</extension></User>";
            _FinAgent.FireLoadingMessage("Sending sign in request to rest");
            string result = SendHTTPRequest(url, userName, password, method, xmlData);
            _FinAgent.FireLoadingMessage("Verifing Rest Connection Result");
            return result;
        }
        /**
           * Sign out the user.
           *
           * @param {String} extension
           *      Agent's extension.
           * @param {Number} reasonCode
           *      Reason code for logging out, or null.
           * @param {Function(Object)} handler
           *      Callback to handle response.
           * @param {Function(Object)} errHandler
           *      Callback to handle error response.
           * @memberOf cisco.desktop.services.Transporter#
       */
        public void SignOut(string userName, string agentID, string password)
        {
            string method = "PUT";
            string url = WebappPath + "/api/User/" + agentID;
            string xmlData = "<User><state>LOGOUT</state></User>";
            string result = SendHTTPRequest(url, userName, password, method, xmlData);
        }
        /**********************************************************************
            * AGENT APIs
        *********************************************************************/
        /**
         * Gets the specified agent's state.
         *
         * @param {String} agentId
         *      Agent's username or ID.
         * @param {Function(Object)} handler
         *      Callback to handle response.
         * @param {Function(Object)} errHandler
         *      Callback to handle error response.
         * @memberOf cisco.desktop.services.Transporter#
         */
        public string GetUserInfo(string userName, string agentID, string password)
        {
            string method = "GET";
            string url = WebappPath + "/api/User/" + agentID;
            string xmlData = string.Empty;
            return SendHTTPRequest(url, userName, password, method, xmlData);
            // return response in the xml 
        }
        public string GetUserDialogs(string userName, string agentID, string password)
        {
            string method = "GET";
            string url = WebappPath + "/api/User/" + agentID + "/Dialogs";
            string xmlData = string.Empty;
            return SendHTTPRequest(url, userName, password, method, xmlData);
        }
        public string GetSystemInfo(string userName, string agentID, string password)
        {
            string method = "GET";
            string url = WebappPath + "/api/SystemInfo";
            string xmlData = string.Empty;
            return SendHTTPRequest(url, userName, password, method, xmlData);
            // return response in the xml 
        }
        /**
          * Changes the agent's state
          *
          * @param {String} agentId
          *      Agent's username or ID
          * @param {String} state
          *      State to change to
          *      (E = applies to UCCE, X = applies to UCCX)
          *      EX           1 - Logout
          *      EX           2 - Not Ready
          *      EX           3 - Ready
          *      EX           4 - Talking
          *      EX           5 - Work Not Ready
          *      E            6 - Work Ready
          *      E            7 - Busy Other
          *      EX           8 - Reserved
          *      E            9 - Unknown
          *      E           10 - Hold
          *      E           11 - Active
          *      E           12 - Paused
          *      E           13 - Interrupted
          *      E           14 - Not Active
          * @param {Number} reasonCode Reason code for the state change (only
          * applies to Not Ready and Logout state changes), or null.
          * @param {Function(Object)} handler Callback to handle response.
          * @param {Function(Object)} errHandler Callback to handle error response.
          * @memberOf cisco.desktop.services.Transporter#
          */
        public string ChangeAgentState(string state, string reasonCode, string agentID, string userName, string password)
        {
            string method = "PUT";
            string url = WebappPath + "/api/User/" + agentID;
            string xmlData = string.Empty;
            string result = string.Empty;
            if (reasonCode != null)
            {
                xmlData = "<User><state>" + state + "</state><reasonCodeId>" + reasonCode + "</reasonCodeId></User>";
            }
            else
            {
                xmlData = "<User><state>" + state + "</state></User>";
            }
            return SendHTTPRequest(url, userName, password, method, xmlData);
        }
        public string GetStatus(string userName, string agentID, string password)
        {
            string method = "GET";
            string url = WebappPath + "/api/User/" + agentID;
            string xmlData = string.Empty;
            return SendHTTPRequest(url, userName, password, method, xmlData);
        }
        /**
         * Make a call.
         *
         * @param {Function(Object)} handler
         *      Callback to handle response.
         * @param {Function(Number)} errHandler
         *      Callback when error making the request.
         * @memberOf cisco.desktop.services.Transporter#
         */
        // working correctly but it needs to be real to get dialogID .
        // try it as transfer call .
        public string MakeCall(string userName, string agentID, string extension, string dialedNumberExt, string password)
        {
            string method = "POST";
            //url = http://finesse1.xyz.com/finesse/api/User/agentID/Dialogs
            string url = WebappPath + "/api/User/" + agentID + "/Dialogs";
            string xmlData = "<Dialog><requestedAction>MAKE_CALL</requestedAction >" + "<fromAddress>" + extension + "</fromAddress>"
                + "<toAddress>" + dialedNumberExt + "</toAddress>" + "</Dialog>";
            return SendHTTPRequest(url, userName, password, method, xmlData);
        }
        /**
        * Answers the call for the specified call ID.
        *
        * @param {String} callId
        *      The ID of the call.
        * @param {Function(Object)} handler
        *      Callback to handle response.
        * @param {Function(Number)} errHandler
        *      Callback when error making the request.
        * @memberOf cisco.desktop.services.Transporter#
        */
        // not tested yet , need to get the dialogID we might change Answer to ANSWER .  
        public string AnswerCall(string userName, string extension, string password, string dialogID)
        {
            string method = "PUT";
            string url = WebappPath + "/api/Dialog/" + dialogID;

            string xmlData = "<Dialog><targetMediaAddress>" + extension + "</targetMediaAddress>" + "<requestedAction>ANSWER</requestedAction></Dialog>";
            return SendHTTPRequest(url, userName, password, method, xmlData);
        }
        // not tested yet , need to get the dialogID we might change Answer to ANSWER .  
        public string RetriveCall(string userName, string extension, string password, string dialogID)
        {
            string method = "PUT";
            string url = WebappPath + "/api/Dialog/" + dialogID;
            // try to add if the connnection is failed dialogId(required): The ID of the dialog 
            string xmlData = "<Dialog><targetMediaAddress>" + extension + "</targetMediaAddress>" + "<requestedAction>RETRIEVE</requestedAction></Dialog>";
            return SendHTTPRequest(url, userName, password, method, xmlData);
        }
        // not tested yet , need to get the dialogID .  
        public string ReleaseCall(string userName, string extension, string password, string dialogID)
        {
            string method = "PUT";
            string url = WebappPath + "/api/Dialog/" + dialogID;
            // try to add if the connnection is failed dialogId(required): The ID of the dialog 
            // the requested Action did not find it the drop call 
            // need to check if it is working or not 

            string xmlData = "<Dialog><targetMediaAddress>" + extension + "</targetMediaAddress>" + "<requestedAction>DROP</requestedAction></Dialog>";
            return SendHTTPRequest(url, userName, password, method, xmlData);
        }
        /**
        * Holds the call for the specified call ID.
        *
        * @param {String} callId
        *      The ID of the call.
        * @param {Function(Object)} handler
        *      Callback to handle response.
        * @param {Function(Number)} errHandler
        *      Callback when error making the request.
        * @memberOf cisco.desktop.services.Transporter#
        */
        // not tested yet , error might be to in xml be hold 
        public string HoldCall(string userName, string extension, string password, string dialogID)
        {
            string method = "PUT";
            string url = WebappPath + "/api/Dialog/" + dialogID;
            // try to add if the connnection is failed dialogId(required): The ID of the dialog 
            // the requested Action did not find it the drop call 
            // need to check if it is working or not 
            string xmlData = "<Dialog><targetMediaAddress>" + extension + "</targetMediaAddress>" + "<requestedAction>HOLD</requestedAction></Dialog>";
            return SendHTTPRequest(url, userName, password, method, xmlData);
        }
        public string TransferCall(string userName, string extension, string password, string dialogID)
        {
            string method = "PUT";
            string url = WebappPath + "/api/Dialog/" + dialogID;
            // try to add if the connnection is failed dialogId(required): The ID of the dialog 
            // the requested Action did not find it the drop call 
            // need to check if it is working or not 
            string xmlData = "<Dialog><targetMediaAddress>" + extension + "</targetMediaAddress>" + "<requestedAction>TRANSFER</requestedAction></Dialog>";
            return SendHTTPRequest(url, userName, password, method, xmlData);
        }
        public string SSTransferCall(string userName, string extension, string password, string dialogID, string dialedNumber)
        {
            string method = "PUT";
            string url = WebappPath + "/api/Dialog/" + dialogID;
            // try to add if the connnection is failed dialogId(required): The ID of the dialog 
            // the requested Action did not find it the drop call 
            // need to check if it is working or not 
            string xmlData = "<Dialog>" + "<toAddress>" + dialedNumber + "</toAddress>" + "<targetMediaAddress>" + extension + "</targetMediaAddress>" + "<requestedAction>TRANSFER_SST</requestedAction></Dialog>";
            return SendHTTPRequest(url, userName, password, method, xmlData);
        }
        public string ConsultCall(string userName, string extension, string password, string dialogID, string dialNumber)
        {
            string method = "PUT";
            string url = WebappPath + "/api/Dialog/" + dialogID;
            // try to add if the connnection is failed dialogId(required): The ID of the dialog 
            // the requested Action did not find it the drop call 
            // need to check if it is working or not 
            string xmlData = "<Dialog>" + "<toAddress>" + dialNumber + "</toAddress>" + "<targetMediaAddress>" + extension + "</targetMediaAddress>" + "<requestedAction>CONSULT_CALL</requestedAction></Dialog>";
            return SendHTTPRequest(url, userName, password, method, xmlData);
        }
        public string ConferenceCall(string userName, string extension, string password, string dialogID, string dialNumber)
        {
            string method = "PUT";
            string url = WebappPath + "/api/Dialog/" + dialogID;
            // try to add if the connnection is failed dialogId(required): The ID of the dialog 
            // the requested Action did not find it the drop call 
            // need to check if it is working or not 
            string xmlData = "<Dialog>" + "<toAddress>" + dialNumber + "</toAddress>" + "<targetMediaAddress>" + extension + "</targetMediaAddress>" + "<requestedAction>CONFERENCE</requestedAction></Dialog>";
            return SendHTTPRequest(url, userName, password, method, xmlData);
        }
        public string KeypadSendDTMF(string userName, string extension, string password, string dialogID, string dtmfString)
        {
            string method = "PUT";
            string url = WebappPath + "/api/Dialog/" + dialogID;
            // try to add if the connnection is failed dialogId(required): The ID of the dialog 
            // the requested Action did not find it the drop call 
            // need to check if it is working or not 
            string xmlData = "<Dialog>" + "<actionParams><ActionParam><name>dtmfString</name><value>" + dtmfString + "</value></ActionParam></actionParams>" + "<targetMediaAddress>" + extension + "</targetMediaAddress>" + "<requestedAction>SEND_DTMF</requestedAction></Dialog>";
            return SendHTTPRequest(url, userName, password, method, xmlData);
        }
        public string UpdateCallData(string userName, string extension, string password, string dialogID, Dictionary<string, string> callVariables, string wrapupReason)
        {
            string method = "PUT";
            string url = WebappPath + "/api/Dialog/" + dialogID;
            //Build Call Variables Block
            string callVarXmlData = null;
            if (callVariables != null)
            {
                foreach (KeyValuePair<string, string> valuePair in callVariables)
                {
                    if (callVarXmlData == null)
                        callVarXmlData = "<callvariables>";
                    callVarXmlData += "<CallVariable><name>" + valuePair.Key + "</name><value>" + valuePair.Value + "</value></CallVariable>";
                }
                if (callVarXmlData != null)
                    callVarXmlData += "</callvariables>";
            }
            //Build Wrap Reason Block
            string callWrapupXmlData = null;
            if (wrapupReason != null)
            {
                callWrapupXmlData = "<wrapUpReason>" + wrapupReason + "</wrapUpReason>";
            }
            //Build Media Properties Block
            string callMediaPropertiesBlock = "<mediaProperties>";
            if (callWrapupXmlData != null)
                callMediaPropertiesBlock += callWrapupXmlData;
            if (callVarXmlData != null)
                callMediaPropertiesBlock += callVarXmlData;
            callMediaPropertiesBlock += "</mediaProperties>";

            // try to add if the connnection is failed dialogId(required): The ID of the dialog 
            // the requested Action did not find it the drop call 
            // need to check if it is working or not 
            string xmlData = "<Dialog><requestedAction>UPDATE_CALL_DATA</requestedAction>" + callMediaPropertiesBlock + "</Dialog>";
            return SendHTTPRequest(url, userName, password, method, xmlData);
        }

        /**
         *  NOT working
         * Subscribes to Agent state events.
         *
         * @param {String} agentId
         *      The Agent's username or Id.
         * @param {Function(Object)} rspHandler
         *      Callback to handle response.
         * @param {Function(Object)} errHandler
         *      Callback when error making the request.
         * @memberOf cisco.desktop.services.Transporter#
         */
        // not working 
        public void SubscribeToState(string userName, string agentID, string password)
        {
            var method = "POST";
            string url = WebappPath + "/api/User/" + agentID;
            string xmlData = "<source>/finesse/api/User/" + userName + "</source>";
            string result = SendHTTPRequest(url, userName, password, method, xmlData);
        }
        public string GetResonCodeList(string userName, string agentID, string password, string category)
        {
            //int counter = 0;
            var method = "GET";
            string url = WebappPath + "/api/User/" + agentID + "/ReasonCodes?category=" + category;
            string xmlData = string.Empty;
            return SendHTTPRequest(url, userName, password, method, xmlData);
        }
        // not tested yet , error might be to in xml be hold
        public string SendHTTPRequest(string uri, string userName, string password, string method, string xmlParam)
        {
            string result = "error";
            if (restClient == null)
            {
                if (_FinAgent.AgentInformation.SSL) {
                    System.Net.ServicePointManager.Expect100Continue = true;
                    if (_FinAgent.AgentInformation.HTTPConnectionType == null)
                        return result + "Http Connection Type is undefined";
                    else if (_FinAgent.AgentInformation.HTTPConnectionType.Equals("Ssl3"))
                        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3;
                    else if (_FinAgent.AgentInformation.HTTPConnectionType.Equals("Tls"))
                        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls;
                    else if (_FinAgent.AgentInformation.HTTPConnectionType.Equals("Tls11"))
                        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11;
                    else if (_FinAgent.AgentInformation.HTTPConnectionType.Equals("Tls12"))
                        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                }
                restClient = new RestClient(WebappPath)
                    {
                    Authenticator = new HttpBasicAuthenticator(userName, password)
                };
            }

            RestRequest request = null;
            if(method.Equals("GET"))
                request = new RestRequest(uri.Substring((WebappPath+"/").Length), Method.GET);
            else if (method.Equals("POST"))
                request = new RestRequest(uri.Substring((WebappPath + "/").Length), Method.POST);
            else if (method.Equals("PUT"))
                request = new RestRequest(uri.Substring((WebappPath + "/").Length), Method.PUT);
            if (request != null)
            {
                if (xmlParam != string.Empty)
                {
                    request.AddHeader("Content-Type", "application/xml");
                    //Parameter parameter = new Parameter();
                    //parameter.Value = xmlParam;
                    byte[] data = Encoding.UTF8.GetBytes(xmlParam);
                    request.AddParameter("application/xml", data, ParameterType.RequestBody);
                    //request.AddParameter(parameter);

                }

                IRestResponse response = restClient.Execute(request);


                if (response != null && response.ResponseStatus == ResponseStatus.Completed)
                {
                    result = response.Content;
                }
                else if (response != null)
                {
                    _FinAgent.FireDebugLogMessage("Error in rest communication, response is:" + response.Content);
                    _FinAgent.FireDebugLogMessage("Error in rest communication, response Exception is:" + response.ErrorException);
                }
            }

            return result;
            
        }
    }
}
