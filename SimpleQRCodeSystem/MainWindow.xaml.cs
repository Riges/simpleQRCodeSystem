using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using SimpleQRCodeSystem.Models;
using SimpleQRCodeSystem.Repositories;
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
            if (regex.IsMatch(searchCode))
            {
                var badge = _badgeService.Find(searchCode);

                if (badge.Id == 0)
                {
                    returnLabel.Content = "Billet NON Valide";
                    this.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("Red");
                } else {
                    if (badge.Used)
                    {
                        returnLabel.Content = "Billet Valide, mais déjà utilisé";
                        this.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("Red");
                    }
                    else
                    {
                        _badgeService.SetUsedAt(searchCode);

                        returnLabel.Content = "Billet Valide";
                        this.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("Green");
                    }
                }
            }
            else if (File.Exists(qrCodeSearch.Text))
            {
                var reader = new StreamReader(File.OpenRead(@qrCodeSearch.Text));
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split('|');
                    if (values.Length == 21)
                    {
                        _badgeService.Insert(values[1]);
                    }
                }
                returnLabel.Content = "Donnés importés";

                this.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("LightSkyBlue");
            }

            qrCodeSearch.Text = "";
        }
    }
}
