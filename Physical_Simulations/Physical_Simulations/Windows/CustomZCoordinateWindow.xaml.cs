using System.Windows;

namespace Physical_Simulations.Windows
{
    /// <summary>
    /// Interaction logic for CustomZCoordinateWindow.xaml
    /// </summary>
    public partial class CustomZCoordinateWindow : Window
    {
        public float ZPosition { get; private set; }

        public CustomZCoordinateWindow()
        {
            InitializeComponent();
        }

        public void InitializeValues(float zPosition)
        {
            sliderBodyZCoordinate.Value = zPosition;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ZPosition = (float)sliderBodyZCoordinate.Value;

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
