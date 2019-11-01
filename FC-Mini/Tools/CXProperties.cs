using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC_Mini.Tools
{
    public class CXProperties
    {
        private Dictionary<String, String> list;

        private String filename;

        public bool fileExist { get; set; }

        public CXProperties(String file)
        {
            fileExist = reload(file);
        }

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

        public bool reload()
        {
            return reload(this.filename);
        }

        public bool reload(String filename)
        {
            this.filename = filename;
            list = new Dictionary<String, String>();

            if (System.IO.File.Exists(filename))
                loadFromFile(filename);
            else
                return false;
            return true;
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
    }
}
