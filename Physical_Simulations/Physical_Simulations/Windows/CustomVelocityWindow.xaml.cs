using System.Windows;

namespace Physical_Simulations.Windows
{
    /// <summary>
    /// Interaction logic for CustomVelocityWindow.xaml
    /// </summary>
    public partial class CustomVelocityWindow : Window
    {
        public float VelocityX { get; private set; }
        public float VelocityY { get; private set; }
        public float VelocityZ { get; private set; }

        public CustomVelocityWindow()
        {
            InitializeComponent();
        }

        public void InitializeValues(float velocityX, float velocityY, float velocityZ)
        {
            sliderX.Value = velocityX;
            sliderY.Value = velocityY;
            sliderZ.Value = velocityZ;
        }


        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            VelocityX = (float)sliderX.Value;
            VelocityY = (float)sliderY.Value;
            VelocityZ = (float)sliderZ.Value;

            DialogResult = true;
            Close();
        }
    }
}
