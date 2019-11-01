using FinesseClient.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using static FinesseClient.Model.Dialog.MediaPropertiesClass;

namespace FUCX_WPF
{
    class VariableLocator : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (parameter.ToString().Equals("CallDialPadForm"))
                {
                    if (value == null)
                        return Visibility.Visible;
                    else if ((value as string).Equals("Keypad"))
                        return Visibility.Hidden;
                    else
                        return Visibility.Visible;
                }
                else if (parameter.ToString().Equals("IconColor"))
                {
                    VoiceStatus voiceStatus = value as VoiceStatus;
                    if (voiceStatus == null)
                        return new SolidColorBrush(Colors.Gray);
                    else if (voiceStatus.Status.Equals("READY"))
                        return new SolidColorBrush(Colors.YellowGreen);
                    else if (voiceStatus.Status.Equals("NOT_READY"))
                        return new SolidColorBrush(Colors.Red);
                    else if (voiceStatus.Status.Equals("TALKING") || voiceStatus.Status.Equals("RESERVED"))
                        return new SolidColorBrush(Colors.Blue);
                    else if (voiceStatus.Status.Equals("HOLD"))
                        return new SolidColorBrush(Colors.Orange);
                    else
                        return new SolidColorBrush(Colors.Gray);
                }
                else
                {
                    Dialog dialog = value as Dialog;
                    if (dialog != null)
                        if (dialog.MediaProperties != null)
                            if (dialog.MediaProperties.CallVariables != null)
                            {
                                foreach (CallVariableClass callVariable in dialog.MediaProperties.CallVariables)
                                {
                                    if (callVariable.Name.Equals(parameter.ToString()))
                                    {
                                        if (callVariable.Value != null && callVariable.Value.Trim().Length > 0)
                                            return callVariable.Value;
                                        else
                                            return "No data!!";
                                    }

                                }
                            }

                    return "";
                }
            }
            catch (Exception) {
                if (parameter.ToString().Equals("CallDialPadForm"))
                    return Visibility.Visible;
                else if (parameter.ToString().Equals("IconColor"))
                    return new SolidColorBrush(Colors.Gray);
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
