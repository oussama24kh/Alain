using MahApps.Metro.IconPacks;
using System.Windows;
using System.Windows.Media;

namespace project_03
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    /// 
    public enum CustomMessageBoxImage
    {
        Information,
        Success,
        Warning,
        Error,
        Question
    }
    public partial class CustomMessageBox : Window
    {
        public MessageBoxResult Result { get; private set; } = MessageBoxResult.None;
        public CustomMessageBox()
        {
            InitializeComponent();
        }
        // Updated Show method with optional custom button labels and icons
        public static MessageBoxResult Show(
            string message,
            string title = "Message",
            MessageBoxButton button = MessageBoxButton.OK,
            CustomMessageBoxImage icon = CustomMessageBoxImage.Information,
            string primaryButtonText = "OK",
            string secondaryButtonText = "Cancel",
            PackIconMaterialKind primaryIcon = PackIconMaterialKind.Check,
            PackIconMaterialKind secondaryIcon = PackIconMaterialKind.Close,
            Window? owner = null)
        {
            var msgBox = new CustomMessageBox
            {
                Owner = owner ?? Application.Current?.MainWindow
            };

            msgBox.ConfigureDialog(message, title, button, icon, primaryButtonText, secondaryButtonText, primaryIcon, secondaryIcon);
            msgBox.ShowDialog();
            return msgBox.Result;
        }
        private void ConfigureDialog(
            string message,
            string title,
            MessageBoxButton button,
            CustomMessageBoxImage icon,
            string primaryText,
            string secondaryText,
            PackIconMaterialKind primaryIconKind,
            PackIconMaterialKind secondaryIconKind)
        {
            TxtTitle.Text = title;
            TxtMessage.Text = message;

            // Configure Main Icon & Accent Color
            switch (icon)
            {
                case CustomMessageBoxImage.Success:
                    MsgIcon.Kind = PackIconMaterialKind.CheckCircle;
                    MsgIcon.Foreground = (Brush)FindResource("GreenBrush");
                    IconBorder.Background = new SolidColorBrush(Color.FromArgb(40, 16, 185, 129));
                    break;
                case CustomMessageBoxImage.Warning:
                    MsgIcon.Kind = PackIconMaterialKind.Alert;
                    MsgIcon.Foreground = (Brush)FindResource("AmberBrush");
                    IconBorder.Background = new SolidColorBrush(Color.FromArgb(40, 245, 158, 11));
                    break;
                case CustomMessageBoxImage.Error:
                    MsgIcon.Kind = PackIconMaterialKind.AlertCircle;
                    MsgIcon.Foreground = new SolidColorBrush(Color.FromRgb(239, 68, 68));
                    IconBorder.Background = new SolidColorBrush(Color.FromArgb(40, 239, 68, 68));
                    break;
                case CustomMessageBoxImage.Question:
                    MsgIcon.Kind = PackIconMaterialKind.HelpCircle;
                    MsgIcon.Foreground = (Brush)FindResource("PrimaryBrush");
                    IconBorder.Background = new SolidColorBrush(Color.FromArgb(40, 37, 99, 235));
                    break;
                case CustomMessageBoxImage.Information:
                default:
                    MsgIcon.Kind = PackIconMaterialKind.Information;
                    MsgIcon.Foreground = (Brush)FindResource("PrimaryBrush");
                    IconBorder.Background = new SolidColorBrush(Color.FromArgb(40, 37, 99, 235));
                    break;
            }

            // Set Button Text & Icons
            TxtOk.Text = primaryText;
            OkIcon.Kind = primaryIconKind;

            if (button == MessageBoxButton.OKCancel || button == MessageBoxButton.YesNo)
            {
                BtnCancel.Visibility = Visibility.Visible;
                TxtCancel.Text = secondaryText;
                SecondaryIcon.Kind = secondaryIconKind;
            }
            else
            {
                BtnCancel.Visibility = Visibility.Collapsed;
            }
        }
        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.OK;
            DialogResult = true;
            Close();
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Cancel;
            DialogResult = false;
            Close();
        }
    }
}