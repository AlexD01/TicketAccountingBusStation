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

    public partial class createbilet : Window
    {

        string connectionString;
        SqlDataAdapter adapterm;
        DataTable MarshrutTable;

        SqlDataAdapter adapterb;
        DataTable BiletTable;
        DataGrid grid;
        DataGrid grid1;
        public createbilet(DataGrid g1,DataGrid g2)
        {
            InitializeComponent();
            // получаем строку подключения из app.config
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            MarshrutGrid.RowEditEnding += MarshrutGrid_RowEditEnding;
            Mid.PreviewTextInput += TextBox1_PreviewTextInput;
            Placenum.PreviewTextInput += TextBox1_PreviewTextInput;
            Placesign.PreviewTextInput += TextBox1_PreviewTextInput;
            BiletGrid.RowEditEnding += BiletGrid_RowEditEnding;
            grid = g1;
            grid1 = g2;
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
            grid1.ItemsSource = MarshrutTable.DefaultView;
            connection.Close();

        }

        private void BiletGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            UpdateDBB();
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
            grid.ItemsSource = BiletTable.DefaultView;
            connection.Close();

        }
        private void MarshrutGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            UpdateDBM();
        }

        private void TextBox1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsNumber(Convert.ToChar(e.Text))) e.Handled = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT * FROM Marshrut";
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


        }

        private void CreateBilet_Click(object sender, RoutedEventArgs e)
        {
            DateTime? datee = Date.SelectedDate;
            if (Mid.Text == "" || StartP.Text == "" || StopP.Text == "" || TStart.Text == "" || Tall.Text == "" || Placenum.Text == "" || Placesign.Text == "" || datee == null) { MessageBox.Show("Заполните все поля"); return; }
            if (TStart.Text.IndexOf(':') == -1) { MessageBox.Show("Время не правильное"); return; }
            string[] tm = TStart.Text.Split(':');
            if (Convert.ToInt32(tm[0]) > 23 || Convert.ToInt32(tm[0]) < 0 || Convert.ToInt32(tm[1]) > 59 || Convert.ToInt32(tm[1]) < 0) { MessageBox.Show("Время не правильное"); return; }
            if (Convert.ToInt32(Placesign.Text) < 0 || Convert.ToInt32(Placesign.Text) > 1) { MessageBox.Show(" 1 или 0"); return; }
            DataTable temptable = new DataTable();

            string sql = "Select * from Marshrut where RoutelD =" + Mid.Text;
            SqlConnection connection = null;
            connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(sql, connection);
            adapterm = new SqlDataAdapter(command);
            connection.Open();
            adapterm.Fill(temptable);
            connection.Close();
            if (temptable.Rows.Count == 0) { MessageBox.Show("Такого маршрута нету"); return; }
            DataRowView comtemp = temptable.DefaultView[0];
            int idbus = (int)comtemp.Row.ItemArray[5];
            int idvod = (int)comtemp.Row.ItemArray[6];
            string[] tstr = comtemp.Row.ItemArray[3].ToString().Split(';');
            int nach = 0;
            int kon = 0;
            for (int i = 0; i < tstr.Length; i++)
            {
                if (StartP.Text == tstr[i] && nach == 0) nach = i + 1;
                if (StopP.Text == tstr[i] && kon == 0) kon = i + 1;
            }

            if (nach == 0 || kon == 0) { MessageBox.Show("Пунктов в маршруте нету"); return; }
            if (kon < nach) { MessageBox.Show("Конечный пункт раньше начального"); return; }

            temptable.Clear();
            sql = "Select * from Bilet where PlaceNumber = " + Placenum.Text + " and RoutelD = " + Mid.Text;
            connection = null;
            connection = new SqlConnection(connectionString);
            command = new SqlCommand(sql, connection);
            adapterm = new SqlDataAdapter(command);
            connection.Open();
            adapterm.Fill(temptable);
            connection.Close();

            if (temptable.Rows.Count != 0) { MessageBox.Show("Билет с таким номером существует"); return; }


            temptable.Clear();
            sql = "Select * from Bilet where RoutelD = " + Mid.Text;
            connection = null;
            connection = new SqlConnection(connectionString);
            command = new SqlCommand(sql, connection);
            adapterm = new SqlDataAdapter(command);
            connection.Open();
            adapterm.Fill(temptable);
            connection.Close();
            int kolvbil = temptable.Rows.Count;
            temptable.Clear();
            sql = "Select * from Bus where BusNumber = " + idbus;
            connection = null;
            connection = new SqlConnection(connectionString);
            command = new SqlCommand(sql, connection);
            adapterm = new SqlDataAdapter(command);
            connection.Open();
            adapterm.Fill(temptable);
            connection.Close();

            int numinbus= (int)temptable.DefaultView[0].Row.ItemArray[16];
            
            if (kolvbil == numinbus) {MessageBox.Show("Нету свободных мест"); return; }
            DateTime dd = new DateTime(datee.Value.Year, datee.Value.Month, datee.Value.Day, Convert.ToInt32(tm[0]), Convert.ToInt32(tm[1]), datee.Value.Second);

            if (dd < DateTime.Now) { MessageBox.Show("Время отправления уже прошло"); return; }

            string ddd = dd.Year + "-"+dd.Month+"-"+dd.Day+" "+dd.Hour+":"+dd.Minute+":"+dd.Second;

            sql = "Insert into Bilet(RoutelD,PointStart,PointStop,DateStart,TimeAll,PlaceNumber,PlaceSign,BusNumber,DriverlD) VALUES(" + Mid.Text + ",'" + StartP.Text + "','" + StopP.Text + "','" + ddd + "','" + Tall.Text + "'," + Placenum.Text + "," + Placesign.Text + "," + idbus + "," + idvod + ") ";
            connection = null;
            connection = new SqlConnection(connectionString);
            command = new SqlCommand(sql, connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            UpdateDBM();
            UpdateDBB();

        }
    }
}
