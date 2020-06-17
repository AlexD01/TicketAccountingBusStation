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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string connectionString;
        SqlDataAdapter adapter;
        DataTable BusTable;

        SqlDataAdapter adapterv;
        DataTable VoditelTable;

        SqlDataAdapter adapterb;
        DataTable BiletTable;

        SqlDataAdapter adapterm;
        DataTable MarshrutTable;

        public MainWindow()
        {
            InitializeComponent();
            // получаем строку подключения из app.config
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            BusGrid.RowEditEnding += BusGrid_RowEditEnding;
            VoditelGrid.RowEditEnding += VoditelGrid_RowEditEnding;
            BiletGrid.RowEditEnding += BiletGrid_RowEditEnding;
            MarshrutGrid.RowEditEnding += MarshrutGrid_RowEditEnding;
           
           
        }

        private void BusGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            UpdateDB();
        }


        private void VoditelGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            UpdateDBV();
        }

        private void BiletGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            UpdateDBB();
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



            sql = "SELECT * FROM Bilet";
            BiletTable = new DataTable();
            SqlConnection connectionb = null;
            try
            {
                connectionb = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connectionb);
                adapterb = new SqlDataAdapter(command);
              
                connectionb.Open();
                adapterb.Fill(BiletTable);
                BiletGrid.ItemsSource = BiletTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connectionb != null)
                    connectionb.Close();
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
            connection.Close();
        }

        private void UpdateDBB()
        {
            SqlCommandBuilder comandbuilder = new SqlCommandBuilder(adapterb);
            adapterb.Update(BiletTable);


            BiletTable.Clear();
            string sql = "Select * from Bilet";
            SqlConnection connection = null;
            connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(sql, connection);
            adapterb = new SqlDataAdapter(command);
            connection.Open();
            adapterb.Fill(BiletTable);
            BiletGrid.ItemsSource = BiletTable.DefaultView;
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
            connection.Close();
        }

       

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (BusGrid.SelectedItems != null)
            {
                for (int i = 0; i < BusGrid.SelectedItems.Count; i++)
                {
                    DataRowView datarowView = BusGrid.SelectedItems[i] as DataRowView;
                    if (datarowView != null)
                    {
                        DataRow dataRow = (DataRow)datarowView.Row;
                        dataRow.Delete();
                    }
                }
            }
            UpdateDB();
        }

        

        private void DeleteButton1_Click(object sender, RoutedEventArgs e)
        {
            if (VoditelGrid.SelectedItems != null)
            {
                for (int i = 0; i < VoditelGrid.SelectedItems.Count; i++)
                {
                    DataRowView datarowView = VoditelGrid.SelectedItems[i] as DataRowView;
                    if (datarowView != null)
                    {
                        DataRow dataRow = (DataRow)datarowView.Row;
                        dataRow.Delete();
                    }
                }
            }
            UpdateDBV();
        }

        
        private void DeleteButton2_Click(object sender, RoutedEventArgs e)
        {
            if (BiletGrid.SelectedItems != null)
            {
                for (int i = 0; i < BiletGrid.SelectedItems.Count; i++)
                {
                    DataRowView datarowView = BiletGrid.SelectedItems[i] as DataRowView;
                    if (datarowView != null)
                    {
                        DataRow dataRow = (DataRow)datarowView.Row;
                        dataRow.Delete();
                    }
                }
            }
            UpdateDBB();
        }

        

        private void DeleteButton3_Click(object sender, RoutedEventArgs e)
        {
            if (MarshrutGrid.SelectedItems != null)
            {
                for (int i = 0; i < MarshrutGrid.SelectedItems.Count; i++)
                {
                    DataRowView datarowView = MarshrutGrid.SelectedItems[i] as DataRowView;
                    if (datarowView != null)
                    {
                        DataRow dataRow = (DataRow)datarowView.Row;
                        object d =dataRow.ItemArray[1];

                        string sql = "update BUS set DriverlD= null, RoutelD= null where BusNumber =" + dataRow.ItemArray[5];

                        SqlConnection connection = null;
                        connection = new SqlConnection(connectionString);
                        SqlCommand command = new SqlCommand(sql, connection);
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();

                        sql = "update Voditel set BusNumber=null, RoutelD=null where DriverlD =" + dataRow.ItemArray[6];
                        connection = null;
                        connection = new SqlConnection(connectionString);
                        command = new SqlCommand(sql, connection);
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                        sql = "delete from Bilet where RoutelD =" + dataRow.ItemArray[4]; ;
                        connection = null;
                        connection = new SqlConnection(connectionString);
                        command = new SqlCommand(sql, connection);
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();


                        dataRow.Delete();
                    }
                }
            }
            UpdateDB();
            UpdateDBV();
            UpdateDBM();
            UpdateDBB();
        }

        
        private void Createmarshrut_Click(object sender, RoutedEventArgs e)
        {
            createmarshrut dd;
             dd = new createmarshrut(MarshrutGrid,BusGrid,VoditelGrid);
            dd.Show();
        }

        
        private void ff_Click(object sender, RoutedEventArgs e)
        {
            createbilet dd;
             dd = new createbilet(BiletGrid,MarshrutGrid);
            dd.Show();
        }

     
        private void Createbus_Click(object sender, RoutedEventArgs e)
        {
            createbus dd;
            dd = new createbus(BusGrid);
            dd.Show();
        }

        private void createvoditel_Click(object sender, RoutedEventArgs e)
        {
            createvoditel dd;
            dd = new createvoditel(VoditelGrid);
            dd.Show();
        }

        private void butinfbus_Click(object sender, RoutedEventArgs e)
        {

            BusGrid.Visibility = Visibility.Visible;
            VoditelGrid.Visibility = Visibility.Hidden;
            MarshrutGrid.Visibility = Visibility.Hidden;
            BiletGrid.Visibility = Visibility.Hidden;
            gridbus.Visibility = Visibility.Visible;
            gridvoditel.Visibility = Visibility.Hidden;
            gridmarshrut.Visibility = Visibility.Hidden;
            gridbilet.Visibility = Visibility.Hidden;
        }

        private void butinfvoditel_Click(object sender, RoutedEventArgs e)
        {
            BusGrid.Visibility = Visibility.Hidden;
            VoditelGrid.Visibility = Visibility.Visible;
            MarshrutGrid.Visibility = Visibility.Hidden;
            BiletGrid.Visibility = Visibility.Hidden;
            gridbus.Visibility = Visibility.Hidden;
            gridvoditel.Visibility = Visibility.Visible;
            gridmarshrut.Visibility = Visibility.Hidden;
            gridbilet.Visibility = Visibility.Hidden;
        }

        private void butinfmarshrut_Click(object sender, RoutedEventArgs e)
        {
            BusGrid.Visibility = Visibility.Hidden;
            VoditelGrid.Visibility = Visibility.Hidden;
            MarshrutGrid.Visibility = Visibility.Visible;
            BiletGrid.Visibility = Visibility.Hidden;
            gridbus.Visibility = Visibility.Hidden;
            gridvoditel.Visibility = Visibility.Hidden;
            gridmarshrut.Visibility = Visibility.Visible;
            gridbilet.Visibility = Visibility.Hidden;
        }

        private void butinfbilet_Click(object sender, RoutedEventArgs e)
        {
            BusGrid.Visibility = Visibility.Hidden;
            VoditelGrid.Visibility = Visibility.Hidden;
            MarshrutGrid.Visibility = Visibility.Hidden;
            BiletGrid.Visibility = Visibility.Visible;
            gridbus.Visibility = Visibility.Hidden;
            gridvoditel.Visibility = Visibility.Hidden;
            gridmarshrut.Visibility = Visibility.Hidden;
            gridbilet.Visibility = Visibility.Visible;
        }

        private void image_Click(object sender, RoutedEventArgs e)
        {
            if (id.Text == "") return;
            int idd = Convert.ToInt32(id.Text);
            DataTable temptable = new DataTable();
            string sql = "Select Picture from Bus where BusNumber =" + idd;
            SqlConnection connection = null;
            connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(sql, connection);
            adapter = new SqlDataAdapter(command);
            connection.Open();
            adapter.Fill(temptable);
            connection.Close();
            if (temptable.Rows.Count == 0) { MessageBox.Show("Автобус с таким номером не существует"); return; }
            DataRowView comtemp = temptable.DefaultView[0];
            string img1 = (string)comtemp.Row.ItemArray[0];
            var uri = new Uri(img1);
            var bitmap = new BitmapImage(uri);
            img.Source = bitmap;
        }
    }

}
