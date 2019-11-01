using FinesseClient.Model;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace FUCX_WPF
{
    public class RecentLocator : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (parameter.ToString().Equals("CallIcon"))
                {
                    Dialog dialog = value as Dialog;
                    if (dialog == null)
                        return PackIconKind.DotsHorizontal;
                    else if (dialog.State.Equals("FAILED")) // Missed Call
                    {
                        if (dialog.CallDirection.Equals("Out"))
                            return PackIconKind.PhoneHangup;
                        else
                            return PackIconKind.PhoneMissed;
                    }
                    else if (dialog.State.Equals("DROPPED")) // Unanswered Call
                    {
                        if (dialog.CallDirection.Equals("Out"))
                            return PackIconKind.PhoneHangup;
                        else
                            return PackIconKind.PhoneMissed;
                    }
                    else
                    {
                        if (dialog.CallDirection.Equals("Out"))
                            return PackIconKind.PhoneOutgoing;
                        else
                            return PackIconKind.PhoneIncoming;
                    }
                }
                else if (parameter.ToString().Equals("CallIconColor"))
                {
                    Dialog dialog = value as Dialog;
                    if (dialog == null)
                        return new SolidColorBrush(Colors.Transparent);
                    else if (dialog.State.Equals("FAILED")) // Missed Call
                    {
                        return new SolidColorBrush(Colors.Red);
                    }
                    else if (dialog.State.Equals("DROPPED")) // Unanswered Call
                    {
                        return new SolidColorBrush(Colors.Blue);
                    }
                    else
                    {
                        if (dialog.CallDirection.Equals("Out"))
                            return new SolidColorBrush(Colors.Black);
                        else
                            return new SolidColorBrush(Colors.Transparent);
                    }
                }
                return "";
            }
            catch (Exception) {
                if (parameter.ToString().Equals("CallIcon"))
                    return PackIconKind.DotsHorizontal;
                else if (parameter.ToString().Equals("CallIconColor"))
                    return new SolidColorBrush(Colors.Transparent);
                else
                    return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
