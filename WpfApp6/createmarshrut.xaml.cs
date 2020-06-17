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
using System.Windows.Shapes;


using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Collections;

namespace WpfApp6
{
    /// <summary>
    /// Логика взаимодействия для createmarshrut.xaml
    /// </summary>
    public partial class createmarshrut : Window
    {


        string connectionString;
        SqlDataAdapter adapter;
        DataTable BusTable;

        SqlDataAdapter adapterv;
        DataTable VoditelTable;

        SqlDataAdapter adapterm;
        DataTable MarshrutTable;


        DataGrid grid;
        DataGrid grid1;
        DataGrid grid2;

        public createmarshrut(DataGrid g1,DataGrid g2,DataGrid g3)
        {
            InitializeComponent();
            // получаем строку подключения из app.config
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            BusGrid.RowEditEnding += BusGrid_RowEditEnding;
            VoditelGrid.RowEditEnding += VoditelGrid_RowEditEnding;
            MarshrutGrid.RowEditEnding += MarshrutGrid_RowEditEnding;
            IDM.PreviewTextInput += TextBox1_PreviewTextInput;
            IDB.PreviewTextInput += TextBox1_PreviewTextInput;
            IDV.PreviewTextInput += TextBox1_PreviewTextInput;
            grid = g1;
            grid1 = g2;
            grid2 = g3;
        }

        private void CreateM_Click(object sender, RoutedEventArgs e)
        {
            if (IDM.Text == "" || IDB.Text == "" || IDV.Text == "" || PPM.Text == "") { MessageBox.Show("Заполните все поля"); return; }

            DataTable temptable = new DataTable();
            string sql = "Select * from Bus where DriverlD is not null and RoutelD is not null and BusNumber =" + IDB.Text;
            SqlConnection connection = null;
            connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(sql, connection);
            adapter = new SqlDataAdapter(command);
            connection.Open();
            adapter.Fill(temptable);

            connection.Close();

            if (temptable.Rows.Count != 0) { MessageBox.Show("Автобус занят", "", MessageBoxButton.OK, MessageBoxImage.Asterisk); return; }

            temptable.Clear();
            sql = "Select * from Bus where BusNumber =" + IDB.Text;
            connection = null;
            connection = new SqlConnection(connectionString);
            command = new SqlCommand(sql, connection);
            adapter = new SqlDataAdapter(command);
            connection.Open();
            adapter.Fill(temptable);

            connection.Close();

            if (temptable.Rows.Count == 0) { MessageBox.Show("Автобус не существует", "", MessageBoxButton.OK, MessageBoxImage.Asterisk); return; }


            temptable.Clear();
            sql = "Select * from Voditel where BusNumber is not  null and RoutelD is not null and  DriverlD =" + IDV.Text;
            connection = null;
            connection = new SqlConnection(connectionString);
            command = new SqlCommand(sql, connection);
            adapter = new SqlDataAdapter(command);
            connection.Open();
            adapter.Fill(temptable);
            connection.Close();
            if (temptable.Rows.Count != 0) { MessageBox.Show("Водитель занят", "", MessageBoxButton.OK, MessageBoxImage.Asterisk); return; }
            temptable.Clear();
            sql = "Select * from Voditel where DriverlD =" + IDV.Text;
            connection = null;
            connection = new SqlConnection(connectionString);
            command = new SqlCommand(sql, connection);
            adapter = new SqlDataAdapter(command);
            connection.Open();
            adapter.Fill(temptable);

            connection.Close();
           
            if (temptable.Rows.Count == 0) { MessageBox.Show("Водитель не существует", "", MessageBoxButton.OK, MessageBoxImage.Asterisk); return; }






            temptable.Clear();
            sql = "Select * from Marshrut where BusNumber is not null and DriverlD is not null and RoutelD =" + IDM.Text;
            connection = null;
            connection = new SqlConnection(connectionString);
            command = new SqlCommand(sql, connection);
            adapter = new SqlDataAdapter(command);
            connection.Open();
            adapter.Fill(temptable);
            connection.Close();
            if (temptable.Rows.Count != 0) { MessageBox.Show("Такой маршрут уже создан", "", MessageBoxButton.OK, MessageBoxImage.Asterisk); return; }


            temptable.Clear();
            sql = "update BUS set DriverlD=" + IDV.Text + ", RoutelD=" + IDM.Text + " where BusNumber =" + IDB.Text;

            connection = null;
            connection = new SqlConnection(connectionString);
            command = new SqlCommand(sql, connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            sql = "update Voditel set BusNumber=" + IDB.Text + ", RoutelD=" + IDM.Text + " where DriverlD =" + IDV.Text;
            connection = null;
            connection = new SqlConnection(connectionString);
            command = new SqlCommand(sql, connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            sql = "Insert into Marshrut(Comment,RoutelD,BusNumber,DriverlD) VALUES('" + PPM.Text + "'," + IDM.Text + "," + IDB.Text + "," + IDV.Text + ") ";

            connection = null;
            connection = new SqlConnection(connectionString);
            command = new SqlCommand(sql, connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            UpdateDB();
            UpdateDBV();
            UpdateDBM();
            
        }

        private void BusGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            UpdateDB();
        }


        private void VoditelGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            UpdateDBV();
        }

        private void MarshrutGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            UpdateDBM();
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



            sql = "SELECT * FROM Voditel";
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



            


            sql = "SELECT * FROM Marshrut";
            MarshrutTable = new DataTable();
            SqlConnection connectionm = null;
            try
            {
                connectionm = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connectionm);
                adapterm = new SqlDataAdapter(command);
                connectionm.Open();
                adapterm.Fill(MarshrutTable);
                MarshrutGrid.ItemsSource = MarshrutTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connectionm != null)
                    connectionm.Close();
            }



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
            grid1.ItemsSource = BusTable.DefaultView;
            connection.Close();
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
            grid2.ItemsSource = VoditelTable.DefaultView;
            connection.Close();
        }

       

        private void UpdateDBM()
        {
            SqlCommandBuilder comandbuilder = new SqlCommandBuilder(adapterm);
            adapterm.Update(MarshrutTable);

            MarshrutTable.Clear();
            string sql = "Select * from Marshrut";
            SqlConnection connection = null;
            connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(sql, connection);
            adapterm = new SqlDataAdapter(command);
            connection.Open();
            adapterm.Fill(MarshrutTable);
            MarshrutGrid.ItemsSource = MarshrutTable.DefaultView;
            grid.ItemsSource = MarshrutTable.DefaultView;
            connection.Close();
        }

        private void TextBox1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsNumber(Convert.ToChar(e.Text))) e.Handled = true;
        }


      
    }
}
