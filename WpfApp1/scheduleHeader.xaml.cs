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
    /// Interaction logic for scheduleHeader.xaml
    /// </summary>
    public partial class scheduleHeader : UserControl
    {
        public scheduleHeader()
        {
            InitializeComponent();
        }

        public scheduleHeader(string shiftText, int gridColumn)
        {
            InitializeComponent();
            this.ShiftName.Text = "\n" + shiftText;

            if (gridColumn % 2 == 0)
            {
                this.ShiftName.Background = Brushes.Azure;
            }
            else
            {
                var new_color = new BrushConverter();
                this.ShiftName.Background = (Brush)new_color.ConvertFrom("#FFBDE5E5");
            }


            Grid.SetRow(this, 0);
            Grid.SetColumn(this, gridColumn);
        }

    }
}
