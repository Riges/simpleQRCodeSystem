using SimpleQRCodeSystem.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleQRCodeSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var db = new SQLiteConnection("databse.db");
            db.CreateTable<Badge>();
        }

        private void SearchQRCode_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SearchQRCode_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("The New command was invoked with value :" + qrCodeSearch.Text.Trim().ToLower());
        }
    }
}
