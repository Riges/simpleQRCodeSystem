using SimpleQRCodeSystem.Model;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private SQLiteConnection m_dbConnection;

        public MainWindow()
        {
            InitializeComponent();
            string initSql = "";
            if (!File.Exists("database.sqlite"))
            {
                SQLiteConnection.CreateFile("database.sqlite");
                initSql = "CREATE TABLE badge(id INTEGER PRIMARY KEY AUTOINCREMENT, code VARCHAR(50) UNIQUE, usedAt DateTime);";
            }
            m_dbConnection = new SQLiteConnection("Data Source=database.sqlite;Version=3;");
            m_dbConnection.Open();
            if (initSql != "")
            {
                SQLiteCommand command = new SQLiteCommand(initSql, m_dbConnection);
                command.ExecuteNonQuery();
            }
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
                Badge badge = new Badge();
                string sql = "SELECT * FROM badge WHERE code = @code LIMIT 1";
                SQLiteCommand cmd = new SQLiteCommand(sql, m_dbConnection);
                cmd.Parameters.AddWithValue("@code", searchCode);

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (!reader.IsClosed && reader.Read())
                    {
                        badge.Id = Int32.Parse(reader["id"].ToString());
                        badge.code = reader["code"].ToString();
                        if (reader["usedAt"].ToString() != "")
                        {
                            badge.Used = true;
                        }
                        else
                        {
                            badge.Used = false;
                        }
                        cmd.Cancel();
                        reader.Close();
                    }
                }

                if (badge.Id == 0)
                {
                    qrCodeSearch.Text = "Not FOUND !";
                } else {
                    if (badge.Used)
                    {
                        qrCodeSearch.Text = "Found but already Used !";
                    }
                    else
                    {
                        SQLiteCommand command = new SQLiteCommand(
                            "UPDATE badge set usedAt = datetime() WHERE code = @code;",
                            m_dbConnection
                        );
                        command.Parameters.AddWithValue("@code", searchCode);
                        command.ExecuteNonQuery();

                        qrCodeSearch.Text = "OK !";
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
                        SQLiteCommand command = new SQLiteCommand(
                            "INSERT OR IGNORE INTO badge (id, code, usedAt) VALUES (null, @code, null);", 
                            m_dbConnection
                        );
                        command.Parameters.AddWithValue("@code", values[1]);
                        command.ExecuteNonQuery();
                    }
                }
                qrCodeSearch.Text = "";
            }
        }
    }
}
