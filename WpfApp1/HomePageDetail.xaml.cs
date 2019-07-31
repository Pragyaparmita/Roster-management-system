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
    /// Interaction logic for HomePageDetail.xaml
    /// </summary>
    public partial class HomePageDetail : UserControl
    {
        public HomePageDetail()
        {
            InitializeComponent();
        }

        public HomePageDetail(string DayText, int gridRow)
        {
            InitializeComponent();
            this.DaysLabel.Text = "\n" + DayText;


            if (gridRow% 2 == 0)
            {
                this.DaysLabel.Background = Brushes.Azure;
            }
            else
            {
                var new_color = new BrushConverter();
                this.DaysLabel.Background = (Brush)new_color.ConvertFrom("#FFBDE5E5");
            }


            Grid.SetRow(this, gridRow);
            Grid.SetColumn(this, 0);
        }
    }
}
