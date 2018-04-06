using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FirstFloor.ModernUI.Windows.Controls;
using HtmlAgilityPack;

namespace KalkulatorWidok.Pages.NewScript
{
    /// <summary>
    /// Interaction logic for NewScript.xaml
    /// </summary>
    public partial class NewScriptHomePage : UserControl
    {
        BBCodeBlock bbCodeBlock = new BBCodeBlock();

        public NewScriptHomePage()
        {
            InitializeComponent();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            KalkulatorWidok.NewScript.clearFields();
            KalkulatorWidok.NewScript.clearProducts();
            busyIndicator.Visibility = Visibility.Visible;

            String urlAddress = TextAddressUrl.Text;
            String productLanguage = TextAddressUrl.Text.Contains("/pl/") ? "pl" : "eng";

            var web = new HtmlWeb();

            var doc = web.Load(urlAddress);

            var fieldsDisplayNames = doc.DocumentNode.SelectNodes("//p[@class='c-product-meta-label c-product-margin-1 c-font-uppercase c-font-bold']")
                  .ToList();

            List<ProductField> fieldsList = new List<ProductField>();

            fieldsDisplayNames.ForEach(element =>
            {
                var sibling = element.NextSibling;
                var values = sibling.ChildNodes.ToList();
                List<String> valuesNames = new List<string>();
                values.ForEach(val =>
                {
                    valuesNames.Add(val.InnerText);
                });
                fieldsList.Add(new ProductField()
                {
                    DisplayName = element.InnerText.Remove(element.InnerText.Length - 1),
                    IdValue = element.NextSibling.GetAttributeValue("id", "wrong_id"),
                    Values = valuesNames
                });
            });

            List<ProductDetails> productsList = new List<ProductDetails>();

            var productName = doc.DocumentNode.SelectNodes("//div[@class='c-content-title-1']//h3")
            .ToList();
            var productNodeId = doc.DocumentNode.SelectNodes("//link[@rel='shortlink']")
                .Select(p => p.GetAttributeValue("href", "undefined")).ToList();

            productName.ForEach(element =>
            {
                productsList.Add(new ProductDetails()
                {
                    DisplayName = element.InnerText,
                    IsActive = true,
                    ProductLanguage = productLanguage,
                    ProductNode = productNodeId.First().Substring(productNodeId.First().IndexOf("/node/") + 6)
                });
            });



            string newUrlAddress;
            if (productLanguage == "pl")
            {
                newUrlAddress = urlAddress.Replace("/pl/", "/en/");
            }
            else
            {
                newUrlAddress = urlAddress.Replace("/en/", "/pl/");
            }
            var document = web.Load(newUrlAddress);
            if (web.StatusCode == HttpStatusCode.OK)
            {
                MessageBoxButton btn = MessageBoxButton.OKCancel;
                var result = ModernDialog.ShowMessage("Wykryto, że produkt posiada kilka wersji językowych. Czy dodać wszystkie znalezione wersje językowe?", "Dodać pozostałe wersje językowe?", btn);
                if (result == MessageBoxResult.OK)
                {
                    productName = document.DocumentNode.SelectNodes("//div[@class='c-content-title-1']//h3")
.ToList();
                    productNodeId = document.DocumentNode.SelectNodes("//link[@rel='shortlink']")
                        .Select(p => p.GetAttributeValue("href", "undefined")).ToList();

                    productName.ForEach(element =>
                    {
                        productsList.Add(new ProductDetails()
                        {
                            DisplayName = element.InnerText,
                            IsActive = true,
                            ProductLanguage = newUrlAddress.Contains("/pl/") ? "pl" : "eng",
                            ProductNode = productNodeId.First().Substring(productNodeId.First().IndexOf("/node/") + 6)
                        });
                    });

                    KalkulatorWidok.NewScript.Products = productsList;
                }
            }
            KalkulatorWidok.NewScript.Products = productsList;
            KalkulatorWidok.NewScript.Fields = fieldsList;



            busyIndicator.Visibility = Visibility.Hidden;
            try
            {
                bbCodeBlock.LinkNavigator.Navigate(new Uri("/Pages/NewScript/NewScriptPropertiesPage.xaml", UriKind.Relative), this);
            }
            catch (Exception error)
            {
                ModernDialog.ShowMessage(error.Message, FirstFloor.ModernUI.Resources.NavigationFailed, MessageBoxButton.OK);
            }

        }

        private void TextAddressUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return) Accept_Click(sender, e);
        }
    }
}
