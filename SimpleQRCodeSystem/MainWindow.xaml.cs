using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using SimpleQRCodeSystem.Models;
using SimpleQRCodeSystem.Services;

namespace SimpleQRCodeSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IBadgeService _badgeService;

        public MainWindow(IBadgeService badgeService)
        {
            _badgeService = badgeService;
            InitializeComponent();
        }

        private void SearchQRCode_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SearchQRCode_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Regex regex = new Regex(@"^\d+$");
            var searchCode = qrCodeSearch.Text.Trim().ToLower();
            BadgeResult badgeResult = new BadgeResult();
            if (regex.IsMatch(searchCode))
            {
                badgeResult = _badgeService.Find(searchCode);
            }
            else if (File.Exists(qrCodeSearch.Text))
            {
                badgeResult = _badgeService.Import(qrCodeSearch.Text);
            }

            returnLabel.Content = badgeResult.Label;
            this.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(badgeResult.Color);

            qrCodeSearch.Text = "";
        }
    }
}
