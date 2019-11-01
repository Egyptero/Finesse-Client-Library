using FinesseClient.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using static FinesseClient.Model.Dialog.MediaPropertiesClass;

namespace FUC_WPF
{
    class VariableLocator : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Dialog dialog = value as Dialog;
            if(dialog != null)
                if(dialog.MediaProperties != null)
                    if(dialog.MediaProperties.CallVariables != null)
                    {
                        foreach(CallVariableClass callVariable in dialog.MediaProperties.CallVariables)
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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter;
        }
    }
}
