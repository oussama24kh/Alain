using System.Windows;

namespace project_03.Car
{
    /// <summary>
    /// Interaction logic for CarDetailsWindow.xaml
    /// </summary>
    public partial class CarDetailsWindow : Window
    {
        public CarDetailsWindow(Model.Car car)
        {
            InitializeComponent();
            CarDetailsControl.LoadCarData(car);
        }
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}