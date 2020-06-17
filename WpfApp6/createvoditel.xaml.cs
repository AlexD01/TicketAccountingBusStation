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



using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Collections;

using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using Microsoft.Win32;
namespace WpfApp6
{
    /// <summary>
    /// Логика взаимодействия для createvoditel.xaml
    /// </summary>
    public partial class createvoditel : Window
    {


        string connectionString;

        SqlDataAdapter adapterv;
        DataTable VoditelTable;
        DataGrid grid;

        public createvoditel( DataGrid g1)
        {
            InitializeComponent();
            // получаем строку подключения из app.config
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            VoditelGrid.RowEditEnding += VoditelGrid_RowEditEnding;
            idv.PreviewTextInput += TextBox1_PreviewTextInput;
            stazv.PreviewTextInput += TextBox1_PreviewTextInput;
            yearv.PreviewTextInput += TextBox1_PreviewTextInput;
            grid = g1;
        }
        private void TextBox1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsNumber(Convert.ToChar(e.Text))) e.Handled = true;
        }

        private void VoditelGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            UpdateDBV();
        }

        private void UpdateDBV()
        {
            SqlCommandBuilder comandbuilder = new SqlCommandBuilder(adapterv);
            adapterv.Update(VoditelTable);

            VoditelTable.Clear();
            string sql = "Select * from Voditel";
            SqlConnection connection = null;
            connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(sql, connection);
            adapterv = new SqlDataAdapter(command);
            connection.Open();
            adapterv.Fill(VoditelTable);
            VoditelGrid.ItemsSource = VoditelTable.DefaultView;
            grid.ItemsSource = VoditelTable.DefaultView;
            connection.Close();
        }

        private void CreateVoditel_Click(object sender, RoutedEventArgs e)
        {
            if (namev.Text == "" || famv.Text == "" || otchv.Text == "" || stazv.Text == "" || yearv.Text == "" || catev.Text == "" || classv.Text == "" || idv.Text == "") { return; }
            DataTable temptable = new DataTable();
            string sql = "Select * from Voditel where DriverlD =" + idv.Text;
            SqlConnection connection = null;
            connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(sql, connection);
            adapterv = new SqlDataAdapter(command);
            connection.Open();
            adapterv.Fill(temptable);
            connection.Close();

            if (temptable.Rows.Count != 0) { MessageBox.Show("Водитель с таким номером уже существует"); return; }

            sql = "Insert into Voditel(LastName,FirstName,Patronymic,Experience,Year,Category,Class,DriverlD) VALUES('" + namev.Text +"','" + famv.Text + "','" + otchv.Text + "','" + stazv.Text + "','" + yearv.Text + "','" + catev.Text + "','" + classv.Text + "','" + idv.Text + "') ";

            connection = null;
            connection = new SqlConnection(connectionString);
            command = new SqlCommand(sql, connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            UpdateDBV();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT * FROM Voditel";
            VoditelTable = new DataTable();
            SqlConnection connectionv = null;
            try
            {
                connectionv = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connectionv);
                adapterv = new SqlDataAdapter(command);

                connectionv.Open();
                adapterv.Fill(VoditelTable);
                VoditelGrid.ItemsSource = VoditelTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connectionv != null)
                    connectionv.Close();
            }
        }
    }
}
