using FinesseClient.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace FinesseDE
{
    public class CXProperties : ObservableObject
    {
        #region fields
        private Dictionary<String, String> list;
        private String filename;
        #endregion

        #region Properties
        public Color PageBackground { get { return FromHex(get("PageBackground", "#00FFFFFF")); } set { set("PageBackground",value.ToString()); OnPropertyChanged("PageBackground"); } }
        public Color AgentStatusbarBackground { get { return FromHex(get("AgentStatusbarBackground", "#FFBCBCBC")); } set { set("AgentStatusbarBackground", value.ToString()); OnPropertyChanged("AgentStatusbarBackground"); } }
        public Color StatusbarBackground { get { return FromHex(get("StatusbarBackground", "#FFBCBCBC")); } set { set("StatusbarBackground", value.ToString()); OnPropertyChanged("StatusbarBackground"); } }
        public Color DialpadBackground { get { return FromHex(get("DialpadBackground", "#FF1B4F78")); } set { set("DialpadBackground", value.ToString()); OnPropertyChanged("DialpadBackground"); } }
        public Color DialogBackground { get { return FromHex(get("DialogBackground", "#FFF5F5F5")); } set { set("DialogBackground", value.ToString()); OnPropertyChanged("DialpadBackground"); } }
        public Color SelectedDialogBackground { get { return FromHex(get("SelectedDialogBackground", "#FFBCBCBC")); } set { set("SelectedDialogBackground", value.ToString()); OnPropertyChanged("SelectedDialogBackground"); } }
        public Color TabBackground { get { return FromHex(get("TabBackground", "#FFD2691E")); } set { set("TabBackground", value.ToString()); OnPropertyChanged("TabBackground"); } }
        public Color TabHeaderBackground { get { return FromHex(get("TabHeaderBackground", "#FFBCBCBC")); } set { set("TabHeaderBackground", value.ToString()); OnPropertyChanged("TabHeaderBackground"); } }
        public Color SelectedHeaderTabBackground { get { return FromHex(get("SelectedHeaderTabBackground", "#FF1B4F78")); } set { set("SelectedHeaderTabBackground", value.ToString()); OnPropertyChanged("SelectedHeaderTabBackground"); } }
        public Color SplitterBackground { get { return FromHex(get("SplitterBackground", "#FFFFFFFF")); } set { set("SplitterBackground", value.ToString()); OnPropertyChanged("SplitterBackground"); } }
        public Color SparatorColor { get { return FromHex(get("SparatorColor", "#FFFFFFFF")); } set { set("SparatorColor", value.ToString()); OnPropertyChanged("SparatorColor"); } }
        public Color GadgetBackground { get { return FromHex(get("GadgetBackground", "#FFC1D4DB")); } set { set("GadgetBackground", value.ToString()); OnPropertyChanged("GadgetBackground"); } }
        public Color GadgetHeaderBackground { get { return FromHex(get("GadgetHeaderBackground", "#FFBCBCBC")); } set { set("GadgetHeaderBackground", value.ToString()); OnPropertyChanged("GadgetHeaderBackground"); } }
        public Color PageForground { get { return FromHex(get("PageForground", "#FFFFFF")); } set { set("PageForground", value.ToString()); OnPropertyChanged("PageForground"); } }
        public Color AgentStatusbarForground { get { return FromHex(get("AgentStatusbarForground", "#FF000000")); } set { set("AgentStatusbarForground", value.ToString()); OnPropertyChanged("AgentStatusbarForground"); } }
        public Color StatusbarForground { get { return FromHex(get("StatusbarForground", "#FF000000")); } set { set("StatusbarForground", value.ToString()); OnPropertyChanged("StatusbarForground"); } }
        public Color DialpadForground { get { return FromHex(get("DialpadForground", "#FFFFFFFF")); } set { set("DialpadForground", value.ToString()); OnPropertyChanged("DialpadForground"); } }
        public Color DialogForground { get { return FromHex(get("DialogForground", "#FF000000")); } set { set("DialogForground", value.ToString()); OnPropertyChanged("DialpadForground"); } }
        public Color SelectedDialogForground { get { return FromHex(get("SelectedDialogForground", "#FF000000")); } set { set("SelectedDialogForground", value.ToString()); OnPropertyChanged("SelectedDialogForground"); } }
        public Color TabForground { get { return FromHex(get("TabForground", "#FFD2691E")); } set { set("TabForground", value.ToString()); OnPropertyChanged("TabForground"); } }
        public Color TabHeaderForground { get { return FromHex(get("TabHeaderForground", "#FFFFFFFF")); } set { set("TabHeaderForground", value.ToString()); OnPropertyChanged("TabHeaderForground"); } }
        public Color SelectedHeaderTabForground { get { return FromHex(get("SelectedHeaderTabForground", "#FFFFFFFF")); } set { set("SelectedHeaderTabForground", value.ToString()); OnPropertyChanged("SelectedHeaderTabForground"); } }
        public Color GadgetForground { get { return FromHex(get("GadgetForground", "#FF000000")); } set { set("GadgetForground", value.ToString()); OnPropertyChanged("GadgetForground"); } }
        public Color GadgetHeaderForground { get { return FromHex(get("GadgetHeaderForground", "#FF000000")); } set { set("GadgetHeaderForground", value.ToString()); OnPropertyChanged("GadgetHeaderForground"); } }
        public String BackgroundImage { get { return "/Images/Desktop/"+SelectedBackgroundImage+".jpg"; } }
        public String SelectedBackgroundImage { get { return get("BackgroundImage", "Default"); } set { set("BackgroundImage", value); OnPropertyChanged("BackgroundImage"); OnPropertyChanged("SelectedBackgroundImage"); } }
        #endregion
        #region Constructors
        public CXProperties()
        {
            var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FinesseDETheme.properties");
            reload(fileName);
        }

        public CXProperties(String file)
        {
            reload(file);
        }
        #endregion
        public String get(String field, String defValue)
        {
            return (get(field) == null) ? (defValue) : (get(field));
        }
        public String get(String field)
        {
            return (list.ContainsKey(field)) ? (list[field]) : (null);
        }

        public void set(String field, Object value)
        {
            if (value == null)
                return;
            field = this.trimUnwantedChars(field);
            value = this.trimUnwantedChars(value.ToString());

            if (!list.ContainsKey(field))
                list.Add(field, value.ToString());
            else
                list[field] = value.ToString();
        }

        public string trimUnwantedChars(string toTrim)
        {
            toTrim = toTrim.Replace(";", string.Empty);
            toTrim = toTrim.Replace("#", string.Empty);
            toTrim = toTrim.Replace("'", string.Empty);
            toTrim = toTrim.Replace("=", string.Empty);
            return toTrim;
        }

        public void Save()
        {
            Save(this.filename);
        }

        public void Save(String filename)
        {
            this.filename = filename;

            if (!System.IO.File.Exists(filename))
                System.IO.File.Create(filename);

            System.IO.StreamWriter file = new System.IO.StreamWriter(filename);

            foreach (String prop in list.Keys.ToArray())
                if (!String.IsNullOrWhiteSpace(list[prop]))
                    file.WriteLine(prop + "=" + list[prop]);

            file.Close();
        }

        public void reload()
        {
            reload(this.filename);
        }

        public void reload(String filename)
        {
            this.filename = filename;
            list = new Dictionary<String, String>();

            if (System.IO.File.Exists(filename))
                loadFromFile(filename);
            else
                System.IO.File.Create(filename);
        }

        private void loadFromFile(String file)
        {
            foreach (String line in System.IO.File.ReadAllLines(file))
            {
                if ((!String.IsNullOrEmpty(line)) &&
                    (!line.StartsWith(";")) &&
                    (!line.StartsWith("#")) &&
                    (!line.StartsWith("'")) &&
                    (line.Contains('=')))
                {
                    int index = line.IndexOf('=');
                    String key = line.Substring(0, index).Trim();
                    String value = line.Substring(index + 1).Trim();

                    if ((value.StartsWith("\"") && value.EndsWith("\"")) ||
                        (value.StartsWith("'") && value.EndsWith("'")))
                    {
                        value = value.Substring(1, value.Length - 2);
                    }

                    try
                    {
                        //ignore duplicates
                        list.Add(key, value);
                    }
                    catch { return; }
                }
            }
        }
        private Color FromHex(string colorcode)
        {
            if (!colorcode.StartsWith("#"))
                colorcode = "#" + colorcode;
            System.Drawing.Color myColor = System.Drawing.ColorTranslator.FromHtml(colorcode);
            return Color.FromArgb(myColor.A, myColor.R, myColor.G, myColor.B);
        }
    }
}
