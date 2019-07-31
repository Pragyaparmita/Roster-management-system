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
using System.Data;
using System.IO;
using MySql.Data.MySqlClient;
using System.Drawing;
using Microsoft.Win32;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for compactUserDetail.xaml
    /// </summary>
    public partial class compactUserDetail : UserControl
    {
        public compactUserDetail()
        {
            InitializeComponent();
        }

        public compactUserDetail(int id, int GridRow)
        {
            InitializeComponent();


            string connectionstring = "SERVER=localhost;DATABASE=employeedatabase;UID=root;PASSWORD=Thisisit123#;";
            MySqlConnection connection = new MySqlConnection(connectionstring);
            MySqlCommand cmd = new MySqlCommand("select first_name, last_name, image from employeedatabase.employeedata where Id=" + id, connection);
            connection.Open();

            MySqlDataReader empReader = cmd.ExecuteReader();
            try
            {
                while (empReader.Read())
                {

                    compactEmpName.Text = empReader["First_name"].ToString() + " " + empReader["Last_Name"].ToString();
                    compactEmpId.Text = id.ToString();

                    byte[] data = (byte[])(empReader["Image"]);

                    if (data == null)
                    {
                        compactEmpImage.Source = null;
                    }
                    else
                    {

                        MemoryStream strm = new MemoryStream();
                        strm.Write(data, 0, data.Length);
                        strm.Position = 0;
                        System.Drawing.Image img = System.Drawing.Image.FromStream(strm);
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        ms.Seek(0, SeekOrigin.Begin);
                        bi.StreamSource = ms;
                        bi.EndInit();
                        compactEmpImage.Source = bi;
                    }
                }
                connection.Close();
            }
            catch (Exception)
            {
            }
            connection.Close();
        }
    }
}
