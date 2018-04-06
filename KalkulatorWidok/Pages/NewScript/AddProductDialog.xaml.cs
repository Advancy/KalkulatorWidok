using FirstFloor.ModernUI.Windows.Controls;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KalkulatorWidok.Pages.NewScript
{
    /// <summary>
    /// Interaction logic for AddProductDialog.xaml
    /// </summary>
    public partial class AddProductDialog : ModernDialog
    {
        public AddProductDialog()
        {
            InitializeComponent();
            Buttons = new Button[] { OkButton, CancelButton };
        }
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
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
                fieldsList.Add(new ProductField()
                {
                    DisplayName = element.InnerText.Remove(element.InnerText.Length - 1),
                    IdValue = element.NextSibling.GetAttributeValue("id", "wrong_id")
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
                var result = ShowMessage("Wykryto, że produkt posiada kilka wersji językowych. Czy dodać wszystkie znalezione wersje językowe?", "Dodać pozostałe wersje językowe?", btn);
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
                }
            }
            KalkulatorWidok.NewScript.AddProductStatus addResult = KalkulatorWidok.NewScript.AddProducts(fieldsList, productsList);
            switch (addResult)
            {
                case KalkulatorWidok.NewScript.AddProductStatus.NullList:
                    throw new NotImplementedException();
                case KalkulatorWidok.NewScript.AddProductStatus.ExistAlready:
                    ShowMessage("Wykryto, że produkt istnieje już na liście.", "Błąd wprowadzonych danych.", MessageBoxButton.OK);
                    break;
                case KalkulatorWidok.NewScript.AddProductStatus.DifferentFields:
                    ShowMessage("Wykryto, że produkt posiada inne pola niż aktualnie tworzony skrypt. Dodanie produktu nie jest możliwe.", "Błąd wprowadzonych danych.", MessageBoxButton.OK);
                    break;
                case KalkulatorWidok.NewScript.AddProductStatus.Sucess:
                    ShowMessage("Produkt został dodany.", "Produkt został dodany.", MessageBoxButton.OK);
                    break;
            }
            busyIndicator.Visibility = Visibility.Hidden;

        }
        private void TextAddressUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return) Accept_Click(sender, e);
        }
    }
}
