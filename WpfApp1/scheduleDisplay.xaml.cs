using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;


namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for ScheduleDisplay.xaml
    /// </summary>
    public partial class ScheduleDisplay : UserControl
    {
        public ScheduleDisplay()
        {
            InitializeComponent();
        }


        public ScheduleDisplay(TimeSchedule _TimeSchedule, int GridRow, int GridColum)
        {
            InitializeComponent();
            this.txbEmployee.Text = _TimeSchedule.Name;
            this.txbTime.Text = _TimeSchedule.Time;

            this.scheduleButton.BorderThickness = new Thickness(0, 0, 0, 0);
            if (GridRow % 2 == 0)
            {
                this.scheduleButton.Background = Brushes.Azure;
                this.scheduleButton.BorderBrush = Brushes.Azure;
            }
            else
            {
                var new_color = new BrushConverter();
                this.scheduleButton.Background = (Brush)new_color.ConvertFrom("#FFBDE5E5");
                this.scheduleButton.BorderBrush = (Brush)new_color.ConvertFrom("#FFBDE5E5");
            }


            this.ToolTipName.Text = _TimeSchedule.Name;
            string connectionstring = "SERVER=localhost;DATABASE=employeedatabase;UID=root;PASSWORD=Thisisit123#;";
            MySqlConnection connection;
            MySqlCommand cmd;
            connection = new MySqlConnection(connectionstring);

            try
            {
                
                cmd = new MySqlCommand("select Sunday_Start, Sunday_End,Monday_Start, Monday_End,Tuesday_Start, Tuesday_End,Wednesday_Start, Wednesday_End,Thursday_Start, Thursday_End,Friday_Start, Friday_End,Last_Name from employeedatabase.employeedata where id='" + _TimeSchedule.Id + "'", connection);
                connection.Open();
                MySqlDataReader empReader = cmd.ExecuteReader();
                while (empReader.Read())
                {
                    this.ToolTipBody.Text = "Sunday:            " + empReader[0].ToString() + " to " + empReader[1].ToString() + "\nMonday:          " + empReader[2].ToString() + " to " + empReader[3].ToString() + "\nTuesday            " + empReader[4].ToString() + " to " + empReader[5].ToString() + "\nWednesday:     " + empReader[6].ToString() + " to " + empReader[7].ToString() + "\nThusday:           " + empReader[8].ToString() + " to " + empReader[9].ToString() + "\nFriday:               " + empReader[10].ToString() + " to " + empReader[11].ToString();
                    this.ToolTipName.Text += " " + empReader[12];
                    //byte[] data = (byte[])(empReader["Image"]);

                    //if (data == null)
                    //{
                    //    this.ToolTipImage.Source = null;
                    //}
                    //else
                    //{

                    //    MemoryStream strm = new MemoryStream();
                    //    strm.Write(data, 0, data.Length);
                    //    strm.Position = 0;
                    //    System.Drawing.Image img = System.Drawing.Image.FromStream(strm);
                    //    BitmapImage bi = new BitmapImage();
                    //    bi.BeginInit();
                    //    MemoryStream ms = new MemoryStream();
                    //    img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                    //    ms.Seek(0, SeekOrigin.Begin);
                    //    bi.StreamSource = ms;
                    //    bi.EndInit();
                    //    this.ToolTipImage.Source = bi;
                    //}

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            connection.Close();
            

            this.ToolTipId.Text = "Id:   " + _TimeSchedule.Id;

            Grid.SetRow(this, GridRow);
            Grid.SetColumn(this, GridColum);

        }

        private void txbEmployee_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win = (MainWindow)Window.GetWindow(this);
            win.suggestionDisplay(Grid.GetRow(this), Grid.GetColumn(this));

            this.txbTime.Text = "";
            this.txbEmployee.Text = "";
            this.ToolTipName.Text = "";
            this.ToolTipBody.Text = "This slot is empty";
            this.ToolTipId.Text = "";
        }
    }
}