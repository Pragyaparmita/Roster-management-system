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
    /// Interaction logic for sugesstion.xaml
    /// </summary>
    public partial class suggestion : UserControl
    {
        private int row, column, dbId;
        public suggestion()
        {
            InitializeComponent();
        }

        public suggestion(string name, int id, int GridRow, int GridColumn, int day, int shift)
        {
            InitializeComponent();

            this.suggestionName.Text = name;
            this.suggestionID.Text = id.ToString();

            if (name == "Suggestion"||name =="Others")
            {
                this.suggestionName.Text = "\n" + name;
                this.suggestionID.Text = "";
            }

            this.scheduleButton.BorderThickness = new Thickness(0, 0, 0, 0);

            var new_color = new BrushConverter();
            this.scheduleButton.Background = (Brush)new_color.ConvertFrom("#FFBDE5E5");
            this.scheduleButton.BorderBrush = (Brush)new_color.ConvertFrom("#FFBDE5E5");

            this.ToolTipName.Text = Name;
            this.ToolTipBody.Text = "Sunday:\nMonday:\nTuesday\nWednesday\nThusday\nFriday";
            this.ToolTipId.Text = "ID";

            Grid.SetRow(this, GridRow);
            Grid.SetColumn(this, GridColumn);

            row = day;
            column = shift;
            dbId = id;
        }


        private void scheduleButton_Click(object sender, RoutedEventArgs e)
        {

            //UPDATE `employeedatabase`.`employeedata` SET `Sunday_Start` = '12' WHERE(`id` = '2');
            try
            {
                string[] days = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
                int dbColumn = column + 1;

                string connectionstring = "SERVER=localhost;DATABASE=employeedatabase;UID=root;PASSWORD=Thisisit123#;";
                MySqlConnection connection = new MySqlConnection(connectionstring);
                MySqlCommand cmd = new MySqlCommand("UPDATE `employeedatabase`.`employeeshift` SET `Shift" + dbColumn.ToString() + "` = '" + this.suggestionID.Text + "' WHERE (`Day` = '" + days[row] + "');", connection);
                connection.Open();
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                connection.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            MainWindow win = (MainWindow)Window.GetWindow(this);
            win.fetch();


            win.suggestionStack.Children.Clear();
        }
    }
}
