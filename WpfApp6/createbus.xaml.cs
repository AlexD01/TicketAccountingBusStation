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
    /// Логика взаимодействия для createbus.xaml
    /// </summary>
    public partial class createbus : Window
    {

        string connectionString;
        SqlDataAdapter adapter;
        DataTable BusTable;
        DataGrid grid;
        public createbus(DataGrid g1)
        {
            InitializeComponent();
            // получаем строку подключения из app.config
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            BusGrid.RowEditEnding += BusGrid_RowEditEnding;
            idb.PreviewTextInput += TextBox1_PreviewTextInput;
            mestb.PreviewTextInput += TextBox1_PreviewTextInput;
            godb.PreviewTextInput += TextBox1_PreviewTextInput;
            remb.PreviewTextInput += TextBox1_PreviewTextInput;
            prob.PreviewTextInput += TextBox1_PreviewTextInput;
            grid = g1;
        }


        private void TextBox1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsNumber(Convert.ToChar(e.Text))) e.Handled = true;
        }

        private void BusGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            UpdateDB();
        }

        private void UpdateDB()
        {
            SqlCommandBuilder comandbuilder = new SqlCommandBuilder(adapter);
            adapter.Update(BusTable);

            BusTable.Clear();
            string sql = "Select * from Bus";
            SqlConnection connection = null;
            connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(sql, connection);
            adapter = new SqlDataAdapter(command);
            connection.Open();
            adapter.Fill(BusTable);
            BusGrid.ItemsSource = BusTable.DefaultView;
            grid.ItemsSource = BusTable.DefaultView;
            connection.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.FileName = "";
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Image|*.bmp;*.png;*.jpeg;*.jpg";
            Nullable<bool> result1 = dialog.ShowDialog();
            if (result1 == true)
            {
                string filename = dialog.FileName;     
                var uri = new Uri(filename);
                var bitmap = new BitmapImage(uri);
                img.Source = bitmap;
                image.Content = filename;
            }
        }

        private void CreateBus_Click(object sender, RoutedEventArgs e)
        {
            if (idb.Text == "" || markb.Text == "" || modb.Text == "" || image.Content == "" || mestb.Text == "" || godb.Text == "" || remb.Text == "" || prob.Text == "") { return; }
            DataTable temptable = new DataTable();
            string sql = "Select * from Bus where BusNumber =" + idb.Text;
            SqlConnection connection = null;
            connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(sql, connection);
            adapter = new SqlDataAdapter(command);
            connection.Open();
            adapter.Fill(temptable);
            connection.Close();

            if (temptable.Rows.Count != 0) { MessageBox.Show("Автобус с таким номером уже существует"); return; }
            string file = "D:\\img\\" + modb.Text + ".bmp";
            File.Copy(image.Content.ToString(), file);

            sql = "Insert into Bus(BusNumber,Brand,Picture,Model,Capacity,YearBus,YearRepair,Distance) VALUES(" + idb.Text + ",'" + markb.Text + "','" +file + "','" + modb.Text + "'," + mestb.Text + "," + godb.Text + "," + remb.Text + "," + prob.Text +") ";

            connection = null;
            connection = new SqlConnection(connectionString);
            command = new SqlCommand(sql, connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            UpdateDB();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT * FROM BUS";
            BusTable = new DataTable();
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);

                connection.Open();
                adapter.Fill(BusTable);
                BusGrid.ItemsSource = BusTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }
    }
}
