using FinesseClient.Model;
using CXConnect.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using static FinesseClient.Model.Dialog.MediaPropertiesClass;
using System.Collections.ObjectModel;
using FinesseClient.Common;

namespace CXConnect
{
    public class DialogExtensionLocator : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (parameter.ToString().Equals("ShowCallsPanel"))
                {
                    MTObservableCollection<Dialog> dialogs = value as MTObservableCollection<Dialog>;
                    if (dialogs == null)
                        return Visibility.Hidden;
                    else if (dialogs.Count > 0)
                        return Visibility.Visible;
                    else
                        return Visibility.Hidden;
                }
                else if (parameter.ToString().Equals("ShowCallsPage"))
                {
                    MTObservableCollection<Dialog> dialogs = value as MTObservableCollection<Dialog>;
                    if (dialogs == null)
                        return false;
                    else if (dialogs.Count > 0)
                        return true;
                    else
                        return false;
                }
                else if (parameter.ToString().Equals("CallVariablesStatus"))
                {
                    Dialog dialog = value as Dialog;
                    bool dataFound = true;
                    if (dialog != null && dialog.MediaProperties != null && dialog.MediaProperties.CallVariables != null)
                    {
                        foreach (CallVariableClass callVariable in dialog.MediaProperties.CallVariables)
                        {
                            if (callVariable.Name.Equals("callVariable5"))
                                if (callVariable.Value != null && callVariable.Value.Trim() != "")
                                    dataFound = true;
                            if (callVariable.Name.Equals("callVariable2"))
                                if (callVariable.Value != null && callVariable.Value.Trim() != "")
                                    dataFound = true;
                            if (callVariable.Name.Equals("callVariable6"))
                                if (callVariable.Value != null && callVariable.Value.Trim() != "")
                                    dataFound = true;
                        }
                    }
                    if (dataFound)
                        return Visibility.Visible;
                    else
                        return Visibility.Collapsed;
                }
                else if (parameter.ToString().Equals("SendToIVRButton"))
                {
                    Dialog dialog = value as Dialog;
                    string callVariable7 = string.Empty;
                    if (dialog != null && dialog.MediaProperties != null && dialog.MediaProperties.CallVariables != null)
                    {
                        foreach (CallVariableClass callVariable in dialog.MediaProperties.CallVariables)
                        {
                            if (callVariable.Name.Equals("callVariable7"))
                                if (callVariable.Value != null && callVariable.Value.Trim() != "")
                                    callVariable7 = callVariable.Value;
                        }
                    }
                    if (callVariable7.Equals("Retail-Int") || callVariable7.Equals("VIP") || callVariable7.Equals("VIP-Intl") || callVariable7.Equals("Retail"))
                    {
                        return true;
                    }
                    return false;
                }
                else
                {
                    DialogExtension dialogExtension = value as DialogExtension;
                    if (parameter.ToString().Equals("DialPad"))
                    {
                        if (dialogExtension == null)
                            return Visibility.Collapsed;
                        else if (dialogExtension.DialPadStatus)
                            return Visibility.Visible;
                        else
                            return Visibility.Collapsed;
                    }
                    else if (parameter.ToString().Equals("UpdateRoutingData"))
                    {
                        if (dialogExtension == null)
                            return Visibility.Collapsed;
                        else if (dialogExtension.UpdateRoutingData)
                            return Visibility.Visible;
                        else
                            return Visibility.Collapsed;
                    }
                    else if (parameter.ToString().Equals("CallButtonItem"))
                    {
                        if (dialogExtension == null)
                            return true;
                        else if (dialogExtension.Enabled)
                            return true;
                        else
                            return false;
                    }
                    else if (parameter.ToString().Equals("CallWaiting"))
                    {
                        if (dialogExtension == null)
                            return "0";
                        else if (dialogExtension.Disabled)
                            return "1";
                        else
                            return "0";
                    }
                    else if (parameter.ToString().Equals("WaitingMessage"))
                    {
                        if (dialogExtension == null)
                            return "";
                        else if (dialogExtension.Message != null)
                            return dialogExtension.Message;
                        else
                            return "";
                    }
                }
                return "";
            }
            catch (Exception)
            {
                if (parameter.ToString().Equals("ShowCallsPanel"))
                    return Visibility.Hidden;
                else if (parameter.ToString().Equals("ShowCallsPage"))
                    return false;
                else if (parameter.ToString().Equals("CallVariablesStatus"))
                    return Visibility.Collapsed;
                else if (parameter.ToString().Equals("SendToIVRButton"))
                    return false;
                else if (parameter.ToString().Equals("DialPad"))
                    return Visibility.Collapsed;
                else if (parameter.ToString().Equals("UpdateRoutingData"))
                    return Visibility.Collapsed;
                else if (parameter.ToString().Equals("CallButtonItem"))
                    return false;
                else if (parameter.ToString().Equals("CallWaiting"))
                    return "0";
                else if (parameter.ToString().Equals("WaitingMessage"))
                    return "";
                else
                    return "";

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter;
        }
    }
}
