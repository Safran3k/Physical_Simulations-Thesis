using System.Windows;

namespace Physical_Simulations.Windows
{
    /// <summary>
    /// Interaction logic for CustomMassWindow.xaml
    /// </summary>
    public partial class CustomMassWindow : Window
    {
        public double BodyMass { get; private set; }
        public CustomMassWindow()
        {
            InitializeComponent();
        }

        public void InitializeValues(double mass)
        {
            sliderBodyMass.Value = mass;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            BodyMass = sliderBodyMass.Value;

            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
