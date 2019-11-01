using FinesseClient.Model;
using FUC_WPF.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using static FinesseClient.Model.Dialog.MediaPropertiesClass;

namespace FUC_WPF
{
    public class DialogExtensionLocator : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter.ToString().Equals("CallVariablesStatus"))
            {
                Dialog dialog = value as Dialog;
                bool dataFound = false; 
                if(dialog != null && dialog.MediaProperties != null && dialog.MediaProperties.CallVariables != null)
                {
                    foreach(CallVariableClass callVariable in dialog.MediaProperties.CallVariables)
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
                    return Visibility.Visible;
                }
                return Visibility.Hidden; 
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
                else if (parameter.ToString().Equals("CallButtons"))
                {
                    if (dialogExtension == null)
                        return Visibility.Visible;
                    else if (dialogExtension.Enabled)
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
                else if (parameter.ToString().Equals("CallButtonsWaiting"))
                {
                    if (dialogExtension == null)
                        return Visibility.Collapsed;
                    else if (dialogExtension.Disabled)
                        return Visibility.Visible;
                    else
                        return Visibility.Collapsed;
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
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter;
        }
    }
}
