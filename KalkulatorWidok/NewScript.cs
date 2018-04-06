using System;
using System.Collections.Generic;
using System.Linq;

namespace KalkulatorWidok
{
    public static class NewScript
    {
        public enum AddProductStatus { NullList, ExistAlready, DifferentFields, Sucess };
        private static List<ProductField> _fields = new List<ProductField>();
        private static List<ProductDetails> _products = new List<ProductDetails>();

        internal static List<ProductDetails> Products { get => _products; set => _products = value; }
        internal static List<ProductField> Fields { get => _fields; set => _fields = value; }

        internal static void clearFields()
        {
            Fields.Clear();
        }

        internal static void clearProducts()
        {
            Products.Clear();
        }

        internal static AddProductStatus AddProducts(List<ProductField> fields, List<ProductDetails> products)
        {
            AddProductStatus result = AddProductStatus.NullList;
            if (fields == null) return result;
            result = AddProductStatus.ExistAlready;
            if (Fields.All(e => fields.Any(o => o.IdValue.Equals(e.IdValue, StringComparison.OrdinalIgnoreCase))))
            {
                products.ForEach(p =>
                {
                    if (!Products.Any(e => p.ProductNode.Equals(e.ProductNode, StringComparison.OrdinalIgnoreCase)))
                    {
                        Products.Add(p);
                        result = AddProductStatus.Sucess;
                    }
                });
            }
            else result = AddProductStatus.DifferentFields;
            return result;
        }
    }
}
