using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace KalkulatorWidok.Pages.NewScript
{
    /// <summary>
    /// Interaction logic for NewScriptPropertiesPage.xaml
    /// </summary>
    public partial class NewScriptPropertiesPage : UserControl, IContent
    {
        BBCodeBlock bbCodeBlock = new BBCodeBlock();
        public class ProductFieldModel
        {
            public string DisplayName { get; set; }
            public string FieldName { get; set; }
            public bool IsActive { get; set; }
            public new String ToString => "DisplayName: " + DisplayName + " FieldName: " + FieldName + " IsActive: " + IsActive;
        }

        public class ProductDetailsModel
        {
            public string DisplayName { get; set; }
            public string IdValue { get; set; }
            public string ProductLanguage { get; set; }
            public bool IsActive { get; set; }
        }

        public NewScriptPropertiesPage()
        {
            InitializeComponent();
        }

        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedFrom(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedTo(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
            Console.WriteLine("OnNavigatedTo");
            DG1.DataContext = GetFieldsData();
            DG2.DataContext = GetProductsDetailsData();
        }

        public void OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            // TODO: Navigate backwards, is there a need to do something here ?
            //throw new NotImplementedException();
        }

        private ObservableCollection<ProductFieldModel> GetFieldsData()
        {
            var productFields = new ObservableCollection<ProductFieldModel>();

            KalkulatorWidok.NewScript.Fields.ForEach(field =>
            {
                productFields.Add(new ProductFieldModel { DisplayName = field.DisplayName, FieldName = field.IdValue, IsActive = field.IsActive });
            });
            return productFields;
        }

        private ObservableCollection<ProductDetailsModel> GetProductsDetailsData()
        {
            var productsDetails = new ObservableCollection<ProductDetailsModel>();

            KalkulatorWidok.NewScript.Products.ForEach(product =>
            {
                productsDetails.Add(item: new ProductDetailsModel { DisplayName = product.DisplayName, IdValue = product.ProductNode, ProductLanguage = product.ProductLanguage, IsActive = product.IsActive });
            });
            return productsDetails;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddProductDialog();
            var result = dialog.ShowDialog();
            if (result.HasValue && (bool)result)
            {
                DG1.DataContext = GetFieldsData();
                DG2.DataContext = GetProductsDetailsData();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var rows = GetDataGridRows(DG1);
            foreach (DataGridRow row in rows)
            {
                ProductFieldModel rowView = (ProductFieldModel)row.Item;
            }
            String script = "<? php $node = menu_get_object(); if ($node &&";
            KalkulatorWidok.NewScript.Products.ForEach(p => {
                script += "$node->nid == '" + p.ProductNode + "' " + "|| ";
            });
            script = script.Substring(script.Length - 3);
            script += "): ?>";
            script += "<script>";
            script += "var product_rules = {";
            KalkulatorWidok.NewScript.Fields.ForEach(f => {
                script += f.IdValue + ":";
            });

            //   < script >
            //     var product_rules = {
            //                field_swiatlo:
            //                {
            //          operator: '+',
            //          values: [0, 0, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000, 1100]
            //                },
            //      field_powieszchnia:
            //                {
            //          operator: '*',
            //          values: [1, 1, 1.1]
            //      },
            //      field_mocowanie:
            //                {
            //                    related_fields:
            //                    {
            //                        field_swiatlo:
            //                        {
            //                  operator: '+',
            //                  values:
            //                            {
            //                                0: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
            //                      1: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
            //                      2: [0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120]
            //    }
            //}
            //          }
            //      },
            //      field_zestaw_montazowy: {
            //          operator: '+',
            //          values: [0, 0, 200]
            //      }
            //    };
            //  </script>
            //  <? php endif; ?>
            try
            {
                bbCodeBlock.LinkNavigator.Navigate(new Uri("/Pages/NewScript/NewScriptFieldSettings.xaml", UriKind.Relative), this);
            }
            catch (Exception error)
            {
                ModernDialog.ShowMessage(error.Message, FirstFloor.ModernUI.Resources.NavigationFailed, MessageBoxButton.OK);
            }
        }

        private IEnumerable<DataGridRow> GetDataGridRows(DataGrid grid)
        {
            var itemsSource = grid.ItemsSource as IEnumerable;
            if (null == itemsSource) yield return null;
            foreach (var item in itemsSource)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (null != row) yield return row;
            }
        }
    }
}
