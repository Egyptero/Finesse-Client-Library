using FinesseClient.Common;
using FinesseClient.License.Commands;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Management;
using FinesseClient.License.Security;
using System.IO;

namespace FinesseClient.License.Model
{
    public partial class AgentLicense: ObservableObject
    {
        private const string URI = "http://ec2-18-222-194-188.us-east-2.compute.amazonaws.com:8080/api/";
        #region fields
        private string _name = string.Empty;
        private string _email = string.Empty;
        private string _phone = string.Empty;
        private string _companyName = string.Empty;
        private string _package = "Trial";
        private string _token;
        private string _license;
        private ICommand _freeTrialCommand;
        private ICommand _orderCommand;
        private ICommand _saveCommand;
        private ICommand _activateCommand;
        private ICommand _submitCommand;

        #endregion

        #region properties
        public String Name { get { return _name; } set { _name = value; OnPropertyChanged("Name"); OnPropertyChanged("FreeTrialCommand"); OnPropertyChanged("OrderCommand"); } }
        public String Email { get { return _email; } set { _email = value; OnPropertyChanged("Email"); OnPropertyChanged("FreeTrialCommand"); OnPropertyChanged("OrderCommand");  } }
        public String Phone { get { return _phone; } set { _phone = value; OnPropertyChanged("Phone"); OnPropertyChanged("FreeTrialCommand"); OnPropertyChanged("OrderCommand");  } }
        public String CompanyName { get { return _companyName; } set { _companyName = value; OnPropertyChanged("CompanyName"); OnPropertyChanged("FreeTrialCommand"); OnPropertyChanged("OrderCommand");  } }
        public String Token { get { return _token; } set { _token = value; OnPropertyChanged("Token"); OnPropertyChanged("ActivateCommand"); } }
        public String License { get { return _license; } set { _license = value; OnPropertyChanged("License"); OnPropertyChanged("SaveCommand"); } }
        public String Package { get { return _package; } set { _package = value; OnPropertyChanged("Package"); OnPropertyChanged("SubmitCommand"); } }
        public ICommand OrderCommand
        {
            get
            {
                if(_orderCommand == null)
                {
                    _orderCommand = new RelayCommand(param => this.Order(), param => this.CanOrder());
                }
                return _orderCommand;
            }
        }


        public ICommand FreeTrialCommand {
            get {
                if (_freeTrialCommand == null)
                {
                    _freeTrialCommand = new RelayCommand(param => this.RequestFreeTrial(), param => this.CanRequestFreeTrial());
                }
                return _freeTrialCommand;
            } }
        public ICommand SaveCommand
        {
            get
            {
                if(_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(param => this.Save(), param => this.CanSave());
                }
                return _saveCommand;
            }

        }
        public ICommand ActivateCommand
        {
            get
            {
                if(_activateCommand == null)
                {
                    _activateCommand = new RelayCommand(param => this.Activate(), param => this.CanActivate());
                }
                return _activateCommand;
            }
        }
        public ICommand SubmitCommand
        {
            get
            {
                if(_submitCommand == null)
                {
                    _submitCommand = new RelayCommand(param => this.Submit(), param => this.CanSubmit());
                }
                return _submitCommand;
            }
        }
        #endregion

        #region userMethods
        private bool CanSubmit()
        {
            if (!Package.Equals("Trial"))
                return true;
            return false;
        }
        private void Submit()
        {
            var client = new RestClient(URI + "order");
            var request = new RestRequest(Method.POST)
            {
                RequestFormat = RestSharp.DataFormat.Json
            };
            request.AddHeader("Content-Type", "application/json");
            dynamic orderData = new { name = Name, email = Email, phone = Phone, company = CompanyName,package = Package };
            request.AddJsonBody(orderData);
            IRestResponse response = client.Execute(request);
            MessageBox.Show(response.Content);
        }
        private bool CanActivate()
        {
            if (string.IsNullOrEmpty(Token))
                return false;

            if (string.IsNullOrEmpty(License))
                return true;

            return false;
        }

        private void Activate()
        {
            var client = new RestClient(URI+"activate");
            var request = new RestRequest(Method.POST)
            {
                RequestFormat = RestSharp.DataFormat.Json
            };
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("x-auth-token", Token);
            dynamic machineData = new { machineId = UniqueMachineId()};
            request.AddJsonBody(machineData);
            IRestResponse response = client.Execute(request);

            if (response != null && (int)response.StatusCode == 200)
            {
                string license = response.Headers.ToList().Find(param => param.Name == "x-fcl-license").Value.ToString();
                if (license != null)
                    License = license;
            }
            else
                MessageBox.Show(response.Content);
        }

        private bool CanSave()
        {
            if (string.IsNullOrEmpty(License))
                return false;
            return true;
        }

        private void Save()
        {
            string filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), UniqueMachineId().Replace("-","")+".lic");
            if (!System.IO.File.Exists(filename))
                System.IO.File.Create(filename).Close();

            System.IO.StreamWriter file = new System.IO.StreamWriter(filename);
            file.WriteLine(License);
            file.Close();
            MessageBox.Show("license saved.. Thanks for using Finesse Client. For any support please contact maref@firemisc.com. \nPlease close the application and restart in order to activate license");
        }

        private bool CanOrder()
        {
            if (string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(CompanyName))
                return false;

            if (string.IsNullOrEmpty(Token))
                return true;
            return false;
        }

        private void Order()
        {
            //TODO We need to show dialog to package selection and quantity
            // After user input , data will be sent to server for ordering and reply with payment instruction in email and message.
            // After payment , and email is received the user will be changed manually by Mamdouh
            Order order = new Order(this);
            order.Show();
        }

        private bool CanRequestFreeTrial()
        {
            if (string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(CompanyName))
                return false;
            if (string.IsNullOrEmpty(Token))
                return true;
            return false;
        }

        private void RequestFreeTrial()
        {
            var client = new RestClient(URI+"trial");
            dynamic trialData = new { name = Name, email = Email, phone = Phone, company = CompanyName };
            var request = new RestRequest(Method.POST)
            {
                RequestFormat = RestSharp.DataFormat.Json
            };
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(trialData);
            IRestResponse response = client.Execute(request);
            MessageBox.Show(response.Content);
        }
        private string UniqueMachineId()
        {
            return FingerPrint.Value();
        }
        #endregion
    }
}
