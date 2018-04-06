using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalkulatorWidok
{
    class ProductDetails
    {
        private String _displayName;
        private String _productNode;
        private String _productLanguage;
        private Boolean _isActive = true;

        public string DisplayName { get => _displayName; set => _displayName = value; }
        public string ProductNode { get => _productNode; set => _productNode = value; }
        public string ProductLanguage { get => _productLanguage; set => _productLanguage = value; }
        public bool IsActive { get => _isActive; set => _isActive = value; }
    }
}
