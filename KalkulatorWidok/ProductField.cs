using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalkulatorWidok
{
    class ProductField
    {
        private String _displayName;
        private String _idValue;
        private Boolean _isActive = true;
        private List<String> _values;

        public string DisplayName { get => _displayName; set => _displayName = value; }
        public string IdValue { get => _idValue; set => _idValue = value; }
        public bool IsActive { get => _isActive; set => _isActive = value; }
        public List<string> Values { get => _values; set => _values = value; }
    }
}
