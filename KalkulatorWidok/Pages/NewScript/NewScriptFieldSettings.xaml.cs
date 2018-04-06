using System.Windows.Controls;

namespace KalkulatorWidok.Pages.NewScript
{
    /// <summary>
    /// Interaction logic for NewScriptFieldSettings.xaml
    /// </summary>
    public partial class NewScriptFieldSettings : UserControl
    {
        public NewScriptFieldSettings()
        {
            InitializeComponent();
            KalkulatorWidok.NewScript.Fields.ForEach(f =>
            {
                if (f.IsActive)
                {
                    TextBlock text = new TextBlock
                    {
                        Text = f.DisplayName
                    };

                    ComboBox box = new ComboBox();
                    box.Items.Add("test123");
                    StackPanel stackPanel = new StackPanel
                    {
                        Orientation = Orientation.Vertical
                    };

                    stackPanel.Children.Add(text);
                    stackPanel.Children.Add(box);
                    Pola.Children.Add(stackPanel);
                }
            });
        }
    }
}
