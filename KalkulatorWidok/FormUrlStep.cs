using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using FirstFloor.ModernUI.Presentation;

namespace KalkulatorWidok
{
    public class FormUrlStep
        : NotifyPropertyChanged, IDataErrorInfo
    {
        private string addressUrl;

        public string AddressUrl
        {
            get { return this.addressUrl; }
            set
            {
                if (this.addressUrl != value)
                {
                    this.addressUrl = value;
                    OnPropertyChanged("AddressUrl");
                }
            }
        }

        public string Error
        {
            get { return null; }
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "AddressUrl")
                {
                    return string.IsNullOrEmpty(this.addressUrl) ? "Wymagane pole." : null;
                }
                return null;
            }
        }
    }
}
