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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for DataGridDetail.xaml
    /// </summary>
    public partial class DataGridDetail : UserControl
    {
        public DataGridDetail()
        {
            InitializeComponent();
            List<compactUser> users = new List<compactUser>();
            users.Add(new compactUser() { Id = 1, Name = "John Doe", Shift = "1", ImageUrl = "http://www.wpf-tutorial.com/images/misc/john_doe.jpg" });
            users.Add(new compactUser() { Id = 2, Name = "Jane Doe", Shift = "2", ImageUrl = "C:/Users/user/documents/profile.jpg" });
            users.Add(new compactUser() { Id = 3, Name = "Sammy Doe", Shift = "3" });

            compactDetailGrid.ItemsSource = users;
        }
    }

    public class compactUser
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Shift { get; set; }
        public String ImageUrl { get; set; }
    }
}
