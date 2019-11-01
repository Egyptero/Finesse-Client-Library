using CXDE.Server_Side;
using System.Collections.Generic;
using static CXDE.Server_Side.Dialog.MediaProperties;

namespace CXDE.RJB_Customization
{
    public class IVROptions
    {
        public enum TransferType
        {
            INQ,
            COMP,
            SUPP
        }
        private string[] IVROptionsGroupA = { "Survey", "Current Accounts", "Transfer & Payments", "Credit Card Services", "Financing Services", "Online,Mobile & Other Services" };
        private string[] IVROptionsGroupB = { "Survey" };
        public const string SURVEY_EXT = "8555";
        public const string SKILLGROUP_EXT = "8999";
        private const string SkillGroup_CreditCard_Mass_Comp = "CreditCard_Mass_Comp";
        private const string SkillGroup_CreditCard_Mass_Inq = "CreditCard_Mass_Inq";
        private const string SkillGroup_CreditCard_Mass_Supp = "CreditCard_Mass_Supp";

        private const string SkillGroup_CreditCard_VIP_Comp = "CreditCard_VIP_Comp";
        private const string SkillGroup_CreditCard_VIP_Inq = "CreditCard_VIP_Inq";
        private const string SkillGroup_CreditCard_VIP_Supp = "CreditCard_VIP_Supp";

        private const string SkillGroup_CurrentAccount_Mass_Comp = "CurrentAccount_Mass_Comp";
        private const string SkillGroup_CurrentAccount_Mass_Inq = "CurrentAccount_Mass_Inq";
        private const string SkillGroup_CurrentAccount_Mass_Supp = "CurrentAccount_Mass_Supp";

        private const string SkillGroup_CurrentAccount_VIP_Comp = "CurrentAccount_VIP_Comp";
        private const string SkillGroup_CurrentAccount_VIP_Inq = "CurrentAccount_VIP_Inq";
        private const string SkillGroup_CurrentAccount_VIP_Supp = "CurrentAccount_VIP_Supp";

        private const string SkillGroup_Finance_Mass_Comp = "Finance_Mass_Comp";
        private const string SkillGroup_Finance_Mass_Inq = "Finance_Mass_Inq";
        private const string SkillGroup_Finance_Mass_Supp = "Finance_Mass_Supp";

        private const string SkillGroup_Finance_VIP_Comp = "Finance_VIP_Comp";
        private const string SkillGroup_Finance_VIP_Inq = "Finance_VIP_Inq";
        private const string SkillGroup_Finance_VIP_Supp = "Finance_VIP_Supp";

        private const string SkillGroup_Online_Mass_Comp = "Online_Mass_Comp";
        private const string SkillGroup_Online_Mass_Inq = "Online_Mass_Inq";
        private const string SkillGroup_Online_Mass_Supp = "Online_Mass_Supp";

        private const string SkillGroup_Online_VIP_Comp = "Online_VIP_Comp";
        private const string SkillGroup_Online_VIP_Inq = "Online_VIP_Inq";
        private const string SkillGroup_Online_VIP_Supp = "Online_VIP_Supp";

        private const string SkillGroup_Transfers_Mass_Comp = "Transfers_Mass_Comp";
        private const string SkillGroup_Transfers_Mass_Inq = "Transfers_Mass_Inq";
        private const string SkillGroup_Transfers_Mass_Supp = "Transfers_Mass_Supp";

        private const string SkillGroup_Transfers_VIP_Comp = "Transfers_VIP_Comp";
        private const string SkillGroup_Transfers_VIP_Inq = "Transfers_VIP_Inq";
        private const string SkillGroup_Transfers_VIP_Supp = "Transfers_VIP_Supp";

        private string _CallVariable7;
        private string _DialogID;
        private Dialog _Dialog;
        private FinAgent _FinAgent;
        private bool _Male;
        private TransferType _TransferType;
        public string _CurrentSkill { get; set; }
        public string _TargetSkill { get; set; }
        public IVROptions(FinAgent finAgent, string dialogID)
        {
            _DialogID = dialogID;
            _FinAgent = finAgent;
            _Dialog = finAgent.FindDialog(_DialogID);
            if (_Dialog != null)
            {
                if (((CallVariable)_Dialog._MediaProperties._CallVariables[6])._Value != null)
                    _CallVariable7 = ((CallVariable)_Dialog._MediaProperties._CallVariables[6])._Value;//Read the value of Call Variable 7
                else
                    _CallVariable7 = "";
            }
        }
        public string[] GetTransferOptions()
        {
            if (_CallVariable7 == null)
                return IVROptionsGroupB;

            if (_CallVariable7.Equals("Retail-Int") || _CallVariable7.Equals("VIP") || _CallVariable7.Equals("VIP-Intl") || _CallVariable7.Equals("Retail"))
                return IVROptionsGroupA;
            else
                return IVROptionsGroupB;
        }
        public bool IsExtendedParametersEnabled()
        {
            if (_CallVariable7 == null)
                return false;

            if (_CallVariable7.Equals("Retail-Int") || _CallVariable7.Equals("VIP") || _CallVariable7.Equals("VIP-Intl") || _CallVariable7.Equals("Retail"))
                return true;
            else
                return false;
        }
        private bool UpdateCallData(string selection)
        {
            if (selection == null || selection == "")
                return false;
            if (selection.Equals("Survey"))
            {
                Dictionary<string, string> callVariablesData = new Dictionary<string, string>();
                if (_FinAgent != null && _FinAgent._agentInformation != null)
                {
                callVariablesData.Add("callVariable8", "AgentId=" + _FinAgent._agentInformation.AgentID);
                callVariablesData.Add("callVariable9", _FinAgent._agentInformation.Extension);
                }
                if (_Dialog != null && _Dialog._MediaProperties != null && _Dialog._MediaProperties._CallVariables != null && _Dialog._MediaProperties._CallVariables.Count >= 2)
                {
                if (((CallVariable)_Dialog._MediaProperties._CallVariables[1])._Value != null)
                callVariablesData.Add("callVariable2", ((CallVariable)_Dialog._MediaProperties._CallVariables[1])._Value);
                }
                _FinAgent.UpdateCallData(_DialogID, callVariablesData, null);
                    return true;
            }
            else
            {
                string lang = string.Empty;
                string skill = string.Empty;
                string cic = string.Empty;
                string ciscoguid = string.Empty;
                string dialedNumber = string.Empty;

                if (_Dialog != null && _Dialog._MediaProperties != null && _Dialog._MediaProperties._CallVariables != null && _Dialog._MediaProperties._CallVariables.Count >= 7)
                    dialedNumber = ((CallVariable)_Dialog._MediaProperties._CallVariables[6])._Value;
                if (_Dialog != null && _Dialog._MediaProperties != null && _Dialog._MediaProperties._CallVariables != null && _Dialog._MediaProperties._CallVariables.Count >= 10)
                    ciscoguid = ((CallVariable)_Dialog._MediaProperties._CallVariables[9])._Value;
                if (_Dialog != null && _Dialog._MediaProperties != null && _Dialog._MediaProperties._CallVariables != null && _Dialog._MediaProperties._CallVariables.Count >= 4)
                    cic = ((CallVariable)_Dialog._MediaProperties._CallVariables[3])._Value;
                if (_Dialog != null && _Dialog._MediaProperties != null && _Dialog._MediaProperties._CallVariables != null && _Dialog._MediaProperties._CallVariables.Count >= 5)
                    skill = ((CallVariable)_Dialog._MediaProperties._CallVariables[4])._Value;
                if (_Dialog != null && _Dialog._MediaProperties != null && _Dialog._MediaProperties._CallVariables != null && _Dialog._MediaProperties._CallVariables.Count >= 2)
                    lang = ((CallVariable)_Dialog._MediaProperties._CallVariables[1])._Value;

                _CurrentSkill = skill.ToString();

                string targetSkill = null;
                int selectedIndex = 0;
                for (int counter = 0; counter < IVROptionsGroupA.Length; counter++)
                    if (IVROptionsGroupA[counter].Equals(selection))
                        selectedIndex = counter;

                if (lang == null || lang.Equals(""))
                    lang = "NA";
                if (skill == null || skill.Equals(""))
                {
                    skill = "NA";
                    dialedNumber = dialedNumber + "_NA";
                }
                if (dialedNumber == null || dialedNumber.Equals(""))
                    dialedNumber = "NA";
                if (cic == null || cic.Equals(""))
                    cic = "NA";
                if (ciscoguid == null || ciscoguid.Equals(""))
                    ciscoguid = "NA";

                if (skill.Contains("VIP"))
                {
                    switch (_TransferType)
                    {
                        case TransferType.INQ:
                            switch (selectedIndex)
                            {
                                case 1:
                                    targetSkill = SkillGroup_CurrentAccount_VIP_Inq;
                                    break;
                                case 2:
                                    targetSkill = SkillGroup_Transfers_VIP_Inq;
                                    break;
                                case 3:
                                    targetSkill = SkillGroup_CreditCard_VIP_Inq;
                                    break;
                                case 4:
                                    targetSkill = SkillGroup_Finance_VIP_Inq;
                                    break;
                                case 5:
                                    targetSkill = SkillGroup_Online_VIP_Inq;
                                    break;
                            }

                            break;
                        case TransferType.COMP:
                            switch (selectedIndex)
                            {
                                case 1:
                                    targetSkill = SkillGroup_CurrentAccount_VIP_Comp;
                                    break;
                                case 2:
                                    targetSkill = SkillGroup_Transfers_VIP_Comp;
                                    break;
                                case 3:
                                    targetSkill = SkillGroup_CreditCard_VIP_Comp;
                                    break;
                                case 4:
                                    targetSkill = SkillGroup_Finance_VIP_Comp;
                                    break;
                                case 5:
                                    targetSkill = SkillGroup_Online_VIP_Comp;
                                    break;
                            }
                            break;
                        case TransferType.SUPP:
                            switch (selectedIndex)
                            {
                                case 1:
                                    targetSkill = SkillGroup_CurrentAccount_VIP_Supp;
                                    break;
                                case 2:
                                    targetSkill = SkillGroup_Transfers_VIP_Supp;
                                    break;
                                case 3:
                                    targetSkill = SkillGroup_CreditCard_VIP_Supp;
                                    break;
                                case 4:
                                    targetSkill = SkillGroup_Finance_VIP_Supp;
                                    break;
                                case 5:
                                    targetSkill = SkillGroup_Online_VIP_Supp;
                                    break;
                            }
                            break;
                    }
                }
                else
                {
                    switch (_TransferType)
                    {
                        case TransferType.INQ:
                            switch (selectedIndex)
                            {
                                case 1:
                                    targetSkill = SkillGroup_CurrentAccount_Mass_Inq;
                                    break;
                                case 2:
                                    targetSkill = SkillGroup_Transfers_Mass_Inq;
                                    break;
                                case 3:
                                    targetSkill = SkillGroup_CreditCard_Mass_Inq;
                                    break;
                                case 4:
                                    targetSkill = SkillGroup_Finance_Mass_Inq;
                                    break;
                                case 5:
                                    targetSkill = SkillGroup_Online_Mass_Inq;
                                    break;
                            }

                            break;
                        case TransferType.COMP:
                            switch (selectedIndex)
                            {
                                case 1:
                                    targetSkill = SkillGroup_CurrentAccount_Mass_Comp;
                                    break;
                                case 2:
                                    targetSkill = SkillGroup_Transfers_Mass_Comp;
                                    break;
                                case 3:
                                    targetSkill = SkillGroup_CreditCard_Mass_Comp;
                                    break;
                                case 4:
                                    targetSkill = SkillGroup_Finance_Mass_Comp;
                                    break;
                                case 5:
                                    targetSkill = SkillGroup_Online_Mass_Comp;
                                    break;
                            }
                            break;
                        case TransferType.SUPP:
                            switch (selectedIndex)
                            {
                                case 1:
                                    targetSkill = SkillGroup_CurrentAccount_Mass_Supp;
                                    break;
                                case 2:
                                    targetSkill = SkillGroup_Transfers_Mass_Supp;
                                    break;
                                case 3:
                                    targetSkill = SkillGroup_CreditCard_Mass_Supp;
                                    break;
                                case 4:
                                    targetSkill = SkillGroup_Finance_Mass_Supp;
                                    break;
                                case 5:
                                    targetSkill = SkillGroup_Online_Mass_Supp;
                                    break;
                            }
                            break;

                    }
                }


                // After Setting parameters
                // Checking Male option
                if (!skill.Equals("NA"))
                {
                    if (skill.Contains("_F"))
                    {
                        if (!_Male)
                        {
                            if (targetSkill.Contains("_Mass"))
                                targetSkill = targetSkill.Replace("_Mass", "_F_Mass");
                            if (targetSkill.Contains("_VIP"))
                                targetSkill = targetSkill.Replace("_VIP", "_F_VIP");
                        }
                    }
                }

                Dictionary<string, string> callVariablesData = new Dictionary<string, string>();
                callVariablesData.Add("callVariable8", "AgentId=" + _FinAgent._agentInformation.AgentID);
                callVariablesData.Add("callVariable9", _FinAgent._agentInformation.Extension);
                callVariablesData.Add("callVariable2", lang);
                callVariablesData.Add("callVariable6", "CTI");
                callVariablesData.Add("callVariable5", targetSkill);
                callVariablesData.Add("callVariable7", dialedNumber);
                _TargetSkill = targetSkill.ToString();

                if (_FinAgent.UpdateCallData(_DialogID, callVariablesData, null))
                    return true;
            }

            return false;
        }
        public string GetExtension(string selection, bool male, TransferType type)
        {
            _Male = male;
            _TransferType = type;
            if (_Dialog == null)
                return null;
            if (UpdateCallData(selection))
            {
                if (selection.Equals("Survey"))
                    return SURVEY_EXT;
                else
                    return SKILLGROUP_EXT;
            }
            return SURVEY_EXT; // By Default Route to Survey
        }
    }
}
